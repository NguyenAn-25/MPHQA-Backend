using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using FluentValidation;

namespace DotNetSkeleton.Modules.JobModule.Validators;

public sealed class CreatePerConnectionJobRequestDtoValidator
    : AbstractValidator<CreatePerConnectionJobRequestDto>
{
    public CreatePerConnectionJobRequestDtoValidator()
    {
        RuleFor(request => request.Title)
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Tiêu đề công việc không được để trống.");

        RuleFor(request => request.Description)
            .Must(description => !string.IsNullOrWhiteSpace(description))
            .WithMessage("Mô tả công việc không được để trống.");

        RuleFor(request => request.Address)
            .Must(address => !string.IsNullOrWhiteSpace(address))
            .WithMessage("Địa chỉ công việc không được để trống.");

        RuleFor(request => request.Images)
            .Must(images => images is { Count: >= 1 and <= 4 })
            .WithMessage("Công việc phải có từ 1 đến 4 hình ảnh.");

        RuleForEach(request => request.Images)
            .Must(image => !string.IsNullOrWhiteSpace(image))
            .WithMessage("Đường dẫn hình ảnh không được để trống.");
    }
}
