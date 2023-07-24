using AutoMapper;
using MISA.WebFresher042023.Demo.Common.DTO;
using MISA.WebFresher042023.Demo.Common.DTO.Account;
using MISA.WebFresher042023.Demo.Common.DTO.Employee;
using MISA.WebFresher042023.Demo.Common.Entity;
using MISA.WebFresher042023.Demo.Common.Exceptions;
using MISA.WebFresher042023.Demo.Common.Resources;
using MISA.WebFresher042023.Demo.Core.Interface.Repositories;
using MISA.WebFresher042023.Demo.Core.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher042023.Demo.Core.Services
{
    public class AccountService : BaseService<Account, AccountDTO, AccountCreatedDTO, AccountUpdateDTO>, IAccountService
    {
        #region Field
        private readonly IAccountRepository _accountRepository; 
        #endregion

        #region Constructor
        public AccountService(IAccountRepository accountRepository, IMapper mapper) : base(accountRepository, mapper)
        {
            _accountRepository = accountRepository;
        }
        #endregion


        #region Methods

        /// <summary>
        /// Lay danh sach tai khoan con theo danh sach tai khoan cha
        /// </summary>
        /// <param name="listParentId"></param>
        /// <returns></returns>
        public async Task<List<AccountDTO>> GetListAccountByListParentId(List<Guid> listParentId)
        {

            string listIdStr = string.Join(",", listParentId.Select(id => $"'{id.ToString()}'"));

            var records = await _accountRepository.GetListAccountByListParentId(listIdStr);

            if (records == null) return new List<AccountDTO>();
            var recordsDTO = _mapper.Map<List<AccountDTO>>(records);

            return  (List<AccountDTO>)recordsDTO;
        }

        /// <summary>
        /// lay danh sach tai khoan theo id tai khoan cha
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<AccountDTO>> GetListAccountByParentId(Guid parentId)
        {
            var result = await _accountRepository.GetListAccountByParentId(parentId);
            var accountsDTO = _mapper.Map<List<AccountDTO>>(result);
            return accountsDTO;
        }

        /// <summary>
        /// them tai khoan
        /// </summary>
        /// <returns>them tai khoan</returns>
        /// Created by: vdtien(17/6/2023)
        public override async Task<AccountDTO?> InsertAsync(AccountCreatedDTO accountCreateDTO)
        {
            var account = _mapper.Map<Account>(accountCreateDTO);
            // check dup employeecode
            var dupCode = await _accountRepository.IsDupAccountCodeRepositoryAsync(account.AccountCode, null);
            if (dupCode != null)
            {
                var errMsg = String.Format(ResourceVN.UserMsg_DupEmployeeCode, dupCode);
                var errsMsgs = new List<string>();
                errsMsgs.Add(errMsg);
                var errsMore = new Dictionary<string, List<string>>();
                errsMore.Add("AccountCode", errsMsgs);
                throw new DupCodeException(errsMsgs, errsMore);
            }
 

            // kiem tra neu co ParenId thi phai ton tai parentNode
            account.AccountId = Guid.NewGuid();
            account.Grade = 0;
            account.IsParent = 0;
            account.Status = 0;
            account.NumberChilds = 0;
            account.CreatedBy = Guid.NewGuid();
            account.CreatedDate = DateTime.Now;
            //account.ModifiedBy = null;
            //account.ModifiedDate = null;
            var parentNode = await _accountRepository.GetAsync(account.ParentId);

            if (account.ParentId != null)
            {
                if (parentNode == null)
                {
                    throw new NotFoundException();
                }
                // check tiền tố 
                bool isPrefix = account.AccountCode.StartsWith(parentNode.AccountCode);
                if(isPrefix == false)
                {
                    // mã tài khoản không hợp lệ

                    var errsMsgs = new List<string>();
                    errsMsgs.Add("Số tài khoản không hợp lệ. Số tài khoản chi tiết phải bắt đầu bằng số của Tài khoản tổng hợp");
                    var errsMore = new Dictionary<string, List<string>>();
                    errsMore.Add("AccountCode", errsMsgs);
                    throw new DupCodeException(errsMsgs, errsMore);

                }

                account.Grade = parentNode.Grade + 1;
            }

            var res = await _accountRepository.InsertAsync(account);
            if(res == null)
            {
                throw new Exception();
            }
            // cap nhat parentNode
            if(parentNode != null)
            {
                parentNode.IsParent = 1;
                parentNode.NumberChilds +=1;

                await _accountRepository.UpdateAsync(parentNode);
            }
            var accountDTO = _mapper.Map<AccountDTO>(res);

            return accountDTO;
        }

        public override async Task<AccountDTO?> UpdateAsync(Guid recordId, AccountUpdateDTO accountUpdateDTO)
        {
            var account = _mapper.Map<Account>(accountUpdateDTO);
            account.AccountId = recordId;
            var accountExist = await _accountRepository.GetAsync(account.AccountId);
            if(accountExist == null)
            {
                throw new NotFoundException();
            }
            // check dup employeecode
            var dupCode = await _accountRepository.IsDupAccountCodeRepositoryAsync(account.AccountCode, account.AccountId);
            if (dupCode != null)
            {
                var errMsg = String.Format(ResourceVN.UserMsg_DupEmployeeCode, dupCode);
                var errsMsgs = new List<string>();
                errsMsgs.Add(errMsg);
                var errsMore = new Dictionary<string, List<string>>();
                errsMore.Add("AccountCode", errsMsgs);
                throw new DupCodeException(errsMsgs, errsMore);
            }


           
            account.Grade = accountExist.Grade;
            account.IsParent = accountExist.IsParent;
            account.NumberChilds = accountExist.NumberChilds;
            account.CreatedBy = accountExist.CreatedBy;
            account.CreatedDate = accountExist.CreatedDate;
            account.ModifiedBy = Guid.NewGuid();
            account.ModifiedDate = DateTime.Now;

            // kiểm tra parentNode cũ và parentNode mới
           
            var parentNode = await _accountRepository.GetAsync(account.ParentId);

            if (account.ParentId != null)
            {
                // có ParentId mà parent là null => exception
                if (parentNode == null)
                {
                    throw new NotFoundException();
                }
                // check bac
                if(accountExist.Grade < parentNode.Grade || accountExist.AccountId == parentNode.AccountId)
                {
                    // lỗi bậc của con nhỏ hơn bậc của cha
                    throw new Exception();
                }

                // check tien to

                bool isPrefix = account.AccountCode.StartsWith(parentNode.AccountCode);
                if (isPrefix == false)
                {
                    // mã tài khoản không hợp lệ

                    var errsMsgs = new List<string>();
                    errsMsgs.Add("Số tài khoản không hợp lệ. Số tài khoản chi tiết phải bắt đầu bằng số của Tài khoản tổng hợp");
                    var errsMore = new Dictionary<string, List<string>>();
                    errsMore.Add("AccountCode", errsMsgs);
                    throw new DupCodeException(errsMsgs, errsMore);

                }
                account.Grade = parentNode.Grade + 1;
            }

            // check thay đổi nhánh
            if(account.ParentId != accountExist.ParentId)
            {
                if(account.IsParent == 1)
                {
                    var errsMsgs = new List<string>();
                    errsMsgs.Add("Tài khoản tổng hợp thay đổi thì tài khoản hiện tại phải là lá");
                    var errsMore = new Dictionary<string, List<string>>();
                    errsMore.Add("ParentId", errsMsgs);
                    throw new DupCodeException(errsMsgs, errsMore);
                }
            }

            var res = await _accountRepository.UpdateAsync(account);
            if (res == null)
            {
                throw new Exception();
            }
            if (account.ParentId != accountExist.ParentId)
            {
                // cap nhat parenttOld
                var parentOld = await _accountRepository.GetAsync(accountExist.ParentId);
                if (parentOld != null)
                {
                    parentOld.NumberChilds -= 1;
                    parentOld.IsParent = parentOld.NumberChilds > 0 ? 1 : 0;
                    await _accountRepository.UpdateAsync(parentOld);
                }
            }
        
            // cap nhat parentNode
            if (parentNode != null)
            {
                parentNode.IsParent = 1;
                parentNode.NumberChilds += 1;

                await _accountRepository.UpdateAsync(parentNode);
            }
            var accountDTO = _mapper.Map<AccountDTO>(res);

            return accountDTO;


            //return  new AccountDTO();
        }

        /// <summary>
        /// xoa 1 tai khoan theo id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="DupCodeException"></exception>
        public override async Task<int> DeleteAsync(Guid accountId)
        {
            // check tai khoan co phai la parent nodee hay khong
            var account = await _accountRepository.GetAsync(accountId);
            if (account == null) throw new NotFoundException();
            if(account?.IsParent == null || account?.IsParent == 0)
            {
               

                var res = await _accountRepository.DeleteAsync(accountId);

                if(res == 0)
                {
                    throw new Exception();
                }
                // giam number child cua parent node
                var parentNode = await _accountRepository.GetAsync(account.ParentId);
                if (parentNode != null)
                {
                    parentNode.NumberChilds = parentNode.NumberChilds - 1 > 0 ? parentNode.NumberChilds - 1 : 0;
                    parentNode.IsParent = parentNode.NumberChilds > 0 ? 1 : 0;

                    await _accountRepository.UpdateAsync(parentNode);
                }

                return res;
            } else
            {
                var errsMsgs = new List<string>();
                errsMsgs.Add("Xóa không thành công. Không thể xóa danh mục cha nếu chưa xóa danh mục con.");
                var errsMore = new Dictionary<string, List<string>>();
                errsMore.Add("AccountCode", errsMsgs);
                throw new DupCodeException(errsMsgs, errsMore);
            }
        }

        /// <summary>
        /// lay danh sach ban ghi theo cay
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keySearch"></param>
        /// <param name="isRoot"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public async Task<ListRecords<AccountDTO>?> GetListTreeAsync(int pageSize, int pageNumber, string keySearch,bool isRoot, int grade)
        {

            int linit = pageSize <= 0 ? 10 : pageSize;
            int offset = pageNumber <= 0 ? 0 : (pageNumber - 1) * linit;
            var results = await _accountRepository.GetListTreeAsync(linit, offset, keySearch,isRoot,grade);
            var records = results?.Data ?? new List<Account>();
            var recordsDTO = _mapper.Map<List<AccountDTO>>(records);
            var res = new ListRecords<AccountDTO>()
            {
                TotalRecord = results?.TotalRecord ?? 0,
                TotalRoot = results?.TotalRoot ?? 0,
                Data = recordsDTO ?? new List<AccountDTO>()
            };

            return res;
        }

        #endregion
    }
}
