using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.Net.Commons;
using System.IO;
using OpenQA.Selenium;
using NUnit.Allure.Core;

namespace SeleniumAutomationLibrary.Utils
{
    public class AllureInfrastructure
    {
        private AllureLifecycle _allure;
        private Status _overallTestStatus;

        public AllureInfrastructure()
        {
            _allure = AllureLifecycle.Instance;
            _overallTestStatus = Status.passed;
        }

        public void UpdateOverallTestStatus(Status status)
        {
            _allure.UpdateTestCase(testResult =>
            {
                testResult.status = _overallTestStatus;
               
            });
            _overallTestStatus=status;
        }

        public void ExecuteAndReportPass(string message, string screenShot)
        {
            ExecuteAndRetry(() => ExecuteAndReport("Test: IWeb Element element found", Status.passed, message, screenShot));
        }

        public void ExecuteAndReportWarning(string message, string screenShot)
        {
            ExecuteAndRetry(() => ExecuteAndReport("Test: IWeb Element element not found", Status.broken, message, screenShot));
        }

        public void ExecuteAndReportFailed(string message, string screenShot)
        {
            ExecuteAndRetry(() => ExecuteAndReport("Test: IWeb Element element not found", Status.failed, message, screenShot));
        }

        private void ExecuteAndRetry(Action action)
        {
            int retryCount = 3;
            int currentAttempt = 0;
            bool success = false;

            while (currentAttempt < retryCount && !success)
            {
                try
                {
                    action();
                    success = true;
                }
                catch (Exception ex)
                {
                    currentAttempt++;
                    Console.WriteLine($"Attempt {currentAttempt} failed with error: {ex.Message}");
                    if (currentAttempt < retryCount)
                    {
                        Console.WriteLine("Retrying...");
                    }
                    else
                    {
                        Console.WriteLine("Max retry count reached. Exiting...");
                    }
                }
            }

            if (!success)
            {
                throw new Exception("Action failed after maximum retry attempts.");
            }
        }

        private void ExecuteAndReport(string stepName, Status status, string message, string screenshotPath = null)
        {
            if (_allure == null)
            {
                Console.WriteLine("Error: Allure context is not initialized. Cannot start step.");
                return;
            }

            // Create a new StepResult with the necessary details
            StepResult stepResult = new StepResult
            {
                name = stepName,
                status = status,
                statusDetails = new StatusDetails
                {
                    message = message
                }
            };

            Console.WriteLine($"Starting Allure Step: {stepName} with status: {status}");

            try
            {
                // Start the step without explicitly passing a UUID
                _allure.StartStep(stepResult);

                // If a screenshot path is provided, add the screenshot as an attachment
                if (!string.IsNullOrEmpty(screenshotPath) && File.Exists(screenshotPath))
                {
                    AllureApi.AddAttachment("Screenshot on Failure", "image/png", screenshotPath);
                }

                // Simulate a step failure if the status is 'failed'
                if (status == Status.failed)
                {
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                stepResult.status = Status.failed;
                stepResult.statusDetails.message = $"Error message: {message},\nException message: {ex.Message}";
                Console.WriteLine($"Exception in Allure Step: {ex.Message}");

                // Update the step status manually in the context
                _allure.UpdateStep(step => step.status = Status.failed);
                _allure.UpdateTestCase(testResult => testResult.status = Status.failed);
            }
            finally
            {
                try
                {
                    // Stop the step using the context-aware method
                    _allure.StopStep();
                    Console.WriteLine($"Stopped Allure Step: {stepName}");
                }
                catch (Exception stopEx)
                {
                    Console.WriteLine("Error stopping Allure step: " + stopEx.Message);
                }
            }
        }
    }
}
