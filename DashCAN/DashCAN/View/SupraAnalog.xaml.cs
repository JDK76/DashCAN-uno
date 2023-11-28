namespace DashCAN.View
{
    public sealed partial class SupraAnalog : Page
    {
        public SupraAnalog()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel.Main();
        }
    }
}
