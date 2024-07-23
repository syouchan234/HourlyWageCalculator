namespace HourlyWageCalculator;

public partial class SubMain : ContentPage
{
	public SubMain()
	{
		InitializeComponent();
	}
	private async void MainPagePreview(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("Main");
	}
}