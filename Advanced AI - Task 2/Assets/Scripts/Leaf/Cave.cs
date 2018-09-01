using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave : Leaf {

    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;

		if (context.ReturntoCave == true) 
		{
			Debug.Log("In Cave");
			context.BTEnd = true;
			return NodeStatus.SUCCESS;
		} 
		else if (context.ReturntoCave == false) 
		{
			Debug.Log("Returning to Cave");
			return NodeStatus.RUNNING;
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
