using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels;

public class DashboardViewModel
{
    public DashboardViewModel() : this(new List<Race>(), new List<Club>()) {}

    public DashboardViewModel(List<Race> races, List<Club> clubs)
    {
        Races = races;
        Clubs = clubs;
    }


    public List<Race> Races { get; set; }
    public List<Club> Clubs { get; set; }
}
