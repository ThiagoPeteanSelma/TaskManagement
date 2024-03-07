using FluentValidation;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Validator
{
    public class FilterProjectValidator : AbstractValidator<FilterProject>
    {
        public FilterProjectValidator() 
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
