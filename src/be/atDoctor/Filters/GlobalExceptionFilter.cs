using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Health.Configuration;

namespace atDoctor.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = false;
            Logger.Log(context);
        }

        public void Dispose()
        {

        }
    }
}
