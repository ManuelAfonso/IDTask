using Services;
using Services.DTOs;
using Services.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISoldierInfoService _soldierInfoService;
        private readonly ISoldierLocationService _soldierLocationService;

        public MainWindow(
            ISoldierInfoService soldierInfoService,
            ISoldierLocationService soldierLocationService)
        {
            _soldierInfoService = soldierInfoService ??
                throw new ArgumentNullException(nameof(soldierInfoService));
            _soldierLocationService = soldierLocationService ??
                throw new ArgumentNullException(nameof(soldierLocationService));
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (sender as Button).IsEnabled = false;
                
                // get list of soldiers including location (if any)
                var soldiers = await _soldierInfoService.GetAllAsync(20, 1);
                if (!soldiers.Any())
                {
                    throw new Exception("database is empty"); 
                }

                var lat = Random.Shared.Next(30);
                var lon = Random.Shared.Next(60);
                var soldier = soldiers.ElementAt(Random.Shared.Next(20));
                if (soldier != null)
                {
                    var request = new UpdateSoldierLocationRequestDTO(
                        soldier.SoldierId,
                        new LocationDTO(lat, lon),
                        DateTime.UtcNow,
                        SourceType.User);
                    await _soldierLocationService.UpdateSoldierLocationAsync(request);
                }

                // get positions in "map"
                var positions = await _soldierLocationService.GetLocationsInMapAsync(
                    new LocationDTO(0, 0),
                    new LocationDTO(30, 60));

                if (positions.FirstOrDefault(p => p.SoldierId == soldier.SoldierId) == default)
                {
                    throw new Exception("soldier should be in positions");
                }

                // get details for soldier in marker
                var soldierInfo = await _soldierInfoService.GetSoldierInfoWithLocationAsync(soldier.SoldierId);
                if (soldierInfo.Location == null || 
                    soldierInfo.Location.Latitude != lat ||
                    soldierInfo.Location.Longitude != lon)
                {
                    throw new Exception("soldier with wrong location");
                }
            }
            finally
            {
                (sender as Button).IsEnabled = true;
            }
        }
    }
}