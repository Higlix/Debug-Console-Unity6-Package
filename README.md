# Reka Debug Console

A lightweight, extensible in-game debug console for Unity. Define commands as ScriptableObjects, subscribe to them from your own scripts, and execute them at runtime through a console overlay.

## What the Package Provides

The package ships two core ScriptableObject types in `Runtime/`:

- **Command** (`Create > DebugConsole > Command`) — A ScriptableObject that represents a single console command. Each command has a name, description, and an extension (e.g. `test.random.hello`). Other scripts subscribe to its `OnCommandExecuted` event to run logic when the command is executed.
- **ConsoleSettings** (`Create > Console > ConsoleSettings`) — A ScriptableObject that holds configuration: whether the console is visible on start, and the list of registered Command assets.

Everything else — the console UI, input handling, and sample scripts — is provided in the **Samples** folder as a starting point. You are free to build your own console implementation on top of the two core types.

## Requirements

- Unity 2021.3 or later
- Input System (`com.unity.inputsystem`)
- TextMeshPro (`com.unity.textmeshpro`)
- Unity UI (`com.unity.ugui`)

## Installation

### Git URL (recommended)

In your Unity project, open **Window > Package Manager > + > Add package from git URL...** and enter:

```
https://github.com/Higlix/Debug-Console-Unity6-Package.git
```

### Local / From Disk

1. Clone or download this repository.
2. In Unity, open **Window > Package Manager > + > Add package from disk...**
3. Navigate to the package folder and select `package.json`.

## Quick Start (using the Sample)

The **Samples** folder contains a ready-to-use console setup. After installing the package, import the sample from the Package Manager (**Reka Debug Console > Samples > Basic Setup > Import**).

The sample includes:

| Item                                                 | Description                                                                        |
| ---------------------------------------------------- | ---------------------------------------------------------------------------------- |
| `Prefabs/Console.prefab`                             | A preconfigured Console with UI, input field, and buttons. Drag it into any scene. |
| `Prefabs/RandomObject.prefab`                        | An example GameObject that subscribes to a command.                                |
| `ScriptableObjects/ConsoleSettingsDefault.asset`     | Default settings with the sample command already registered.                       |
| `ScriptableObjects/Commands/test.random.hello.asset` | A sample Command asset.                                                            |
| `Scene/Test.unity`                                   | A test scene with everything wired up.                                             |

### Steps

1. Open the sample scene `Test.unity`, or drag `Console.prefab` into your own scene.
2. Press **Play**, then press **F3** to toggle the console.
3. Type `test.random.hello` and press **Enter**. The `RandomObject` in the scene will log the result.

### Creating New Commands

1. Right-click in the Project window > **Create > DebugConsole > Command**.
2. Fill in the fields:
    - **Command Name** — a display name (e.g. "Spawn Enemy").
    - **Command Description** — what the command does.
    - **Command Extension** — the identifier users type in the console (e.g. `debug.spawn.enemy`). Only letters, numbers, and dots are allowed.
3. Open your **ConsoleSettings** asset (e.g. `ConsoleSettingsDefault`) and add the new Command to the **Registered Commands** list.

### Subscribing to a Command

To run your own logic when a command is executed, request the command from the Console singleton and subscribe to its event:

```csharp
using UnityEngine;
using Reka.Runtime.CommandSO;

public class MyHandler : MonoBehaviour
{
    private void Start()
    {
        // Request a registered command by its extension
        Command command = Console.Instance.RequestCommand("debug.spawn.enemy");

        if (command != null)
        {
            command.OnCommandExecuted += OnSpawnEnemy;
        }
    }

    private void OnSpawnEnemy(string commandExtension, string[] args)
    {
        // args contains everything after the command extension, split by space
        Debug.Log($"Spawning enemy! Args: {string.Join(", ", args)}");
    }
}
```

- `Console.Instance.RequestCommand(extension)` returns the `Command` ScriptableObject registered with that extension, or `null` if not found.
- `command.OnCommandExecuted` fires when the user types and submits that command. The first parameter is the extension string; the second is an array of space-separated arguments.

## Package Structure

```
com.reka.debug.console/
├── package.json
├── README.md
├── Runtime/
│   ├── Reka.Debug.asmdef
│   ├── Command/
│   │   └── Command.cs
│   └── ConsoleSettings/
│       └── ConsoleSettings.cs
└── Samples~/
    ├── Reka.Debug.Sample.asmdef
    ├── Art/
    ├── Prefabs/
    │   ├── Console.prefab
    │   └── RandomObject.prefab
    ├── Scripts/
    │   ├── RandomObject.cs
    │   └── Console/
    │       ├── Console.cs
    │       └── ConsoleView.cs
    ├── Scene/
    │   └── Test.unity
    └── ScriptableObjects/
        ├── ConsoleSettingsDefault.asset
        └── Commands/
            └── test.random.hello.asset
```

## License

See [LICENSE](./LICENSE) for details.
