using RunGroupWebApp.Data.Enums;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels;

public class EditRaceViewModel
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int AddressId { get; set; }
    public required Address Address { get; set; }
    public IFormFile Image { get; set; }
    public required string URL { get; set; }
    public required RaceCategory RaceCategory { get; set; }
}
