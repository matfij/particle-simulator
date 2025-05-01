namespace SimulatorUI;

public partial class ShareModal : ContentPage
{
	public ShareModal()
	{
		InitializeComponent();
	}

	private async void OnShare(object sender, EventArgs e)
	{
		var name = NameEntry.Text;
		NameError.IsVisible = false;
		if (name == null || name.Length < 4 || name.Length > 20)
		{
			NameError.IsVisible = true;
			return;
		}
		// TODO aws-1410
		// make request to UploadLambda to get S3 url
		// serialize simulation
		// upload simulation to bucket
        await DisplayAlert("Success", $"{name} was shared.", "Close");
		await Navigation.PopModalAsync();
    }

	private async void OnCancel(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}