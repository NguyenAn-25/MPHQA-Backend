using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using FluentValidation;

namespace DotNetSkeleton.Modules.JobModule.Validators;

public sealed class UpdateJobRequestDtoValidator : AbstractValidator<UpdateJobRequestDto>
{
    public UpdateJobRequestDtoValidator()
    {
        RuleFor(request => request.Title)
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .When(request => request.Title is not null)
            .WithMessage("Tiêu đề công việc không được để trống.");

        RuleFor(request => request.Description)
            .Must(description => !string.IsNullOrWhiteSpace(description))
            .When(request => request.Description is not null)
            .WithMessage("Mô tả công việc không được để trống.");

        RuleFor(request => request.Address)
            .Must(address => !string.IsNullOrWhiteSpace(address))
            .When(request => request.Address is not null)
            .WithMessage("Địa chỉ công việc không được để trống.");

        RuleFor(request => request.Images)
            .Must(images => images is { Count: >= 1 and <= 4 })
            .When(request => request.Images is not null)
            .WithMessage("Công việc phải có từ 1 đến 4 hình ảnh.");

        RuleForEach(request => request.Images)
            .Must(image => !string.IsNullOrWhiteSpace(image))
            .When(request => request.Images is not null)
            .WithMessage("Đường dẫn hình ảnh không được để trống.");
    }
}
