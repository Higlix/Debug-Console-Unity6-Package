# Reka Debug Console

An in-game debug console for Unity. Type commands at runtime to trigger custom logic, view logs, warnings, and errors — all from a draggable, resizable overlay.

## Requirements

- Unity 2021.3 or later
- Input System (`com.unity.inputsystem`)
- TextMeshPro (`com.unity.textmeshpro`)
- Unity UI (`com.unity.ugui`)

## Installation

### Git URL (recommended)

In your Unity project, open **Window → Package Manager → + → Add package from git URL…** and enter:

```
https://github.com/Higlix/Debug-Console-Unity6-Package.git
```

### Local / From Disk

1. Clone or download this repository.
2. In Unity, open **Window → Package Manager → + → Add package from disk…**
3. Navigate to the package folder and select `package.json`.

## Quick Start

0. **Import the samples**
   In the Package Manager, find **Reka Debug Console**, expand **Samples**, and click **Import** next to **Basic Setup**. This copies the sample prefab, scene, settings, and commands into your `Assets/` folder where they are fully editable.

1. **Create a ConsoleSettings asset**
   Right-click in the Project window → **Create → Console → ConsoleSettings**.
   Configure whether the console is visible on start and assign your commands.

2. **Create Command assets**
   Right-click in the Project window → **Create → DebugConsole → Command**.
   Fill in:
    - **Command Name** — display name (e.g. "Hello World").
    - **Command Description** — what the command does.
    - **Command Extension** — the identifier typed in the console (e.g. `test.hello`). Only letters, numbers, and dots are allowed.

3. **Add the Console to your scene**
    - Add a GameObject with the `Console` component.
    - Assign the `ConsoleView` and `ConsoleSettings` references in the Inspector.
    - If using the included sample prefab, drag `Console.prefab` into your scene.

4. **Press Play**
    - Press **F3** to toggle the console.
    - Type a command extension (e.g. `test.hello`) and press **Enter** or click the Enter button.

## Listening to Commands

Subscribe to a Command asset's `OnCommandExecuted` event from any MonoBehaviour:

```csharp
using UnityEngine;
using Reka.DebugConsole;

public class MyHandler : MonoBehaviour
{
    [SerializeField] private Command _helloCommand;

    private void OnEnable()
    {
        _helloCommand.OnCommandExecuted += HandleHello;
    }

    private void OnDisable()
    {
        _helloCommand.OnCommandExecuted -= HandleHello;
    }

    private void HandleHello(string extension, string[] args)
    {
        Debug.Log($"Hello executed with {args.Length} arguments");
    }
}
```

The first parameter is the command extension string; the second is an array of space-separated arguments (everything after the command name).

## Console Features

- **Draggable & resizable** — drag the console window to reposition; drag the corner to resize.
- **Log capture** — all `Debug.Log`, `Debug.LogWarning`, and `Debug.LogError` messages are displayed with color coding and timestamps.
- **Toggle** — press **F3** to show/hide the console at any time.

## Package Structure

```
com.reka.debug.console/
├── package.json
├── README.md
├── Runtime/
│   ├── Reka.Debug.asmdef
│   ├── Command/
│   │   └── Command.cs
│   ├── Console/
│   │   ├── Console.cs
│   │   └── ConsoleView.cs
│   └── ConsoleSettings/
│       └── ConsoleSettings.cs
└── Samples~/                        (importable via Package Manager)
    ├── Art/
    ├── Prefabs/
    │   └── Console.prefab
    ├── Scene/
    │   └── Test.unity
    └── ScriptableObjects/
        ├── ConsoleSettingsDefault.asset
        └── Commands/
            └── test.random.hello.asset
```

## License

See [LICENSE](./LICENSE) for details.
