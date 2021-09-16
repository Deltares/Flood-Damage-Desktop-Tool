﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FDT.Backend.PersistenceLayer.FileObjectModel;
using FDT.Backend.PersistenceLayer.IFileObjectModel;
using FDT.Backend.ServiceLayer.IExeHandler;

namespace FDT.Backend.ServiceLayer.ExeHandler
{
    public class FiatPythonWrapper : IExeWrapper
    {
        private readonly string _exeFileName = "fiat_objects.exe";
        private ValidationReport _exeReport;
        private IOutputData _usedOutputData;

        public string ExeDirectory { get; set; }
        public string ExeFilePath => Path.Combine(ExeDirectory, _exeFileName);

        public FiatPythonWrapper()
        {
            // By default the exe will be located at the same level as the exe where we are working.
            ExeDirectory = Directory.GetCurrentDirectory();
            _exeReport = new ValidationReport();
        }

        public void Run(IOutputData outputData)
        {
            if (outputData == null)
                throw new ArgumentNullException(nameof(outputData));
            if (string.IsNullOrEmpty(outputData.ConfigurationFilePath))
                throw new ArgumentNullException(nameof(IOutputData.ConfigurationFilePath));
            if (!File.Exists(outputData.ConfigurationFilePath))
                throw new FileNotFoundException(outputData.ConfigurationFilePath);

            string arguments = $"--config {outputData.ConfigurationFilePath}";
            // Possibly consider using EnableRaisingEvents and subscribe to Process.Exited.
            // Process runProcess = Process.Start(ExeFilePath, arguments);
            using (Process runProcess = new Process())
            {
                runProcess.StartInfo.FileName = ExeFilePath;
                runProcess.StartInfo.Arguments = arguments;
                runProcess.StartInfo.RedirectStandardError = true;
                runProcess.Start();
                runProcess?.WaitForExit();
                _exeReport.AddIssue(runProcess.StandardError.ReadToEnd());
                _usedOutputData = outputData;
            }
        }

        public ValidationReport GetValidationReport()
        {
            ValidateUsedOutputData(_usedOutputData);
            return _exeReport;
        }

        public bool ValidateUsedOutputData(IOutputData outputData)
        {
            if (outputData == null)
                throw new ArgumentNullException(nameof(outputData));
            outputData.ValidateParameters();

            string resultsDir = Directory.GetParent(outputData.ConfigurationFilePath)?.FullName;
            string generatedBasinDir = Path.Combine(resultsDir, outputData.BasinName);
            string generatedScenarioDir = Path.Combine(generatedBasinDir, outputData.ScenarioName);
            if (!Directory.Exists(generatedScenarioDir))
            {
                _exeReport.AddIssue($"Scenario directory not found at {generatedScenarioDir}.");
                return false;
            }

            const string resultFilesPattern = "*_results.csv";
            string[] resultFiles = Directory.GetFiles(generatedScenarioDir, resultFilesPattern);
            if (!resultFiles.Any())
            {
                _exeReport.AddIssue($"No result files found at {generatedScenarioDir}.");
                return false;
            }
            
            // Run successful, we can move the configuration file to the basin directory.
            string newConfigurationFilePath =
                Path.Combine(generatedBasinDir, Path.GetFileName(outputData.ConfigurationFilePath));
            File.Move(outputData.ConfigurationFilePath, newConfigurationFilePath);
            return true;
        }
    }
}