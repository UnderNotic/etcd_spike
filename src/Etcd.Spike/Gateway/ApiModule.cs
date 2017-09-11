using Gateway;
using Nancy;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class ApiModule : NancyModule
{
    private static readonly HttpClient httpClient = new HttpClient();
    private static readonly ServiceDiscovery serviceDiscovery = new ServiceDiscovery();
    private static readonly Random random = new Random();

    public ApiModule()
    {

        Get["/", runAsync: true] = async (args, ct) =>
        {
            return await HandleRequest();
        };

        Get["/send/{nodeType}", runAsync: true] = async (args, ct) =>
        {
            return await HandleRequest(args.nodeType);
        };
    }

    private async Task<HttpStatusCode> HandleRequest()
    {
        var allHosts = serviceDiscovery.availableNodes.Values.SelectMany(_ => _).Select(s => s.Address).Distinct();
        await Task.WhenAll(allHosts.Select(async address => await httpClient.GetAsync(address)));
        return HttpStatusCode.OK;
    }

    private async Task<HttpStatusCode> HandleRequest(string nodeType)
    {
        var hosts = serviceDiscovery.availableNodes[nodeType].Select(s => s.Address).ToArray();
        var host = hosts[random.Next(0, hosts.Length)];
        await httpClient.GetAsync(host);
        return HttpStatusCode.OK;
    }
}