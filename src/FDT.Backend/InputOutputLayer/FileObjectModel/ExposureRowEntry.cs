﻿using System;
using FDT.Backend.InputOutputLayer.IFileObjectModel;

namespace FDT.Backend.InputOutputLayer.FileObjectModel
{
    public class ExposureRowEntry : IRowEntry
    {
        public static readonly string ExposureFileName = "exposure.csv";
        private string _exposureRelativePath;
        public string ExposureRelativePath => _exposureRelativePath;

        public ExposureRowEntry(string selectedBasin)
        {
            if (string.IsNullOrEmpty(selectedBasin))
                throw new ArgumentNullException(nameof(selectedBasin));
            _exposureRelativePath = $"Exposure\\{selectedBasin}\\{ExposureFileName}";
        }
    }
}