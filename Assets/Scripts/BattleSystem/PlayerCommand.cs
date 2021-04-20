using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommand : SubStateMachineBase<PlayerCommand,BattleMain>
{
	private void Awake()
	{
		SetState(new PlayerCommand.Standby(this));
	}

	private class Standby : StateBase<PlayerCommand>
	{
		public Standby(PlayerCommand _machine) : base(_machine)
		{
		}
	}


}
