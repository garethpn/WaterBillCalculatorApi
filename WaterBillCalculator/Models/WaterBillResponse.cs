namespace WaterBillCalculator.Models;

public class WaterBillResponse
{
    public List<MeterReadingResponse> MeterReadings { get; set; } = new();
    public decimal Remainder { get; set; }
}