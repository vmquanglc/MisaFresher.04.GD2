﻿using AutoMapper;
using MISA.WebFresher042023.Demo.Common.DTO;
using MISA.WebFresher042023.Demo.Common.DTO.Employee;
using MISA.WebFresher042023.Demo.Common.Entity;
using MISA.WebFresher042023.Demo.Common.Exceptions;
using MISA.WebFresher042023.Demo.Core.Interface.Repositories;
using MISA.WebFresher042023.Demo.Core.Interface.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MISA.WebFresher042023.Demo.Core.Services
{
    public abstract class BaseService<TEntity, TEntityDTO, TEntityCreateDTO, TEntityUpdateDTO> : IBaseService<TEntityDTO, TEntityCreateDTO, TEntityUpdateDTO>
    {
        protected readonly IBaseRepository<TEntity> _baseRepository;
        protected readonly IMapper _mapper;

        protected BaseService(IBaseRepository<TEntity> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// lay danh sach tat ca nhan vien
        /// </summary>
        /// <returns>danh sach tat ca nhan vien</returns>
        /// Created by: vdtien (19/6/2023)
        public async Task<List<TEntityDTO>> GetAllAsync()
        {
            var records = await _baseRepository.GetAllAsync();
            if (records == null) return new List<TEntityDTO>();
            var recordsDTO = _mapper.Map<List<TEntityDTO>>(records);
            return recordsDTO;
        }

        /// <summary>
        /// lay danh sach theo paging va filter
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keySearch"></param>
        /// <returns>danh sach ban ghi</returns>
        /// Created by: vdtien (19/6/2023)
        public async Task<ListRecords<TEntityDTO>> GetListAsync(int pageSize, int pageNumber, string keySearch)
        {
            int linit = pageSize <= 0 ? 10 : pageSize;
            int offset = pageNumber <= 0 ? 0 : (pageNumber - 1) * linit;
            var results = await _baseRepository.GetListAsync(linit, offset, keySearch);
            var records = results?.Data ?? new List<TEntity>();
            var recordsDTO = _mapper.Map<List<TEntityDTO>>(records);
            var res = new ListRecords<TEntityDTO>()
            {
                TotalRecord = results?.TotalRecord ?? 0,
            
                Data = recordsDTO ?? new List<TEntityDTO>()
            };

            return res;
        }

        /// <summary>
        /// lay 1 ban ghi theo id
        /// </summary>
        /// <param name="recordId">id ban ghi</param>
        /// <returns>ban ghi</returns>
        /// <exception cref="NotFoundException"></exception>
        /// Created by: vdtien (19/6/2023)
        public async Task<TEntityDTO?> GetAsync(Guid? recordId)
        {
            var entity = await _baseRepository.GetAsync(recordId);
            if (entity == null)
            {
                throw new NotFoundException();
            }
            var entityDTO = _mapper.Map<TEntityDTO>(entity);
            return entityDTO;
        }

        /// <summary>
        /// them moi 1 ban ghi
        /// </summary>
        /// <param name="entityCreateDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// Created by: vdtien (19/6/2023)
        public virtual async Task<TEntityDTO?> InsertAsync(TEntityCreateDTO entityCreateDTO)
        {
            var tableName = typeof(TEntity).Name;
            var entity = _mapper.Map<TEntity>(entityCreateDTO);

            var fieldId = entity?.GetType().GetProperty($"{tableName}Id");
            if (fieldId != null && fieldId.CanWrite)
            {
                fieldId.SetValue(entity, Guid.NewGuid());

            }
            if (entity is Base)
            {


                var fieldCreatedBy = entity.GetType().GetProperty("CreatedBy");
                if (fieldCreatedBy != null && fieldCreatedBy.CanWrite)
                {
                    fieldCreatedBy.SetValue(entity, Guid.NewGuid());

                }
                var fieldCreatedDate = entity.GetType().GetProperty("CreatedDate");
                if (fieldCreatedDate != null && fieldCreatedDate.CanWrite)
                {
                    fieldCreatedDate.SetValue(entity, DateTime.Now);

                }
                var fieldModifiedBy = entity.GetType().GetProperty("ModifiedBy");
                if (fieldModifiedBy != null && fieldModifiedBy.CanWrite)
                {
                    fieldModifiedBy.SetValue(entity, Guid.NewGuid());

                }
                var fieldModifiedDate = entity.GetType().GetProperty("ModifiedDate");
                if (fieldModifiedDate != null && fieldModifiedDate.CanWrite)
                {
                    fieldModifiedDate.SetValue(entity, DateTime.Now);

                }
            }

            var result = await _baseRepository.InsertAsync(entity);
            if (result == null)
            {
                // throw loi server
                throw new Exception();
            }
            var entityDTO = _mapper.Map<TEntityDTO>(result);
            return entityDTO;
        }

        /// <summary>
        /// cap nhat mot ban ghi
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// Created by: vdtien (19/6/2023)
        public virtual Task<TEntityDTO?> UpdateAsync(Guid recordId, TEntityUpdateDTO record)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Xoa 1 ban ghi theo id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// Created by: vdtien (19/6/2023)
        public virtual async Task<int> DeleteAsync(Guid recordId)
        {
            var entityExist = await _baseRepository.GetAsync(recordId);
            if (entityExist == null)
            {
                throw new NotFoundException();
            }
            var result = await _baseRepository.DeleteAsync(recordId);
            if (result == 0)
            {
                throw new Exception();
            }

            return result;
        }

        /// <summary>
        /// xoa nhieu ban ghi
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>so ban ghi bi anh huong</returns>
        /// Created by: vdtien (20/6/2023)
        public async Task<int> DeleteMultiAsync(List<Guid> listId)
        {
            string listIdStr = string.Join(",", listId.Select(id => $"'{id.ToString()}'"));

            var results = await _baseRepository.DeleteMultiAsync(listIdStr);
            return results;
        }

        /// <summary>
        /// lay danh sach ban ghi theo filter
        /// </summary>
        /// <returns>danh sach ban ghi</returns>
        /// Created by: vdtien (27/6/2023)
        public async Task<List<TEntityDTO>> GetListByFilterAsync(string keySearch)
        {
            var records = await _baseRepository.GetListByFilterAsync(keySearch);
            if (records == null) return new List<TEntityDTO>();
            var recordsDTO = _mapper.Map<List<TEntityDTO>>(records);
            return recordsDTO;
        }

    }
}
