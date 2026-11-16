using System.Text.RegularExpressions;

namespace PRG_MAUI_Car_Register.Model
{
    public abstract class Vehicle
    {
        // Medlemsvariabler
        private string registrationNumber = string.Empty;
        private string manufacturer = string.Empty;
        private string model = string.Empty;
        private int year;

        // Subklasserna (Car, Motorcycle, Truck) bestämmer själva vad de heter
        public abstract string VehicleTypeName { get; }

        // Get-Set för att hålla variablerna privata, och för att validera inkommande värden från UI
        public string RegistrationNumber
        {
            get { return registrationNumber; }

            set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length == 6)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (!char.IsLetter(value[i]))
                            throw new ArgumentException("Inkorrekt registreringsnummer: De första tre tecknen måste vara bokstäver.");
                    }

                    for (int i = 3; i < 5; i++)
                    {
                        if (!char.IsDigit(value[i]))
                            throw new ArgumentException("Inkorrekt registreringsnummer: Det fjärde och femte tecknet måste vara siffror.");
                    }

                    if (!char.IsDigit(value[5]) && !char.IsLetter(value[5]))
                        throw new ArgumentException("Inkorrekt registreringsnummer: Det sjätte tecknet måste vara en siffra eller en bokstav.");
                }
                else
                {
                    throw new ArgumentException("Ett registreringsnummer måste bestå av exakt 6 tecken, med tre bokstäver följt av två siffror och en siffra eller bokstav.");
                }

                registrationNumber = value.ToUpper();
            }
        }

        public string Model
        {
            get => model;
            set => model = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Modell måste anges");
        }

        public string Manufacturer
        {
            get => manufacturer;
            set => manufacturer = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Tillverkare måste anges");
        }

        public int Year
        {
            get => year;
            set => year = !Regex.IsMatch(value.ToString(), @"^[1-2][0-9][0-9][0-9]$")
                ? throw new ArgumentException("Årsmodell måste vara ett fyrsiffrigt tal mellan 1000 och 2999")
                : (value < 1895 || value > DateTime.Now.Year)
                    ? throw new ArgumentException($"Årsmodell måste vara ett fyrsiffrigt tal mellan 1895 och {DateTime.Now.Year}")
                    : value;
        }

        public string DisplayText => ToString();

        public override string ToString() => $"{registrationNumber}\t{VehicleTypeName}\t{manufacturer}\t{model}\t{year}";
    }
}