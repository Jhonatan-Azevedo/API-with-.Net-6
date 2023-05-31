using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Data.Dtos;

public class UpdateMovieDto
{
    [Required(ErrorMessage = "O titulo do filme é obrigatório")]
    public string Title { get; set; }

    [Required(ErrorMessage = "O genero do filme é obrigatório")]
    [StringLength(50, ErrorMessage = "O tamanho do genero não pode exceder 50 caracteres")]
    public string Genre { get; set; }

    [Required]
    [Range(70, 600, ErrorMessage = "A duração deve ser entre 70 e 600 minutos")]
    public int Duration { get; set; }
}
