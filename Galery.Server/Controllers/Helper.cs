using Galery.Server.Service.Infrostructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Galery.Server.Controllers
{
    static class Helper
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.FindFirst(c => c.Type == "Id").Value);
        }

        public static IActionResult GetResult<T>(this Controller controller, OperationResult<T> operRes, bool isSingle)
        {
            if (operRes.Succeeded)
            {
                if (!isSingle)
                    return controller.Ok(operRes.Results);
                else
                    return controller.Ok(operRes.Results.First());
            }
            {
                foreach (var e in operRes.ErrorMessages)
                {
                    controller.ModelState.AddModelError(e.Property, e.Message);
                }
                return controller.BadRequest(controller.ModelState);
            }
        }
    }
}
