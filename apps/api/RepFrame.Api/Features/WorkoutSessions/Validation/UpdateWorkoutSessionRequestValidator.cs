using FluentValidation;

namespace RepFrame.Api.Features.WorkoutSessions.Validation;

public class UpdateWorkoutSessionRequestValidator : AbstractValidator<UpdateWorkoutSessionRequest>
{
    public UpdateWorkoutSessionRequestValidator()
    {
        RuleFor(x => x.Note)
            .MaximumLength(500)
            .WithMessage("Note must not exceed 500 characters");
    }
}
