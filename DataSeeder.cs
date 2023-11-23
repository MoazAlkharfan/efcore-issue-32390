// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using ConsoleApp1;

class DataSeeder(IDbContextFactory<Db> dbFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        await db.Database.EnsureCreatedAsync(ct);

        if (await db.Tags.AnyAsync(ct))
        {
            return;
        }

        db.Tags.AddRange(Enumerable.Range(0, 1000).Select(x => new Tag
        {
            Label = x.ToString(),
            Settings = new List<TagSetting>
            {
                new TagSetting
                {
                    Name = "Test",
                    Value = "Setting found"
                },
                new TagSetting
                {
                    Name = "Test2",
                    Value = "other setting"
                },
            }
        }));

        await db.SaveChangesAsync(ct);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
