using MISA.WebFresher042023.Demo.Common.DTO;
using MISA.WebFresher042023.Demo.Common.DTO.Account;
using MISA.WebFresher042023.Demo.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher042023.Demo.Core.Interface.Services
{
    /// <summary>
    /// interface service account
    /// created by: vdtien (18/7/2023)
    /// </summary>
    public interface IAccountService:IBaseService<AccountDTO, AccountCreatedDTO, AccountUpdateDTO>
    {
        /// <summary>
        /// lay danh sach tai khoan theo id tai khoan cha
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>danh sach tai khoan</returns>
        /// created by: vdtien (18/7/2023)
        public Task<List<AccountDTO>> GetListAccountByParentId(Guid parentId);


        /// <summary>
        /// Lay danh sach tai khoan con theo danh sach tai khoan cha
        /// </summary>
        /// <param name="parentList"></param>
        /// <returns></returns>
        public Task<List<AccountDTO>> GetListAccountByListParentId(List<Guid> listParentId);

        /// <summary>
        /// lay danh sach ban ghi phan cap cay theo paging va filter
        /// </summary>
        /// <returns>danh sach ban ghi</returns>
        /// Created by: vdtien (18/6/2023)
        public Task<ListRecords<AccountDTO>?> GetListTreeAsync(int pageSize, int pageNumber, string keySearch,bool isRoot, int grade);
    }
}
