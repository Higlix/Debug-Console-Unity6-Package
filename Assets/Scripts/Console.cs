using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Reka.DebugConsole.Settings;
using UnityEngine.Rendering;

namespace Reka.DebugConsole
{
	public sealed class Console : MonoBehaviour
	{
		public static Console Instance { get; private set; }

		private readonly Dictionary<string, Command> _commandMap = new Dictionary<string, Command>();
		
		[SerializeField] private Command[] _commands;
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

			RegisterCommands();
		}

		private void Start()
		{
			if (_consoleSettings.ConsoleVisibleOnStart)
			{
				_consoleView.gameObject.SetActive(true);
			}
			else
			{
				_consoleView.gameObject.SetActive(false);
			}
		}

		private void Update()
		{
			if (Keyboard.current.f3Key.wasPressedThisFrame)
			{
				_consoleView.gameObject.SetActive(!_consoleView.gameObject.activeSelf);
			}
		}

		private void RegisterCommands()
		{
			foreach (var command in _commands)
			{
				if (_commandMap.ContainsKey(command.CommandExtension) || !Command.IsValidCommandExtension(command.CommandExtension))
				{
					Debug.LogError($"Command extension {command.CommandExtension} is invalid");
					continue;
				}
				_commandMap.Add(command.CommandExtension, command);
				Debug.Log($"Command {command.CommandExtension} registered");
			}
		}

		public Command RequestCommand(string commandExtension)
		{
			if (_commandMap.TryGetValue(commandExtension, out var commandObject))
			{
				return commandObject;
			}
			else
			{
				Debug.LogError($"Command {commandExtension} not found");
				return null;
			}
		}

		public void ExecuteCommand(string command)
		{
			string[] commandArgs =  Command.GetCommandArgs(command);
			string commandExtension = Command.GetCommandExtension(command);

			

			if (commandExtension == "" || !Command.IsValidCommandExtension(commandExtension))
			{
				Debug.LogError($"Command {commandExtension} is invalid");
				return;
			}
			commandArgs.TryRemoveElementsInRange(0, 1, out Exception error);	

			Command commandObject = RequestCommand(commandExtension);

			if (commandObject == null)
			{
				Debug.LogError($"Command {commandExtension} not found");
				return;
			}	

			for (int i = 1; i < commandArgs.Length; i++)
			{
				commandArgs[i] = commandArgs[i].Replace(" ", "");
			}
			Debug.Log($"Executing command {commandExtension} with args {string.Join(", ", commandArgs)}");
			commandObject.RaiseCommandExecuted(commandExtension, commandArgs);


		}
	}
}