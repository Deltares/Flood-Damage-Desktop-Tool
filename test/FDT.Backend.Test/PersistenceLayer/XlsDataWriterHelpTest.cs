﻿using System;
using System.Linq;
using ClosedXML.Excel;
using FDT.Backend.DomainLayer.IDataModel;
using FDT.Backend.PersistenceLayer;
using NSubstitute;
using NUnit.Framework;

namespace FDT.Backend.Test.PersistenceLayer
{
    public class XlsDataWriterHelpTest
    {

        static object[] GetWorksheetInvalidArgumentsCases =
        {
            new object[] { null, null },
            new object[] { Substitute.For<IXLWorkbook>(), string.Empty },
            new object[] { Substitute.For<IXLWorkbook>(), null }
        };

        [Test]
        [TestCaseSource(nameof(GetWorksheetInvalidArgumentsCases))]
        public void TestGetWorksheetInvalidArgumentsThrowsException(IXLWorkbook workbook, string tabName)
        {
            TestDelegate testAction = () => XlsDataWriteHelper.GetWorksheet(workbook, tabName);
            Assert.That(testAction, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        [TestCaseSource(typeof(PersistenceLayerTestData), nameof(PersistenceLayerTestData.InvalidIBasin))]
        public void TestValidateBasinDataThrowsWhenInvalidIBasin(IBasin testCaseBasin, Type exceptionType, string exceptionMessage)
        {
            TestDelegate testAction = () => XlsDataWriteHelper.ValidateFloodDamageBasinData(testCaseBasin);
            Assert.That(testAction, Throws.TypeOf(exceptionType).With.Message.Contains(exceptionMessage));
        }

        [Test]
        public void TestValidateBasinDataThrowsNothingWhenValidBasinData()
        {
            // 1. Define test case.
            IBasin basin = Substitute.For<IBasin>();
            IScenario scenario = Substitute.For<IScenario>();
            
            basin.BasinName.Returns("ValidBasinName");
            basin.Projection.Returns("Projection");
            basin.Scenarios.Returns(new[] { scenario });
            scenario.ScenarioName.Returns("ValidScenarioName");

            // 2. Define test action.
            TestDelegate testAction = () => XlsDataWriteHelper.ValidateFloodDamageBasinData(basin);

            // 3. Validate final expectations.
            Assert.That(testAction, Throws.Nothing);
        }

        [Test]
        public void TestValidateFloodMapsWithReturnPeriodThrowsWhenNullCollectionGiven()
        {
            TestDelegate testAction = () =>
                XlsDataWriteHelper.ValidateFloodMapsWithReturnPeriod(null);
            Assert.That(testAction, Throws.TypeOf<ArgumentNullException>().With.Message.Contains("floodMaps"));
        }

        [Test]
        public void TestValidateFloodMapsWithReturnPeriodThrowsNothingWhenEmptyCollectionGiven()
        {
            TestDelegate testAction = () =>
                XlsDataWriteHelper.ValidateFloodMapsWithReturnPeriod(Enumerable.Empty<IFloodMapWithReturnPeriod>());
            Assert.That(testAction, Throws.Nothing);
        }

        [Test]
        public void TestValidateFloodMapsWithReturnPeriodThrowsNothingWhenValidData()
        {
            var testFloodMap = Substitute.For<IFloodMapWithReturnPeriod>();
            testFloodMap.ReturnPeriod.Returns(42);
            TestDelegate testAction = () =>
                XlsDataWriteHelper.ValidateFloodMapsWithReturnPeriod(new []{ testFloodMap });
            Assert.That(testAction, Throws.Nothing);
        }
    }
}