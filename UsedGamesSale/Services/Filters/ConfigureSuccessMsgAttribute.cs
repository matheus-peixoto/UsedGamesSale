using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedGamesSale.Services.Filters
{
    public class ConfigureSuccessMsgAttribute : Attribute, IActionFilter
    {
        private string _msg;

        public ConfigureSuccessMsgAttribute(string msg)
        {
            _msg = msg;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Controller controller = (Controller)context.Controller;
            controller.TempData.Remove("MSG_S");
            controller.TempData["MSG_S"] = _msg;
        }
    }
}
