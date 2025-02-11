using WaterBillCalculator.Data;
using WaterBillCalculator.Models;

namespace WaterBillCalculator.Interfaces;

public interface IWaterBillService
{
    WaterBillResponse GetBillBreakdown(BillDetails billDetails);
    IEnumerable<MeterDetails> GetAllMeterDetails();
}