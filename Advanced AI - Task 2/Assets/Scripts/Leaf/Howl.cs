using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Howl : Leaf {

    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;

		if (context.howl == true) 
		{
			Debug.Log("Howl");
			context.howlCount = 1;
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
