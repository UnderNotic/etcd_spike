using Nancy;
using System;

public class ApiModule : NancyModule
{
    public ApiModule()
    {
        Get["/"] = _ =>
        {
            return HandleRequest();
        };

        Get["/{path*}"] = _ =>
        {
            return HandleRequest();
        };
    }

    private HttpStatusCode HandleRequest()
    {
        Console.WriteLine($"Request to {this.Request.Url} - {this.Request.UserHostAddress}");
        return HttpStatusCode.OK;
    }
}
