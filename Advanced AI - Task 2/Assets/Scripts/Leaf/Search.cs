using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : Leaf {

    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;

		if (context.FindRabbit == false) 
		{
			Debug.Log ("Finding Rabbit");
			return NodeStatus.RUNNING;
		}
		else if (context.FindRabbit == true) 
		{
			Debug.Log ("Found Rabbit");
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
