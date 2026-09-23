using System.ComponentModel.DataAnnotations;

namespace Stadsdelsmammor.Models;

public enum MammaStatus
{
    [Display(Name = "Aktiv")]
    Aktiv,

    [Display(Name = "Pausad")]
    Pausad
}

public class Mamma
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Förnamn måste anges.")]
    [StringLength(100)]
    [Display(Name = "Förnamn")]
    public string Fornamn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Efternamn måste anges.")]
    [StringLength(100)]
    [Display(Name = "Efternamn")]
    public string Efternamn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefonnummer måste anges.")]
    [StringLength(50)]
    [Display(Name = "Telefonnummer")]
    public string Telefonnummer { get; set; } = string.Empty;

    [Required(ErrorMessage = "Område måste anges.")]
    [StringLength(100)]
    [Display(Name = "Område")]
    public string Omrade { get; set; } = string.Empty;

    [Required(ErrorMessage = "Språk måste anges.")]
    [StringLength(100)]
    [Display(Name = "Språk")]
    public string Sprak { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Status")]
    public MammaStatus Status { get; set; } = MammaStatus.Aktiv;

    [Display(Name = "Skapad")]
    public DateTime Skapad { get; set; } = DateTime.UtcNow;

    [Display(Name = "Senast uppdaterad")]
    public DateTime? SenastUppdaterad { get; set; }
}
