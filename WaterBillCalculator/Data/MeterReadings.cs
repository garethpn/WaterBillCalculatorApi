using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace WaterBillCalculator.Data;

[PrimaryKey("Id")]
public class MeterReadings
{
    public int Id { get; set; }
    public DateTime ReadingDate { get; set; }
    public decimal? PreviousReading { get; set; }
    public decimal Reading { get; set; }
    public int MeterId { get; set; }
    public int BillId { get; set; }
    public decimal? CalculatedBillShare { get; set; }
    
    // Navigation property
    [JsonIgnore]
    public MeterDetails Meter { get; set; }
}