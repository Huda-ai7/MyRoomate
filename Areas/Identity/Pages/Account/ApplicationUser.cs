using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using FindARoomate.Areas.Identity.Pages.Account;
public class ApplicationUser : IdentityUser
{
    public int CityId { get; set; }
    [ForeignKey("CityId")]
    public City cities { get; set; }

    public int GanderId { get; set; }
    [ForeignKey("GanderId")]
    public Gander ganders { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}