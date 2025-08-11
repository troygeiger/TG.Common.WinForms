# TG.Common.WinForms

Windows Forms utility components extracted from the broader TG.Common library. This package contains small, reusable dialog/forms and helpers that I found myself re‑implementing across projects.

## Contents

Currently included:

| Component | Purpose |
|-----------|---------|
| `InputBox` | Simple modal dialog to prompt the user for a single text value (optionally multi‑line or password, and can integrate a search dialog). |
| `SearchFormBase` | Base class you can derive from to implement a search / selection popup that feeds back into `InputBox`. |
| `WaitForm` | Minimal waiting/progress (marquee) dialog with optional parent‑centering and auto‑close timeout. |
| `ExMessageBox` | Placeholder for a richer message box (currently minimal). |
| `ValueOptions` | Enumeration controlling how a returned value from a search form is applied (`Replace`, `Append`, `AppendSemiColonSeparated`). |
| `ShortcutManager` | Stub for creating Windows shortcuts (not supported in .NET SDK build targets here—always throws). |

## Target Frameworks

Multi‑targeted for:

`net8.0-windows; net6.0-windows; net48; net472`

Pick the highest your application supports. All targets require Windows (WinForms).

## Installation

NuGet (example):

```powershell
dotnet add package TG.Common.WinForms
```

Or add a PackageReference manually in your project file.

## InputBox Usage

```csharp
// Basic usage
using var dlg = new InputBox("User Name", "Enter the user name:", string.Empty);
if (dlg.ShowDialog() == DialogResult.OK)
{
  string value = dlg.Value; // user input
}

// Allow blank values and multi-line
var notes = new InputBox("Notes", "Enter notes:", "") { AllowBlankValue = true, MultiLine = true };
```

### Integrating a Search Form

Derive from `SearchFormBase` and set `ResultValue` and optionally `ValueReplaceOption` before closing with `DialogResult.OK`.

```csharp
public class CustomerSearchForm : SearchFormBase
{
  public CustomerSearchForm(IEnumerable<string> customers)
  {
    // Build UI (omitted)
  }

  private void SelectCustomer(string name)
  {
    ResultValue = name;
    ValueReplaceOption = ValueOptions.Replace;
    DialogResult = DialogResult.OK;
    Close();
  }
}

// Usage with InputBox
var input = new InputBox("Customer", "Select customer:", string.Empty, typeof(CustomerSearchForm));
```

## WaitForm Usage

```csharp
// Show (non-blocking thread spins the dialog) with default message
WaitForm.ShowForm();

// Update / show with custom message centered over a parent and auto-close after 5 seconds
WaitForm.ShowForm("Processing records...", this, 5000);

// When done
WaitForm.CloseForm();
```

## ExMessageBox

Currently a scaffold for a richer message box. The static `Show(string message)` is present but not yet implemented. (Subject to change or removal.) Prefer the built‑in `MessageBox.Show` unless you extend this class.

## ShortcutManager

The methods are stubs that intentionally throw `NotSupportedException` under these targets. They document intended future functionality for creating `.lnk` shortcuts. If you need this today, implement a platform-specific version (e.g., COM interop with `IWshRuntimeLibrary`) in a .NET Framework only project.

## Versioning

Version numbers are derived using [MinVer](https://github.com/adamralph/minver) from Git tags (prefix `v`). Pre‑release identifiers default to `preview.0` until a stable tag is created.

## Roadmap / Ideas

- Flesh out `ExMessageBox` (buttons, icons, copyable text, details expander).
- Optional cancellation / progress reporting for `WaitForm`.
- Pluggable value transformers for `InputBox`.
- Implement functional `ShortcutManager` for supported frameworks.

## Contributing

Feel free to open issues or PRs on GitHub: [https://github.com/troygeiger/TG.Common.WinForms](https://github.com/troygeiger/TG.Common.WinForms)

## License

MIT
