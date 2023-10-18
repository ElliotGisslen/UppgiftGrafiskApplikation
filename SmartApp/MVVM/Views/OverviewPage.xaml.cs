using SmartApp.MVVM.ViewModels;

namespace SmartApp.MVVM.Views;

public partial class OverviewPage : ContentPage
{

    private bool isUpdating = false;
    public OverviewPage(OverviewViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}


    private void OnToggleDevice(object sender, ToggledEventArgs e)
    {
        if (isUpdating)
            return;

        if (BindingContext is OverviewViewModel viewModel && sender is Switch deviceSwitch)
        {
            var deviceViewModel = deviceSwitch.BindingContext as DeviceItemViewModel;

            if (deviceViewModel != null)
            {
                isUpdating = true;

                viewModel.ToggleDeviceCommand.Execute(deviceViewModel);

                // Note: You may want to handle the asynchronous logic within the ToggleDevice method

                // Update the Switch state based on the result of the toggle operation
                deviceSwitch.IsToggled = deviceViewModel.IsActive;

                isUpdating = false;
            }
        }
    }
}