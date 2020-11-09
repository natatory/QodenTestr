using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApp
{
    // TODO 4: unauthorized users should receive 401 status code
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet]
        public ValueTask<Account> Get()
        {
            return _accountService.LoadOrCreateAsync(
                User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value); /* TODO 3: Get user id from cookie */
                

            //return _accountService.LoadOrCreateAsync(HttpContext.User.Identities
            //.FirstOrDefault(ci => ci.AuthenticationType == "ApplicationCookie")
            //.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType).Value);
        }

        //TODO 5: Endpoint should works only for users with "Admin" Role
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public Account GetByInternalId([FromRoute] int id)
        {
            return _accountService.GetFromCache(id);
        }

        [Authorize]
        [HttpPost("counter")]
        public async Task UpdateAccount()
        {
            //Update account in cache, don't bother saving to DB, this is not an objective of this task.
            var account = await Get();
            account.Counter++;
        }
    }
}