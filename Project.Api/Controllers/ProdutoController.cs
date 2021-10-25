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
    public class ProdutoController : Controller
    {
        protected readonly ProdutoService _service;
        protected readonly ProdutoRepository _rep;
        protected readonly ILogger _logger;
        protected readonly EnviromentInfo _env;

        public ProdutoController(ProdutoService service, ProdutoRepository rep, ILoggerFactory logger, EnviromentInfo env)
        {
            this._service = service;
            this._rep = rep;
            this._logger = logger.CreateLogger<UsuarioController>();
            this._env = env;
        }


        [Authorize]
        [HttpGet("GetData")]
        public virtual async Task<IActionResult> GetData([FromQuery] ProdutoFilter filters)
        {
            var result = new HttpResult<dynamic>(this._logger);
            try
            {
                var searchResult = await this._rep.GetGrid(filters);
                return result.ReturnCustomResponse(searchResult);
            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Pessoa", filters);
            }
        }
        
        [Authorize]
        [HttpGet("GetDetails")]
        public virtual async Task<IActionResult> GetDetails([FromQuery] ProdutoFilter filters)
        {
            var result = new HttpResult<dynamic>(this._logger);
            try
            {
                var searchResult = await this._rep.GetGrid(filters);
                return result.ReturnCustomResponse(searchResult);
            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Pessoa", filters);
            }
        }

        [Authorize]
        [HttpGet("GetById")]
        public virtual async Task<IActionResult> GetById([FromQuery] ProdutoFilter filters)
        {
            var result = new HttpResult<dynamic>(this._logger);
            try
            {
                var searchResult = await this._rep.GetById(filters);
                return result.ReturnCustomResponse(searchResult);
            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Pessoa", filters);
            }
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] ProdutoDtoSave dto)
        {
            var result = new HttpResult<ProdutoDtoSave>(this._logger);
            try
            {
                var returnModel = await this._service.Save(dto);
                return result.ReturnCustomResponse(returnModel);

            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Pessoa", dto);
            }
        }

        [Authorize]
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] ProdutoDtoSave dto)
        {
            var result = new HttpResult<dynamic>(this._logger);
            try
            {
                var returnModel = await this._service.SavePartial(dto);
                return result.ReturnCustomResponse(returnModel);

            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Produtos", dto);
            }
        }

        [Authorize]
        [HttpDelete]
        public virtual async Task<IActionResult> Delete(ProdutoDto dto)
        {
            var result = new HttpResult<ProdutoDto>(this._logger);
            try
            {
                await this._service.Remove(dto);
                return result.ReturnCustomResponse(dto);
            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Produtos", dto);
            }
        }



    }
}
