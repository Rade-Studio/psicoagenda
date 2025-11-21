using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PsicoAgenda.Api.Filters;

public class FluentValidationAsyncActionFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _provider;

    public FluentValidationAsyncActionFilter(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var arg in context.ActionArguments)
        {
            if (arg.Value is null) continue;

            var argType = arg.Value.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argType);
            var validator = _provider.GetService(validatorType) as IValidator;
            if (validator == null) continue;

            var validationContext = new ValidationContext<object>(arg.Value);
            var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                context.Result = new BadRequestObjectResult(new { Errors = errors });
                return;
            }
        }

        await next();
    }
}
