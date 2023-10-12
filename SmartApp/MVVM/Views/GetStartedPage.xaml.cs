using SmartApp.MVVM.ViewModels;

namespace SmartApp.MVVM.Views;

public partial class GetStartedPage : ContentPage
{
	public GetStartedPage(GetStartedViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}