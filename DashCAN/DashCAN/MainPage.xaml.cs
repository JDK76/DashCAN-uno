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
            var vm = new ViewModel.Main(ViewModel.DataSource.CanBus);
            //cc.Content = new View.SupraAnalog() { DataContext = vm };
            cc.Content = new View.SupraDigital() { DataContext = vm };
            //lv.ItemsSource = vm.Messages;
        }

        private void Grid_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
#if HAS_UNO
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().ExitFullScreenMode();
#endif
        }
    }
}