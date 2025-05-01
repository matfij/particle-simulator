namespace SimulatorUI;

public partial class ShareModal : ContentPage
{
	public ShareModal()
	{
		InitializeComponent();
	}

	private async void OnShare(object sender, EventArgs e)
	{
		var name = NameEntry.Text?.Trim();
		if (!ValidateName(name))
		{
			return;
		}
		// TODO aws-1410
		// make request to UploadLambda to get S3 url
		// serialize simulation
		// upload simulation to bucket
        await DisplayAlert("Success", $"{name} was shared.", "Close");
		await Navigation.PopModalAsync();
    }

	private bool ValidateName(string? name)
	{
        NameError.IsVisible = false;
        if (
			string.IsNullOrEmpty(name) 
			|| name.Length < 4 
			|| name.Length > 20
			|| name.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
        {
            NameError.IsVisible = true;
            return false;
        }
		return true;
    }

	private async void OnCancel(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}
