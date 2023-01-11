using System;
using System.Collections.Generic;

namespace SparkFishDemo.Resources.DB;

public partial class DefaultRange
{
    public int RangeId { get; set; }

    public int? ThresholdStart { get; set; }

    public int? ThresholdEnd { get; set; }

    public bool? IsDefault { get; set; }

    public virtual ICollection<RangeSearchHistory> RangeSearchHistories { get; } = new List<RangeSearchHistory>();
}
