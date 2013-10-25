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
            int numberOfRequestedSeats = request.SeatCount;

            var unreservedSeats = availableSeatsService.GetUnreservedSeats("");
            if (unreservedSeats.Count >= numberOfRequestedSeats)
            {
                return new Reservation("", "id", unreservedSeats.Take(numberOfRequestedSeats).ToList());
            }

            return new Reservation("", "", new List<Seat>());
        }
    }
}
