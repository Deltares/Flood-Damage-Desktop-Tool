﻿using FDT.Backend.DomainLayer.DataModel;
using FDT.Backend.DomainLayer.IDataModel;
using NUnit.Framework;

namespace FDT.Backend.Test.DomainLayer.DataModel
{
    public class FloodDamageDomainTest
    {
        [Test]
        public void TestConstructor()
        {
            var testDomain = new FloodDamageDomain();
            Assert.That(testDomain, Is.Not.Null);
            Assert.That(testDomain, Is.InstanceOf<IFloodDamageDomain>());
            Assert.That(testDomain.FloodDamageBasinData, Is.Not.Null);
            Assert.That(testDomain.FloodDamageBasinData, Is.InstanceOf<IBasin>());
            Assert.That(testDomain.Paths, Is.Not.Null);
            Assert.That(testDomain.Paths, Is.InstanceOf<IApplicationPaths>());
        }
    }
}