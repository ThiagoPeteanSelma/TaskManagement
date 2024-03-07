using FluentValidation;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Validator
{
    public class FilterUserValidator : AbstractValidator<FilterUser>
    {
        public FilterUserValidator() 
        { 
            RuleFor(x=> x.Email).EmailAddress();        
        }
    }
}
