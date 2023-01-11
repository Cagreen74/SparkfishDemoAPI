using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using SparkFishDemo.Core.Managers.Sequence;
using SparkFishDemo.Utility.Authentication.App;
using SparkFishDemo.Utility.Authentication.User;
using System.Text;
using SparkFishDemo.Core.DI;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddScoped<JWTService, JWTService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.CoreServiceDependencies(builder.Configuration);
builder.Services.AddTransient<ISequenceManager, SequenceManager>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Dev"), b => b.MigrationsAssembly("SparkFishDemo.Resources")), ServiceLifetime.Transient);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
  options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddRoleManager<RoleManager<IdentityRole>>();
builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(7));

// Configure JWTOptions
var jwtOptions = new JWTOptions();
builder.Configuration.GetSection(nameof(JWTOptions)).Bind(jwtOptions);
var jwtKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Key));
builder.Services.Configure<JWTOptions>(options =>
{
  options.Issuer = jwtOptions.Issuer;
  options.Audience = jwtOptions.Audience;
  options.SigningCredentials = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256);
});
builder.Services.AddAuthentication(op =>
{
  op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
  o.SaveToken = true;
  o.TokenValidationParameters = new TokenValidationParameters()
  {
    ValidateIssuerSigningKey = true,
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidAudience = jwtOptions.Audience,
    ValidIssuer = jwtOptions.Issuer,
    IssuerSigningKey = jwtKey,
    ClockSkew = TimeSpan.Zero
  };

  o.Events = new JwtBearerEvents
  {
    OnAuthenticationFailed = context =>
    {
      if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
      {
        context.Response.Headers.Add("Token-Expired", "true");
      }
      return Task.CompletedTask;
    }
  };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allo

app.UseEndpoints(x => x.MapControllers());

app.Run();
