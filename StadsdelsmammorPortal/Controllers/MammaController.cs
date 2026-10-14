using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stadsdelsmammor.Data;
using Stadsdelsmammor.Models;
using Stadsdelsmammor.ViewModels;

namespace Stadsdelsmammor.Controllers;

[Authorize(Roles = "Admin")]
public class MammaController : Controller
{
    private readonly ApplicationDbContext _context;

    public MammaController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string? sok,
        string? sprak,
        string? omrade,
        MammaStatus? status)
    {
        var query = _context.Mammor
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(sok))
        {
            var sokText = sok.Trim();

            query = query.Where(m =>
                m.Fornamn.Contains(sokText) ||
                m.Efternamn.Contains(sokText) ||
                m.Telefonnummer.Contains(sokText));
        }

        if (!string.IsNullOrWhiteSpace(sprak))
        {
            query = query.Where(m => m.Sprak == sprak);
        }

        if (!string.IsNullOrWhiteSpace(omrade))
        {
            query = query.Where(m => m.Omrade == omrade);
        }

        if (status.HasValue)
        {
            query = query.Where(m => m.Status == status.Value);
        }

        var mammor = await query
            .OrderBy(m => m.Efternamn)
            .ThenBy(m => m.Fornamn)
            .ToListAsync();

        var model = new MammaIndexViewModel
        {
            Mammor = mammor,
            Sok = sok,
            Sprak = sprak,
            Omrade = omrade,
            Status = status,
            TillgangligaSprak = await GetSprakOptionsAsync(sprak),
            TillgangligaOmraden = await GetOmradeOptionsAsync(omrade),
            TillgangligaStatusar = GetStatusOptions(status)
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var mamma = await _context.Mammor
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mamma is null)
        {
            return NotFound();
        }

        return View(mamma);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(CreateFormModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MammaFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.TillgangligaStatusar =
                GetStatusOptions(model.Status);

            return View(model);
        }

        var mamma = new Mamma
        {
            Fornamn = model.Fornamn.Trim(),
            Efternamn = model.Efternamn.Trim(),
            Telefonnummer = model.Telefonnummer.Trim(),
            Omrade = model.Omrade.Trim(),
            Sprak = model.Sprak.Trim(),
            Status = model.Status,
            Skapad = DateTime.UtcNow
        };

        _context.Mammor.Add(mamma);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Medlemmen skapades.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var mamma = await _context.Mammor
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mamma is null)
        {
            return NotFound();
        }

        return View(ToFormModel(mamma));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        MammaFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            model.TillgangligaStatusar =
                GetStatusOptions(model.Status);

            return View(model);
        }

        var mamma = await _context.Mammor
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mamma is null)
        {
            return NotFound();
        }

        mamma.Fornamn = model.Fornamn.Trim();
        mamma.Efternamn = model.Efternamn.Trim();
        mamma.Telefonnummer = model.Telefonnummer.Trim();
        mamma.Omrade = model.Omrade.Trim();
        mamma.Sprak = model.Sprak.Trim();
        mamma.Status = model.Status;
        mamma.SenastUppdaterad = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Medlemmen uppdaterades.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var mamma = await _context.Mammor
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mamma is null)
        {
            return NotFound();
        }

        return View(mamma);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var mamma = await _context.Mammor
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mamma is null)
        {
            return NotFound();
        }

        _context.Mammor.Remove(mamma);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Medlemmen togs bort.";

        return RedirectToAction(nameof(Index));
    }

    private MammaFormViewModel CreateFormModel()
    {
        return new MammaFormViewModel
        {
            Status = MammaStatus.Aktiv,
            TillgangligaStatusar = GetStatusOptions(
                MammaStatus.Aktiv)
        };
    }

    private static MammaFormViewModel ToFormModel(Mamma mamma)
    {
        return new MammaFormViewModel
        {
            Id = mamma.Id,
            Fornamn = mamma.Fornamn,
            Efternamn = mamma.Efternamn,
            Telefonnummer = mamma.Telefonnummer,
            Omrade = mamma.Omrade,
            Sprak = mamma.Sprak,
            Status = mamma.Status,
            TillgangligaStatusar = GetStatusOptions(
                mamma.Status)
        };
    }

    private async Task<List<SelectListItem>> GetSprakOptionsAsync(
        string? selectedValue)
    {
        var values = await _context.Mammor
            .AsNoTracking()
            .Select(m => m.Sprak)
            .Distinct()
            .OrderBy(value => value)
            .ToListAsync();

        return values
            .Select(value => new SelectListItem
            {
                Text = value,
                Value = value,
                Selected = value == selectedValue
            })
            .ToList();
    }

    private async Task<List<SelectListItem>> GetOmradeOptionsAsync(
        string? selectedValue)
    {
        var values = await _context.Mammor
            .AsNoTracking()
            .Select(m => m.Omrade)
            .Distinct()
            .OrderBy(value => value)
            .ToListAsync();

        return values
            .Select(value => new SelectListItem
            {
                Text = value,
                Value = value,
                Selected = value == selectedValue
            })
            .ToList();
    }

    private static List<SelectListItem> GetStatusOptions(
        MammaStatus? selectedValue)
    {
        return Enum.GetValues<MammaStatus>()
            .Select(status => new SelectListItem
            {
                Text = status == MammaStatus.Aktiv
                    ? "Aktiv"
                    : "Pausad",
                Value = status.ToString(),
                Selected = selectedValue == status
            })
            .ToList();
    }
}
