using StockExchange.Utility.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppPofile.Utility.ModelValidateion
{
    public sealed class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var resultContent = new BasicResponse() { HttpCode = "97", HttpMessage = "輸入資料錯誤" };
                context.Result = new BadRequestObjectResult(resultContent);
            }
        }
    }
}
