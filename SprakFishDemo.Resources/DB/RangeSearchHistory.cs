using System;
using System.Collections.Generic;

namespace SparkFishDemo.Resources.DB;

public partial class RangeSearchHistory
{
    public int SearchHistoryId { get; set; }

    public DateTime? SearchDate { get; set; }

    public int? DefaultRangeId { get; set; }

    public int? ActiveStatusId { get; set; }

    public int? RangeStart { get; set; }

    public int? RangeEnd { get; set; }

    public int? ReturnedValue { get; set; }

    public virtual ActiveStatus? ActiveStatus { get; set; }

    public virtual DefaultRange? DefaultRange { get; set; }
}
