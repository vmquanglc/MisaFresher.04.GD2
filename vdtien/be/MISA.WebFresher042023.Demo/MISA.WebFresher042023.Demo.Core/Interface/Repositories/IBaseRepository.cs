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
    /// interface Base repository
    /// </summary>
    /// <typeparam name="TEnitty"></typeparam>
    /// Created by: vetien (19/6/2023)
    public interface IBaseRepository<TEnitty>
    {

        /// <summary>
        /// lay thong tin ban ghi theo id
        /// </summary>
        /// <param name="recordId">id ban ghi</param>
        /// <returns>nhan vien</returns>
        /// Created by: vdtien (18/6/2023)
        public Task<TEnitty?> GetAsync(Guid? recordId);

        /// <summary>
        /// lay thong tin tat ca ban ghi
        /// </summary>
        /// <returns>danh sach ban ghi</returns>
        /// Created by: vdtien (18/6/2023)
        public Task<List<TEnitty>> GetAllAsync();

        /// <summary>
        /// lay danh sach ban ghi theo paging va filter
        /// </summary>
        /// <returns>danh sach ban ghi</returns>
        /// Created by: vdtien (18/6/2023)
        public Task<ListRecords<TEnitty>?> GetListAsync(int limit, int offset, string keySearch);

        /// <summary>
        /// lay danh sach ban ghi theo filter
        /// </summary>
        /// <returns>danh sach ban ghi</returns>
        /// Created by: vdtien (27/6/2023)
        public Task<List<TEnitty>> GetListByFilterAsync(string keySearch);


        /// <summary>
        /// them moi 1 ban ghi
        /// </summary>
        /// <returns>ban ghi</returns>
        /// Created by: vdtien (18/6/2023)
        public Task<TEnitty?> InsertAsync(TEnitty record);

        /// <summary>
        /// cap nhat 1 ban ghi
        /// </summary>
        /// <returns>ban ghi</returns>
        /// Created by: vdtien (18/6/2023)
        public Task<TEnitty?> UpdateAsync(TEnitty record);

        /// <summary>
        /// xoa 1 ban ghi
        /// </summary>
        /// <returns>so ban ghi bi anh huong</returns>
        /// Created by: vdtien (18/6/2023)
        public Task<int> DeleteAsync(Guid recordId);

        /// <summary>
        /// xoa nhieu ban ghi
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>so ban ghi bi anh huong</returns>
        /// Created by: vdtien (20/6/2023)
        public Task<int> DeleteMultiAsync(string listId);
    }
}
