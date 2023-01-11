using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SparkFishDemo.Resources.Handlers.Sequence;

namespace SparkFishDemo.Core.DI
{
  public static class CoreInjection
  {
    public static IServiceCollection CoreServiceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddTransient<ISequenceHandler, SequenceHandler>();
      services.AddTransient(x => configuration);
      return services;
    }
  }
}
