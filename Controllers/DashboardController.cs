using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;
using RunGroupWebApp.ViewModels.Users;
using CloudinaryDotNet.Actions;

namespace RunGroupWebApp.Controllers;
public class DashboardController : Controller
{
    private readonly IDashboardRepository _dasboardRepository;
    public DashboardController(IDashboardRepository dasboardRepository)
    {
        _dasboardRepository = dasboardRepository;
    }

    public IActionResult Index()
    {
        List<Race> userRaces = _dasboardRepository.GetAllUserRaces();
        List<Club> userClubs = _dasboardRepository.GetAllUserClubs();

        DashboardViewModel dashboardViewModel = new DashboardViewModel(userRaces, userClubs);
        return View(dashboardViewModel);
    }
}
