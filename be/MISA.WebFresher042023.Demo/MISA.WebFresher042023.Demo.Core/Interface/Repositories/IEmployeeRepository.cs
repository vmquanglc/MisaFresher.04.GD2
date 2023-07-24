using MISA.WebFresher042023.Demo.Common.DTO;
using MISA.WebFresher042023.Demo.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher042023.Demo.Core.Interface.Repositories
{
    /// <summary>
    /// interface employee repository
    /// </summary>
    /// Created by: vdtien (18/6/2023)
    /// Modified by: vdtien (19/6/2023)
    public interface IEmployeeRepository: IBaseRepository<Employee>
    {
        /// <summary>
        /// lay ma nhan vien moi
        /// </summary>
        /// <returns>ma nhan vien moi</returns>
        /// Created by: vdtien (18/8/2023)
        public Task<string> GetNewEmployeeCodeRepositoryAsync();

        /// <summary>
        /// Kiem tra ma trung
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeId"></param>
        /// <returns>ma trung hoac null</returns>
        /// Created by: vdtien (18/6/2023)
        public Task<string?> IsDupEmployeecodeRepositoryAsync(string employeeCode, Guid? employeeId);
    }
}
