using Microsoft.AspNetCore.Identity;
using SparkFishDemo.Utility.Authentication.User;

namespace SparkFishDemo.Utility.Authentication.Common
{
  public class Util
  {
    public string HashPw(ApplicationUser newUser, string password)
    {
      var passwordHasher = new PasswordHasher<ApplicationUser>();
      var hashedPassWord = passwordHasher.HashPassword(newUser, password);
      return hashedPassWord;
    }
  }
}
