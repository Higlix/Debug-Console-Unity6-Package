using UnityEngine;

namespace Reka.DebugConsole.Settings
{
	[CreateAssetMenu(fileName = "ConsoleSettings", menuName = "Console/ConsoleSettings")]
	public class ConsoleSettings : ScriptableObject
	{
		[Header("Console Settings")]
		[SerializeField] private bool _consoleVisibleOnStart = true;

		public bool ConsoleVisibleOnStart { get => _consoleVisibleOnStart; }
	}
}