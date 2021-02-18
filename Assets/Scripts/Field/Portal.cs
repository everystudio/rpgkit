using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class Portal : MonoBehaviour
	{
		public Vector2 m_ExitOffset;
		public Portal m_portalTarget;
		private Room m_roomExist;
		public Room RoomExist
		{
			get { return m_roomExist; }
		}

		private void Awake()
		{
			Room[] rooms = GameObject.FindObjectsOfType<Room>();
			foreach (Room r in rooms)
			{
				r.m_vcam.Priority = 0;
				if (r.GetComponent<Collider>().bounds.Contains(transform.position))
				{
					m_roomExist = r;
				}
			}
			if(m_roomExist == null)
			{
				Debug.LogError("ルームの中にいません");
			}

		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "Player")
			{
				collision.gameObject.transform.position =
					m_portalTarget.transform.position + new Vector3(
						m_portalTarget.m_ExitOffset.x,
						m_portalTarget.m_ExitOffset.y, 0.0f);

				m_roomExist.Exit();
				m_portalTarget.RoomExist.Enter(collision.transform);
				//m_portalTarget.RoomExist.m_vcam.Follow = collision.gameObject.transform;
			}
		}
		private void OnDrawGizmos()
		{
			if(m_portalTarget != null)
			{
				// draws an arrow from this teleporter to its destination
				RPGDebug.DrawGizmoArrow(
					this.transform.position,
					(m_portalTarget.transform.position + new Vector3(
						m_portalTarget.m_ExitOffset.x,
						m_portalTarget.m_ExitOffset.y,0.0f)) - this.transform.position, Color.cyan, 1f, 25f);
				// draws a point at the exit position 
				RPGDebug.DebugDrawCross(this.transform.position + new Vector3(
						m_ExitOffset.x,
						m_ExitOffset.y, 0.0f), 0.5f, Color.yellow);
				RPGDebug.DrawPoint(this.transform.position + new Vector3(
						m_ExitOffset.x,
						m_ExitOffset.y, 0.0f), Color.yellow, 0.5f);

			}
		}
	}

}


