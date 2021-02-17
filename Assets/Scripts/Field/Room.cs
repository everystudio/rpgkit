using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Room : MonoBehaviour
{
	public CinemachineVirtualCamera vcam;

	public void Enter()
	{
		vcam.Priority = 10;
	}

	public void Exit()
	{
		vcam.Priority = 0;
	}
}
