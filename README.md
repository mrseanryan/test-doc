# test-doc README

Generate a **markdown summary of your C# unit tests** via a simple C# script (CSX script executed via dotnet script or CSI.exe).

- currently supports **NUnit based tests**

## Usage

**test-doc** is a CSX script (C# script). There are two main ways to execute a CSX script:

- via dotnet script (.NET Core 6 or 7)
- via CSI.exe (installed with Microsoft Visual Studio)

### Usage - via dotnet script

1. Check that .NET Core 6 or 7 is installed:

`dotnet --version`

2. Install the 'script' tool of dotnet

`dotnet tool install -g dotnet-script`

3. Build your unit tests and note the path to the assembly (the DLL file).

4. Open a command prompt.

5. CD to this directory

6. Execute the C# script via `dotnet script`:

`dotnet script test-doc.csx <path to the unit tests DLL>`

This outputs a summary of all unit tests in markdown format.

### Usage - via CSI.exe (installed with Microsoft Visual Studio)

1. Build your unit tests and note the path to the assembly (the DLL file).

2. Open a `Developer Command Prompt`. This is installed with **Microsoft Visual Studio** and you can search for it via `Windows-Key + Q`. OR if you have .NET Core 5 or 6 installed then `csi` may be available.

3. CD to this directory

4. Execute the C# script via CSI:

`csi test-doc.csx <path to the unit tests DLL>`

This outputs a summary of all unit tests in markdown format.

## Example output

```
# Tests in MyCompany.Project1.MyApp.CoreTests

## Test class NumberParsingTests
- It_should_parse_a_number
- It_should_not_parse_a_date

## Test class InterestCalculator
- It_should_calculate_interest_for_one_year_at_10_percent
- It_should_calculate_interest_for_5_years_at_15_percent
```

## Dependencies

The C# script requires tooling to execute the CSX script.

There are 2 options:

1. [recommended] [dotnet core](https://dotnet.microsoft.com/en-us/download/dotnet) and [dotnet script](https://github.com/dotnet-script/dotnet-script)

OR

2. [CSI](https://learn.microsoft.com/en-us/archive/msdn-magazine/2016/january/essential-net-csharp-scripting) to execute.

This is installed as part of **Microsoft Visual Studio**.

## Compatibility

Tested with:
- dotnet script (dotnet version 6.0.400)
- Microsoft (R) Visual C# Interactive Compiler version 4.3.0-3.22401.3

The version of the tool (`dotnet script` or else `CSI.exe`) determines which .NET framework will be used to execute. This limits which .NET assemblies (DLL files) can be loaded for analysis.

So for example, if the CSI tool is .NET Core based, then it might not be able to load Windows specific .NET assemblies. In that case, it is beter to use `dotnet script`.
