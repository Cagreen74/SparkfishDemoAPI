using Microsoft.AspNetCore.Identity;

namespace SparkFishDemo.Utility.Authentication.User
{
  public class ApplicationUser : IdentityUser
  {
    public string ? FirstName { get; set; }
    public string ? LastName { get; set; }
    public string ? RoleId { get; set; }
    public override bool  TwoFactorEnabled { get; set; } = false;
    public override bool EmailConfirmed { get; set; } = true;
    public DateTime? CreateDate { get; set; } = DateTime.Now;
  }
}
