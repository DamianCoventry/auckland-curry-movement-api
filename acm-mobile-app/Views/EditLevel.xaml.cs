using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

public partial class EditLevel : ContentPage
{
	public EditLevel()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LevelName.Text))
        {
            var toast = Toast.Make("A name for the level is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        if (string.IsNullOrWhiteSpace(LevelDescription.Text))
        {
            var toast = Toast.Make("A description for the level is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        if (string.IsNullOrWhiteSpace(RequiredAttendances.Text))
        {
            var toast = Toast.Make("A number for the required attendances is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        // TODO: actually add it
        await Shell.Current.GoToAsync("//manage_level");
    }

    public async void OnClickCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }
}
