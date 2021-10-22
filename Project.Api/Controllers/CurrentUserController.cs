using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Common.Domain.Model;
using Common.API;
using System.Threading.Tasks;
using Project.Core.Services;
using Project.Core.Dto;

namespace Project.Core.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CurrentUserController : Controller
    {

        private readonly CurrentUser _user;
        private readonly ILogger _logger;
        private UsuarioService _pessoaService;

        public CurrentUserController(CurrentUser user, UsuarioService pessoaService, ILoggerFactory logger)
        {
            this._user = user;
            this._logger = logger.CreateLogger<CurrentUserController>();
            this._logger.LogInformation("AccountController init success");

            this._pessoaService = pessoaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = new HttpResult<dynamic>(this._logger);
            try
            {
                return await Task.Run(() =>
                {
                    var claims = this._user.GetClaims();
                    if (claims.IsAny())
                    {
                        return result.ReturnCustomResponse(claims);
                    }

                    return result.ReturnCustomResponse(new { warning = "No Claims found!" });

                });

            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Seed - CurrentUser");
            }

        }

        //[Authorize]
        //[HttpPost("Verify")]
        //public virtual async Task<IActionResult> Verify([FromBody] UsuarioDto dto)
        //{
        //    var result = new HttpResult<dynamic>(this._logger, this._pessoaService);
        //    try
        //    {
        //        var returnModel = await this._pessoaService.VerificarExistencia(dto);
        //        return result.ReturnCustomResponse(returnModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        return result.ReturnCustomException(ex, "Usuario", dto);
        //    }
        //}


    }
}
