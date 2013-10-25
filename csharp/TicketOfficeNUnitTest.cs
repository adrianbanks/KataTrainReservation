using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace KataTrainReservation
{
    [TestFixture]
    public class TicketOfficeNUnitTest
    {
        private const string trainId = "express_2013";
        private const string bookingId = "1234";

        private static IBookingIdService MakeBookingIdService(string nextBookingId = null)
        {
            var bookingIdService = Substitute.For<IBookingIdService>();
            bookingIdService.GetNextId().Returns(nextBookingId);
            return bookingIdService;
        }

        private static IAvailableSeatsService MakeAvailableSeatsService(params Seat[] availableSeats)
        {
            var availableSeatsService = Substitute.For<IAvailableSeatsService>();
            availableSeatsService.GetUnreservedSeats(trainId).ReturnsForAnyArgs(availableSeats);
            return availableSeatsService;
        }

        [Test]
        public void MakeReservation_GivesReservationWithEmptyBookingID_WhenTheTrainIsFull()
        {
            var bookingIdService = MakeBookingIdService();
            var availableSeatsService = MakeAvailableSeatsService();
            var ticketOffice = new TicketOffice(bookingIdService, availableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Empty);
        }

        [Test]
        public void MakeReservation_GivesReservationWithNonEmptyBookingID_WhenTheTrainIsNotFull_AndASingleSeatIsAskedFor()
        {
            var bookingIdService = MakeBookingIdService(bookingId);
            var availableSeatsService = MakeAvailableSeatsService(new Seat("A", 1));
            var ticketOffice = new TicketOffice(bookingIdService, availableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Not.Empty);
        }

        [Test]
        public void MakeReservation_GivesTheSeat_WhenOnlyOneSeatIsAvailable()
        {
            var bookingIdService = MakeBookingIdService();
            var availableSeat = new Seat("A", 1);
            var availableSeatsService = MakeAvailableSeatsService(availableSeat);
            var ticketOffice = new TicketOffice(bookingIdService, availableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            var bookedSeat = reservation.Seats.Single();
            Assert.That(bookedSeat, Is.EqualTo(availableSeat));
        }

        [Test]
        public void MakeReservation_GivesTwoSeats_WhenMoreThanTwoSeatsAreAvailable()
        {
            var bookingIdService = MakeBookingIdService();
            var availableSeat1 = new Seat("A", 1);
            var availableSeat2 = new Seat("A", 2);
            var availableSeat3 = new Seat("A", 3);
            var availableSeatsService = MakeAvailableSeatsService(availableSeat1, availableSeat2, availableSeat3);
            var ticketOffice = new TicketOffice(bookingIdService, availableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 2);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.Seats.Count(), Is.EqualTo(2));
        }

        [Test]
        public void MakeReservation_GetsTheBookingIdFromTheBookingService_WhenASuccessfulBookingIsMade()
        {
            var bookingIdService = MakeBookingIdService(bookingId);
            var availableSeat = new Seat("A", 1);
            var availableSeatsService = MakeAvailableSeatsService(availableSeat);
            var ticketOffice = new TicketOffice(bookingIdService, availableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.EqualTo(bookingId));
        }

        [Test]
        public void MakeReservation_GivesEmptyReservation_WhenThereAreNotEnoughSeatsInACoach()
        {
            var bookingIdService = MakeBookingIdService(bookingId);
            var availableSeatsService = MakeAvailableSeatsService(new Seat("A", 1), new Seat("A", 2), new Seat("B", 1), new Seat("B", 2));
            var ticketOffice = new TicketOffice(bookingIdService, availableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 4);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Empty);
        }
    }
}
