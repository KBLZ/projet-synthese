using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.ViewModels;

namespace AvaloniaApplication1.Views;

public partial class AccueilView : UserControl
{
    public AccueilView()
    {
        InitializeComponent();
    }

    private void btn_PrevQc_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainVM = (MainWindowViewModel)this.VisualRoot?.DataContext;
        mainVM.PageCourante = new ParcourirViewModel(mainVM, TypeSection.PrevisionQuebec);
    }

    private void btn_CompQc_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainVM = (MainWindowViewModel)this.VisualRoot?.DataContext;
        mainVM.PageCourante = new ParcourirViewModel(mainVM, TypeSection.ComparaisonQuebec); 
    }

    private void btn_PrevCan_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainVM = (MainWindowViewModel)this.VisualRoot?.DataContext;
        mainVM.PageCourante = new ParcourirViewModel(mainVM, TypeSection.PrevisionCanada);
    }

    private void btn_CompCan_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainVM = (MainWindowViewModel)this.VisualRoot?.DataContext;
        mainVM.PageCourante = new ParcourirViewModel(mainVM, TypeSection.ComparaisonCanada);
    }
}