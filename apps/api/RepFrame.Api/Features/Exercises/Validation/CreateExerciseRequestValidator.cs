using FluentValidation;

namespace RepFrame.Api.Features.Exercises.Validation;

public class CreateExerciseRequestValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Exercise name is required")
            .MaximumLength(200)
            .WithMessage("Exercise name must not exceed 200 characters");

        RuleFor(x => x.MuscleGroup)
            .MaximumLength(100)
            .WithMessage("Muscle group must not exceed 100 characters");
    }
}
