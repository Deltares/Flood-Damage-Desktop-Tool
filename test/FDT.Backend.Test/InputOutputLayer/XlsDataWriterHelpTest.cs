﻿using System;
using ClosedXML.Excel;
using FDT.Backend.InputOutputLayer;
using NSubstitute;
using NUnit.Framework;

namespace FDT.Backend.Test.InputOutputLayer
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
    }
}