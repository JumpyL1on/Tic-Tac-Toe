using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public ExceptionFilter(ProblemDetailsFactory problemDetailsFactory)
        {
            _problemDetailsFactory = problemDetailsFactory;
        }

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case BusinessException:
                case SecurityTokenException:
                    HandleException(context, StatusCodes.Status400BadRequest);

                    break;
                case ForbiddenException:
                    HandleException(context, StatusCodes.Status403Forbidden);

                    break;
                default:
                    HandleException(context, StatusCodes.Status500InternalServerError);

                    break;
            }
        }

        private void HandleException(ExceptionContext context, int status)
        {
            var problemDetails = _problemDetailsFactory
                .CreateProblemDetails(context.HttpContext, status, detail: context.Exception.Message);

            context.Result = new ObjectResult(problemDetails);
        }
    }
}