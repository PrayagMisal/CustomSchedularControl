using CustomSchedularControl.ViewModels;

namespace CustomSchedularControl.Views;

public partial class HomePage 
{
	public HomePage()
	{
		InitializeComponent();
        SchedularControl.StartUpdatingCurrentTimeDashedLine();
    }
}