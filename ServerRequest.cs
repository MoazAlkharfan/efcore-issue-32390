// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;

/// <summary>
/// Does http requests to the mapped route after 10 seconds of application start "/"
/// </summary>
class ServerRequest(IServer server, IHttpClientFactory clientFactory) : IHostedService
{
    private readonly HttpClient client = clientFactory.CreateClient(nameof(ServerRequest));

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = initTimer();

        return Task.CompletedTask;
    }

    private async Task initTimer()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));

        // Get the address of the https ip
        var address = server.Features.GetRequiredFeature<IServerAddressesFeature>().Addresses.First(x => x.StartsWith("https"));

        this.client.BaseAddress = new Uri(address);

        while (true)
        {
            // Simulate pressure on server
            await Task.WhenAll(
                Enumerable.Range(0, 500)
                .Select(x => client.GetAsync("/")));
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}