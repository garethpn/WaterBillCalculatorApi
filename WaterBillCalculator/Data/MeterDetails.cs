namespace WaterBillCalculator.Data;

public class MeterDetails
{
    public int Id { get; set; }
    public string? MeterNumber { get; set; }
    public string MeterLocation { get; set; }
    public string MeterName { get; set; }
    public int? ParentId { get; set; }
    
    // Navigation property
    public MeterDetails ParentMeter { get; set; }
    public ICollection<MeterDetails> ChildMeters { get; set; }
    public ICollection<MeterReadings> MeterReadings { get; set; }
}