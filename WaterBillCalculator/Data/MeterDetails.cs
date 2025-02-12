using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace WaterBillCalculator.Data;

[PrimaryKey("Id")]
public class MeterDetails
{
    public int Id { get; set; }
    public string? MeterNumber { get; set; }
    public string MeterLocation { get; set; }
    public string MeterName { get; set; }
    public int? ParentId { get; set; }

    [NotMapped]
    public decimal? LatestReading
    {
        get
        {
            if (MeterReadings != null && MeterReadings.Count == 0)
            {
                return null;
            }
            return MeterReadings?.OrderByDescending(m => m.ReadingDate).First().Reading;
        }
    }
    
    // Navigation property
    [JsonIgnore]
    public MeterDetails ParentMeter { get; set; }
    
    [JsonIgnore]
    public ICollection<MeterDetails> ChildMeters { get; set; }
    
    [JsonIgnore]
    public ICollection<MeterReadings>? MeterReadings { get; set; }
}