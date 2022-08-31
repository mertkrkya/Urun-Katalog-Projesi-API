using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Entities;

namespace UrunKatalogProjesi.API.Filters
{
    public class ValidateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(r => r.Errors).Select(r => r.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(new ResponseEntity(errorMessage: errors.First())); //Burası düzeltilebilir.
            }
        }
    }
}
