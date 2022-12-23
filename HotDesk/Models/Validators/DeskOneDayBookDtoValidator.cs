using FluentValidation;
using HotDesk.Entities;

namespace HotDesk.Models.Validators
{
    public class DeskOneDayBookDtoValidator : AbstractValidator<DeskOneDayBookDto>
    {
        public DeskOneDayBookDtoValidator(HotDeskDbContext dbContext) 
        {
            RuleFor(x => x.DeskId).NotEmpty();
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.BookDay).NotEmpty();
            RuleFor(x => x.BookDay).GreaterThan(DateTime.Now);
            RuleFor(x => x).Custom((value, context) =>
            {
                var bookDay = value.BookDay;

                //if the same desk was found between existing order date, cannot reserve
                var isreserved = dbContext.Reservations.FirstOrDefault(x => x.LocationId == value.LocationId &&
                                                            x.DeskId == value.DeskId &&
                                                            x.StartDate <= value.BookDay &&
                                                            x.EndDate >= value.BookDay); 
                if (isreserved != null)
                {
                    context.AddFailure("BookDay", "Cannot make reservation. This desk is already booked during selected time");
                }
            });
        }
    }
}
