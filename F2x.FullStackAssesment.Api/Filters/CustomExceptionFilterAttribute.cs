using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using F2xFullStackAssesment.Infraestructure.Exceptions;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace F2xFullStackAssesment.Api.Filters
{
    [ExcludeFromCodeCoverage, AttributeUsage(AttributeTargets.All)]
    public sealed class CustomExceptionFilterAttribute : ExceptionFilterAttribute, IActionFilter
    {

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Metodo intencionalmente vacío por la implementación de la interfaz
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null)
            {
                throw new InvalidProgramException("No existe un contexto en la aplicación");
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            int hashCode;
            var exceptionType = context.Exception;
            string errorMessage;

            switch (exceptionType)
            {
                case ArgumentNullException argumentNullException:
                case ArgumentException argumentException:
                    errorMessage = string.IsNullOrWhiteSpace(context.Exception?.InnerException?.Message) ? context.Exception.Message : context.Exception.InnerException.Message;
                    hashCode = HttpStatusCode.BadRequest.GetHashCode();
                    break;
                case CustomRestClientException customRestClientException:
                    errorMessage = string.Format(CultureInfo.InvariantCulture, "{0}",
                        await GetContentFromCustomRestClientExceptionAsync(customRestClientException.HttpResponseMessage).ConfigureAwait(false));
                    hashCode = HttpStatusCode.InternalServerError.GetHashCode();
                    break;
                case EntityNotFoundException entityNotFoundException:
                    errorMessage = string.Format(CultureInfo.InvariantCulture, "{0}", entityNotFoundException.Message);
                    hashCode = HttpStatusCode.NotFound.GetHashCode();
                    break;
                case InvoiceCollectedConflictException invoiceCollectedConflictException:
                    errorMessage = string.Format(CultureInfo.InvariantCulture, "{0}", invoiceCollectedConflictException.Message);
                    hashCode = HttpStatusCode.Conflict.GetHashCode();
                    break;
                case PhotoFineDocumentConflictException photoFineDocumentConflictException:
                    errorMessage = string.Format(CultureInfo.InvariantCulture, "{0}", photoFineDocumentConflictException.Message);
                    hashCode = HttpStatusCode.Conflict.GetHashCode();
                    break;
                default:
                    errorMessage = string.IsNullOrWhiteSpace(context.Exception?.InnerException?.Message) ? context.Exception.Message : context.Exception.InnerException.Message;
                    hashCode = HttpStatusCode.InternalServerError.GetHashCode();
                    break;
            }
            context.Result = new ContentResult
            {
                Content = errorMessage,
                ContentType = "text/html; charset=utf-8",
                StatusCode = hashCode
            };
        }

        private static async Task<string> GetContentFromCustomRestClientExceptionAsync(HttpResponseMessage httpResponseMessage)
        {
            if (HttpStatusCode.Unauthorized.Equals(httpResponseMessage.StatusCode))
            {
                return $"No tiene permisos para realizar la peticion {httpResponseMessage.RequestMessage.Method} " +
                    $"al servicio {httpResponseMessage.RequestMessage.RequestUri}: {httpResponseMessage.StatusCode.GetHashCode()}";
            }

            return await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
