using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace WaterBillCalculator.Data;

[PrimaryKey("Id")]
public class BillDetails
{
    public int Id { get; set; }
    
    public DateTime BillDate { get; set; }
    public decimal StandingCharge { get; set; }
    public decimal? PreviousReading { get; set; }
    public decimal CurrentReading { get; set; }
    public decimal? BilledUnits { get; set; }
    public decimal BilledAmount { get; set; }
    public decimal UnitPrice { get; set; }
    
    // Navigation property
    public required ICollection<MeterReadings> MeterReadings { get; set; } = new List<MeterReadings>();
}