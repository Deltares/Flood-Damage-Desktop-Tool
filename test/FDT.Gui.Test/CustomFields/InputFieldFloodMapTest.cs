﻿using NUnit.Framework;
using FDT.Gui.CustomFields;
using System;
using System.Threading;
using FDT.Gui.ViewModels;
using FDT.TestUtils;

namespace FDT.Gui.Test.CustomFields
{
    [TestFixture, Apartment(ApartmentState.STA)]
    class InputFieldFloodMapTest
    {
        [Test]
        [STAThread]
        public void TestGivenInputFieldReturnPeriodOnlyWithRisk()
        {
            InputFieldFloodMap inputField = new InputFieldFloodMap();
            const string eventFloodMapStr = "Event Flood Map";
            const int eventReturnPeriod = 24;
            const string riskFloodMapStr = "Risk Flood Map";
            const int riskReturnPeriod = 42;
            var eventFloodMap = new FloodMap()
            {
                MapPath = eventFloodMapStr,
                ReturnPeriod = eventReturnPeriod,
            };
            var riskFloodMap = new FloodMapWithReturnPeriod()
            {
                MapPath = riskFloodMapStr,
                ReturnPeriod = riskReturnPeriod
            };
            var listMaps = new BaseFloodMap[] {eventFloodMap, riskFloodMap};
            inputField.FloodMap = listMaps[0];
            WpfTestHelper testHelper = new WpfTestHelper(inputField, "Switching Scenario Type", () =>
            {
                inputField.FloodMap = inputField.FloodMap == eventFloodMap ? riskFloodMap : eventFloodMap;
            });
            testHelper.ShowDialog();
        }

        [Test]
        [STAThread]
        public void TestGivenInputFieldSetPathUpdatesField()
        {
            InputFieldFloodMap inputField = new InputFieldFloodMap();
            const string someValue = "We/Just/Set/Some/Path";
            const int riskReturnPeriod = 42;
            var eventFloodMap = new FloodMap()
            {
                MapPath = someValue,
                ReturnPeriod = riskReturnPeriod,
            };
            inputField.FloodMap = eventFloodMap;

            WpfTestHelper testHelper = new WpfTestHelper(inputField, "Setting Map Path Value", () =>
            {
                eventFloodMap.MapPath = eventFloodMap.MapPath == someValue ? string.Empty : someValue;
            });
            testHelper.ShowDialog();
        }
    }
}
