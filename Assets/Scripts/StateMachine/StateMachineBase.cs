using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineBase<T> : MonoBehaviour
{
	protected StateBase<T> stateCurrent;

	public void SetState(StateBase<T> _state)
	{
		if (stateCurrent != null)
		{
			stateCurrent.OnExitState();
		}
		stateCurrent = _state;
		stateCurrent.OnEnterState();
	}
	private void Update()
	{
		if (stateCurrent != null)
		{
			stateCurrent.OnUpdateState();
		}
	}

}
