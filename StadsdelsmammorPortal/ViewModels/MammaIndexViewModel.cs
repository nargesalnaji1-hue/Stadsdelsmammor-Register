using Microsoft.AspNetCore.Mvc.Rendering;
using Stadsdelsmammor.Models;

namespace Stadsdelsmammor.ViewModels;

public class MammaIndexViewModel
{
    public IReadOnlyList<Mamma> Mammor { get; set; } = [];

    public string? Sok { get; set; }

    public string? Sprak { get; set; }

    public string? Omrade { get; set; }

    public MammaStatus? Status { get; set; }

    public List<SelectListItem> TillgangligaSprak { get; set; } = [];

    public List<SelectListItem> TillgangligaOmraden { get; set; } = [];

    public List<SelectListItem> TillgangligaStatusar { get; set; } = [];
}
