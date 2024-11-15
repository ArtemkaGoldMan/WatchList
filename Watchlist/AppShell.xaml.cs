using Watchlist.Pages;

namespace Watchlist
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MovieDataPage), typeof(MovieDataPage));
        }

    }
}
