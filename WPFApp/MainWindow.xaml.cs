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

                // change soldiers location
                var soldier = soldiers.FirstOrDefault();
                if (soldier != null)
                {
                    var request = new UpdateSoldierLocationRequestDTO(
                        soldier.SoldierId,
                        new LocationDTO(14.124M, 29.876M),
                        DateTime.UtcNow, 
                        SourceType.User);
                    await _soldierLocationService.UpdateSoldierLocationAsync(request);
                }

                // get positions in "map"
                var positions = await _soldierLocationService.GetLocationsInMapAsync(
                    new LocationDTO(0, 5),
                    new LocationDTO(40, 30));

                // get details for soldier in marker
                var soldierInfo = await _soldierInfoService.GetSoldierInfoWithLocationAsync(soldier.SoldierId);
            }
            finally
            {
                (sender as Button).IsEnabled = true;
            }
        }
    }
}