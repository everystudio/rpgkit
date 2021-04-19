using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMain : StateMachineBase<BattleMain>
{
	private void Start()
	{
		SetState(new BattleMain.Standby(this));
	}

	private class Standby : StateBase<BattleMain>
	{
		public Standby(StateMachineBase<BattleMain> _machine) : base(_machine)
		{
		}
		public override void OnUpdateState()
		{
			if(Input.GetKeyDown(KeyCode.U))
			{
				machine.SetState(new BattleMain.Opening(machine));
			}
		}
	}

	private class Opening : StateBase<BattleMain>
	{
		public Opening(StateMachineBase<BattleMain> _machine) : base(_machine)
		{
		}
		public override void OnUpdateState()
		{
			if (Input.GetKeyDown(KeyCode.U))
			{
				machine.SetState(new BattleMain.PlayerCommand(machine));
			}
		}
	}

	private class PlayerCommand : StateBase<BattleMain>
	{
		public PlayerCommand(StateMachineBase<BattleMain> _machine) : base(_machine)
		{
		}
	}
}















