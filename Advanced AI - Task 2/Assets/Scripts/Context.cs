using UnityEngine;
using System.Collections;

public class Context : BehaviourState
{
	public bool FindRabbit = false;
	public bool MovetoRabbit = false;
	public bool KillRabbit = false;
	public bool howl = false;
	public bool ReturntoCave = false;

	public float howlCount = 0;
	public float RabbitCount = 1;
	public bool BTEnd = false;
}
