using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using System.Linq;

namespace KataTrainReservation
{
    [TestFixture]
    public class TicketOfficeNUnitTest
    {
        private const string trainId = "express_2013";

        [Test]
        public void MakeReservation_GivesReservationWithEmptyBookingID_WhenTheTrainIsFull()
        {
            var mockAvailableSeatsService = Substitute.For<IAvailableSeatsService>();
            mockAvailableSeatsService.GetUnreservedSeats(trainId).ReturnsForAnyArgs(new List<Seat>());
            var ticketOffice = new TicketOffice(mockAvailableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Empty);
        }

        [Test]
        public void MakeReservation_GivesReservationWithNonEmptyBookingID_WhenTheTrainIsNotFull_AndASingleSeatIsAskedFor()
        {
            var mockAvailableSeatsService = Substitute.For<IAvailableSeatsService>();
            mockAvailableSeatsService.GetUnreservedSeats(trainId).ReturnsForAnyArgs(new List<Seat>{new Seat("A", 1)});
            var ticketOffice = new TicketOffice(mockAvailableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Not.Empty);
        }

        [Test]
        public void MakeReservation_GivesTheASeat_WhenOnlyOneSeatIsAvailable()
        {
            var mockAvailableSeatsService = Substitute.For<IAvailableSeatsService>();
            var availableSeat = new Seat("A", 1);
            mockAvailableSeatsService.GetUnreservedSeats(trainId).ReturnsForAnyArgs(new List<Seat>{availableSeat});
            var ticketOffice = new TicketOffice(mockAvailableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            var bookedSeat = reservation.Seats.Single();
            Assert.That(bookedSeat, Is.EqualTo(availableSeat));
        }
    }
}
