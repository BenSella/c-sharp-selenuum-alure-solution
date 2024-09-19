# Selenium Automation Library

## Overview
This project is a **Selenium Automation Library** that provides essential utilities to help developers write automated browser tests using Selenium WebDriver. It includes common methods to interact with web elements, handle multiple browser windows, capture screenshots, and handle waits effectively.

Additionally, it integrates with **Allure Reports**, allowing developers to generate detailed and visually rich test reports to monitor and review test results.

## Features
- **WebDriver Initialization**: Easily initialize ChromeDriver with options for headless mode.
- **Wait Mechanisms**: Both implicit and explicit waits are included to wait for elements or specific conditions.
- **Window Handling**: Switch between multiple browser windows or tabs.
- **Element Interaction**: Scroll, hover, or click on elements using JavaScript.
- **Screenshots**: Capture full-page screenshots with dynamic filenames.
- **Allure Reporting**: Integrated with Allure for generating attractive and informative test reports.

## Project Structure
,,,
SeleniumAutomationLibrary/
├── Utils/
│   └── AutomationInfrastructureHelper.cs  # Contains utility methods for Selenium automation
│   └── AllureInfrastructure.cs            # Contains utility methods for Allure Reports
├── SeleniumAutomationLibrary.csproj        # Project file for building the library
└── ReadMe.txt
,,,

to use allure reports:
1: activate powershell (not admin)
2: enter following commands:
	Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
	iwr -useb get.scoop.sh | iex
	scoop install allure	
3: Verify the java installation, usualy locatd at "C:\Program Files\Java\jdk-'last version'"
if doesnt exist you should folow below steps to install jdk 

3: in a test code:
using OpenQA.Selenium;
using SeleniumAutomationLibrary.Utils;
using NUnit.Framework;
using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using Allure.NUnit;
namespace 'project name space'
 [AllureNUnit]
  public class 'project class'
    {
	private AllureInfrastructure _allure;
	
        [SetUp]
		public void Setup()
		{
		_allure = new AllureInfrastructure(); 
		}
		[Test]
        [AllureTag("Smoke", "Magento")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("Magento Demo Site Testing")]
        public void Test1()



# Install Jdk:
Download the JDK:

Visit the Oracle JDK download page or the OpenJDK download page.
Download the latest stable version or LTS version like JDK 17. For OpenJDK, choose the appropriate version and download the Windows installer.
Run the Installer:

Once downloaded, run the installer and follow the instructions.
By default, it installs Java in C:\Program Files\Java\jdk-<version>. Note down this directory.
Step 2: Set JAVA_HOME Environment Variable
Open Environment Variables:

Right-click on This PC or Computer on your desktop or in File Explorer, then select Properties.
Go to Advanced system settings > Environment Variables.
Create JAVA_HOME:

In the System variables section, click New.
Set the Variable name to JAVA_HOME.
Set the Variable value to the path of the JDK installation, for example :C:\Program Files\Java\jdk-17

Add JAVA_HOME to Path:

In the System variables section, find the Path variable and select Edit.
Add a new entry : "%JAVA_HOME%\bin"

Step 3: Verify Installation
Open a New Command Prompt:

Open a new Command Prompt or PowerShell window.
Check Java Installation:

Type the following command and press Enter
java -version
