using UnityEngine;
using Reka.DebugConsole;

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
		Debug.Log($"RandomObject: Doing something with command extension {commandExtension} and args {string.Join(", ", args)}");
	}
}