using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace ZConnector.GlobalHanlders
{
    public class GlobalModelStateValidatorAttribute : ActionFilterAttribute
    {
        private string GetFirstModelErrorMessage(ModelStateDictionary ModelState)
        {
            return string.Join("\n", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList()
            );
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                /*Controller? controller = context.Controller as Controller;
                object? model = context.ActionArguments.Any()
                   ? context.ActionArguments.First().Value
                   : null;

                context.Result = controller?.View(model) as IActionResult ?? new BadRequestResult();*/

                context.Result = new ContentResult
                {
                    Content = GetFirstModelErrorMessage(context.ModelState),
                    ContentType = "text/plain",
                    StatusCode = 400
                };
            }

            base.OnActionExecuting(context);
        }
    }
}
