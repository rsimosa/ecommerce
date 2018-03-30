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
            // seed data comes from 
            // https://mockaroo.com/

            var sellerId = CreateSeller("santi", "Santi's Cars LLC"); 
            CreateCarProducts(CreateCatalog(sellerId, "Santi's Wacky Used Car Emporium"), "cars.json");
            CreateCarProducts(CreateCatalog(sellerId, "Santi's Wacky Used Car Overflow"), "cars2.json");

            CreateMovieProducts(CreateSeller("russ", "Movie Shack of Russ")); 
            CreateSeller("kyle", "Kyle's Food"); 

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

        void CreateCarProducts(int catalogId, string carFile)
        {
            var cars = Newtonsoft.Json.JsonConvert.DeserializeObject<Car[]>(LoadText(carFile));

            foreach (var car in cars)
            {
                _accessor.CreateProduct(catalogId, $"{car.Year} {car.Make} {car.Model}", true, false, $"Used {car.Year} {car.Make} {car.Model}", "Used car from Santi's Used Car Emporium", car.Make, _random.Next(9000) + 1000);
            }
        }

        class Movie
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Genre { get; set; }
        }


        void CreateMovieProducts(int catalogId)
        {
            var movies = Newtonsoft.Json.JsonConvert.DeserializeObject<Movie[]>(LoadText("movies.json"));

            foreach (var movie in movies)
            {
                _accessor.CreateProduct(catalogId, movie.Title, true, false, movie.Title, movie.Title, "Russ", _random.Next(19) + 1);
            }
        }


        string LoadText(string filename)
        {
            var text = "";
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (var r in resources)
            {
                if (r.Contains(filename))
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
            return text;
        }

    }
}
