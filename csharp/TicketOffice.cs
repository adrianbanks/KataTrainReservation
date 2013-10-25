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
            if (availableSeatsService.GetUnreservedSeats("").Any())
            {
                return new Reservation("", "id", new List<Seat>());
            }

            return new Reservation("", "", new List<Seat>());
        }
    }
}
