namespace KittyClaws.Api.Validation;

using FluentValidation;
using KittyClaws.Api.Requests;

public class CreateKittyClawsRequestValidator : AbstractValidator<CreateKittyClawsRequest>
{
    public CreateKittyClawsRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}

public class UpdateKittyClawsRequestValidator : AbstractValidator<UpdateKittyClawsRequest>
{
    public UpdateKittyClawsRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}
