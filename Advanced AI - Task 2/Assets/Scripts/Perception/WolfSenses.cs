using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception
{
	public class WolfSenses : MonoBehaviour 
	{
		public GameObject target;
		private CharacterController characterController;
		public float viewAngle = 200f;
		public float sightRange = 50f;
		public float engageTargetAngle = 20f;
		public float CorrectionFactor = 20f;

		void Start () 
		{
			characterController = GetComponent<CharacterController>();
		}
	
		void Update () 
		{
			if(target == null)
			{
				target = GameObject.FindGameObjectWithTag("Rabbit");
				TargetSightCheck();
			}
		}

		public bool TargetSightCheck()
		{
			if (target != null)
			{

				float DistanceToRabbit = Vector3.Distance(target.transform.position, transform.position);

				if (sightRange >= DistanceToRabbit)
				{

					Vector3 targetDirection = target.transform.position - transform.position;
					float targetAngle = Vector3.Angle(targetDirection, transform.forward);
					targetAngle = System.Math.Abs(targetAngle);

					if (targetAngle < (viewAngle / 2))
					{
						CharacterController targetCharacterController = target.GetComponent<CharacterController>();
						RaycastHit hitData;

						LayerMask playerMask = 1 << 9;
						LayerMask aiMask = 1 << 10;
						LayerMask coverMask = 1 << 8;
						LayerMask mask = coverMask | playerMask | aiMask;

						float targetHeight = targetCharacterController.height;
						float height = characterController.height;

						Vector3 eyePos = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
						Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y - (targetHeight / 2.0f), target.transform.position.z);
						Vector3 dir = (targetPos - transform.position).normalized;

						bool hit = Physics.Raycast(eyePos, dir, out hitData, sightRange, mask.value);
						Debug.DrawRay(eyePos, dir * sightRange, Color.blue);

						if (hit)
						{
							if (hitData.collider.tag == target.gameObject.tag)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool IsBehindCoverCheck(GameObject Wolf, GameObject target)
		{
			float[] checkY = { 0.1f, 0.5f, 1f };
			bool[] covered = { false, false, false };
			float range = 3f;
			RaycastHit hit;
			LayerMask coverMask = 1 << 9;
			Vector3 checkPosition;
			Vector3 direction = (target.transform.position - Wolf.transform.position).normalized;

			for(int n = 0; n < checkY.Length; n++)
			{
				checkPosition = new Vector3(Wolf.transform.position.x, Wolf.transform.position.y + checkY[n], Wolf.transform.position.z);
				covered[n] = Physics.Raycast(checkPosition, direction, out hit, range, coverMask.value);
			}
			return ((covered[0] || covered[1] && !covered[2]));
		}
	}
}