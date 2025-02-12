using WaterBillCalculator.Data;
using WaterBillCalculator.Interfaces;
using WaterBillCalculator.Models;

namespace WaterBillCalculator.Services;

public class WaterBillService : IWaterBillService
{
    private readonly WaterBillContext _context;

    public WaterBillService(WaterBillContext context)
    {
        _context = context;
    }
    
    public WaterBillResponse GetBillBreakdown(BillDetails billDetails)
    {
        CalculateBillShare(billDetails);
        
        var meterReadings = _context.Readings
            .Where(mr => mr.BillId == billDetails.Id)
            .Select(mr => new MeterReadingResponse
            {
                MeterName = mr.Meter.MeterName,
                CalculatedBillShare = mr.CalculatedBillShare
            })
            .ToList();

        return new WaterBillResponse
        {
            MeterReadings = meterReadings,
            Remainder = GetBillDifference(billDetails)
        };
    }

    public IEnumerable<MeterDetails> GetAllMeterDetails()
    {
        var meterDetails = _context.Meters.ToList();
        return meterDetails;
    }

    private void CalculateBillShare(BillDetails billDetails)
    {
        foreach (var meterReading in billDetails.MeterReadings)
        {
            // Retrieve the latest reading from the database if available
            var latestReading = _context.Readings
                .Where(mr => mr.MeterId == meterReading.MeterId && mr.ReadingDate < meterReading.ReadingDate)
                .OrderByDescending(mr => mr.ReadingDate)
                .FirstOrDefault();

            // Calculate the difference between the current reading and the latest reading (or PreviousReading if no latest reading is available)
            decimal previousReading = latestReading?.Reading ?? meterReading.PreviousReading ?? 0;
            decimal readingDifference = meterReading.Reading - previousReading;

            // Calculate the CalculatedBillShare
            meterReading.CalculatedBillShare = Math.Round((billDetails.StandingCharge / billDetails.MeterReadings.Count) + (readingDifference * billDetails.UnitPrice), 2);
            
            // Add the meter reading to the context
            _context.Readings.Add(meterReading);
        }

        _context.SaveChanges();
    }
    
    private decimal GetBillDifference(BillDetails billDetails)
    {
        CalculateBillShare(billDetails);

        decimal totalCalculatedBillShare = billDetails.MeterReadings
            .Sum(mr => Math.Round(mr.CalculatedBillShare ?? 0, 2));

        decimal difference = Math.Round(billDetails.BilledAmount - totalCalculatedBillShare, 2);

        return difference;
    }
}