﻿using System;
using System.Collections.Generic;
using System.Linq;
using FDT.Backend.IDataModel;
using FDT.Gui.ViewModels;
using NSubstitute;
using NUnit.Framework;
using IFloodMap = FDT.Gui.ViewModels.IFloodMap;

namespace FDT.Gui.Test.ViewModels
{
    public class BackendConverterTest
    {
        [Test]
        public void TestConvertBasinThrowsExceptionWithNullArguments()
        {
            TestDelegate testAction = () => BackendConverter.ConvertBasin(null, string.Empty);
            Assert.That(testAction, Throws.Exception.TypeOf<ArgumentNullException>().With.Message.Contains("basinScenarios"));
        }

        [Test]
        public void TestConvertScenariosThrowsExceptionWithNullArguments()
        {
            TestDelegate testAction = () => BackendConverter.ConvertScenarios(null).ToArray();
            Assert.That(testAction, Throws.Exception.TypeOf<ArgumentNullException>().With.Message.Contains("scenarios"));
        }

        [Test]
        public void TestConvertFloodMapsThrowsExceptionWithNullArguments()
        {
            TestDelegate testAction = () => BackendConverter.ConvertFloodMaps(null).ToArray();
            Assert.That(testAction, Throws.Exception.TypeOf<ArgumentNullException>().With.Message.Contains("floodMaps"));
        }

        [Test]
        [TestCase(false, typeof(Backend.DataModel.FloodMap))]
        [TestCase(true, typeof(Backend.DataModel.FloodMapWithReturnPeriod))]
        public void TestConvertFloodMaps(bool hasReturnPeriod, Type expectedType)
        {
            var testFloodMap = Substitute.For<IFloodMap>();
            testFloodMap.HasReturnPeriod.Returns(hasReturnPeriod);
            IEnumerable<IFloodMapBase> convertedFloodMaps = Enumerable.Empty<IFloodMapBase>();
            TestDelegate testAction = () =>
                convertedFloodMaps = BackendConverter.ConvertFloodMaps(new[] {testFloodMap});
            Assert.That(testAction, Throws.Nothing);
            Assert.That(convertedFloodMaps.Single(), Is.InstanceOf<IFloodMapBase>());
            Assert.That(convertedFloodMaps.Single(), Is.InstanceOf(expectedType));
        }
    }
}