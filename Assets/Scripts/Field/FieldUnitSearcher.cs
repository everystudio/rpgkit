using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogamelib;

namespace rpgkit
{
	public class FieldUnitSearcher : MonoBehaviour
	{
		public SequencePlayer m_sequencePlayer;
		public TalkBase m_talkTarget;
		public ChestBase m_chestTarget;
		private void Update()
		{
			m_talkTarget = null;
			RaycastHit2D raycastHit2D = Physics2D.Raycast(
				gameObject.transform.position,
				GetComponent<FieldUnitCore>().m_fieldUnitProperty.direction,
				1.0f,
				LayerMask.GetMask(new string[] { "people" , "chest" }));

			if (raycastHit2D.collider != null)
			{
				m_sequencePlayer = raycastHit2D.collider.gameObject.GetComponent<SequencePlayer>();
				m_talkTarget = raycastHit2D.collider.gameObject.GetComponent<TalkBase>();
				m_chestTarget = raycastHit2D.collider.gameObject.GetComponent<ChestBase>();
			}
		}
	}
}




