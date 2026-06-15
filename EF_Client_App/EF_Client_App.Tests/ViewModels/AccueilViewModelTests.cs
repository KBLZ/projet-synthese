/*using AvaloniaApplication1.ViewModels;
using EF_Client_UI_Avalonia;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace EF_Client_App.Tests.ViewModels;

public class AccueilViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeMainWindowViewModelProperties()
    {
        var mockDataService = new Mock<IDataService>();
        mockDataService.Setup(d => d.Arrays).Returns(new List<EF_Client_App_Entity.Array>());

        var mockServiceProvider = new Mock<IServiceProvider>();
        var mainVM = new MainWindowViewModel(mockDataService.Object, mockServiceProvider.Object);

        mainVM.TypeRecherche = "Test";
        mainVM.Tableau = "Test";
        mainVM.Banque1 = "Test";
        mainVM.Banque2 = "Test";
        mainVM.IsAccueil = false;

        _ = new AccueilViewModel(mainVM);

        mainVM.TypeRecherche.Should().Be("Prévisions économiques");
        mainVM.Tableau.Should().StartWith("Bonjour ").And.EndWith(" !");
        mainVM.Banque1.Should().BeEmpty();
        mainVM.Banque2.Should().BeEmpty();
        mainVM.IsAccueil.Should().BeTrue();
    }
}
*/