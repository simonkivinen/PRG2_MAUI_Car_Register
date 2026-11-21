using System.Collections.ObjectModel;
using PRG_MAUI_Car_Register.Model;

namespace PRG_MAUI_Car_Register.View
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Vehicle> vehicleList = new ObservableCollection<Vehicle>();
        public MainPage()
        {
            InitializeComponent();
            pickerType.SelectedIndex = 0;
        }

        private void OnRegisterClicked(object sender, EventArgs e)
        {
            try
            {
                Vehicle vehicle = pickerType.SelectedIndex switch
                {
                    0 => new Car(),
                    1 => new Motorcycle(),
                    2 => new Truck(),
                    _ => throw new ArgumentException("Välj en fordonstyp.")
                };

                vehicle.RegistrationNumber = entryRegistrationNumber.Text;
                vehicle.Manufacturer = entryManufacturer.Text;
                vehicle.Model = entryModel.Text;

                
                if (!int.TryParse(entryYear.Text, out int year))
                    throw new ArgumentException("Årsmodell måste vara ett heltal.");
                vehicle.Year = year;

                vehicleList.Add(vehicle);
                vehicleCollectionView.ItemsSource = null;
                vehicleCollectionView.ItemsSource = vehicleList;

                ClearTextFields();
            }

          
            catch (ArgumentException ex)
            {
                DisplayAlert("Fel", ex.Message, "OK");
            }
        }

        private void OnRadioCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value != true) return;

          
            IEnumerable<Vehicle> filteredList;

            if (radioCar.IsChecked)
            {
                filteredList = vehicleList.Where(v => v is Car);
            }
            else if (radioMC.IsChecked)
            {
                filteredList = vehicleList.Where(v => v is Motorcycle);
            }
            else if (radioTruck.IsChecked)
            {
                filteredList = vehicleList.Where(v => v is Truck);
            }
            else
            {
                filteredList = vehicleList;
            }

            vehicleCollectionView.ItemsSource = filteredList;
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            string searchTerm = entrySearchRegistrationNumber.Text?.ToLower() ?? string.Empty;

            if (string.IsNullOrEmpty(searchTerm))
            {
                labelSearchResult.Text = "Ange ett registreringsnummer för att söka.";
                return;
            }

            var foundVehicle = vehicleList.FirstOrDefault(v => v.RegistrationNumber?.ToLower() == searchTerm);

            if (foundVehicle != null)
            {
                labelSearchResult.Text = $"Fordon hittat:\n" +
                                         $"Registreringsnummer: {foundVehicle.RegistrationNumber}\n" +
                                         $"Tillverkare: {foundVehicle.Manufacturer}\n" +
                                         $"Modell: {foundVehicle.Model}\n" +
                                         $"Årsmodell: {foundVehicle.Year}\n" +   // NY
                                         $"Typ: {foundVehicle.VehicleTypeName}";
            }
            else
            {
                labelSearchResult.Text = "Inget fordon hittades med det registreringsnumret.";
            }
        }

        private void ClearTextFields()
        {
            entryRegistrationNumber.Text = string.Empty;
            entryManufacturer.Text = string.Empty;
            entryModel.Text = string.Empty;
            entryYear.Text = string.Empty;   
        }
    }
}