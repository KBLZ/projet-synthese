using EF_Client_App_BL;
using EF_Client_App_Entity;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EF_Client_App.Tests.BL;

public class MatchingTests
{
    [Fact]
    public void PopulateDescriptionsSeries_ShouldPopulateSeriesProperly()
    {
        // Arrange
        var arrays = new List<EF_Client_App_Entity.Array>();
        
        var seriePlaceholder = new Serie { ID = "S1" };
        var description = new Description 
        { 
            FirstLineArray = new List<Serie> { seriePlaceholder }
        };
        var descriptions = new List<Description> { description };
        
        var fullSerie = new Serie { ID = "S1", Frequency = 'Q' };
        var series = new List<Serie> { fullSerie };
        
        var matching = new Matching(arrays, descriptions, series);
        
        // Act
        matching.PopulateDescriptionsSeries();
        
        // Assert
        description.FirstLineArray.Should().HaveCount(1);
        description.FirstLineArray.First().Frequency.Should().Be('Q');
    }
}
