using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMain : StateMachineBase<BattleMain>
{
	[SerializeField] private GameObject m_goRootBattleLog;
	[SerializeField] private GameObject m_prefBattleLog;

	public void ClearBattleLog()
	{
		m_prefBattleLog.SetActive(false);
		foreach( Transform tf in m_goRootBattleLog.transform.GetComponentsInChildren<Transform>())
		{
			Debug.LogError(tf.name);
		}
	}


	private void Start()
	{
		SetState(new BattleMain.Standby(this));
	}

	private class Standby : StateBase<BattleMain>
	{
		public Standby(BattleMain _machine) : base(_machine)
		{
			Debug.Log("Standby");

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
		public Opening(BattleMain _machine) : base(_machine)
		{
			Debug.Log("Opening");

			machine.ClearBattleLog();

			UIAssistant.Instance.ShowPage("Battle");
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
		public PlayerCommand(BattleMain _machine) : base(_machine)
		{
			Debug.Log("PlayerCommand");
		}
		public override void OnEnterState()
		{
			base.OnEnterState();
			machine.SetState(new BattleMain.TurnStart(machine));
		}
		public override void OnUpdateState()
		{
			base.OnUpdateState();
		}
	}

	private class TurnStart : StateBase<BattleMain>
	{
		public TurnStart(BattleMain _machine) : base(_machine)
		{
			Debug.Log("TurnStart");
		}
		public override void OnEnterState()
		{
			base.OnEnterState();
			machine.SetState(new BattleMain.Play(machine));
		}
	}
	private class Play : StateBase<BattleMain>
	{
		public Play(BattleMain _machine) : base(_machine)
		{
			Debug.Log("Play");
		}
		public override void OnUpdateState()
		{
			base.OnUpdateState();
			if (Input.GetKeyDown(KeyCode.Z))
			{
				machine.SetState(new BattleMain.Win(machine));
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				machine.SetState(new BattleMain.Lose(machine));
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				machine.SetState(new BattleMain.TurnEnd(machine));
			}
		}
	}

	private class TurnEnd : StateBase<BattleMain>
	{
		public TurnEnd(BattleMain _machine) : base(_machine)
		{
			Debug.Log("TurnEnd");
		}
		public override void OnUpdateState()
		{
			base.OnUpdateState();
			if (Input.GetKeyDown(KeyCode.Z))
			{
				machine.SetState(new BattleMain.TurnStart(machine));
			}
		}
	}

	private class Win : StateBase<BattleMain>
	{
		public Win(BattleMain _machine) : base(_machine)
		{
			Debug.Log("Win");
		}
		public override void OnEnterState()
		{
			base.OnEnterState();
			machine.SetState(new BattleMain.Result(machine));
		}
	}

	private class Lose : StateBase<BattleMain>
	{
		public Lose(BattleMain _machine) : base(_machine)
		{
			Debug.Log("Lose");
		}

		public override void OnUpdateState()
		{
			base.OnUpdateState();
			if (Input.GetKeyDown(KeyCode.Z))
			{
				machine.SetState(new BattleMain.GameOver(machine));
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				machine.SetState(new BattleMain.Continue(machine));
			}
		}
	}

	private class Result : StateBase<BattleMain>
	{
		public Result(BattleMain _machine) : base(_machine)
		{
			Debug.Log("Result");
		}
	}

	private class GameOver : StateBase<BattleMain>
	{
		public GameOver(BattleMain _machine) : base(_machine)
		{
			Debug.Log("GameOver");
		}
	}

	private class Continue : StateBase<BattleMain>
	{
		public Continue(BattleMain _machine) : base(_machine)
		{
		}
	}
}















