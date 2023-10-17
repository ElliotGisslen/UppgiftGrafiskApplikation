using DataAccess.Contexts;
using DataAccess.Models.Entities;
using SmartApp.MVVM.ViewModels;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SmartApp.MVVM.Views;

public partial class GetStartedPage : ContentPage
{

    private readonly SmartAppDbContext _context;

    public GetStartedPage(GetStartedViewModel viewModel, SmartAppDbContext context)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _context = context;
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();

            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Task.Delay(500);
                await cameraView.StartCameraAsync();
            });

        }
    }

    private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (Regex.IsMatch(args.Result[0].Text, @"HostName=.*;SharedAccessKeyName=.*;SharedAccessKey=.*"))
            {
                try
                {
                    _context.Settings.Add(new SmartAppSettings { ConnectionString = args.Result[0].Text });
                    await _context.SaveChangesAsync();

                    await Shell.Current.GoToAsync(nameof(OverviewPage));
                }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
            }

        });
    }
}