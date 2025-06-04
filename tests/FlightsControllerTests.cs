namespace UnitTests.Tests;
using FlightInformationAPI.Controllers;
using FlightInformationAPI.DTOs;
using FlightInformationAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class FlightsControllerTests
{
    private readonly Mock<IFlightService> _flightServiceMock;
    private readonly Mock<ILogger<FlightsController>> _loggerMock;
    private readonly FlightsController _controller;

    public FlightsControllerTests()
    {
        _flightServiceMock = new Mock<IFlightService>();
        _loggerMock = new Mock<ILogger<FlightsController>>();
        _controller = new FlightsController(_flightServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithPagedFlights()
    {
        // Arrange
        var flights = new List<FlightDto> { new FlightDto { Id = 1, FlightNumber = "FN1" } };
        _flightServiceMock.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(flights);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(flights, okResult.Value);
    }

    [Fact]
    public async Task GetById_Found_ReturnsOkWithFlight()
    {
        // Arrange
        var flight = new FlightDto { Id = 1, FlightNumber = "FN1" };
        _flightServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(flight);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(flight, okResult.Value);
    }

    [Fact]
    public async Task GetById_NotFound_ReturnsNotFound()
    {
        // Arrange
        _flightServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((FlightDto?)null);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtActionWithFlight()
    {
        // Arrange
        var createDto = new FlightCreateDto { FlightNumber = "FN1", Airline = "A", DepartureAirport = "D", ArrivalAirport = "A", DepartureTime = DateTime.Now, ArrivalTime = DateTime.Now.AddHours(2), Status = "On Time" };
        var flight = new FlightDto { Id = 1, FlightNumber = "FN1" };
        _flightServiceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(flight);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(flight, createdResult.Value);
        Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
        Assert.Equal(1, createdResult.RouteValues["id"]);
    }

    [Fact]
    public async Task Update_FlightExists_ReturnsNoContent()
    {
        // Arrange
        var updateDto = new FlightUpdateDto { FlightNumber = "FN1", Airline = "A", DepartureAirport = "D", ArrivalAirport = "A", DepartureTime = DateTime.Now, ArrivalTime = DateTime.Now.AddHours(2), Status = "On Time" };
        _flightServiceMock.Setup(s => s.ExistsAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Update(1, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _flightServiceMock.Verify(s => s.UpdateAsync(1, updateDto), Times.Once);
    }

    [Fact]
    public async Task Update_FlightNotFound_ReturnsNotFound()
    {
        // Arrange
        var updateDto = new FlightUpdateDto { FlightNumber = "FN1", Airline = "A", DepartureAirport = "D", ArrivalAirport = "A", DepartureTime = DateTime.Now, ArrivalTime = DateTime.Now.AddHours(2), Status = "On Time" };
        _flightServiceMock.Setup(s => s.ExistsAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.Update(1, updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _flightServiceMock.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<FlightUpdateDto>()), Times.Never);
    }

    [Fact]
    public async Task Delete_FlightExists_ReturnsNoContent()
    {
        // Arrange
        _flightServiceMock.Setup(s => s.ExistsAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _flightServiceMock.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task Delete_FlightNotFound_ReturnsNotFound()
    {
        // Arrange
        _flightServiceMock.Setup(s => s.ExistsAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _flightServiceMock.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Search_ReturnsOkWithResults()
    {
        // Arrange
        var flights = new List<FlightDto> { new FlightDto { Id = 1, FlightNumber = "FN1" } };
        _flightServiceMock.Setup(s => s.SearchAsync("A", "D", "A", null, null, 1, 10)).ReturnsAsync(flights);

        // Act
        var result = await _controller.Search("A", "D", "A", null, null, 1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(flights, okResult.Value);
    }
}

