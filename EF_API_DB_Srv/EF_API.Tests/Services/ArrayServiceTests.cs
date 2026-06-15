using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.DTO;
using EF_API_DB_Srv_DAL.Oracle.Repositories;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EF_API.Tests.Services;

public class ArrayServiceTests
{
    [Fact]
    public void GetFiltereds_WhenDataExists_ShouldReturnMappedModels()
    {
        // Arrange
        var dbContextMock = new Mock<DBContext>();
        var arrays = new List<DTO_Array>
        {
            new DTO_Array(101, "Title 1", "SubTitle 1")
        };
        dbContextMock.Setup(x => x.Arrays).ReturnsDbSet(arrays);

        var repository = new ArrayRepository(dbContextMock.Object);
        var service = new EF_API.Services.Array(repository);

        // Act
        var result = service.GetFiltereds(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        
        var firstItem = result.First();
        firstItem.ArrayId.Should().Be(101);
        firstItem.Title.Should().Be("Title 1");
        firstItem.SubTitle.Should().Be("SubTitle 1");
    }

    [Fact]
    public void GetFiltereds_WhenNoDataExists_ShouldReturnEmptyList()
    {
        // Arrange
        var dbContextMock = new Mock<DBContext>();
        dbContextMock.Setup(x => x.Arrays).ReturnsDbSet(new List<DTO_Array>());

        var repository = new ArrayRepository(dbContextMock.Object);
        var service = new EF_API.Services.Array(repository);

        // Act
        var result = service.GetFiltereds(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
