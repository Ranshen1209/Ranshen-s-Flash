# KittenHyper Flash Tool

This tool is specifically developed for flashing KittenHyper devices. It simplifies the process of installing new firmware, making it accessible even for users without extensive technical background.

## Official Website

For more information about KittenHyper, visit the official website: [KittenHyper Official](http://kittenhyper.gitee.io)

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

What things you need to install the software and how to install them:

- Windows 10 1803 or later.
- .NET Framework 4.7.2 or later must be installed on your machine.

### Installing

A step-by-step series of examples that tell you how to get a development environment running:

1. Clone the repository or download the ZIP file.

```bash
git clone https://github.com/Ranshen1209/Ranshen-s-Flash.git
```

2. Navigate to the cloned or downloaded directory.

3. Build the project using Visual Studio.

4. After building the project, the `bin` directory will be configured with necessary files and folders for operation.

```bash
/bin
|-- /platform-tools           # Contains Google's adb and fastboot executables.
|-- /resources                # Resources.
|-- /drivers                  # Contains device drivers, specifically from Xiaomi.

```