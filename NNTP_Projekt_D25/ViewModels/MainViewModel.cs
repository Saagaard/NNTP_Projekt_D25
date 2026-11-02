using CommunityToolkit.Mvvm.ComponentModel;

namespace NNTP_Projekt_D25.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "NNTP Client";
    
    
}
