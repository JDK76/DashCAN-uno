namespace DashCAN
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            cc.Content = new View.SupraAnalog() { DataContext = new ViewModel.Main() };
        }

        private void Grid_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
#if HAS_UNO
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().ExitFullScreenMode();
#endif
        }
    }
}