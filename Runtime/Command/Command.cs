using System;
using UnityEngine;
using System.Collections.Generic;

namespace Reka.DebugConsole
{
	[CreateAssetMenu(fileName = "Command", menuName = "DebugConsole/Command")]
	public class Command : ScriptableObject
	{
		/// <summary>Subscribe to run logic whenever any command is executed. Args are the raw input split by space (args[0] = command name).</summary>
		public event Action<string, string[]> OnCommandExecuted;


		/// <summary>Raises OnCommandExecuted. Called by the console when a command is run.</summary>
		public void RaiseCommandExecuted(string commandExtension, string[] args) =>
			OnCommandExecuted?.Invoke(commandExtension, args);

		[SerializeField] private string _commandName;
		[TextArea(3, 8)]
		[SerializeField] private string _commandDescription;
		[SerializeField] private string _commandExtension;
		public string CommandName { get => _commandName; }
		public string CommandDescription { get => _commandDescription; }
		public string CommandExtension { get => _commandExtension; }
		public string[] CommandPath { get; private set; }


		public static bool IsValidCommandExtension(string commandExtension)
		{
			foreach (var character in commandExtension)
			{
				if (!char.IsLetterOrDigit(character) && character != '.')
				{
					return false;
				}
			}
			return true;
		}

		public static string[] GetCommandArgs(string command)
		{
			string[] commandArgs = command.Split(' ');
			List<string> filteredCommandArgs = new List<string>();
			
			if (commandArgs.Length <= 1)
			{
				return new string[0];
			}
			for (int i = 1; i < commandArgs.Length; i++)
			{
				if (commandArgs[i].Contains(" ") || commandArgs[i] == "")
				{
					continue;
				}
				filteredCommandArgs.Add(commandArgs[i]);
			}

			return filteredCommandArgs.ToArray();
		}

		public static string GetCommandExtension(string command)
		{
			string[] commandArgs = command.Split(' ');
			if (commandArgs.Length <= 0)
			{
				return "";
			}
			return commandArgs[0];
		}

		private void Awake()
		{
			try
			{
				if (string.IsNullOrEmpty(_commandName))
				{
					throw new System.Exception("Command name is not assigned");
				}
				if (string.IsNullOrEmpty(_commandDescription))
				{
					throw new System.Exception("Command description is not assigned");
				}
				if (string.IsNullOrEmpty(_commandExtension))
				{
					throw new System.Exception("Command extension is not assigned");
				}

				if (!IsValidCommandExtension(_commandExtension))
				{
					throw new System.Exception("Command extension contains invalid characters (only letters, numbers, and dots are allowed)");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"Error initializing command {_commandName}: {ex.Message}");
				return;
			}

			CommandPath = _commandExtension.Split('.');
		}


	}
}
