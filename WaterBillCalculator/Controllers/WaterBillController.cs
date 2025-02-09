using Microsoft.AspNetCore.Mvc;
using WaterBillCalculator.Data;
using WaterBillCalculator.Interfaces;
using WaterBillCalculator.Models;
using WaterBillCalculator.Services;

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

    [HttpGet("{billId}")]
    public ActionResult<WaterBillResponse> GetBillBreakdown(BillDetails billDetails)
    {
        var breakdown = _waterBillService.GetBillBreakdown(billDetails);
        if (breakdown == null)
        {
            return NotFound();
        }
        return Ok(breakdown);
    }
}