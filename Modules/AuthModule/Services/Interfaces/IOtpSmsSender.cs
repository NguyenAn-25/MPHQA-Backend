namespace DotNetSkeleton.Modules.AuthModule.Services.Interfaces;

/// <summary>
/// OTP delivery boundary. The current implementation logs codes and does not send SMS.
/// </summary>
public interface IOtpSmsSender
{
    Task SendAsync(string phone, string code, CancellationToken cancellationToken);
}
