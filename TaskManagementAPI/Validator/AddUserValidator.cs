using FluentValidation;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Validator
{
    public class AddUserValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
