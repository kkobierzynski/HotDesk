using FluentValidation;
using HotDesk.Entities;

namespace HotDesk.Models.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        private int[] allowedRoleId = { 1, 2 };
        public CreateUserDtoValidator(HotDeskDbContext dbContext) 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Email).Custom((value, context) =>
            {
                if (dbContext.Users.Any(user => user.Email == value))
                {
                    context.AddFailure("Email", "This email already exists, please select another one");
                }
            });
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x=>x.ConfirmedPassword).NotEmpty();
            RuleFor(x => x.Password).Equal(x => x.ConfirmedPassword);
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.RoleId).Custom((value, context) =>
            {
                if (!allowedRoleId.Contains(value))
                {
                    context.AddFailure("RoleId", $"RoleId can only have {string.Join(",",allowedRoleId)} values");
                }
            });
        }
    }
    
}
