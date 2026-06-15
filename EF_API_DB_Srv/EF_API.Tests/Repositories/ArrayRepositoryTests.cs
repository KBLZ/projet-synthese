using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.DTO;
using EF_API_DB_Srv_DAL.Oracle.Repositories;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EF_API.Tests.Repositories;

public class ArrayRepositoryTests
{
    [Fact]
    public void GetDatas_WithDTOArray_ShouldReturnFilteredData()
    {
        // Arrange
        var dbContextMock = new Mock<DBContext>();
        var arrays = new List<DTO_Array>
        {
            new DTO_Array(101, "Title 1", "SubTitle 1"),
            new DTO_Array(102, "Title 2", "SubTitle 2"),
            new DTO_Array(201, "Title 3", "SubTitle 3")
        };

        dbContextMock.Setup(x => x.Arrays).ReturnsDbSet(arrays);

        var repository = new ArrayRepository(dbContextMock.Object);
        
        // Act
        // Selection = 1 -> min = 100, max = 200 (based on Utility logic)
        var result = repository.GetDatas<DTO_Array>(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(x => x.ArrayId).Should().ContainInOrder(101, 102);
    }

    [Fact]
    public void GetDatas_WithUnsupportedType_ShouldThrowNotSupportedException()
    {
        // Arrange
        var dbContextMock = new Mock<DBContext>();
        var repository = new ArrayRepository(dbContextMock.Object);

        // Act
        Action act = () => repository.GetDatas<DTO_Note>(1);

        // Assert
        act.Should().Throw<NotSupportedException>()
           .WithMessage("*Unsupported Type*");
    }
}
