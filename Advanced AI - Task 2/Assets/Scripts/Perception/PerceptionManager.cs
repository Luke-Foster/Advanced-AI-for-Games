using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception
{
	public class PerceptionManager : MonoBehaviour 
	{
		private List<GameObject> Wolves;
		private List<PerceptionStimulus> StimBuffer;
		private List<GameObject> Rabbits;

		void Awake()
		{
			Wolves = new List<GameObject>();
			StimBuffer = new List<PerceptionStimulus>();
			Rabbits = GetRabbits();
		}
			
		void Start () 
		{
			StartCoroutine(StimulusCheck());
		}
	
		private List<GameObject> GetRabbits()
		{
			GameObject[] tempAI = GameObject.FindGameObjectsWithTag("Rabbit");

			List<GameObject> chars = new List<GameObject>(tempAI);

			return chars;
		}

		public void Register(GameObject registry)
		{
			Wolves.Add(registry);
		}

		public void AcceptStimulus(PerceptionStimulus stimulus)
		{
			StimBuffer.Add(stimulus);
		}

		IEnumerator StimulusCheck()
		{
			yield return new WaitForSeconds(0.1f);
			GenerateVisualStims();
			ProcessStimBuffer();
			StartCoroutine(StimulusCheck());
		}   
			
		private void ProcessStimBuffer()
		{
			PerceptionTracker perceptionTracker;

			foreach (GameObject W in Wolves)
			{

				perceptionTracker = W.GetComponent<PerceptionTracker>();

				foreach (PerceptionStimulus s in StimBuffer)
				{
					if (Filter(perceptionTracker, W, s))
					{
						if(s.StimType == PerceptionStimulus.StimulusTypes.AudioMovement||s.StimType == PerceptionStimulus.StimulusTypes.AudioAttack)
						{
							float distance = Mathf.Abs((s.stimLoc - W.transform.position).magnitude);
							if(distance < s.stimRadius)
							{
								perceptionTracker.FilteredStimulus(s);

							}
						}

						else if( s.StimType == PerceptionStimulus.StimulusTypes.VisualCanSee && s.stimSecondary == W)
						{
							perceptionTracker.FilteredStimulus(s);
						}
					}
				}
			}
			StimBuffer.Clear();
		}

		private void GenerateVisualStims()
		{

			Vector3 dir;

			foreach (GameObject w in Wolves)
			{

				foreach (GameObject r in Rabbits)
				{
					if (CanSeeWolves(w.transform, r.transform, out dir) && w != r)
					{                    
						StimBuffer.Add(new PerceptionStimulus(PerceptionStimulus.StimulusTypes.VisualCanSee, r, r.transform.position, dir, 0f, w));
						w.GetComponent<BTController>().Rabbit01 = r;
					}
				}
			}
		}

		private static bool CanSeeWolves(Transform Reg, Transform Wol, out Vector3 Dir)
		{

			float Height = Reg.GetComponent<CharacterController>().height;
			WolfSenses wolfSenses = Reg.GetComponent<WolfSenses>();

			Dir = Wol.position - Reg.position;

			float DistanceToRabbit = Vector3.Distance(Reg.position, Wol.position);

			if(wolfSenses.sightRange > DistanceToRabbit)
			{
				float angle = Vector3.Angle(Dir, Reg.forward);
				angle = System.Math.Abs(angle);
				if (angle < (wolfSenses.viewAngle / 2))
				{
					RaycastHit hitData;
					LayerMask playerMask = 1 << 9;
					LayerMask coverMask = 1 << 8;
					LayerMask aiMask = 1 << 10;
					LayerMask mask = coverMask | playerMask | aiMask;

					float targetHeight = (Wol.GetComponent<CharacterController>().height / 1.25f);

					Vector3 registrantEyePosition = new Vector3(Reg.position.x, Reg.position.y + Height, Reg.position.z);
					Vector3 targetBodyPosition = new Vector3(Wol.transform.position.x, Wol.transform.position.y + targetHeight, Wol.transform.position.z);
					Vector3 rayDirection = (targetBodyPosition - registrantEyePosition).normalized;

					bool hit = Physics.Raycast(registrantEyePosition, rayDirection, out hitData, wolfSenses.sightRange, mask.value);
					Debug.DrawRay(registrantEyePosition, rayDirection * wolfSenses.sightRange, Color.red);

					if (hit)
					{
						if (hitData.collider.tag == "Rabbit")
							return true;
						}
					}
				}
			return false;
		}

		private bool Filter(PerceptionTracker tracker, GameObject Rabbit, PerceptionStimulus perceptionStimulus)
		{
			if (perceptionStimulus.stimSource != Rabbit)
			{
				PerceptionTracker.AgentStatus Type = tracker.agentStatus;

				int validAgentTypes = tracker.agentStatusFilter;

				PerceptionTracker.AgentStatus sourceAgentType;

				if(perceptionStimulus.stimSource.GetComponent<PerceptionTracker>() == null)
				{
					sourceAgentType = PerceptionTracker.AgentStatus.Wolf;
				}else
				{
					sourceAgentType = perceptionStimulus.stimSource.GetComponent<PerceptionTracker>().agentStatus;
				}

				if ((sourceAgentType == PerceptionTracker.AgentStatus.Rabbit) && (Type == PerceptionTracker.AgentStatus.Wolf) && ((validAgentTypes & PerceptionTracker.RABBIT) != 0)) return true;
				if ((sourceAgentType == PerceptionTracker.AgentStatus.Rabbit) && (Type == PerceptionTracker.AgentStatus.Rabbit) && ((validAgentTypes & PerceptionTracker.WOLF) != 0)) return true;
				if ((sourceAgentType == PerceptionTracker.AgentStatus.Rabbit) && (Type == PerceptionTracker.AgentStatus.Neutral) && ((validAgentTypes & PerceptionTracker.NEUTRAL) != 0)) return true;

				if ((sourceAgentType == PerceptionTracker.AgentStatus.Wolf) && (Type == PerceptionTracker.AgentStatus.Wolf) && ((validAgentTypes & PerceptionTracker.WOLF) != 0)) return true;
				if ((sourceAgentType == PerceptionTracker.AgentStatus.Wolf) && (Type == PerceptionTracker.AgentStatus.Rabbit) && ((validAgentTypes & PerceptionTracker.RABBIT) != 0)) return true;
				if ((sourceAgentType == PerceptionTracker.AgentStatus.Wolf) && (Type == PerceptionTracker.AgentStatus.Neutral) && ((validAgentTypes & PerceptionTracker.NEUTRAL) != 0)) return true;

				if ((sourceAgentType == PerceptionTracker.AgentStatus.Neutral) && (Type == PerceptionTracker.AgentStatus.Wolf) && ((validAgentTypes & PerceptionTracker.NEUTRAL) != 0)) return true;
				if ((sourceAgentType == PerceptionTracker.AgentStatus.Neutral) && (Type == PerceptionTracker.AgentStatus.Rabbit) && ((validAgentTypes & PerceptionTracker.NEUTRAL) != 0)) return true;
				if ((sourceAgentType == PerceptionTracker.AgentStatus.Neutral) && (Type == PerceptionTracker.AgentStatus.Neutral) && ((validAgentTypes & PerceptionTracker.NEUTRAL) != 0)) return true;

				return false;

			}
			else { return false; }


		}
	}
}