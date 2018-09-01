using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception 
{
	public class PerceptionTracker : MonoBehaviour 
	{
		public enum Responses { ResNothing,ResRun,ResAttack}
		private const float WaitTime = 0.5f;
		public PerceptionManager perceptionManager;
		public const int NEUTRAL = 0;
		public const int WOLF = 1;
		public const int RABBIT = 1;
	
		public enum AgentStatus { Neutral,Wolf,Rabbit};
		public AgentStatus agentStatus = AgentStatus.Rabbit;

		public int agentStatusFilter;

		void Awake()
		{
			perceptionManager = GameObject.Find("wolf_02").GetComponent<PerceptionManager>();
			agentStatusFilter = RABBIT;
		}

		public void FilteredStimulus(PerceptionStimulus percepStimulus)
		{
			Debug.Log("Type : " + percepStimulus.StimType + " from " + percepStimulus.stimSource);
		}

		public Responses GetResponse()
		{
			return Responses.ResNothing;
		}
	}
}