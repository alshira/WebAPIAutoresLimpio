﻿using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace WebAPIAutores.Filtros
{
    public class FiltroDeExcepcion : ExceptionFilterAttribute
    {
        private readonly ILogger<FiltroDeExcepcion> logger;

        public FiltroDeExcepcion(ILogger<FiltroDeExcepcion> logger)
        {
            this.logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);

            base.OnException(context);
        }
    }
}