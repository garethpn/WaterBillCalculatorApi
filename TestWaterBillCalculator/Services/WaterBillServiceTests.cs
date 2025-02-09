using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using WaterBillCalculator.Data;
using WaterBillCalculator.Services;
using WaterBillCalculator.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestWaterBillCalculator.Services
{
    [TestFixture]
    public class WaterBillServiceTests
    {
        private WaterBillService _service;
        private Mock<WaterBillContext> _contextMock;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<WaterBillContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _contextMock = new Mock<WaterBillContext>(options);
            _service = new WaterBillService(_contextMock.Object);
        }

        [Test]
        public void GetBillBreakdown_ShouldReturnCorrectBillShare()
        {
            // Arrange
            var meterDetails = new List<MeterDetails>
            {
                new MeterDetails { Id = 1, MeterName = "Meter 1" },
                new MeterDetails { Id = 2, MeterName = "Meter 2" }
            };

            var meterReadings = new List<MeterReadings>
            {
                new MeterReadings { BillId = 1, MeterId = 1, Reading = 5, ReadingDate = new DateTime(2023, 12, 01), CalculatedBillShare = 0, Meter = meterDetails[0]},
                new MeterReadings { BillId = 1, MeterId = 2, Reading = 10, ReadingDate = new DateTime(2023, 12, 01), CalculatedBillShare = 0, Meter = meterDetails[1]}
            };
            
            var billDetails = new BillDetails
            {
                Id = 1,
                StandingCharge = 10.0m,
                UnitPrice = 2.0m,
                MeterReadings = meterReadings
            };

            _contextMock.Setup(c => c.Readings).ReturnsDbSet(meterReadings);
            _contextMock.Setup(c => c.Meters).ReturnsDbSet(meterDetails);

            // Act
            var result = _service.GetBillBreakdown(billDetails);

            // Assert
            Assert.That(result.MeterReadings.Count, Is.EqualTo(2));
            Assert.That(result.MeterReadings[0].MeterName, Is.EqualTo("Meter 1"));
            Assert.That(result.MeterReadings[0].CalculatedBillShare, Is.EqualTo(15.0m));
            Assert.That(result.MeterReadings[1].MeterName, Is.EqualTo("Meter 2"));
            Assert.That(result.MeterReadings[1].CalculatedBillShare, Is.EqualTo(25.0m));
        }
    }
}