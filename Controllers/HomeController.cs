using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunGroupWebApp.Helpers;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;
using System.Diagnostics;
using System.Globalization;

namespace RunGroupWebApp.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IClubRepository _clubRepository;

    public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository)
    {
        _logger = logger;
        _clubRepository = clubRepository;
    }

    public async void MapHomeViewModel(HomeViewModel homeViewModel, IPInfo ipInfo)
    {
        homeViewModel.City = ipInfo.City;
        homeViewModel.Region = ipInfo.Region;

        if (!string.IsNullOrWhiteSpace(homeViewModel.City)) 
        {
            homeViewModel.Clubs = await _clubRepository.GetByCity(homeViewModel.City);
            return;
        }
        homeViewModel.Clubs = new List<Club>();
    }

    public async Task<IActionResult> Index()
    {
        IPInfo ipInfo = new IPInfo();

        HomeViewModel homeViewModel = new HomeViewModel();

        try
        {
            string url = "https://ipinfo.io/json?token=b570a81b67a718";
            string response = await new HttpClient().GetStringAsync(url);

            ipInfo = JsonConvert.DeserializeObject<IPInfo>(response)!;

            RegionInfo regionInfo = new RegionInfo(ipInfo.Country);
            ipInfo.Country = regionInfo.EnglishName;

            MapHomeViewModel(homeViewModel, ipInfo);

            return View(homeViewModel);
        }
        catch(Exception)
        {
            homeViewModel.Clubs = new List<Club>();
        }

        return View(homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
