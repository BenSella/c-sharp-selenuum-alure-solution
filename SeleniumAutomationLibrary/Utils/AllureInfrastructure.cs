using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.Net.Commons;
using System;
using NUnit.Framework;


namespace SeleniumAutomationLibrary.Utils
{
    internal class AllureInfrastructure
    {
        private AllureLifecycle _allure;
        public AllureInfrastructure()
        {
            _allure = AllureLifecycle.Instance;
        }
        public void ExecuteAndReportPass(string message)
        {
            ExecuteAndRetry(() => ExecuteAndReport("Test: IWeb Element element found", Status.passed, message));
        }
        public void ExecueAndReportWarining(string message)
        {
            ExecuteAndRetry(() => ExecuteAndReport("Test: IWeb Element element not found", Status.broken, message));
        }
        public void ExecuteAndReportFailed(string message)
        {
            ExecuteAndRetry(() => ExecuteAndReport("Test: IWeb Element element not found", Status.failed, message));
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
        private void ExecuteAndReport(string stepName, Status status, string message)
        {
            StepResult result = new StepResult
            {
                name = stepName,
                status = status,
                statusDetails = new StatusDetails
                {
                    message = message
                }
            };
            _allure.StartStep(result);
            try
            {
                // Simulating the action that might fail and be retried
                if (status == Status.failed)
                {
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                result.status = Status.failed;
                result.statusDetails.message = $"error messge:  {message},\n exception message:{ex.Message}";
            }
            finally
            {
                _allure.StopStep();
            }
        }
    }
}
