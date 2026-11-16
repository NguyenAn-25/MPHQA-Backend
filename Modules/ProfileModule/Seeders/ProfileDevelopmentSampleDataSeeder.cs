using System.Security.Cryptography;
using System.Text;
using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Enums;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.ProfileModule.Entities;
using DotNetSkeleton.Modules.ProfileModule.Enums;
using DotNetSkeleton.Modules.SkillModule.Entities;
using DotNetSkeleton.Shared.Persistence;
using DotNetSkeleton.Shared.Persistence.Seeders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.ProfileModule.Seeders;

public sealed class ProfileDevelopmentSampleDataSeeder(IWebHostEnvironment environment) : ISeeder
{
    private const int SampleUserCount = 60;

    private static readonly string[] FamilyNames =
    [
        "Nguyễn", "Trần", "Lê", "Phạm", "Hoàng", "Phan", "Vũ", "Đặng", "Bùi", "Đỗ", "Ngô", "Dương"
    ];

    private static readonly string[] MiddleNames = ["Văn", "Thị", "Minh", "Gia", "Quốc"];

    private static readonly string[] GivenNames =
    [
        "An", "Bình", "Chi", "Dũng", "Hà", "Huy", "Lan", "Linh", "Minh", "Nam", "Ngọc", "Phúc"
    ];

    private static readonly string[] Addresses =
    [
        "Quận 1, Thành phố Hồ Chí Minh",
        "Quận Bình Thạnh, Thành phố Hồ Chí Minh",
        "Quận Gò Vấp, Thành phố Hồ Chí Minh",
        "Quận Hải Châu, Thành phố Đà Nẵng",
        "Quận Thanh Khê, Thành phố Đà Nẵng",
        "Quận Hoàn Kiếm, Thành phố Hà Nội",
        "Quận Cầu Giấy, Thành phố Hà Nội",
        "Thành phố Biên Hòa, Đồng Nai"
    ];

    private static readonly string[] Companies =
    [
        "Nhà hàng Bếp Việt",
        "Cửa hàng Tiện Lợi 24h",
        "Sự kiện Ánh Dương",
        "Kho vận Miền Nam",
        "Cà phê Góc Phố",
        "Trung tâm Hội nghị Sông Hồng",
        "Studio Mộc",
        "Siêu thị An Tâm"
    ];

    public int Order => 7;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        if (!environment.IsDevelopment())
            return 0;

        var samplePhones = Enumerable.Range(0, SampleUserCount)
            .Select(index => $"+1202555{100 + index:0000}")
            .ToArray();
        var users = await context.Users
            .Where(user => samplePhones.Contains(user.Phone) && user.Role == UserRole.User)
            .OrderBy(user => user.Id)
            .ToListAsync();
        if (users.Count == 0)
            return 0;

        var userIds = users.Select(user => user.Id).ToArray();
        var profilesByUserId = await context.Profiles
            .Where(profile => userIds.Contains(profile.UserId))
            .ToDictionaryAsync(profile => profile.UserId);
        var added = 0;

        foreach (var user in users)
        {
            var sampleIndex = GetSampleIndex(user.Phone);
            if (profilesByUserId.ContainsKey(user.Id))
                continue;

            var profile = CreateProfile(user, sampleIndex);
            context.Profiles.Add(profile);
            profilesByUserId.Add(user.Id, profile);
            added++;
        }

        if (added > 0)
            await context.SaveChangesAsync();

        var profileIds = profilesByUserId.Values.Select(profile => profile.Id).ToArray();
        var existingExperienceProfileIds = await context.ProfileExperiences
            .Where(experience => profileIds.Contains(experience.ProfileId))
            .Select(experience => experience.ProfileId)
            .Distinct()
            .ToHashSetAsync();
        var categories = await context.JobCategories
            .AsNoTracking()
            .Where(category => category.ParentId != null)
            .OrderBy(category => category.Id)
            .Select(category => new SampleCategory(category.Id, category.Name))
            .ToListAsync();
        if (categories.Count > 0)
        {
            var experiences = new List<ProfileExperienceEntity>();
            foreach (var user in users)
            {
                var profile = profilesByUserId[user.Id];
                if (existingExperienceProfileIds.Contains(profile.Id))
                    continue;

                var index = GetSampleIndex(user.Phone);
                var experienceCount = 1 + index % 3;
                for (var experienceIndex = 0; experienceIndex < experienceCount; experienceIndex++)
                {
                    var category = categories[(index * 3 + experienceIndex) % categories.Count];
                    experiences.Add(new ProfileExperienceEntity
                    {
                        ProfileId = profile.Id,
                        JobTitle = category.Name,
                        CompanyName = Companies[(index + experienceIndex) % Companies.Length],
                        Title = "Công việc ngắn hạn",
                        Description = "Làm theo ca hoặc theo lịch cuối tuần, phối hợp với nhóm và hoàn thành nhiệm vụ đúng thời hạn.",
                        JobCategoryId = category.Id,
                        CreatedAt = user.CreatedAt.AddDays(2 + experienceIndex)
                    });
                }
            }

            if (experiences.Count > 0)
            {
                await context.ProfileExperiences.AddRangeAsync(experiences);
                added += experiences.Count;
            }
        }

        var existingSkills = await context.ProfileSkills
            .Where(profileSkill => profileIds.Contains(profileSkill.ProfileId))
            .ToListAsync();
        var skills = await context.Skills
            .AsNoTracking()
            .OrderBy(skill => skill.Id)
            .Select(skill => new SampleSkill(skill.Id))
            .ToListAsync();
        if (skills.Count > 0)
        {
            var skillIdsByProfileId = existingSkills
                .GroupBy(profileSkill => profileSkill.ProfileId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(profileSkill => profileSkill.SkillId).ToHashSet());
            var profileSkills = new List<ProfileSkillEntity>();
            foreach (var user in users)
            {
                var profileId = profilesByUserId[user.Id].Id;
                if (!skillIdsByProfileId.TryGetValue(profileId, out var knownSkillIds))
                {
                    knownSkillIds = [];
                    skillIdsByProfileId.Add(profileId, knownSkillIds);
                }

                var index = GetSampleIndex(user.Phone);
                var targetSkillCount = 3 + index % 3;
                for (var offset = 0; knownSkillIds.Count < targetSkillCount && offset < skills.Count; offset++)
                {
                    var skill = skills[(index * 7 + offset * 11) % skills.Count];
                    if (!knownSkillIds.Add(skill.Id))
                        continue;

                    profileSkills.Add(new ProfileSkillEntity
                    {
                        ProfileId = profileId,
                        SkillId = skill.Id
                    });
                }
            }

            if (profileSkills.Count > 0)
            {
                await context.ProfileSkills.AddRangeAsync(profileSkills);
                added += profileSkills.Count;
            }
        }

        var existingRequests = await context.VerificationRequests
            .Where(request => userIds.Contains(request.SenderId))
            .Select(request => request.SenderId)
            .ToHashSetAsync();
        var identitiesByProfileId = await context.ProfileIdentities
            .Where(identity => profileIds.Contains(identity.ProfileId))
            .ToDictionaryAsync(identity => identity.ProfileId);
        var verificationRequests = new List<VerificationRequestEntity>();
        var identities = new List<ProfileIdentityEntity>();

        foreach (var user in users)
        {
            var index = GetSampleIndex(user.Phone);
            var status = GetSampleVerificationStatus(index);
            if (status is null || existingRequests.Contains(user.Id))
                continue;

            var profile = profilesByUserId[user.Id];
            identitiesByProfileId.TryGetValue(profile.Id, out var identity);
            var idNumber = identity?.CardId ?? $"DEV-{100000 + index:000000}";
            var fullNameOnCard = identity?.FullNameOnCard ?? profile.Fullname;
            var cardFrontImage = identity?.CardFrontImage ?? SampleImage(index, "front");
            var cardBackImage = identity?.CardBackImage ?? SampleImage(index, "back");
            var submittedAt = user.CreatedAt.AddDays(1 + index % 3);
            var reviewedAt = submittedAt.AddHours(6);
            var isAccepted = status == VerificationRequestStatus.Accepted;
            var isRejected = status == VerificationRequestStatus.Rejected;

            verificationRequests.Add(new VerificationRequestEntity
            {
                FullNameOnCard = fullNameOnCard,
                IdNumber = idNumber,
                CardFrontImage = cardFrontImage,
                CardBackImage = cardBackImage,
                PortraitImage = SampleImage(index, "portrait"),
                Status = status.Value,
                RejectionReason = isRejected ? "Ảnh giấy tờ chưa đủ rõ để đối chiếu." : null,
                CreatedAt = submittedAt,
                VerifiedAt = isAccepted ? reviewedAt : null,
                RejectedAt = isRejected ? reviewedAt : null,
                SenderId = user.Id
            });

            if (isAccepted)
            {
                if (identity is null)
                {
                    identities.Add(new ProfileIdentityEntity
                    {
                        ProfileId = profile.Id,
                        CardId = idNumber,
                        FullNameOnCard = fullNameOnCard,
                        CardFrontImage = cardFrontImage,
                        CardBackImage = cardBackImage
                    });
                }

                user.IdentityVerifiedAt ??= reviewedAt;
                user.IdentityRejectedAt = null;
            }
            else if (isRejected)
            {
                user.IdentityVerifiedAt = null;
                user.IdentityRejectedAt = reviewedAt;
            }
        }

        if (verificationRequests.Count > 0)
        {
            await context.VerificationRequests.AddRangeAsync(verificationRequests);
            added += verificationRequests.Count;
        }

        if (identities.Count > 0)
        {
            await context.ProfileIdentities.AddRangeAsync(identities);
            added += identities.Count;
        }

        if (added > 0)
            await context.SaveChangesAsync();

        return added;
    }

    private static ProfileEntity CreateProfile(UserEntity user, int index)
    {
        var group = index / FamilyNames.Length;
        var fullname = string.Join(
            ' ',
            FamilyNames[index % FamilyNames.Length],
            MiddleNames[group % MiddleNames.Length],
            GivenNames[(index * 5 + group) % GivenNames.Length]);

        return new ProfileEntity
        {
            UserId = user.Id,
            Fullname = fullname,
            Note = "Tìm công việc ngắn hạn, thời vụ hoặc theo ca phù hợp với lịch cá nhân.",
            Major = new[] { "Sinh viên", "Dịch vụ", "Kinh doanh", "Truyền thông", "Công nghệ thông tin" }[index % 5],
            Gender = index % 2 == 0 ? "Nam" : "Nữ",
            Birthdate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18 - index % 15).AddDays(index * 17)),
            Address = Addresses[index % Addresses.Length],
            Longitude = 106.6m + index % 30 * 0.001m,
            Latitude = 10.7m + index % 30 * 0.001m,
            Avatar = SampleImage(index, "avatar")
        };
    }

    private static VerificationRequestStatus? GetSampleVerificationStatus(int index) => index switch
    {
        >= 0 and < 12 => VerificationRequestStatus.Accepted,
        >= 12 and < 20 => VerificationRequestStatus.Pending,
        >= 20 and < 28 => VerificationRequestStatus.Rejected,
        >= 28 and < 32 => VerificationRequestStatus.Cancelled,
        _ => null
    };

    private static int GetSampleIndex(string phone) =>
        int.Parse(phone[^4..]) - 100;

    private static string SampleImage(int index, string kind) =>
        $"https://example.test/development/users/{index + 1:000}/{kind}.jpg";

    private sealed record SampleCategory(int Id, string Name);
    private sealed record SampleSkill(int Id);
}
