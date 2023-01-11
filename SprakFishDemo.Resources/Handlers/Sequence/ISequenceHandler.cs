using SparkFishDemo.Resources.DB;

namespace SparkFishDemo.Resources.Handlers.Sequence
{
  public interface ISequenceHandler
  {
    Task<IEnumerable<ActiveStatus>> GetActiveStatuses();
    Task<IEnumerable<DefaultRange>> GetDefaultRanges();
  }
}
