using UnityEngine;
using Reka.Runtime.CommandSO;

namespace Reka.Samples.DebugConsole
{
	public class RandomObject : MonoBehaviour
	{
		public void Start()
		{
			Command command = Console.Instance.RequestCommand("test.random.hello");
			if (command != null)
			{
				Debug.Log($"RandomObject: Command {command.CommandExtension} found");
				command.OnCommandExecuted += DoSomething;
			}
		}

		public void DoSomething(string commandExtension, string[] args)
		{
			if (commandExtension.Length != 0)
			{
				Debug.Log($"RandomObject: Command extension is valid: '{commandExtension}'");
			}
			if (args.Length == 0)
			{
				Debug.Log($"RandomObject: No args provided");
				return;
			}
			if (args.Length == 1)
			{
				Debug.Log($"RandomObject: One arg provided: '{args[0]}'");
				return;
			}
			if (args.Length == 2)
			{
				Debug.Log($"RandomObject: Two args provided: '{args[0]}' '{args[1]}'");
				return;
			}
			if (args.Length == 3)
			{
				Debug.Log($"RandomObject: Three args provided: '{args[0]}' '{args[1]}' '{args[2]}'");
				return;
			}
		}
	}
}