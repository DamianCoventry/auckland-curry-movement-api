#if ANDROID
using Android.App;
using Android.Content;
using Microsoft.Identity.Client;

namespace TodoApp.MAUI
{
    [Activity(Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
        DataHost = "auth",
        DataScheme = "msala34405bc-327c-418c-96c1-a42d35ab0539")]
    public class AndroidMsalActivity : BrowserTabActivity
    {
    }
}
#endif