using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Leaf {

    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;

		if (context.MovetoRabbit == false) 
		{
			Debug.Log ("Moving to Rabbit");
			return NodeStatus.RUNNING;
		}
		else if (context.MovetoRabbit == true) 
		{
			return NodeStatus.SUCCESS;
		}
		else
		{
			return NodeStatus.FAILURE;
		}
    }

    public override void OnReset()
    {

    }
}
