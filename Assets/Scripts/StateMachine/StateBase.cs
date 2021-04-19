using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase<T>
{
	protected StateMachineBase<T> machine;
	public StateBase(StateMachineBase<T> _machine)
	{
		machine = _machine;
	}

	public virtual void OnEnterState()
	{
	}
	public virtual void OnUpdateState()
	{
	}
	public virtual void OnExitState()
	{
	}
}
