using System.Collections.Generic;
using System.Linq;

namespace KataTrainReservation
{
    public class TicketOffice
    {
        private readonly IAvailableSeatsService availableSeatsService;

        public TicketOffice(IAvailableSeatsService availableSeatsService)
        {
            this.availableSeatsService = availableSeatsService;
        }

        public Reservation MakeReservation(ReservationRequest request)
        {
            var unreservedSeats = availableSeatsService.GetUnreservedSeats("");
            if (unreservedSeats.Any())
            {
                return new Reservation("", "id", new List<Seat>{unreservedSeats.First()});
            }

            return new Reservation("", "", new List<Seat>());
        }
    }
}
