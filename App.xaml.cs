using CustomSchedularControl.Views;

namespace CustomSchedularControl
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new HomePage();
        }
    }
}