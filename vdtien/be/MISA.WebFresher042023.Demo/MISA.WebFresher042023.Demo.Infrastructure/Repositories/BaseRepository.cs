

using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WebFresher042023.Demo.Common.Commons;
using MISA.WebFresher042023.Demo.Common.DTO;
using MISA.WebFresher042023.Demo.Common.Entity;
using MISA.WebFresher042023.Demo.Common.Exceptions;
using MISA.WebFresher042023.Demo.Core.Interface.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace MISA.WebFresher042023.Demo.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    {

        #region Field
        protected readonly string? _connectionString;
        #endregion

        #region Constructor
        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"];
        }
        #endregion

        #region Methods
        /// <summary>
        /// lay danh sach tat ca ban ghi
        /// </summary>
        /// <returns>danh sach tat ca ban ghi</returns>
        /// Created by: vdtien (19/6/2023)
        public async Task<List<TEntity>> GetAllAsync()
        {
            // chuan bi cau lenh
            var tableName = typeof(TEntity).Name;
            string procName = String.Format(Procedures.GET_ALL, tableName);
            // chuan bi tham so 

            // thuc hien ket noi database
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {

                var results = await mySqlConnection.QueryAsync<TEntity>(procName, commandType: System.Data.CommandType.StoredProcedure);

                return (List<TEntity>)results;
            }
        }

        /// <summary>
        /// lay danh sach ban ghi theo paging va filter
        /// </summary>
        /// <returns>danh sach ban ghi</returns>
        /// Created by: vdtien (19/6/2023)
        public async Task<ListRecords<TEntity>?> GetListAsync(int limit, int offset, string keySearch)
        {
            // chuan bi cau lenh
            var tableName = typeof(TEntity).Name;
            string procName = String.Format(Procedures.GET_LIST, tableName);
            // chuan bi tham so 
            var parameters = new DynamicParameters();
            parameters.Add("@v_Limit", limit);
            parameters.Add("@v_Offset", offset);
            parameters.Add("@v_KeySearch", keySearch);
            // Khởi tạo kết nối tới DB MariaDB
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {
                var results = await mySqlConnection.QueryMultipleAsync(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var totalRecords = results.Read<int>().FirstOrDefault();
                var records = results.Read<TEntity>().ToList();
                return new ListRecords<TEntity>()
                {
                    TotalRecord = totalRecords,
               
                    Data = records
                };

            }
        }

        /// <summary>
        /// lay 1ban ghi theo id
        /// </summary>
        /// <returns>ban ghi</returns>
        /// Created by: vdtien (19/6/2023)
        public async Task<TEntity?> GetAsync(Guid? recordId)
        {  // chuan bi cau lenh
            var tableName = typeof(TEntity).Name;
            string procName = String.Format(Procedures.GET_BY_ID, tableName);
            // chuan bi tham so 
            var parameters = new DynamicParameters();
            parameters.Add($"@v_{tableName}Id", recordId);
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {

                var result = await mySqlConnection.QueryFirstOrDefaultAsync<TEntity>(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        /// <summary>
        /// them moi 1 ban ghi
        /// </summary>
        /// <returns>ban ghi</returns>
        /// Created by: vdtien (19/6/2023)
        public async Task<TEntity?> InsertAsync(TEntity record)
        {
            // chuan bi cau lenh
            var tableName = typeof(TEntity).Name;
            string procName = String.Format(Procedures.INSERT, tableName);

            // chuan bi tham so 
            var parameters = new DynamicParameters();
            var recordId = (Guid)record.GetType().GetProperty($"{tableName}Id").GetValue(record);

            // map property của employee với tham số truyền vào database
            foreach (var prop in record.GetType().GetProperties())
            {
                parameters.Add("@v_" + prop.Name, prop.GetValue(record, null));
            }

            // Khởi tạo kết nối tới DB MariaDB
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {

                await mySqlConnection.ExecuteAsync(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var result = await GetAsync(recordId);
                return result;
            }
        }

        /// <summary>
        /// cap nhat 1 ban ghi
        /// </summary>
        /// <returns> ban ghi</returns>
        /// Created by: vdtien (19/6/2023)
        public async Task<TEntity?> UpdateAsync(TEntity record)
        {
            // chuan bi cau lenh
            var tableName = typeof(TEntity).Name;
            string procName = String.Format(Procedures.UPDATE, tableName);

            // chuan bi tham so 
            var parameters = new DynamicParameters();
            var recordId = (Guid)record.GetType().GetProperty($"{tableName}Id").GetValue(record);

            // map property của employee với tham số truyền vào database
            foreach (var prop in record.GetType().GetProperties())
            {
                parameters.Add("@v_" + prop.Name, prop.GetValue(record, null));
            }

            // Khởi tạo kết nối tới DB MariaDB
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {

                await mySqlConnection.ExecuteAsync(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var result = await GetAsync(recordId);
                return result;
            }
        }

        /// <summary>
        /// xoa 1 ban ghi
        /// </summary>
        /// <returns>so ban ghi anh huong</returns>
        /// Created by: vdtien (19/6/2023)
        public virtual async Task<int> DeleteAsync(Guid recordId)
        {
            // chuan bi cau lenh
            var tableName = typeof(TEntity).Name;
            string procName = String.Format(Procedures.DELETE, tableName);

            // chuan bi tham so 
            var parameters = new DynamicParameters();
            parameters.Add($"@v_{tableName}Id", recordId);


            // Khởi tạo kết nối tới DB MariaDB
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {

                var result = await mySqlConnection.ExecuteAsync(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return result;

            }
        }

        /// <summary>
        /// xoa nhieu ban ghi
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>so ban ghi bi anh huong</returns>
        /// Created by: vdtien (20/6/2023)
        public async Task<int> DeleteMultiAsync(string listId)
        {
            // chuan bi cau lenh
            var tableName = typeof(TEntity).Name;
            string procName = String.Format(Procedures.DELETE_MULTI, tableName);

            // chuan bi tham so 
            var parameters = new DynamicParameters();
            parameters.Add("@v_ListId", listId, DbType.String); // Sử dụng DbType.String cho kiểu text trong proc

            // Khởi tạo kết nối tới DB MariaDB
            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            using var transaction = mySqlConnection.BeginTransaction();
            try
            {

                var result = await mySqlConnection.ExecuteAsync(procName, parameters, transaction, commandType: CommandType.StoredProcedure);
                transaction.Commit();
                return result;

            }
            catch (Exception ex)
            {

                transaction.Rollback();
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Lay danh sach ban ghi theo filter
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="keySearch"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<TEntity>> GetListByFilterAsync(string keySearch)
        {
            // chuan bi cau lenh
            var tableName = typeof(TEntity).Name;
            string procName = String.Format(Procedures.GET_LIST_BY_FILTER, tableName);
            // chuan bi tham so 
            var parameters = new DynamicParameters();
            parameters.Add("@v_KeySearch", keySearch);
            // thuc hien ket noi database
            using (var mySqlConnection = new MySqlConnection(_connectionString))
            {

                var results = await mySqlConnection.QueryAsync<TEntity>(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return (List<TEntity>)results;
            }
        }
        #endregion
    }
}
