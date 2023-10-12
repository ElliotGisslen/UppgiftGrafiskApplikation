using SmartApp.MVVM.Views;

namespace SmartApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(OverviewPage), typeof(OverviewPage));
            Routing.RegisterRoute(nameof(GetStartedPage), typeof(GetStartedPage));

        }
    }
}