namespace SparkFishDemo.Utility.Authentication.App
{
  public class TokenResponse
    {
      public string Token { get; set; }
      public string RefreshToken { get; set; }
      public int ExpiresOn { get; set; }

      public TokenResponse(string accessToken, int expiresOn)
      {
        Token = accessToken;
        ExpiresOn = expiresOn;
      }
    }
}
