using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace acm_mobile_app.Views;

public partial class EditMember : ContentPage
{
	public EditMember()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public async void OnClickOK(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MemberName.Text))
        {
            var toast = Toast.Make("A name for the member is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        if (string.IsNullOrWhiteSpace(Sponsor.Text))
        {
            var toast = Toast.Make("A sponsor for the member is required", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        // TODO: actually add it
        await Shell.Current.GoToAsync("//manage_members");
    }
}