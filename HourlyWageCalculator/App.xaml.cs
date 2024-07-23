namespace HourlyWageCalculator
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Routing.RegisterRoute("Main", typeof(MainPage));
            Routing.RegisterRoute("SubMain", typeof(SubMain));
        }
    }
}
