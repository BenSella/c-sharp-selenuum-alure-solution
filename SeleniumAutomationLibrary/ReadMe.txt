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

SeleniumAutomationLibrary/
├── Utils/
│   └── AutomationInfrastructureHelper.cs  # Contains utility methods for Selenium automation
│   └── AllureInfrastructure.cs            # Contains utility methods for Allure Reports
├── SeleniumAutomationLibrary.csproj        # Project file for building the library
└── ReadMe.txt