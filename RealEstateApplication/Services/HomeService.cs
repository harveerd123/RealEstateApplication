﻿using Bogus;
using RealEstateApplication.Models;

namespace RealEstateApplication.Services
{
    public class HomeService
    {
        private readonly HomeContext _context;

        public HomeService(HomeContext context)
        {
            _context = context;
            


        }

        public List<Home> GetHomes()
        {
            return _context.Homes.ToList();
        }

        public void AddHome(Home home)
        {
            _context.Homes.Add(home);
            _context.SaveChanges();
        }

        public Home GetHomeById(int id)
        {
            return _context.Homes.Single(x =>  x.Id == id);
        }

        public void DeleteHome(int id)
        {
            var home = _context.Homes.FirstOrDefault(x => x.Id == id);
            _context.Homes.Remove(home);
            _context.SaveChanges(); 
        }

        public void UpdateHome(Home updatedHome)
        {
            var home = _context.Homes.FirstOrDefault(x => x.Id == updatedHome.Id);

            home.Price = updatedHome.Price;
            home.StreetAddress = updatedHome.StreetAddress;
            home.Area = updatedHome.Area;
            home.Bathrooms = updatedHome.Bathrooms;
            home.Bedrooms = updatedHome.Bedrooms;
            home.GarageSpots = updatedHome.GarageSpots;

            _context.Homes.Update(home);
            _context.SaveChanges();
        }
    }
}
