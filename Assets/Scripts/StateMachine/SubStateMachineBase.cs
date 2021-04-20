using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubStateMachineBase<T,Parent> : StateMachineBase<T>
	where T:StateMachineBase<T> 
	where Parent:StateMachineBase<Parent>
{
	protected StateBase<Parent> stateParent;
}
