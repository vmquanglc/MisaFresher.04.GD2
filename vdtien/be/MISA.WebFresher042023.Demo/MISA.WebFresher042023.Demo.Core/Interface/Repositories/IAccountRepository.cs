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
    /// interface account repository
    /// created by: vdtien (18/7/2023)
    /// </summary>
    public interface IAccountRepository:IBaseRepository<Account>
    {
        /// <summary>
        /// lay danh sach tai khoan theo id tai khoan cha
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public Task<List<Account>> GetListAccountByParentId(Guid parentId);

        /// <summary>
        /// Lay danh sach tai khoan con theo danh sach tai khoan cha
        /// </summary>
        /// <param name="parentList"></param>
        /// <returns></returns>
        public Task<List<Account>> GetListAccountByListParentId(string listParentId);

        /// <summary>
        /// Kiem tra ma trung
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="accountId"></param>
        /// <returns>ma trung hoac null</returns>
        /// Created by: vdtien (18/6/2023)
        public Task<string?> IsDupAccountCodeRepositoryAsync(string accountCode, Guid? accountId);


        /// <summary>
        /// lay danh sach ban ghi phan cap cay theo paging va filter
        /// </summary>
        /// <returns>danh sach ban ghi</returns>
        /// Created by: vdtien (18/6/2023)
        public Task<ListRecords<Account>?> GetListTreeAsync(int limit, int offset, string keySearch,bool isRoot, int grade);
    }
}
