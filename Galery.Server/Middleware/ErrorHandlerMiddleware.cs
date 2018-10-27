using Galery.Server.Service;
using Galery.Server.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Galery.Server.Service.Infrostructure;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;

namespace Galery.Server.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        ///
        /// </summary>
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        ///
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            ErrorMessage mes = new ErrorMessage();
            String reasonPhrase = String.Empty;
            if (exception is NotFoundException nfe)
            {
                mes.Message = nfe.Message;
                reasonPhrase = "NotFoundException";
                code = HttpStatusCode.Conflict;
            }
            else if (exception is DatabaseException dbe)
            {
                mes.Message = dbe.Message;
                reasonPhrase = "DataBaseException";
                code = HttpStatusCode.InternalServerError;
            }
            else
            {
                mes.Message = exception.Message;
                reasonPhrase = "UnexpectedException";
                code = HttpStatusCode.InternalServerError;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            var res = JsonConvert.SerializeObject(mes);
            context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = reasonPhrase;
            return context.Response.WriteAsync(res);
        }
    }
}
