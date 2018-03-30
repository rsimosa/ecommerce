using System;
using System.IO;
using System.Reflection;

namespace DPLRef.eCommerce.Database
{
    class SeedDataManager
    {
        ISeedDataAccessor _accessor;
        Random _random = new Random(1);

        // Constructor Injection
        public SeedDataManager(ISeedDataAccessor accessor)
        {
            _accessor = accessor;
        }

        public void Add()
        {

            var sellerId = CreateSeller("santi", "Santi's Cars LLC"); // Seller ID == 1 -- used in test clients
            CreateCarProduct(CreateCatalog(sellerId, "Santi's Wacky Used Car Emporium"), "cars.json");
            CreateCarProduct(CreateCatalog(sellerId, "Santi's Wacky Used Car Overflow"), "cars2.json");

            CreateSeller("russ", "Russ's Movie Store"); // Seller ID == 2 -- used in test clients
            CreateSeller("kyle", "Kyle's Food"); // Seller ID == 3 -- used for no sales tests -- do not use in test clients

        }

        int CreateSeller(string username, string name)
        {
            return _accessor.CreateSeller(username, name);
        }

        int CreateCatalog(int sellerId, string catalogName)
        {
            return _accessor.CreateCatalog(sellerId, catalogName);
        }

        class Car
        {
            public int Id { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string Year { get; set; }
        }

        void CreateCarProduct(int catalogId, string carFile)
        {
            var text = "";
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (var r in resources)
            {
                if (r.Contains(carFile))
                {
                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(r))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            text = reader.ReadToEnd();
                        }
                    }
                }

            }
            var cars = Newtonsoft.Json.JsonConvert.DeserializeObject<Car[]>(text);

            foreach (var car in cars)
            {
                _accessor.CreateProduct(catalogId, $"{car.Year} {car.Make} {car.Model}", true, false, $"Used {car.Year} {car.Make} {car.Model}", "Used car from Santi's Used Car Emporium", car.Make, _random.Next(9000) + 1000);
            }
        }
    }
}
