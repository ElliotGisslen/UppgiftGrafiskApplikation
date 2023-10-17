using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Contexts;
using DataAccess.Models.Entities;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using SmartApp.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartApp.MVVM.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly SmartAppDbContext _context;
        private readonly IotHubManager _iotHubManager;



        public MainViewModel(SmartAppDbContext context, IotHubManager iotHubManager)
        {
            _context = context;
            _iotHubManager = iotHubManager;
            CheckConfigurationAsync().ConfigureAwait(false);

        }
        



        private async Task CheckConfigurationAsync()
        {
            try
            {
                
                if (await _context.Settings.AnyAsync())
                {
                    await _iotHubManager.InitializeAsync();
                    await Shell.Current.GoToAsync(nameof(OverviewPage));
                }
                    
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); } 
        }

        [RelayCommand]
        async Task GoToGetStarted()
        {
            await Shell.Current.GoToAsync(nameof(GetStartedPage));
        }
    }
}
