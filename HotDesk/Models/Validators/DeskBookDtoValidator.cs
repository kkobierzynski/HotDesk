using FluentValidation;
using HotDesk.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HotDesk.Models.Validators
{
    public class DeskBookDtoValidator : AbstractValidator<DeskBookDto>
    {
        public DeskBookDtoValidator(HotDeskDbContext dbContext, IActionContextAccessor actionContextAccessor) 
        {
            RuleFor(x => x.DeskId).NotEmpty();
            RuleFor(x => x.LocationId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty();
            RuleFor(x => x.StartDate).GreaterThan(DateTime.Now);
            RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate);
            RuleFor(x => x).Custom((value, context) =>
            {
                DateTime startDate = value.StartDate;
                DateTime endDate = value.EndDate;
                TimeSpan span = endDate - startDate;
                int reservedDays = span.Days + 1;
                if (reservedDays > 7)
                {
                    context.AddFailure("EndDate", "Cannot reserve desk for more than one week");
                }

                var reservationId = actionContextAccessor.ActionContext?.RouteData.Values.GetValueOrDefault("id");
                if(reservationId != null)
                {
                    reservationId = int.Parse(reservationId.ToString());
                }                

                //if the same desk was found between existing order date, cannot reserve
                var isreserved = dbContext.Reservations
                    .FirstOrDefault(x => x.LocationId == value.LocationId &&
                                    x.DeskId == value.DeskId &&
                                    x.Id != (int?)reservationId &&
                                        (endDate >= x.StartDate && endDate <= x.EndDate ||
                                        startDate >= x.StartDate && startDate <= x.EndDate ||
                                        startDate <= x.StartDate && endDate >= x.EndDate));

                if (isreserved != null)
                {
                    context.AddFailure("ReservationDate", "Cannot make reservation. This desk is already booked during selected time");
                }
            });
        }
    }
}
