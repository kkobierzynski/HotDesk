using HotDesk.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotDesk.Controllers;
using AutoFixture;
using HotDesk.Models;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Castle.Components.DictionaryAdapter.Xml;
using HotDesk.Entities;

namespace Tests.ControllerTest
{
    public class ReservationControllerTests 
    {
        private Mock<IReservationServices> _reservationServices = new Mock<IReservationServices>();
        private ReservationController _controller;
        private Fixture _fixture;

        public ReservationControllerTests() 
        {
            _fixture = new Fixture();
            _controller = new ReservationController(_reservationServices.Object);

        }

        [Fact]
        public void givenLocationId_WhenAllReservationInLocation_ThenReturnAllReservations()
        {
            //Arrange
            var Id = 1;
            var expectedStatus = StatusCodes.Status200OK;
            var expectedReservation = _fixture.CreateMany<ReservationDto>(3).ToList();
            _reservationServices.Setup(x => x.GetReservedByLocation(It.IsAny<int>())).Returns(expectedReservation);

            //Act
            var actual = _controller.AllReservationsInLocation(Id); 
            var result = actual.Result as OkObjectResult;


            //Assert
            _reservationServices.Verify(x => x.GetReservedByLocation(It.IsAny<int>()), Times.Once());
            actual.Should().BeOfType<ActionResult<List<ReservationDto>>>();
            result.Value.Should().BeEquivalentTo(expectedReservation);
            result.StatusCode.Should().Be(expectedStatus);
            
        }

        [Fact]
        public void givenReservationId_WhenGetReservation_ThenReturnReservation()
        {
            //Arrange
            var id = 1;
            var expectedStatus = StatusCodes.Status200OK;
            var expectedReservation = _fixture.Create<DeskBookDto>();
            _reservationServices.Setup(x => x.GetReservationById(It.IsAny<int>())).Returns(expectedReservation);

            //Act
            var actual = _controller.GetReservation(id);
            var result = actual.Result as OkObjectResult;

            //Assert
            _reservationServices.Verify(x => x.GetReservationById(It.IsAny<int>()), Times.Once);
            actual.Should().BeOfType<ActionResult<DeskBookDto>>();
            result.Value.Should().BeEquivalentTo(expectedReservation);
            result.StatusCode.Should().Be(expectedStatus);
        }

        [Fact]
        public void givenDeskBookDto_WhenReservation_ThenCreatedReservation()
        {
            //Arrange
            var dto = _fixture.Create<DeskBookDto>();
            var expectedStatus = StatusCodes.Status201Created;
            var expectedId = 2;
            _reservationServices.Setup(x => x.MakeReservation(It.IsAny<DeskBookDto>())).Returns(expectedId);

            //Act
            var actual = _controller.Reservation(dto);
            var result = actual as CreatedResult;

            //Assert
            _reservationServices.Verify(x => x.MakeReservation(It.IsAny<DeskBookDto>()), Times.Once);
            result.StatusCode.Should().Be(expectedStatus);
            result.Location.Should().BeEquivalentTo($"api/reservation/{expectedId}");
        }

        [Fact]
        public void givenReservationIdDeskBookDto_WhenChangeReservation_ThenReturnOkStatusCode()
        {
            //Arrange
            var id = 1;
            var dto = _fixture.Create<DeskBookDto>();
            var expectedStatus = StatusCodes.Status200OK;
            _reservationServices.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<DeskBookDto>()));
            
            //Act
            var actual = _controller.ChangeReservation(id, dto);
            var result = actual as OkResult;

            //Assert
            _reservationServices.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<DeskBookDto>()), Times.Once);
            result.StatusCode.Should().Be(expectedStatus);
        }

    }
}
