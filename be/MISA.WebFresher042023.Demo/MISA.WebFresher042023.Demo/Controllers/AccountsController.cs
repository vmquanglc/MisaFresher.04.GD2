using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher042023.Demo.Common.DTO.Account;
using MISA.WebFresher042023.Demo.Core.Interface.Services;
using MISA.WebFresher042023.Demo.Core.Services;

namespace MISA.WebFresher042023.Demo.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountsController : BasesController<AccountDTO, AccountCreatedDTO, AccountUpdateDTO>
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService) : base(accountService)
        {
            _accountService = accountService;
        }




        [HttpGet("filter-tree")]
        public async Task<IActionResult> GetListAsync([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1, [FromQuery] string? keySearch = "", [FromQuery] bool isRoot = false, [FromQuery] int grade = 0)
        {
            var results = await _accountService.GetListTreeAsync(pageSize, pageNumber, keySearch ?? "", isRoot,grade);
            return StatusCode(StatusCodes.Status200OK, results);
        }

        /// <summary>
        /// lay ma nhan vien moi
        /// </summary>
        /// <returns>ma nhan vien moi</returns>
        /// Created by: vdtien (17/6/2023)
        [HttpGet("parent/{parentId}/childrens")]
        public async Task<IActionResult> GetListAccountByParentId([FromRoute] Guid parentId)
        {

            var result = await _accountService.GetListAccountByParentId(parentId);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// lay danh sach tai khoan con theo danh sach id tai khoan cha
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [HttpPost("parents/childrens")]
        public async Task<IActionResult> GetListAccountByListParentId([FromBody] List<Guid> listId)
        {
            var results = await _accountService.GetListAccountByListParentId(listId);
            return StatusCode(StatusCodes.Status200OK, results);
        }

    }
}
