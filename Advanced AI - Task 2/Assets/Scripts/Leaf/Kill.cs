using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : Leaf {

    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;

		if (context.KillRabbit == true) 
		{
			Debug.Log ("Kill Rabbit");
			return NodeStatus.SUCCESS;
		}
		else 
		{
			Debug.Log ("Killed Rabbit");
			return NodeStatus.FAILURE;
		}
    }

	public override void OnReset()
    {

    }
}
