using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace PR.Client
{
    class Program
    {

        static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\nWybierz opcje:");
                Console.WriteLine("1. Dodaj pacjenta");
                Console.WriteLine("2. Wyswietl pacjentow");
                Console.WriteLine("3. Wywolaj bledy");

                int caseSwitch = Convert.ToInt32(Console.ReadLine());

                switch (caseSwitch)
                {
                    case 1:
                        Authorize();
                        createUser();
                        break;
                    case 2:
                        showUsers();
                        break;

                    case 3:
                        makeErrors();
                        break;

                    default:
                        break;
                }

            }
                
        }

        static private async void makeErrors()
        {        
            await client.PutAsync("https://localhost:5000/api/patients", 
                new StringContent("test", Encoding.UTF8, "application/json"));
            await client.PutAsync("https://localhost:5002/api/email", 
                new StringContent("test", Encoding.UTF8, "application/json"));
        }

        static private async void Authorize()
        {
            var app = PublicClientApplicationBuilder.Create("67dd9cfb-4344-4cc8-a2ca-573f6bb4422f")
              .WithAuthority("https://login.microsoftonline.com/146ab906-a33d-47df-ae47-fb16c039ef96/v2.0")
              .WithDefaultRedirectUri()
              .Build();

            var result = await app.AcquireTokenInteractive(new[] { "api://67dd9cfb-4344-4cc8-a2ca-573f6bb4422f/.default" })
           .ExecuteAsync();

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + result.AccessToken);
        }

        static private async void createUser()
        {
           
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

            
            await client.PostAsync("https://localhost:5000/api/patients", //Post/Put
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
