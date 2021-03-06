﻿using System.Collections.Generic;
using System.Linq;

namespace KataTrainReservation
{
    public class TicketOffice
    {
        private readonly IBookingIdService bookingIdService;
        private readonly IAvailableSeatsService availableSeatsService;

        public TicketOffice(IBookingIdService bookingIdService, IAvailableSeatsService availableSeatsService)
        {
            this.bookingIdService = bookingIdService;
            this.availableSeatsService = availableSeatsService;
        }

        public Reservation MakeReservation(ReservationRequest request)
        {
            int numberOfRequestedSeats = request.SeatCount;
            var unreservedSeats = availableSeatsService.GetUnreservedSeats(request.TrainId);
            int totalNumberOfSeats = availableSeatsService.GetTotalNumberOfSeats(request.TrainId);

            if ((unreservedSeats.Count - numberOfRequestedSeats) <= (0.3 * totalNumberOfSeats))
            {
                return new Reservation(request.TrainId, "", new List<Seat>());
            }

            if (unreservedSeats.Count >= numberOfRequestedSeats)
            {
                var seatsGroupedByCoach = unreservedSeats.GroupBy(seat => seat.Coach);

                foreach (var grouping in seatsGroupedByCoach)
                {
                    if (grouping.Count() >= numberOfRequestedSeats)
                    {
                        var reservedSeats = grouping.Take(numberOfRequestedSeats).ToList();
                        return new Reservation(request.TrainId, bookingIdService.GetNextId(), reservedSeats);
                    }
                }
            }

            return new Reservation(request.TrainId, "", new List<Seat>());
        }
    }
}
