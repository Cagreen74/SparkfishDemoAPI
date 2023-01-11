using Microsoft.EntityFrameworkCore;
using SparkFishDemo.Resources.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkFishDemo.Resources.Handlers.Sequence
{
  public class SequenceHandler : DbBase, ISequenceHandler
  {

    /// <summary>
    /// Get the look up table for Active Statuses
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ActiveStatus>> GetActiveStatuses()
    {
      return await _db.ActiveStatuses.AsNoTracking()
                                     .OrderBy(status => status.ActiveStatusDescription)
                                     .ToListAsync();
    }


    /// <summary>
    /// Get the look up table for Active Statuses
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<DefaultRange>> GetDefaultRanges()
    {
      return await _db.DefaultRanges.AsNoTracking()
                                     .OrderBy(range => range.ThresholdStart)
                                     .ToListAsync();
    }
  }
}
