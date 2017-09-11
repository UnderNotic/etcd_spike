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
        Console.WriteLine($"Received request {this.Request.Url}");
        return HttpStatusCode.OK;
    }
}
