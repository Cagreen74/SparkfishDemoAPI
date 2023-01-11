namespace SparkFishDemo.Utility.DTOs.Sequence
{
  public class DefaultRangeDTO
  {
    public int RangeId { get; set; }
    public int? ThresholdStart { get; set; }
    public int? ThresholdEnd { get; set; }
    public bool? IsDefault { get; set; }
    public string RangeDescription { get; set; }
  }
}
