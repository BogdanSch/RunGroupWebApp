using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels.Races;

namespace RunGroupWebApp.Controllers;
public class RaceController : Controller
{
    private readonly IRaceRepository _raceRepository;
    private readonly IPhotoService _photoService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
    {
        _raceRepository = raceRepository;
        _photoService = photoService;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IActionResult> Index()
    {
        IEnumerable<Race> races = await _raceRepository.GetAll();
        return View(races);
    }
    public async Task<IActionResult> Detail(int id)
    {
        Race race = await _raceRepository.GetByIdAsync(id);
        return View(race);
    }
    public IActionResult Create()
    {

        string currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();
        CreateRaceViewModel createRaceViewModel = new CreateRaceViewModel()
        {
            AppUserId = currentUserId
        };
        return View(createRaceViewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateRaceViewModel raceViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(raceViewModel.Image);
            Race race = new()
            {
                Title = raceViewModel.Title,
                Description = raceViewModel.Description,
                Image = result.Url.ToString(),
                Address = new Address()
                {
                    Street = raceViewModel.Address.Street,
                    City = raceViewModel.Address.City,
                    Region = raceViewModel.Address.Region,
                },
                RaceCategory = raceViewModel.RaceCategory,
                AppUserId = raceViewModel.AppUserId,
            };
            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
        ModelState.AddModelError("error-photo", "Photo upload failed!");
        return View(raceViewModel);
    }

    public async Task<IActionResult> Edit(int id)
    {
        Race race = await _raceRepository.GetByIdAsync(id);

        if (race == null)
            return View("Error");

        EditRaceViewModel raceViewModel = new()
        {
            Title = race.Title,
            Description = race.Description,
            AddressId = race.AddressId,
            Address = race.Address,
            URL = race.Image,
            RaceCategory = race.RaceCategory
            //Address = new Address()
            //{
            //    Street = clubViewModel.Address.Street,
            //    City = clubViewModel.Address.City,
            //    Region = clubViewModel.Address.Region,
            //},
        };
        return View(raceViewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditRaceViewModel raceViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit the club!");
            return View("Edit", raceViewModel);
        }

        Race userRace = await _raceRepository.GetByIdAsyncNoTracking(id);

        if (userRace == null)
        {
            return View("Error");
        }

        try
        {
            await _photoService.DeletePhotoAsync(raceViewModel.URL);
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "Could not delete the photo!");
            return View(raceViewModel);
        }

        ImageUploadResult photoResult = await _photoService.AddPhotoAsync(raceViewModel.Image);
        Race updatedRace = new Race()
        {
            Id = id,
            Title = raceViewModel.Title,
            Description = raceViewModel.Description,
            Image = photoResult.Url.ToString(),
            AddressId = raceViewModel.AddressId,
            Address = raceViewModel.Address,
            RaceCategory = raceViewModel.RaceCategory,
        };

        _raceRepository.Update(updatedRace);
        return RedirectToAction("Index");
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            Race userRace = await _raceRepository.GetByIdAsyncNoTracking(id);

            if (userRace == null)
            {
                TempData["Error"] = "Race not found.";
                return RedirectToAction("Index");
            }

            _raceRepository.Delete(userRace);
            TempData["Success"] = "Race deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"An error occurred while deleting the race: {ex.Message}";
        }

        return RedirectToAction("Index");
    }
}
