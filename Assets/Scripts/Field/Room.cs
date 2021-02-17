using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace rpgkit
{
	[RequireComponent(typeof(BoxCollider))]
	public class Room : MonoBehaviour
	{
		public CinemachineVirtualCamera m_vcam;

		public void Enter()
		{
			m_vcam.Priority = 10;
		}

		public void Exit()
		{
			m_vcam.Priority = 0;
		}
	}
}


