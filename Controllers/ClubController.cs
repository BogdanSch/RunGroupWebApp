using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Services;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers;
public class ClubController : Controller
{
    private readonly IClubRepository _clubRepository;
    private readonly IPhotoService _photoService;
    public ClubController(IClubRepository clubRepository, IPhotoService photoService) 
    {
        _clubRepository = clubRepository;
        _photoService = photoService;
    }
    public async Task<IActionResult> Index()
    {
        IEnumerable<Club> clubs = await _clubRepository.GetAll();
        return View(clubs);
    }
    public async Task<IActionResult> Detail(int id)
    {
        Club club = await _clubRepository.GetByIdAsync(id);
        return View(club);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateClubViewModel clubViewModel)
    {
        if (ModelState.IsValid)
        {
            ImageUploadResult result = await _photoService.AddPhotoAsync(clubViewModel.Image);
            Club club = new()
            {
                Title = clubViewModel.Title,
                Description = clubViewModel.Description,
                Image = result.Url.ToString(),
                Address = clubViewModel.Address,
                //Address = new Address()
                //{
                //    Street = clubViewModel.Address.Street,
                //    City = clubViewModel.Address.City,
                //    Region = clubViewModel.Address.Region,
                //},
                ClubCategory = clubViewModel.ClubCategory
            };
            _clubRepository.Add(club);
            return RedirectToAction("Index");
        }
        ModelState.AddModelError("error-photo", "Photo upload failed!");
        return View(clubViewModel);
    }
    public async Task<IActionResult> Edit(int id)
    {
        Club club = await _clubRepository.GetByIdAsync(id);

        if (club == null)
            return View("Error");

        EditClubViewModel clubViewModel = new()
        {
            Title = club.Title,
            Description = club.Description,
            AddressId = club.AddressId,
            Address = club.Address,
            URL = club.Image,
            ClubCategory = club.ClubCategory
            //Address = new Address()
            //{
            //    Street = clubViewModel.Address.Street,
            //    City = clubViewModel.Address.City,
            //    Region = clubViewModel.Address.Region,
            //},
        };
        return View(clubViewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditClubViewModel clubViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit the club!");
            return View("Edit", clubViewModel);
        }

        Club userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

        if (userClub == null)
        {
            return View("Error");
        }
        
        try
        {
            await _photoService.DeletePhotoAsync(clubViewModel.URL);
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "Could not delete the photo!");
            return View(clubViewModel);
        }

        ImageUploadResult photoResult = await _photoService.AddPhotoAsync(clubViewModel.Image);
        Club updatedClub = new Club()
        {
            Id = id,
            Title = clubViewModel.Title,
            Description = clubViewModel.Description,
            Image = photoResult.Url.ToString(),
            AddressId = clubViewModel.AddressId,
            Address = clubViewModel.Address,
            ClubCategory = clubViewModel.ClubCategory,
        };

        _clubRepository.Update(updatedClub);
        return RedirectToAction("Index");
    }
}
