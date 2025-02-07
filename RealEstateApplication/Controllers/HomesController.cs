using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstateApplication.Models;
using RealEstateApplication.Services;
using System.Diagnostics;

namespace RealEstateApplication.Controllers
{

    [Authorize]
    public class HomesController: Controller
    {
        private readonly HomeService _homeService;
        private readonly AddressService _addressService;
        private readonly ILogger<HomesController> _logger;
        public HomesController(ILogger<HomesController> logger, HomeService homeService, AddressService addressService)
        {
            _logger = logger;
            _homeService = homeService;
            _addressService = addressService;   

        }

        [HttpPost]
        public async Task<IActionResult> GetCities(string state)
        {
            var cities = await _addressService.GetCitiesInState(state);
            return Ok(cities);
        }

        [HttpGet]
        public async Task<IActionResult> AddHomeView()
        {
            var statesResult = await _addressService.GetAmericanStates();

            var addHomeViewModel = new AddHomeViewModel
            {
                States = statesResult,
                Cities = new List<string>()
            };

            return View(addHomeViewModel);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetStates()
        //{
        //    var states = await _addressService.GetAmericanStates();
        //    return Ok(states);
        //}

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? minPrice, int? maxPrice, int? minArea, int? maxArea, int pageNumber = 1, int pageSize = 10)
        {


            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tasks = new List<Task<List<string>>>();

            //for (int i = 0; i < 10; i++)
            //{
            //    tasks.Add(_addressService.GetStatesWithDelayAsync(i));
            //}

            //var results = await Task.WhenAll(tasks);

            var homesViewModel = new HomesViewModel();

            try
            {
                var homes = _homeService.GetHomes();

                if (minPrice.HasValue)
                {
                    homes = homes.Where(h => h.Price >= minPrice).ToList();
                }
                if (maxPrice.HasValue)
                {
                    homes = homes.Where(h => h.Price <= maxPrice).ToList();
                }
                if (minArea.HasValue)
                {
                    homes = homes.Where(h => h.Area >= minArea).ToList();
                }
                if (maxArea.HasValue)
                {
                    homes = homes.Where(h => h.Area <= maxArea).ToList();
                }

                int totalItems = homes.Count();
                homes = homes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                homesViewModel.PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                };

                homesViewModel.Homes = homes;
                ViewBag.HomesCount = totalItems;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error fetching home from the database: {ex.Message}";
            }

            homesViewModel.MinPrice = minPrice;
            homesViewModel.MaxPrice = maxPrice;
            homesViewModel.MinArea = minArea;
            homesViewModel.MaxArea = maxArea;

            stopwatch.Stop();

            ViewBag.LoadTestTime = stopwatch.Elapsed.TotalSeconds.ToString("F4");
            return View(homesViewModel);
        }



        [HttpPost]
        public IActionResult AddHome(Home newHome)
        {
            if (!ModelState.IsValid)
            {
                return View("AddHomeView", newHome);
            }

            try
            {
                _homeService.AddHome(newHome);
                TempData["SuccessMessage"] = "Home added successfully!";
                return RedirectToAction("Index", "Homes");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding home: {ex.Message}";
                return View("AddHomeView", newHome);
            }
        }

        [HttpGet]
        public IActionResult HomeDetailView(int id)
        {
            var home = _homeService.GetHomeById(id);
            return View(home);
        }

        [HttpPost]
        public IActionResult Update(Home updatedHome)
        {
            if (!ModelState.IsValid)
            {
                return View("HomeDetailView", updatedHome);
            }

            try
            {
                _homeService.UpdateHome(updatedHome);
                TempData["SuccessMessage"] = "Home updated successfully!";
                return RedirectToAction("Index", "Homes");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating home: {ex.Message}";
                return View("HomeDetailView", updatedHome);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _homeService.DeleteHome(id);
                TempData["SuccessMessage"] = "Home deleted successfully!";
                return new OkResult();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting home: {ex.Message}";
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
