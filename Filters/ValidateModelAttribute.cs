using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetDapper.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="context">The context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }

            base.OnActionExecuting(context);
        }
    }
}
