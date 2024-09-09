namespace Library.ActionFilters;

public class LocalizationRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.ContainsKey("culture"))
            return false;

        var culture = values["culture"].ToString();
        return !string.IsNullOrEmpty(culture) && culture.Length == 2;
    }
}
