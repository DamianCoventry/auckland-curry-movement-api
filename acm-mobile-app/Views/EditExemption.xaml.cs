using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

public partial class EditExemption : ContentPage
{
	public EditExemption()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ShortReason.Text))
        {
            var toast = Toast.Make("A short reason for the exemption is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        // TODO: actually add it
        await Shell.Current.GoToAsync("//manage_exemptions");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }
}