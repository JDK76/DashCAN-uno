namespace DashCAN
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Grid_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
#if HAS_UNO
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().ExitFullScreenMode();
#endif
        }

        public void SetDataContext(ViewModel.ViewModelBase vm)
        {
            cc.Content = new View.SupraDigital() { DataContext = vm };
        }
    }
}