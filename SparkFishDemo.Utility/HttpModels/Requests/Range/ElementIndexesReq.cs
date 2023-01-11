namespace SparkFishDemo.Utility.HttpModels.Requests.Range
{
  public class ElementIndexesReq
  {
    public int Start { get; set; }
    public int End { get; set; }
    public int EnumerationCount { get; set; }
    public IEnumerable<int> ReturnIndexes { get; set; }
  }
}
