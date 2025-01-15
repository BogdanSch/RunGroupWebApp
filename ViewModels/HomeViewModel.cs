using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Models.Club> Clubs { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
}
