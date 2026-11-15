using DotNetSkeleton.Modules.JobModule.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

public sealed class SavedJobSeeder(
    IConfiguration configuration,
    IWebHostEnvironment environment) : ISeeder
{
    private const string UserPhone = "+12025550100";

    public int Order => 7;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        if (!environment.ShouldSeedDevelopmentData(configuration))
            return 0;

        var user = await context.Users
            .SingleOrDefaultAsync(candidate => candidate.Phone == UserPhone);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"SavedJobSeeder requires the user account '{UserPhone}'.");
        }

        var job = await context.Jobs
            .SingleOrDefaultAsync(candidate => candidate.Title == JobSeeder.FirstJobTitle);
        if (job is null)
        {
            throw new InvalidOperationException(
                $"SavedJobSeeder requires the first job '{JobSeeder.FirstJobTitle}'.");
        }

        var alreadySaved = await context.SavedJobs
            .AnyAsync(savedJob => savedJob.UserId == user.Id && savedJob.JobId == job.Id);
        if (alreadySaved)
            return 0;

        context.SavedJobs.Add(new SavedJobEntity
        {
            UserId = user.Id,
            JobId = job.Id
        });
        await context.SaveChangesAsync();

        return 1;
    }
}
