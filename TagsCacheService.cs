// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using ConsoleApp1;

interface ITagsService
{
    IReadOnlyList<Tag> Tags { get; }
}

class TagsCacheService(IDbContextFactory<Db> dbFactory) : IHostedService, ITagsService
{
    public IReadOnlyList<Tag> Tags { get; private set; } = new List<Tag>();

    public async Task InitAsync(CancellationToken ct)
    {
        await using Db db = await dbFactory.CreateDbContextAsync(ct);

        Tags = await db.Tags
            .Include(x => x.Settings)
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync(ct);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await InitAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
