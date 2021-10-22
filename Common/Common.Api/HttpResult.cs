using Common.Domain.CustomExceptions;
using Common.Domain.Interfaces;
using Common.Domain.Model;
using Common.Domain.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Common.API
{
    public abstract class HttpResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public Summary Summary { get; set; }
        public IList<string> Errors { get; set; }
    }

    public class HttpResult<T> : HttpResult
    {
        protected ILogger _logger;
        protected IService _service;

        public HttpResult(ILogger logger)
        {
            base.Summary = new Summary();
            this._logger = logger;
        }

        public HttpResult(ILogger logger, IService service)
            : this(logger)
        {
            this._service = service;
        }

        public dynamic Data { get; set; }

        #region Success

        public HttpResult<T> Success(T data)
        {
            this.StatusCode = HttpStatusCode.OK;
            this.Data = data;
            return this;
        }

        public HttpResult<T> Success(IEnumerable<T> dataList)
        {
            this.StatusCode = HttpStatusCode.OK;
            this.Data = dataList;
            return this;
        }

        public HttpResult<T> Success()
        {
            this.StatusCode = HttpStatusCode.OK;
            return this;
        }

        #endregion

        #region Errors

        public HttpResult<T> Error(IList<string> erros)
        {
            this.StatusCode = HttpStatusCode.InternalServerError;
            this.Errors = FormatErrorMessage(erros);
            return this;
        }

        public HttpResult<T> Error(string erro)
        {
            this.StatusCode = HttpStatusCode.InternalServerError;
            this.Errors = FormatErrorMessage(new List<string> { erro });

            return this;
        }

        private HttpResult<T> BadRequest(string erro)
        {
            this.StatusCode = HttpStatusCode.BadRequest;
            this.Errors = new List<string> { erro };
            return this;
        }

        private HttpResult<T> NotFound(string erro)
        {
            this.StatusCode = HttpStatusCode.NotFound;
            this.Errors = new List<string> { erro };
            return this;
        }

        private HttpResult<T> AlreadyExists(string erro)
        {
            this.StatusCode = HttpStatusCode.Conflict;
            this.Errors = new List<string> { erro };
            return this;
        }

        private HttpResult<T> NotAuthorized(string erro)
        {
            this.StatusCode = HttpStatusCode.Unauthorized;
            this.Errors = new List<string> { erro };
            return this;
        }

        #endregion

        #region ReturnResponse

        public ObjectResult ReturnCustomResponse()
        {
            this.Success();
            return new ObjectResult(this)
            {
                StatusCode = (int)this.StatusCode
            };
        }

        public ObjectResult ReturnCustomResponse(SearchResult<T> searchResult)
        {
            if (this._service.IsNotNull() && this._service.IsInvalid())
            {
                this.Error(this._service.GetValidationErrors());
                return new ObjectResult(this)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            this.Summary = searchResult.Summary;
            this.Success(searchResult.DataList);

            return new ObjectResult(this)
            {
                StatusCode = (int)this.StatusCode
            };
        }

        public ObjectResult ReturnCustomResponse(T returnModel)
        {
            if (this._service.IsNotNull() && this._service.IsInvalid())
            {
                this.Error(this._service.GetValidationErrors());
                return new ObjectResult(this)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            this.Success(returnModel);

            return new ObjectResult(this)
            {
                StatusCode = (int)this.StatusCode
            };

        }

        public ObjectResult ReturnMethodNotAllowed()
        {
            return new ObjectResult(new
            {
                StatusCode = HttpStatusCode.MethodNotAllowed,
                Errors = new string[] { "Este metódo não está liberado para realizar requisições" }
            })
            {
                StatusCode = (int)HttpStatusCode.MethodNotAllowed
            };
        }

        public ObjectResult ReturnCustomException(Exception ex, string appName, object model = null)
        {
            var result = default(HttpResult<T>);

            if ((ex as CustomNotFoundException).IsNotNull())
            {
                result = this.NotFound(ex.Message);
            }

            if ((ex as CustomBadRequestException).IsNotNull())
            {
                result = this.BadRequest(ex.Message);
            }

            if ((ex as CustomNotAutorizedException).IsNotNull())
            {
                result = this.NotAuthorized(ex.Message);
            }

            if ((ex as CustomAlreadyExistsException).IsNotNull())
            {
                result = this.AlreadyExists(ex.Message);
            }

            if ((ex as CustomValidationException).IsNotNull())
            {
                var customEx = ex as CustomValidationException;
                result = this.Error(customEx.Errors);
            }

            var erroMessage = ex.Message;
            if (model.IsNotNull())
            {
                var modelSerialization = JsonConvert.SerializeObject(model);
                erroMessage = string.Format("[{0}] - {1} - [{2}]", appName, ex.Message, modelSerialization);
            }

            result = ExceptionWithInner(ex);

            this._logger.LogCritical("{0} - [1]", erroMessage, ex);

            return new ObjectResult(result) { StatusCode = (int)result.StatusCode };

        }

        private HttpResult<T> ExceptionWithInner(Exception ex)
        {
            if (ex.InnerException.IsNotNull())
            {
                if (ex.InnerException.InnerException.IsNotNull())
                    return this.Error(string.Format("{0}", ex.InnerException.InnerException.Message));
                else
                    return this.Error(string.Format("{0}", ex.InnerException.Message));
            }
            else
            {
                return this.Error(string.Format("{0}", ex.Message));
            }
        }

        #endregion

        public IList<string> FormatErrorMessage(IList<string> erros)
        {
            var _erros = new List<string>();

            foreach (var mensagem in erros)
            {
                var novamensagem = mensagem;
                if (novamensagem.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                    novamensagem = "Não é possível excluir este registro, pois existem outros registros relacionados a ele.";

                _erros.Add(novamensagem);
            }

            return _erros;
        }


    }

}
