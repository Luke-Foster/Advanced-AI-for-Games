using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception
{
	public class PerceptionStimulus : MonoBehaviour 
	{
		public enum StimulusTypes { VisualCanSee,AudioMovement,AudioAttack}
		public StimulusTypes StimType;
		public GameObject stimSource;
		public Vector3 stimLoc;
		public Vector3 stimDir;
		public float stimRadius;
		public GameObject stimSecondary;

		public PerceptionStimulus() { }
		public PerceptionStimulus(StimulusTypes stim, GameObject source, Vector3 loc, Vector3 dir, float radius, GameObject secondary)
		{
			this.StimType = stim;
			this.stimSource = source;
			this.stimLoc = loc;
			this.stimDir = dir;
			this.stimRadius = radius;
			this.stimSecondary = secondary;

		}
	}
}