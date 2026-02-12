using UnityEngine;
using Reka.DebugConsole;

public class ArdaTest : MonoBehaviour
{

	public void Start()
	{
		Command command = Console.Instance?.RequestCommand("arda.test");
		if (command != null)
		{
			command.OnCommandExecuted += Test;
			command.OnCommandExecuted += Test2;
		}
	}

	public void Test(string commandExtension, string[] args)
	{
		Debug.Log("ardanin testi yapildi");
	}

	public void Test2(string commandExtension, string[] args)
	{
		Debug.Log("ardanin test2 yapildi");
	}
}