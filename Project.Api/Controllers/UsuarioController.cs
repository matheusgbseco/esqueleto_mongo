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


        //[Authorize]
        //[HttpGet]
        //public virtual async Task<IActionResult> Get([FromQuery] UsuarioFilter filters)
        //{
        //    var result = new HttpResult<UsuarioDto>(this._logger, this._service);
        //    try
        //    {
        //        var searchResult = await this._service.GetByFiltersPaging(filters);
        //        return result.ReturnCustomResponse(searchResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        return result.ReturnCustomException(ex, "Pessoa", filters);
        //    }
        //}


        //[Authorize]
        //[HttpGet("{id}")]
        //public virtual async Task<IActionResult> Get(Guid id, [FromQuery] UsuarioFilter filters)
        //{
        //    var result = new HttpResult<UsuarioDto>(this._logger, this._service);
        //    try
        //    {
        //        filters.PessoaId = id;
        //        var returnModel = await this._service.GetById(filters);
        //        return result.ReturnCustomResponse(returnModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        return result.ReturnCustomException(ex, "Pessoa", id);
        //    }
        //}

        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] UsuarioDtoSave dto)
        {
            var result = new HttpResult<UsuarioDto>(this._logger);
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

        //[Authorize]
        //[HttpPut]
        //public virtual async Task<IActionResult> Put([FromBody] UsuarioDtoSave dto)
        //{
        //    var result = new HttpResult<UsuarioDto>(this._logger, this._service);
        //    try
        //    {
        //        var returnModel = await this._service.SavePartial(dto);
        //        return result.ReturnCustomResponse(returnModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        return result.ReturnCustomException(ex, "Pessoa", dto);
        //    }
        //}

        //[Authorize]
        //[HttpDelete]
        //public virtual async Task<IActionResult> Delete(UsuarioDto dto)
        //{
        //    var result = new HttpResult<UsuarioDto>(this._logger, this._service);
        //    try
        //    {
        //        await this._service.Remove(dto);
        //        return result.ReturnCustomResponse(dto);
        //    }
        //    catch (Exception ex)
        //    {
        //        return result.ReturnCustomException(ex, "Pessoa", dto);
        //    }
        //}

    }
}
