using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tucson.API.Controllers;
using Tucson.Application.Dto;
using Tucson.Application.Service.Interface;
using Tucson.Domain.Entities;
using Tucson.Domain.Interfaces;
using Tucson.Domain.ValueObject;

namespace Tucson.API.Tests.Controllers
{
    public class ReservationControllerTest
    {
        private readonly Mock<IReservationRepository> _reservationRepositoryMock;
        private readonly Mock<IReservationService> _reservationServiceMock;
        private readonly ReservationController _controller;

        public ReservationControllerTest()
        {
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _reservationServiceMock = new Mock<IReservationService>();
            _controller = new ReservationController(_reservationServiceMock.Object, _reservationRepositoryMock.Object);
        }

        [Fact]
        public void GetReservationsOk()
        {
            var reservations = new List<Reservation>
            {
                new Reservation(
                    1,
                    new Customer(1, "Brisa", MembershipCategoryEnum.Diamond),
                    5,
                    DateTime.Today
                )
            };

            _reservationRepositoryMock.Setup(r => r.GetAllReservations()).Returns(reservations);

            var result = _controller.GetReservations();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task DeleteReservationOk()
        {
            _reservationServiceMock.Setup(s => s.CancelReservationAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _controller.DeleteReservation(1);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteReservationError()
        {
            _reservationServiceMock
                .Setup(s => s.CancelReservationAsync(It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("No se encontró la reserva"));

            var result = await _controller.DeleteReservation(1);

            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task CreateReservationOk()
        {
            var customer = new Customer(1, "Brisa", MembershipCategoryEnum.Gold);
            var reservationDto = new ReservationRequestDto
            {
                CustomerId = customer.Id,
                ReservationDate = DateTime.Today,
                SeatCount = 4
            };

            var reservation = new Reservation(1, customer, 1, reservationDto.ReservationDate);

            _reservationRepositoryMock.Setup(r => r.GetCustomerById(customer.Id)).Returns(customer);
            _reservationServiceMock.Setup(s => s.CreateReservationAsync(customer, reservationDto.ReservationDate, reservationDto.SeatCount))
                                   .ReturnsAsync(reservation);

            var result = await _controller.CreateReservation(reservationDto);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task CreateReservationError()
        {
            _reservationRepositoryMock.Setup(r => r.GetCustomerById(It.IsAny<int>())).Returns((Customer)null);

            var result = await _controller.CreateReservation(new ReservationRequestDto { CustomerId = 99 });

            var notFound = result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound!.StatusCode.Should().Be(404);
        }

        [Fact]
        public void GetWaitingListOk()
        {
            var customer = new Customer(1, "Brisa", MembershipCategoryEnum.Diamond);
            var waitingList = new List<WaitingList>
            {
                new WaitingList(customer, DateTime.Today, 2)
            };

            _reservationRepositoryMock.Setup(r => r.GetWaitingList()).Returns(waitingList);

            var result = _controller.GetWaitingList();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
        }
    }
}
