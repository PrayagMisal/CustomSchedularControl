namespace CustomSchedularControl.Views;

public partial class DemoPage : ContentPage
{
	public DemoPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		App.Current.MainPage.Navigation.PushModalAsync(new HomePage());
    }
}