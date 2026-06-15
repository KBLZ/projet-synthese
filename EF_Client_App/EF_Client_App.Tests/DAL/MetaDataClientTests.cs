using EF_Client_App_DAL;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EF_Client_App.Tests.DAL;

public class MetaDataClientTests
{
    [Fact]
    public async Task GetArraysAsync_ShouldReturnArrays_WhenResponseIsSuccess()
    {
        // Arrange
        var expectedArrays = new List<ArrayDTO> 
        { 
            new ArrayDTO { ArrayId = 1, Title = "Test Title", SubTitle = "Test SubTitle" } 
        };
        
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var jsonResponse = JsonSerializer.Serialize(expectedArrays, jsonOptions);

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var client = new MetaDataClient(httpClient);

        // Act
        var result = await client.GetArraysAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }
}
