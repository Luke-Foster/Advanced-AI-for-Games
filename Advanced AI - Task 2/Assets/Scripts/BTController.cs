using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTController : MonoBehaviour {

	Node behaviourTree;
	Context behaviourState;
	private BTController btcontroller;
	public Transform Rabbit1;
	public Transform cave;
	public GameObject Rabbit01;
	public float speed = 5.0f;
	public float move;
	public float frontRange = 10.0f;
	public float frontAngle = 30.0f; // if angle between forward and los is less than this, the target is at front
	public float closeRange = 2.0f;

	void Awake()
	{
		behaviourTree = CreateBehaviourTree ();
		behaviourState = new Context ();
		move = speed * Time.deltaTime;
	}

	void Start()
	{
		btcontroller = GetComponent<BTController> ();
	}

	public void IsPlayerWithinPerceptionRange()  
	{
		RaycastHit hit;

		if(Physics.Linecast(transform.position, Rabbit1.position, out hit)) 
		{
			if(hit.transform == Rabbit1.transform) 
			{
				if(hit.distance <= closeRange) 
				{
					Debug.DrawLine(transform.position, hit.transform.position, Color.black);
					Rabbit1 = hit.transform.gameObject.GetComponent<BTController>().transform;
					Rabbit01 = hit.transform.gameObject;
				}
				else if(hit.distance <= frontRange) 
				{
					if(Vector3.Angle(transform.forward,hit.transform.position - transform.position) <= frontAngle) 
					{
						Debug.DrawLine(transform.position, hit.transform.position, Color.black);
						Rabbit1 = hit.transform.gameObject.GetComponent<BTController>().transform;
						Rabbit01 = hit.transform.gameObject;
					}
				}
			}
		}
	}

	void Update()
	{
		behaviourState.RabbitCount = GameObject.FindGameObjectsWithTag ("Rabbit").Length;

		if (Rabbit1 != null) 
		{
			IsPlayerWithinPerceptionRange ();
			// Searches for Rabbit 
			if (behaviourState.FindRabbit == false) 
			{
				transform.rotation = Quaternion.RotateTowards (transform.rotation, Rabbit1.rotation, 0.4f);
			}

			// Moves to Rabbit 
			if (behaviourState.MovetoRabbit == false && behaviourState.FindRabbit == true) 
			{
				transform.position = Vector3.MoveTowards (transform.position, Rabbit1.position, move);
			}
		}

			if (behaviourState.RabbitCount >= 1) 
			{
				if (transform.position == Rabbit1.position) 
				{
					behaviourState.MovetoRabbit = true;
					transform.position = transform.position;
					Destroy (Rabbit01);
				}

				if (transform.rotation == Rabbit1.rotation) 
				{
					behaviourState.FindRabbit = true;
				}
			}

			// Kill Rabbit 
			if (behaviourState.RabbitCount == 1) 
			{
				behaviourState.KillRabbit = true; 
			} 
			else 
			{
				behaviourState.KillRabbit = false;
				behaviourState.howl = true;
			}

			if (behaviourState.howlCount == 1) 
			{
				behaviourState.howl = false;
				transform.rotation = Quaternion.RotateTowards (transform.rotation, cave.rotation, 0.9f);
				if (transform.rotation == cave.rotation) 
				{
					transform.position = Vector3.MoveTowards (transform.position, cave.position, move);
				}
			}

			if (transform.position == cave.position) 
			{
				behaviourState.ReturntoCave = true;
			}

			if (behaviourState.BTEnd == true) 
			{
				Debug.Log ("Behaviour Tree has Ended");
				btcontroller.enabled = false;
			}
		}

	void FixedUpdate()
	{
		behaviourTree.Behave(behaviourState);
	}

	Node CreateBehaviourTree()
	{
		Sequence Patrolling = new Sequence ("Patrolling", new Search(), new Move());

		Selector Feeding = new Selector ("Feeding", new Kill(), new Howl (), new Cave ());

		Sequence MainCheck = new Sequence ("MainCheck", Patrolling, Feeding);

		Repeater repeater = new Repeater (MainCheck);
		return repeater;
	}
}
