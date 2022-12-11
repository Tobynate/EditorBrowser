namespace TimEditorBrowser;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();

        CallPage();
    }

    async void CallPage()
    {
        DevicePlatform platform = DeviceInfo.Current.Platform;
        DevicePlatform platformAndroid = DevicePlatform.Android;


        if (platform == DevicePlatform.Android || platform == DevicePlatform.iOS) return;
        else if (platform == DevicePlatform.WinUI) await Navigation.PushAsync(new BrowserPage());

    }


    private async void NavigateButton_Clicked(object sender, EventArgs e)
    {
        CallPage();
    }
}

