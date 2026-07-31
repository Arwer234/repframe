using FluentValidation;

namespace RepFrame.Api.Features.Sets.Validation;

public class CreateSetRequestValidator : AbstractValidator<CreateSetRequest>
{
    public CreateSetRequestValidator()
    {
        RuleFor(x => x.WorkoutSessionId)
            .NotEmpty()
            .WithMessage("WorkoutSessionId is required");

        RuleFor(x => x.ExerciseId)
            .NotEmpty()
            .WithMessage("ExerciseId is required");

        RuleFor(x => x.Number)
            .GreaterThan(0)
            .WithMessage("Set number must be greater than 0");

        RuleFor(x => x.WeightKg)
            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0");

        RuleFor(x => x.Reps)
            .GreaterThan(0)
            .WithMessage("Reps must be greater than 0");

        RuleFor(x => x.Rir)
            .Must(rir => rir == null || (rir >= 0 && rir <= 10))
            .WithMessage("Rir must be between 0 and 10, or null");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Set type is required")
            .Must(t => t is "Working" or "WarmUp" or "TopSet" or "BackOff")
            .WithMessage("Set type must be one of: Working, WarmUp, TopSet, BackOff");
    }
}
