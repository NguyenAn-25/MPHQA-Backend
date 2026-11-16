using DotNetSkeleton.Modules.ExampleModule.DTOs.Requests;
using DotNetSkeleton.Shared.Validation;
using FluentValidation;

namespace DotNetSkeleton.Modules.ExampleModule.Validators;

public class CreateExampleDtoValidator : AbstractValidator<CreateExampleDto>
{
    public CreateExampleDtoValidator()
    {
        RuleFor(x => x.Name)
            .MustBeRequiredText().WithMessage("Tên không được để trống.")
            .MustHaveLengthBetween(3, 20).WithMessage("Tên phải có từ 3 đến 20 ký tự.");
    }
}
