﻿using Bogus;
using RealEstateApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace RealEstateApplication.Services
{
    public class DataSeedService
    {
        private readonly HomeContext _context;

        public DataSeedService(HomeContext context)
        {
            _context = context;
        }

        public void SeedHomes()
        {
            var homes = new List<Home>();

            var imageUrls = new List<string>
        {
            "https://images.unsplash.com/photo-1568605114967-8130f3a36994",
            "https://images.unsplash.com/photo-1570129477492-45c003edd2be",
            "https://images.unsplash.com/photo-1568605114967-8130f3a36994?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            "https://images.unsplash.com/photo-1580587771525-78b9dba3b914",
            "https://images.unsplash.com/photo-1613977257363-707ba9348227",
            "https://images.pexels.com/photos/106399/pexels-photo-106399.jpeg",
            "https://images.pexels.com/photos/9951999/pexels-photo-9951999.jpeg",
            "https://images.pexels.com/photos/5178060/pexels-photo-5178060.jpeg",
            "https://images.pexels.com/photos/8583869/pexels-photo-8583869.jpeg",
            "https://images.pexels.com/photos/7710011/pexels-photo-7710011.jpeg",
            "https://images.pexels.com/photos/5524205/pexels-photo-5524205.jpeg"
        };

            if (!_context.Homes.Any())
            {
                var faker = new Faker("en");

                for (int i = 1; i < 500; i++)
                {
                    var home = new Home
                    {
                        Id = i,
                        Price = faker.Finance.Amount(200000, 700000),
                        StreetAddress = faker.Address.StreetAddress(),
                        State = faker.Address.State(),
                        City = faker.Address.City(),
                        Area = faker.Random.Int(100, 200),
                        Bedrooms = faker.Random.Int(1, 5),
                        Bathrooms = faker.Random.Int(1, 5),
                        GarageSpots = faker.Random.Int(1, 5),
                        ImageUrl = imageUrls[i % imageUrls.Count]
                    };

                    homes.Add(home);
                }

                _context.Homes.AddRange(homes);
            }

            _context.SaveChanges();
        }
    }
}
