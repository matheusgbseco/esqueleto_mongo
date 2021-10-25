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
    public class CategoriaController : Controller
    {
        protected readonly CategoriaService _service;
        protected readonly CategoriaRepository _rep;
        protected readonly ILogger _logger;
        protected readonly EnviromentInfo _env;

        public CategoriaController(CategoriaService service, CategoriaRepository rep, ILoggerFactory logger, EnviromentInfo env)
        {
            this._service = service;
            this._rep = rep;
            this._logger = logger.CreateLogger<UsuarioController>();
            this._env = env;
        }


        [Authorize]
        [HttpGet("GetData")]
        public virtual async Task<IActionResult> GetData([FromQuery] CategoriaFilter filters)
        {
            var result = new HttpResult<dynamic>(this._logger);
            try
            {
                var searchResult = await this._rep.GetByFilters(filters);
                return result.ReturnCustomResponse(searchResult);
            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Pessoa", filters);
            }
        }

        [Authorize]
        [HttpGet("GetById")]
        public virtual async Task<IActionResult> GetById([FromQuery] CategoriaFilter filters)
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
        public virtual async Task<IActionResult> Post([FromBody] CategoriaDtoSave dto)
        {
            var result = new HttpResult<CategoriaDtoSave>(this._logger);
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
        public virtual async Task<IActionResult> Put([FromBody] CategoriaDtoSave dto)
        {
            var result = new HttpResult<dynamic>(this._logger);
            try
            {
                var returnModel = await this._service.SavePartial(dto);
                return result.ReturnCustomResponse(returnModel);

            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Categorias", dto);
            }
        }

        [Authorize]
        [HttpDelete]
        public virtual async Task<IActionResult> Delete(CategoriaDto dto)
        {
            var result = new HttpResult<CategoriaDto>(this._logger);
            try
            {
                await this._service.Remove(dto);
                return result.ReturnCustomResponse(dto);
            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Categorias", dto);
            }
        }

        [Authorize]
        [HttpGet("DataItems")]
        public virtual async Task<IActionResult> DataItems([FromQuery] CategoriaFilter filters)
        {
            var result = new HttpResult<dynamic>(this._logger);
            try
            {
                var items = await this._rep.GetDataItems(filters);
                return result.ReturnCustomResponse(items);
            }
            catch (Exception ex)
            {
                return result.ReturnCustomException(ex, "Assinatura", filters);
            }
        }



    }
}
