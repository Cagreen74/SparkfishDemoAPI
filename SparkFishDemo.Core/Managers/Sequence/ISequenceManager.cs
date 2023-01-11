using SparkFishDemo.Utility.DTOs.Sequence;
using SparkFishDemo.Utility.HttpModels.Requests.Range;

namespace SparkFishDemo.Core.Managers.Sequence
{
  public interface ISequenceManager
  {
    Task<IEnumerable<ActiveStatusDTO>> GetActiveStatuses();
    Task<ICollection<DefaultRangeDTO>> GetDefaultRanges();
    ICollection<RangeElementDTO> GetRangeElements(ElementIndexesReq req);
  }
}
