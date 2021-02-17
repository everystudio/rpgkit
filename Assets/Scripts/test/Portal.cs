using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace test
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class Portal : MonoBehaviour
	{
		public string portal_name;
		public Vector2 exit_offset;
		public CinemachineVirtualCamera vcam;

		public Portal target_portal;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Player")
			{
				other.gameObject.transform.position =
					target_portal.transform.position +
					new Vector3(target_portal.exit_offset.x, target_portal.exit_offset.y);

				if( vcam != null)
				{
					vcam.Priority = 0;
				}
				if( target_portal.vcam != null)
				{
					target_portal.vcam.Priority = 10;
				}
			}
		}

	}

}