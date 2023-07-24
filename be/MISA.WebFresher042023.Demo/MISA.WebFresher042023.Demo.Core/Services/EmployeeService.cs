using AutoMapper;
using MISA.WebFresher042023.Demo.Common.Commons;
using MISA.WebFresher042023.Demo.Common.DTO.Employee;
using MISA.WebFresher042023.Demo.Common.Entity;
using MISA.WebFresher042023.Demo.Common.Enums;
using MISA.WebFresher042023.Demo.Common.Exceptions;
using MISA.WebFresher042023.Demo.Common.Resources;
using MISA.WebFresher042023.Demo.Core.Interface.Excels;
using MISA.WebFresher042023.Demo.Core.Interface.Repositories;
using MISA.WebFresher042023.Demo.Core.Interface.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.WebFresher042023.Demo.Core.Services
{
    public class EmployeeService : BaseService<Employee,EmployeeDTO,EmployeeCreateDTO,EmployeeUpdateDTO> ,IEmployeeService
    {
        protected readonly IEmployeeRepository _employeeRepository;
        protected readonly IDepartmentRepository _departmentRepository;
        private readonly IExcelInfra _excelInfra;
        public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository,IExcelInfra excelInfra, IMapper mapper) : base(employeeRepository, mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _excelInfra = excelInfra;
        }

        /// <summary>
        /// them nhan vien
        /// </summary>
        /// <returns>nhan vien</returns>
        /// Created by: vdtien(17/6/2023)
        public override async Task<EmployeeDTO?> InsertAsync(EmployeeCreateDTO employeeCreateDTO)
        {
            // check dup employeecode
            var dupCode = await _employeeRepository.IsDupEmployeecodeRepositoryAsync(employeeCreateDTO.EmployeeCode, null);
            if (dupCode != null)
            {
                var errMsg = String.Format(ResourceVN.UserMsg_DupEmployeeCode, dupCode);
                var errsMsgs = new List<string>();
                errsMsgs.Add(errMsg);
                var errsMore = new Dictionary<string, List<string>>();
                errsMore.Add("EmployeeCode", errsMsgs);
                throw new DupCodeException(errsMsgs, errsMore);
            }
            // check department exsit
            var department = await _departmentRepository.GetAsync(employeeCreateDTO.DepartmentId);
            if(department == null)
            {
                throw new NotFoundException();
            }


            var result = await base.InsertAsync(employeeCreateDTO);
            return result;
        }

        /// <summary>
        /// cap nhat nhan vien
        /// </summary>
        /// <returns> nhan vien</returns>
        /// Created by: vdtien(17/6/2023)
        public override async Task<EmployeeDTO?> UpdateAsync(Guid employeeId, EmployeeUpdateDTO employeeUpdateDTO)
        {

            var employee = _mapper.Map<Employee>(employeeUpdateDTO);

            // valid employeeId
            //if(employee.EmployeeId != employeeId)
            //{
            //    throw new NotFoundException();
            //}
            employee.EmployeeId = employeeId;


            // validate input

            var employeeExist = await _employeeRepository.GetAsync(employee.EmployeeId);
            if(employeeExist == null)
            {
                throw new NotFoundException();
            }
            employee.CreatedBy = employeeExist.CreatedBy;
            employee.CreatedDate = employeeExist.CreatedDate;
            employee.ModifiedBy = Guid.NewGuid();
            employee.ModifiedDate = new DateTime();

            // check dup employeecode
            var dupCode = await _employeeRepository.IsDupEmployeecodeRepositoryAsync(employee.EmployeeCode, employee.EmployeeId);
            if (dupCode != null)
            {
                var errMsg = String.Format(ResourceVN.UserMsg_DupEmployeeCode, dupCode);
                var errsMsgs = new List<string>();
                errsMsgs.Add(errMsg);
                var errsMore = new Dictionary<string, List<string>>();
                errsMore.Add("EmployeeCode", errsMsgs);
                throw new DupCodeException(errsMsgs, errsMore);

            }
            // check department exsit
            var department = await _departmentRepository.GetAsync(employee.DepartmentId);
            if (department == null)
            {
                throw new NotFoundException();
            }

            var result = await _employeeRepository.UpdateAsync(employee);
            if (result == null)
            {
                // throw loi server
                throw new Exception();
            }
            var employeeDTO = _mapper.Map<EmployeeDTO>(result);
            return employeeDTO;
        }

        /// <summary>
        /// lay ma moi
        /// </summary>
        /// <returns>ma moi</returns>
        /// Created by: vdtien(17/6/2023)
        public async Task<string> GetNewEmployeeCodeSerivceAsync()
        {
            var result = await _employeeRepository.GetNewEmployeeCodeRepositoryAsync();
            return result;
        }

        public async Task<byte[]> ExportEmployeesToExcel(string keySearch)
        {
            // Lấy dữ liệu nhân viên từ nguồn dữ liệu (database, API, ...)
            var employeeData = await GetEmployeeData(keySearch);

            // tao tile cho file
            var title = "Danh sách nhân viên";

            // Sử dụng _excelService để xuất dữ liệu ra file Excel
            var excelData = _excelInfra.ExportToExcel(employeeData, title);

            return excelData;
        }

        /// <summary>
        /// Lay danh sach do vao datatable
        /// </summary>
        /// <returns>datatable</returns>
        /// Created by: vdtien (27/6/2023)
        private async Task<DataTable> GetEmployeeData(string keySearch)
        {
            // Lấy danh sách nhân viên từ database
      
            var employeesEntity = await _employeeRepository.GetListByFilterAsync(keySearch);
            //if (employeesDTO == null) employeesDTO = new List<EmployeeDTO>();
            var employees = _mapper.Map<List<EmployeeExcelDTO>>(employeesEntity);

            // Tạo DataTable với các cột tương ứng
            var data = new DataTable();

            // Lấy danh sách trường của đối tượng Employee
            var properties = typeof(EmployeeExcelDTO).GetProperties();

            // thêm các cột vào datatable dựa trên danh sách trường
            data.Columns.Add("STT");
            foreach (var property in properties)
            {
                var displayNameAttribute = property.GetCustomAttribute<DisplayAttribute>();
                var columnName = displayNameAttribute != null ? displayNameAttribute.GetName() : property.Name;
                data.Columns.Add(columnName);
            }
            //// Thêm các cột vào DataTable dựa trên danh sách trường
            //foreach (var property in properties)
            //{
            //    data.Columns.Add(property.Name);
            //}
            // Đổ dữ liệu từ danh sách nhân viên vào DataTable
            var index = 1;
            foreach (var employee in employees)
            {
                var row = data.NewRow();
                row["STT"] = index;
                index++;
                // Đặt giá trị của từng trường vào các cột tương ứng
                foreach (var property in properties)
                {
                    var displayNameAttribute = property.GetCustomAttribute<DisplayAttribute>();
                    var columnName = displayNameAttribute != null ? displayNameAttribute.GetName() : property.Name;
                    // trường là DateTime
                    if (property.PropertyType == typeof(DateTime?))
                    {
                        var dateTimeValue = (DateTime?)property.GetValue(employee);
                        if (dateTimeValue.HasValue)
                        {
                            var processedDateTime = ConvertDateTimeToString(dateTimeValue.Value);
                            row[columnName] = processedDateTime;
                        }
                        else
                        {
                            row[columnName] = DBNull.Value;
                        }
                    } 
                    else if (property.PropertyType == typeof(Gender?))
                    {
                        var genderValue = (Gender?) property.GetValue(employee);
                        var processedGender = ConvertGender(genderValue);
                        row[columnName] = processedGender;
                    }
                    else
                    {
                        row[columnName] = property.GetValue(employee);
                    }

                }

                data.Rows.Add(row);
            }

            return data;
        }

        /// <summary>
        /// convert datetime về dang dd/mm/yyyy
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// Created by: vdtien (27/6/2023)
        private string ConvertDateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// convert Gender
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        /// Created by: vdtien (27/6/2023)
        private string ConvertGender(Gender? gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return ResourceVN.Male;
               case Gender.Female:
                    return ResourceVN.FeMale;
                case Gender.Other:
                    return ResourceVN.Other;
                default:
                    return "";
            }
        }

    }
}
