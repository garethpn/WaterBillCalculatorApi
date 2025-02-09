namespace WaterBillCalculator.Models;

public class MeterReadingResponse
{
    public string MeterName { get; set; }
    public decimal? CalculatedBillShare { get; set; }
}