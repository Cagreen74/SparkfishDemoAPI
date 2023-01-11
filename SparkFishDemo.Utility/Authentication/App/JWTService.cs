using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace SparkFishDemo.Utility.Authentication.App
{
  public class JWTService
  {
    private JWTOptions _jwtOptions;

    public JWTService(IOptions<JWTOptions> jwtOptions)
    {
      _jwtOptions = jwtOptions.Value;
      ThrowIfInvalidOptions(_jwtOptions);
    }
    public string GenerateTokenWithRandomNumber(int size = 32)
    {
      var randomNumber = new byte[size];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
      }
    }
    public async Task<TokenResponse> GenerateEncodedToken(string userId, string username, string? roleId)
    {
      var identity = GenerateClaimsIdentity(userId, username, roleId);
      var claims = new List<Claim>()
            {
                 new Claim(JwtRegisteredClaimNames.Sub, username),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(ClaimConstants.CLAIM_ROLE),
                 identity.FindFirst(ClaimConstants.CLAIM_ID)
             };
      // Create the JWT security token and encode it.

      var jwt = new JwtSecurityToken(
          _jwtOptions.Issuer,
          _jwtOptions.Audience,
          claims,
          _jwtOptions.NotBefore,
          _jwtOptions.Expiration,
          _jwtOptions.SigningCredentials);
      return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(jwt), (int)_jwtOptions.ValidFor.TotalSeconds);
    }

    private static ClaimsIdentity GenerateClaimsIdentity(string id, string username, string? roleId) => new ClaimsIdentity(new GenericIdentity(username, "Token"), new[]
        {
                new Claim(ClaimConstants.CLAIM_ID, id.ToString()),
                new Claim(ClaimConstants.CLAIM_ROLE, roleId ?? String.Empty)
            });

    /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
    private static long ToUnixEpochDate(DateTime date)
      => (long)Math.Round((date.ToUniversalTime() -
                           new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                          .TotalSeconds);

    private static void ThrowIfInvalidOptions(JWTOptions options)
    {
      if (options == null) throw new ArgumentNullException(nameof(options));

      if (options.ValidFor <= TimeSpan.Zero)
      {
        throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JWTOptions.ValidFor));
      }

      //if (options.SigningCredentials == null)
      //{
      //    throw new ArgumentNullException(nameof(JWTIssuerOptions.SigningCredentials));
      //}

      if (options.JtiGenerator == null)
      {
        throw new ArgumentNullException(nameof(JWTOptions.JtiGenerator));
      }
    }
  }
}
