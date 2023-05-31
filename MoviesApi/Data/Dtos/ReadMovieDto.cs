using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Data.Dtos;

public class ReadMovieDto
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public int Duration { get; set; }
    public DateTime AppointmentTime { get; set; } = DateTime.Now;
}
