using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Deck2MTGA.Web
{
    public class ApiActionModelConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            action.ApiExplorer.IsVisible = action.Controller.Attributes.OfType<RouteAttribute>().Any(a => a.Template.StartsWith("api/"));
        }
    }
}