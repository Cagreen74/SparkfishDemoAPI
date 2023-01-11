using System;
using System.Collections.Generic;

namespace SparkFishDemo.Resources.DB;

public partial class ActiveStatus
{
    public int ActiveStatusId { get; set; }

    public string? ActiveStatusCode { get; set; }

    public string? ActiveStatusDescription { get; set; }

    public virtual ICollection<RangeSearchHistory> RangeSearchHistories { get; } = new List<RangeSearchHistory>();
}
