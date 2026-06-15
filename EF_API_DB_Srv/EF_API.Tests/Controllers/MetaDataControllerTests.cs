using EF_API.Controllers;
using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.DTO;
using EF_API_DB_Srv_DAL.Interfaces;
using EF_API_DB_Srv_DAL.Oracle.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EF_API.Tests.Controllers;

public class MetaDataControllerTests
{
    private readonly MetaDataController _controller;
    private readonly Mock<DBContext> _dbContextMock;

    public MetaDataControllerTests()
    {
        _dbContextMock = new Mock<DBContext>();
        
        // Mock data
        _dbContextMock.Setup(x => x.Arrays).ReturnsDbSet(new List<DTO_Array> { new DTO_Array(101, "Test", "Test") });
        _dbContextMock.Setup(x => x.Descriptions).ReturnsDbSet(new List<DTO_Description>());
        _dbContextMock.Setup(x => x.Notes).ReturnsDbSet(new List<DTO_Note>());
        _dbContextMock.Setup(x => x.Historics).ReturnsDbSet(new List<DTO_Historic>());

        var arrayRepo = new ArrayRepository(_dbContextMock.Object);
        var descRepo = new DescriptionRepository(_dbContextMock.Object);
        var noteRepo = new NoteRepository(_dbContextMock.Object);
        var historicRepo = new HistoricRepository(_dbContextMock.Object);

        var arrayService = new EF_API.Services.Array(arrayRepo);
        var descService = new EF_API.Services.Description(descRepo);
        var noteService = new EF_API.Services.Note(noteRepo);
        var historicService = new EF_API.Services.Historic(historicRepo);
        _controller = new MetaDataController(arrayService, descService, noteService, historicService);
    }

    [Fact]
    public void GetArrays_WithValidSelection_ShouldReturnOkResult()
    {
        // Act
        var result = _controller.GetArrays(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);

        var data = okResult.Value as IEnumerable<EF_API.Models.Array>;
        data.Should().NotBeNull();
        data.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public void GetArrays_WithInvalidSelection_ShouldReturnBadRequest(int selection)
    {
        // Act
        var result = _controller.GetArrays(selection);

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().Be("Must be between 1 and 4");
    }
}
