using Common.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Project.Core.Services;
using Project.Core.Dto;
using Project.Core.Filters;
using Project.Core.Data.Repository;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Common.Domain.Base;
using Common.Domain.Security;

namespace Project.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        protected readonly UsuarioService _service;
        protected readonly UsuarioRepository _rep;
        protected readonly ILogger _logger;
        protected readonly EnviromentInfo _env;

        public UsuarioController(UsuarioService service, UsuarioRepository rep, ILoggerFactory logger, EnviromentInfo env)
        {
            this._service = service;
            this._rep = rep;
            this._logger = logger.CreateLogger<UsuarioController>();
            this._env = env;
        }


        [AllowAnonymous]
        [HttpPost("CriarUsuario")]
        public virtual async Task<IActionResult> CriarUsuario([FromBody] UsuarioDtoSave dto)
        {
            var result = new HttpResult<UsuarioDtoSave>(this._logger);
            try
            {
                var returnModel = await this._service.CriarUsuario(dto);
                return result.ReturnCustomResponse(returnModel);

            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Pessoa", dto);
            }
        }

        [AllowAnonymous]
        [HttpPost("LoginChallenger")]
        public virtual async Task<IActionResult> LoginChallenger([FromBody] UsuarioDtoResult dto, [FromServices] SigningConfigurations signingConfigurations)
        {
            var result = new HttpResult<dynamic>(this._logger);
            try
            {
                var returnModel = await this._service.LoginChallenger(dto, signingConfigurations);
                return result.ReturnCustomResponse(returnModel);
            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Cliente");
            }
        }

    }
}
