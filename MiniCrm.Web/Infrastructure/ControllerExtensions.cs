using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.Infrastructure
{
    public static class ControllerExtensions
    {
        public const string TemporaryMessageKey = "tempMessage";

        /// <summary>
        /// Store a message to be displayed on the next page the user requests.
        /// For example, a success message following a redirect in the POST-Redirect-GET pattern.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="message"></param>
        public static void SetTemporaryMessage(this Controller controller, string message)
        {
            controller.TempData[TemporaryMessageKey] = message;
        }
    }
}
