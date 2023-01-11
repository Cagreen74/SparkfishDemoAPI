using Microsoft.AspNetCore.Mvc;
using SparkFishDemo.Core.Managers.Sequence;
using SparkFishDemo.Utility.DTOs.Sequence;
using SparkFishDemo.Utility.HttpModels.Requests.Range;

namespace SparkfishDemoAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RangeController : ControllerBase
  {

    ISequenceManager sequenceManager;

    public RangeController(ISequenceManager _sequenceManager)
    {
      sequenceManager = _sequenceManager;
    }

    [Route("RangeDefaults")]
    [HttpGet]
    public async Task<ICollection<DefaultRangeDTO>> Get()
    {
      var getDefaultRanges = await sequenceManager.GetDefaultRanges();
      return getDefaultRanges;
    }


    [Route("listify")]
    [HttpPost]
    public ICollection<RangeElementDTO> GetRangeElements(ElementIndexesReq req)
    {
      return sequenceManager.GetRangeElements(req);
    }
  }
}
