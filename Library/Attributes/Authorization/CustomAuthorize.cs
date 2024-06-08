using Microsoft.AspNetCore.Mvc;

namespace Library.Attributes.Authorization;

// custom attribute to move users to pages when they have no required role

public class CustomAuthorize : TypeFilterAttribute
{
    public CustomAuthorize(string roles) : base(typeof(CustomAuthorizeFilter))
    {
        Arguments = new object[] { roles };
    }

}
