using Gateway;
using Nancy;
using System;
using System.Linq;
using System.Net.Http;

public class ApiModule : NancyModule
{
    private static readonly HttpClient httpClient = new HttpClient();
    private static readonly ServiceDiscovery serviceDiscovery = new ServiceDiscovery();

    public ApiModule()
    {
        Get["/send"] = _ =>
        {
            return HandleRequest();
        };
    }

    private HttpStatusCode HandleRequest()
    {
        foreach(var kvp in serviceDiscovery.availableNodes)
        {
            Console.WriteLine($"{kvp.Key} - {kvp.Value.Aggregate(string.Empty, (acc, next) => $"{next.Address}, {acc}")}");
        }

        return HttpStatusCode.OK;
    }
}