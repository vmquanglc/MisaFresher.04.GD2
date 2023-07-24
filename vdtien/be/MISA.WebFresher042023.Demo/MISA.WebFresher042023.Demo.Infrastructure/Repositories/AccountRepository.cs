using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WebFresher042023.Demo.Common.Commons;
using MISA.WebFresher042023.Demo.Common.DTO;
using MISA.WebFresher042023.Demo.Common.Entity;
using MISA.WebFresher042023.Demo.Core.Interface.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.WebFresher042023.Demo.Infrastructure.Repositories
{
    /// <summary>
    /// account repository
    /// created by: vdtien (18/7/2023)
    /// </summary>
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        #region Constructor
        public AccountRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion

        #region Methods
        /// <summary>
        /// lay danh sach tai khoan theo id tai khoan cha
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public async Task<List<Account>> GetListAccountByParentId(Guid parentId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@v_ParentId", parentId);


            // thuc hien ket noi database
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {

                var results = await mySqlConnection.QueryAsync<Account>("Proc_Account_GetListByParentId", parameters, commandType: System.Data.CommandType.StoredProcedure);

                return (List<Account>)results;
            }
        }

        /// <summary>
        /// lay danh sach tài khoản con theo danh sách tài khoản cha
        /// </summary>
        /// <param name="parentList"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Account>> GetListAccountByListParentId(string listParentId)
        {
            // chuan bi tham so 
            var parameters = new DynamicParameters();
            parameters.Add("@v_ListParentId", listParentId, DbType.String); // Sử dụng DbType.String cho kiểu text trong proc

            // Khởi tạo kết nối tới DB MariaDB
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {

                var result = await mySqlConnection.QueryAsync<Account>("Proc_Account_GetAllChildrenByListParentId", parameters, commandType: CommandType.StoredProcedure);
                return (List<Account>)result;
            }
            
        }

        public async Task<string?> IsDupAccountCodeRepositoryAsync(string accountCode, Guid? accountId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@v_AccountCode", accountCode);
            parameters.Add("@v_AccountId", accountId);

            // Khởi tạo kết nối tới DB MariaDB
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {
                var result = await mySqlConnection.QueryFirstOrDefaultAsync<string>("Proc_Account_CheckDupAccountCode", parameters, commandType: System.Data.CommandType.StoredProcedure);

                return result;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="keySearch"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ListRecords<Account>?> GetListTreeAsync(int limit, int offset, string keySearch, bool isRoot, int grade)
        {
            // chuan bi tham so 
            var parameters = new DynamicParameters();
            parameters.Add("@v_Limit", limit);
            parameters.Add("@v_Offset", offset);
            parameters.Add("@v_KeySearch", keySearch);
            parameters.Add("@v_isRoot", isRoot);
            parameters.Add("@v_Grade", grade);
            // Khởi tạo kết nối tới DB MariaDB
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {
                var results = await mySqlConnection.QueryMultipleAsync("Proc_Account_GetListTree", parameters, commandType: System.Data.CommandType.StoredProcedure);

                var totalRecords = results.Read<int>().FirstOrDefault();
                var totalRoots = results.Read<int>().FirstOrDefault();
                var records = results.Read<Account>().ToList();
                return new ListRecords<Account>()
                {
                    TotalRecord = totalRecords,
                    TotalRoot = totalRoots,
                    Data = records
                };

            }
        }
    }
    #endregion
}

