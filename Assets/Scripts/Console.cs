using UnityEngine;
using UnityEngine.InputSystem;
using Reka.DebugConsole.Settings;

namespace Reka.DebugConsole
{
	public sealed class Console : MonoBehaviour
	{
		public static Console Instance { get; private set; }

		[SerializeField] private ConsoleView _consoleView;
		[SerializeField] private ConsoleSettings _consoleSettings;

		private void Awake()
		{
			if (Instance != null)
			{
				DestroyImmediate(gameObject);
				return;
			}
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		private void Start()
		{
			_consoleView.gameObject.SetActive(false);
		}

		private void Update()
		{
			if (Keyboard.current.f3Key.wasPressedThisFrame)
			{
				_consoleView.gameObject.SetActive(!_consoleView.gameObject.activeSelf);
			}
		}

		public void ExecuteCommand(string command)
		{
			_consoleView.AddConsoleTextAsCommand(command);

			if (command == "clear")
			{
				_consoleView.ClearConsoleText();
				return;
			}
			// TODO: run command and log results
		}
	}
}