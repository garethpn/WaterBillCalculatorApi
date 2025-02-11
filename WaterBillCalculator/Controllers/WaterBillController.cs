using Microsoft.AspNetCore.Mvc;
using WaterBillCalculator.Data;
using WaterBillCalculator.Interfaces;
using WaterBillCalculator.Models;

namespace WaterBillCalculator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WaterBillController : ControllerBase
{
    private readonly IWaterBillService _waterBillService;

    public WaterBillController(IWaterBillService waterBillService)
    {
        _waterBillService = waterBillService;
    }

    /// <summary>
    /// Gets the meter details.
    /// </summary>
    /// <returns>All meter details</returns>
    [HttpGet("MeterDetails")]
    public ActionResult<IEnumerable<MeterDetails>> GetMeterDetails()
    {
        var meterDetails = _waterBillService.GetAllMeterDetails();
        return Ok(meterDetails);
    }
    
    /// <summary>
    /// Gets the bill breakdown.
    /// </summary>
    /// <param name="billDetails">The bill details.</param>
    /// <returns>The water bill response.</returns>
    [HttpGet("GetBillBreakdown")]
    public ActionResult<WaterBillResponse> GetBillBreakdown([FromBody] BillDetails billDetails)
    {
        var breakdown = _waterBillService.GetBillBreakdown(billDetails);
        return Ok(breakdown);
    }
}