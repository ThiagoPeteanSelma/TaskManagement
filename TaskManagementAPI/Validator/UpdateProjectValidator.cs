using FluentValidation;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Validator
{
    public class UpdateProjectValidator : AbstractValidator<UpdateProjectRequest>
    {
        public UpdateProjectValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.TeamReach).GreaterThanOrEqualTo(1);
            RuleFor(x => x.DeadLine).Must(BeAValidDate);
            RuleFor(x => x.Budget).GreaterThanOrEqualTo(1);
        }
        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default);
        }
    }
}
