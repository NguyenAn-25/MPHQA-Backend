using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using FluentValidation;

namespace DotNetSkeleton.Modules.JobModule.Validators;

public sealed class CreateConnectionFeeSettingRequestDtoValidator
    : AbstractValidator<CreateConnectionFeeSettingRequestDto>
{
    public CreateConnectionFeeSettingRequestDtoValidator()
    {
        RuleFor(request => request.FeeCoin)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Mức phí kết nối phải lớn hơn hoặc bằng 0.");
    }
}
