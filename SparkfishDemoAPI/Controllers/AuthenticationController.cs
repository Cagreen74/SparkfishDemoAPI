using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SparkFishDemo.Utility.Authentication.App;
using SparkFishDemo.Utility.Authentication.User;
using SparkFishDemo.Utility.HttpModels.Requests.Authentication;

namespace SparkfishDemoAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthenticationController : ControllerBase
  {
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IConfiguration config;
    private readonly JWTService jwtService;

    public AuthenticationController(SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _identityUserManager, IConfiguration _configuration, JWTService _jwtService)
    {
      userManager = _identityUserManager;
      signInManager = _signInManager;
      jwtService = _jwtService;
      config = _configuration;
    }

    /// <summary>
    /// The initial Sign In Request Which Returns a token and refresh token
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [Route("SignIn")]
    [HttpPost]
    [Produces("application/json")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserLoginReq req)
    {

      object response = new object();
      var userAuthenticated = false;

      try
      {
        var signin = await signInManager.PasswordSignInAsync(req.UserName, req.Password, false, false);
        //If user successfully authenticates then we return a token
        //and a refresh token for the user to use
        //for subsequent requests
        userAuthenticated = signin.Succeeded;
        
        if (userAuthenticated)
        {
          var user = await userManager.FindByNameAsync(req.UserName);
          var token = await jwtService.GenerateEncodedToken(user.Id, user.UserName, user.RoleId);

          response = new {
            Security = token,
            User = new { FirstName = user.FirstName, LastName = user.LastName, Role = user.RoleId, Email = user.Email },
            IsAuthenticated = userAuthenticated,
          };
        }
        else
        {
          response = new { IsAuthenticated = userAuthenticated, Message = config["ErrorMessages:Authentication"] };
        }
       
      }
      catch (Exception ex)
      {
        //Would implement logging or a custom action filter to handle errors in the real world
      }

      return Ok(response);
    }
  }
}
