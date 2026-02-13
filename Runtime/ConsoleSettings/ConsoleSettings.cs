using UnityEngine;

namespace Reka.DebugConsole.Settings
{
	[CreateAssetMenu(fileName = "ConsoleSettings", menuName = "Console/ConsoleSettings")]
	public class ConsoleSettings : ScriptableObject
	{
		[Header("Console Settings")]
		[SerializeField] private bool _consoleVisibleOnStart = true;

		[Header("Registered Commands")]
		[SerializeField] private Command[] _commands;

		public Command[] Commands { get => _commands; }
		public bool ConsoleVisibleOnStart { get => _consoleVisibleOnStart; }
	}
}