using SparkFishDemo.Resources.DB;
using SparkFishDemo.Resources.Handlers.Sequence;
using SparkFishDemo.Utility.DTOs.Sequence;
using SparkFishDemo.Utility.HttpModels.Requests.Range;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkFishDemo.Core.Managers.Sequence
{
  public class SequenceManager : ISequenceManager
  {

    private ISequenceHandler sequenceHandler;

    public SequenceManager(ISequenceHandler _sequenceHandler)
    {
      sequenceHandler = _sequenceHandler;
    }




    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ActiveStatusDTO>> GetActiveStatuses()
    {
      var getStatuses = await sequenceHandler.GetActiveStatuses();
     
      return getStatuses.Select(x => new ActiveStatusDTO() { 
      ActiveStatusId = x.ActiveStatusId,
      ActiveStatusCode = x.ActiveStatusCode,
      ActiveStatusDescription = x.ActiveStatusDescription
      }).ToList();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<ICollection<DefaultRangeDTO>> GetDefaultRanges()
    {
      var getDefaultRanges = await sequenceHandler.GetDefaultRanges();

      return getDefaultRanges.Select(x => new DefaultRangeDTO()
      {
        RangeId =  x.RangeId,
        ThresholdEnd = x.ThresholdEnd,
        ThresholdStart = x.ThresholdStart,
        IsDefault = x.IsDefault,
        RangeDescription = $"({x.ThresholdStart} - {x.ThresholdEnd})"
      }).ToList();
    }



    /// <summary>
    /// Get the elements in the given list of supplied indexes
    /// </summary>
    /// <returns></returns>
    public ICollection<RangeElementDTO> GetRangeElements(ElementIndexesReq req)
    {
      ICollection<int> range = Enumerable.Range(req.Start, req.EnumerationCount).ToList();
      return req.ReturnIndexes
                 .Select(c => new RangeElementDTO()
                 {
                   ElementValue = range.ElementAtOrDefault(c),
                   RequestedIndex = c,
                   SearchDisplay = $"Searched {req.Start} to {req.End}",
                 }).ToList();
               
    }
  }
}
