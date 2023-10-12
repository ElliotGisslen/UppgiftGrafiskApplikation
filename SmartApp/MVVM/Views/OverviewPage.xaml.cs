using SmartApp.MVVM.ViewModels;

namespace SmartApp.MVVM.Views;

public partial class OverviewPage : ContentPage
{
	public OverviewPage(OverviewViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}