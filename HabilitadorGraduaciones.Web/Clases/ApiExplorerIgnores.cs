using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace HabilitadorGraduaciones.Web.Interfaces
{
    public class ApiExplorerIgnores : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (action.Controller.ControllerName.Equals("Auth"))
                action.ApiExplorer.IsVisible = false;
            if (action.Controller.ControllerName.Equals("Home"))
                action.ApiExplorer.IsVisible = false;
        }
    }
}

