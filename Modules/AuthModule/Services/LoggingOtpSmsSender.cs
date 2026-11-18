using DotNetSkeleton.Modules.AuthModule.Services.Interfaces;

namespace DotNetSkeleton.Modules.AuthModule.Services;

/// <summary>
/// Local/testing OTP transport. It logs codes instead of sending SMS and must not be used for real users.
/// </summary>
public sealed class LoggingOtpSmsSender(ILogger<LoggingOtpSmsSender> logger) : IOtpSmsSender
{
    public Task SendAsync(string phone, string code, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        logger.LogWarning("OTP for {Phone}: {Code}. SMS delivery is disabled.", phone, code);
        return Task.CompletedTask;
    }
}
