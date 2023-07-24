using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WebFresher042023.Demo.Common.DTO;
using MISA.WebFresher042023.Demo.Common.Entity;
using MISA.WebFresher042023.Demo.Common.Exceptions;
using MISA.WebFresher042023.Demo.Core.Interface.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher042023.Demo.Infrastructure.Repositories
{
    public class EmployeeRepository :BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// lay ma moi
        /// </summary>
        /// <returns>ma moi</returns>
        /// Created by: vdtien(17/6/2023)
        public async Task<string> GetNewEmployeeCodeRepositoryAsync()
        {
            // Khởi tạo kết nối tới DB MariaDB
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {

                var result = await mySqlConnection.QueryFirstOrDefaultAsync<string>("Proc_Employee_getNewEmployeeCode", commandType: System.Data.CommandType.StoredProcedure);

                return result;

            }
        }

        /// <summary>
        /// kiem tra ma trung
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeId"></param>
        /// <returns>ma trung hoac null</returns>
        /// Created by: vdtien (18/6/2023)
        public async Task<string?> IsDupEmployeecodeRepositoryAsync(string employeeCode, Guid? employeeId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@v_EmployeeCode", employeeCode);
            parameters.Add("@v_EmployeeId", employeeId);

            // Khởi tạo kết nối tới DB MariaDB
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {
                var result = await mySqlConnection.QueryFirstOrDefaultAsync<string>("Proc_Employee_CheckDupEmployeeCode", parameters, commandType: System.Data.CommandType.StoredProcedure);

                return result;

            }
        }
    }
}
