using FluentValidation;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Validator
{
    public class AddProjectTaskValidator : AbstractValidator<AddProjectTaskRequest>
    {
        public AddProjectTaskValidator() 
        { 
            RuleFor(x=> x.Name).NotEmpty();
            RuleFor(x=> x.Description).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
