using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PR.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\nWybierz opcje:");
                Console.WriteLine("1. Dodaj pacjenta");
                Console.WriteLine("2. Wyswietl pacjentow");
                Console.WriteLine("3. Zamknij program");

                int caseSwitch = Convert.ToInt32(Console.ReadLine());


                switch (caseSwitch)
                {
                    case 1:
                        createUser();
                        break;
                    case 2:
                        showUsers();
                        break;

                    case 3:
                        isRunning = false;
                        break;

                    default:
                        break;
                }

            }

                
        }

        static private async void createUser()
        {
            HttpClient client = new HttpClient();

            Patient p = new Patient();
            Console.WriteLine("Podaj imie pacjenta:");
            p.FirstName = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko pacjenta:");
            p.LastName = Console.ReadLine();
            Console.WriteLine("Podaj wiek pacjenta:");
            p.Age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Podaj date pozytywnego testu, format dd-mm-rrrr: ");
            p.DateOfPositiveTest = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Podaj adres email:");
            p.EmailAdress = Console.ReadLine();

            string userJson = System.Text.Json.JsonSerializer.Serialize(p);

            await client.PostAsync("https://localhost:5000/api/patients",
                new StringContent(userJson, Encoding.UTF8, "application/json"));

            Console.WriteLine("Dodano pacjenta:");
            Console.WriteLine(
                p.FirstName + " " +
                p.LastName +
                " lat: " + p.Age +
                ", data pozytywnego testu: " + p.DateOfPositiveTest +
                " adres email: " + p.EmailAdress + "\n");

        }

        static private async void showUsers()
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("https://localhost:5000/api/patients");
            string data = await response.Content.ReadAsStringAsync();
            var patients = JsonConvert.DeserializeObject<Patient[]>(data);

            foreach (var patient in patients)
            {
                Console.WriteLine(patient.ToString());
            }

        }

        public class Patient
        {
            public int Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int Age { get; set; }

            public System.DateTime DateOfPositiveTest { get; set; }

            public string EmailAdress { get; set; }

            public override string ToString()
            {
                return Id.ToString() + ". " +
                       FirstName.ToString() + " " +
                       LastName.ToString() + " Wiek: " +
                       Age.ToString() + " Data poz. testu: " +
                       DateOfPositiveTest.ToString() + " Adres email: " +
                       EmailAdress.ToString();
            }

        }
    }
}
