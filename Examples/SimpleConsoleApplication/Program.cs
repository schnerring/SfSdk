using System;
using System.Linq;
using System.Threading.Tasks;
using Nito.AsyncEx;
using SfSdk;
using SfSdk.Contracts;
using SfSdk.Providers;

namespace SimpleConsoleApplication
{
    class Program
    {
        static void Main()
        {
            AsyncContext.Run(() => MainAsync());
        }

        private static async void MainAsync()
        {
            await ChooseCountryAsync();
        }

        private static async Task ChooseCountryAsync()
        {
            Console.WriteLine("Welcome to the SfSdk Example");
            Console.WriteLine();
            Console.WriteLine("Loading countries...");
            var countries = (await new CountryProvider().GetCountriesAsync()).ToList();
            Console.WriteLine("Loading countries finished!");

            for (var i = 0; i < countries.Count; i++)
                Console.WriteLine("{0}:     {1}", i, countries[i].Name);
            
            Console.WriteLine();
            Console.WriteLine("Type a number to select a country");
            var input = Console.ReadLine();
            int parsedInput;
            while (!int.TryParse(input, out parsedInput) && countries.Count <= parsedInput)
            {
                Console.WriteLine("Invalid input, please retry with another value.");
                input = Console.ReadLine();
            }

            var servers = countries[parsedInput].Servers;

            for (var i = 0; i < servers.Count; i++)
                Console.WriteLine("{0}:     {1}", i, servers[i].Name);

            Console.WriteLine();
            Console.WriteLine("Type a number to select a server");
            input = Console.ReadLine();
            while (!int.TryParse(input, out parsedInput) && servers.Count <= parsedInput)
            {
                Console.WriteLine("Invalid input, please retry with another value.");
                input = Console.ReadLine();
            }

            var selectedServerUri = servers[parsedInput].Uri;

            await LoginAsync(selectedServerUri);
        }

        private static async Task LoginAsync(Uri serverUri)
        {
            Console.Write("Username:    ");
            var username = Console.ReadLine();
            Console.Write("Password:    ");
            var passwordHash = Console.ReadLine().ConvertToMd5Hash();
            
            var session = new Session();
            await session.LoginAsync(username, passwordHash, serverUri);

            var character = await session.CharacterScreenAsync();

            Console.WriteLine();
            var charType = typeof (ICharacter);

            // reflection for showing all properties easier
            foreach (var p in charType.GetProperties())
            {
                Console.WriteLine("    {0}: {1}", p.Name, p.GetValue(character));
            }
            
            Console.ReadKey();
        }
    }
}
