using FluentValidation;

namespace HotDesk.Models.Validators
{
    public class AvailableDeskCheckDtoValidator : AbstractValidator<AvailableDeskCheckDto>
    {
        public AvailableDeskCheckDtoValidator() 
        {
            var date = DateTime.Now;
            date = new DateTime(date.Year, date.Month, date.Day);

            RuleFor(x => x.availability).NotEmpty();
            RuleFor(x => x.checkDate).NotEmpty();
            RuleFor(x => x.checkDate).GreaterThanOrEqualTo(date);
        }
    }
}
