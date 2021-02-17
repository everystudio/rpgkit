using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
	public class FieldUnitSearcher : MonoBehaviour
	{
		public TalkBase m_talkTarget;
		private void Update()
		{
			m_talkTarget = null;
			RaycastHit2D raycastHit2D = Physics2D.Raycast(
				gameObject.transform.position,
				GetComponent<FieldUnitCore>().m_fieldUnitProperty.direction,
				3.0f,
				LayerMask.GetMask(new string[] { "people" }));

			if (raycastHit2D.collider != null)
			{
				//Debug.Log(raycastHit2D.collider.gameObject.name);
				m_talkTarget = raycastHit2D.collider.gameObject.GetComponent<TalkBase>();
			}
		}
	}
}




