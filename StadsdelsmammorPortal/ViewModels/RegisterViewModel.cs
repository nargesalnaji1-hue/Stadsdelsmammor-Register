using System.ComponentModel.DataAnnotations;

namespace Stadsdelsmammor.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "E-postadress måste anges.")]
    [EmailAddress(ErrorMessage = "Ange en giltig e-postadress.")]
    [Display(Name = "E-postadress")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lösenord måste anges.")]
    [DataType(DataType.Password)]
    [Display(Name = "Lösenord")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Kom ihåg mig")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "E-postadress måste anges.")]
    [EmailAddress(ErrorMessage = "Ange en giltig e-postadress.")]
    [Display(Name = "E-postadress")]
    public string Email { get; set; } = string.Empty;

    [StringLength(150)]
    [Display(Name = "Fullständigt namn")]
    public string? FullName { get; set; }

    [Required(ErrorMessage = "Lösenord måste anges.")]
    [StringLength(
        100,
        MinimumLength = 8,
        ErrorMessage = "Lösenordet måste innehålla minst 8 tecken.")]
    [DataType(DataType.Password)]
    [Display(Name = "Lösenord")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bekräfta lösenordet.")]
    [Compare(
        nameof(Password),
        ErrorMessage = "Lösenorden matchar inte.")]
    [DataType(DataType.Password)]
    [Display(Name = "Bekräfta lösenord")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
