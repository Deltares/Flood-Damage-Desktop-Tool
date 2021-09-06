﻿using System;
using System.Collections.Generic;
using FDT.Backend.DomainLayer.IDataModel;
using FDT.Backend.PersistenceLayer.IFileObjectModel;

namespace FDT.Backend.PersistenceLayer.FileObjectModel
{
    public class ExposureTabXlsx: ITabXlsx
    {
        private static readonly string _tabName = "Exposure";
        public string TabName => _tabName;
        public IEnumerable<IRowEntry> RowEntries { get; }
        public ExposureRowEntry ExposureRowSingleEntry { get; }
        public ExposureTabXlsx(IBasin basin)
        {
            if (basin == null)
                throw new ArgumentNullException(nameof(basin));

            ExposureRowSingleEntry = new ExposureRowEntry(basin.BasinName);
            RowEntries = new[] { ExposureRowSingleEntry };
        }
    }
}