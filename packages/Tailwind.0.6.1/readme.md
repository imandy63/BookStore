# dotnet-tailwind

Really basic tool to bootstrap Tailwind in .NET Blazor projects.

Run `dotnet tailwind init` to automatically create the necessary build targets and files for a basic Tailwind integration.

## Install
```
dotnet tool install --global tailwind
```

## How to use

1. In the directory with your `.csproj`:

```
dotnet tailwind init
```

2. Build your project. Tailwind will automatically generate `site.css` in `wwwroot/css`

3. Add the `site.css` to your `App.razor` 

```
<link rel="stylesheet" href="css/site.css" />
```

4. Anytime you build the solution, `site.css` will now be regenerated.

## Commands
### init

- Downloads the Tailwind CLI
- Add targets and properties to your tailwind file to automatically build your CSS using the TailwindCLI
- Adds the `app.css` with the Tailwind directives

### reset
- Removes property groups added by the tool
- Removes targets added by the tool
- _Only works on 0.6.0 and later_

## Update the tool
Uninstall the tool and then reinstall.
```
dotnet tool uninstall --global tailwind
dotnet tool install --global tailwind
```

## Release Notes

### 0.6.1
- Updated installation instructions

### 0.6.0
- **BREAKING:** Renamed Targets
- **BREAKING:** Labeled Property Groups
- Added `chmod` command for Linux
- Added `reset` command to clear changes made in the project file