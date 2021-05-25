using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleMain : StateMachineBase<BattleMain>
{
	[SerializeField] private GameObject m_goRootBattleLog;
	[SerializeField] private GameObject m_prefBattleLog;

	[SerializeField] private InputAction m_inputDebugBattleStart;
	[SerializeField] private BattleHUD m_battleHUD;

	private int m_iPlayerCommandIndex;
	public struct BattleCommand
	{
		public int player_index;
		public string command;
	}
	private List<BattleCommand> m_battleCommandList = new List<BattleCommand>();
	
	public void ClearBattleLog()
	{
		m_prefBattleLog.SetActive(false);
		foreach( Transform tf in m_goRootBattleLog.transform.GetComponentsInChildren<Transform>())
		{
			Debug.Log(tf.name);
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
		public override void OnEnterState()
		{
			base.OnEnterState();
			machine.m_inputDebugBattleStart.performed += M_inputDebugBattleStart_performed;
			machine.m_inputDebugBattleStart.Enable();
		}

		public override void OnExitState()
		{
			base.OnExitState();
			machine.m_inputDebugBattleStart.performed -= M_inputDebugBattleStart_performed;
			machine.m_inputDebugBattleStart.Disable();
		}
		private void M_inputDebugBattleStart_performed(InputAction.CallbackContext obj)
		{
			machine.SetState(new BattleMain.Opening(machine));
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
			if (machine.m_inputDebugBattleStart.phase == InputActionPhase.Performed)
			{
				machine.SetState(new BattleMain.TurnStart(machine));
			}
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
			machine.SetState(new BattleMain.PlayerCommandStart(machine));
		}
	}

	private class PlayerCommandStart : StateBase<BattleMain>
	{
		public PlayerCommandStart(BattleMain _machine) : base(_machine)
		{
			Debug.Log("PlayerCommandStart");
		}
		public override void OnEnterState()
		{
			base.OnEnterState();
			machine.m_iPlayerCommandIndex = 0;
			machine.SetState(new BattleMain.CommandTop(machine));
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
			//if (Input.GetKeyDown(KeyCode.Z))
			{
				machine.SetState(new BattleMain.Win(machine));
			}
			//if (Input.GetKeyDown(KeyCode.X))
			{
				machine.SetState(new BattleMain.Lose(machine));
			}
			//if (Input.GetKeyDown(KeyCode.C))
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
			//if (Input.GetKeyDown(KeyCode.Z))
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
			//if (Input.GetKeyDown(KeyCode.Z))
			{
				machine.SetState(new BattleMain.GameOver(machine));
			}
			//if (Input.GetKeyDown(KeyCode.X))
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

	private class CommandTop : StateBase<BattleMain>
	{
		public CommandTop(BattleMain _machine) : base(_machine)
		{
		}
		public override void OnEnterState()
		{
			base.OnEnterState();
			machine.m_battleHUD.m_btnAttack.onClick.AddListener(() =>
			{
				machine.m_iPlayerCommandIndex += 1;
				machine.m_battleCommandList.Add(new BattleCommand()
				{
					player_index = machine.m_iPlayerCommandIndex,
					command = "attack"
				});
				machine.SetState(new BattleMain.CommandTop(machine));
			});
			machine.m_battleHUD.m_btnSkill.onClick.AddListener(() =>
			{
				machine.SetState(new BattleMain.SkillSelectTop(machine));
			});
			machine.m_battleHUD.m_btnItem.onClick.AddListener(() =>
			{
				machine.SetState(new BattleMain.ItemSelectTop(machine));
			});
			machine.m_battleHUD.m_btnBack.onClick.AddListener(() =>
			{
				machine.m_iPlayerCommandIndex -= 1;
			});
			machine.m_battleHUD.m_btnBack.interactable = 0 < machine.m_iPlayerCommandIndex;
		}
		public override void OnExitState()
		{
			base.OnExitState();
			machine.m_battleHUD.m_btnAttack.onClick.RemoveAllListeners();
			machine.m_battleHUD.m_btnSkill.onClick.RemoveAllListeners();
			machine.m_battleHUD.m_btnItem.onClick.RemoveAllListeners();
			machine.m_battleHUD.m_btnBack.onClick.RemoveAllListeners();
		}

	}

	private class SkillSelectTop : StateBase<BattleMain>
	{
		public SkillSelectTop(BattleMain _machine) : base(_machine)
		{
		}
		public override void OnEnterState()
		{
			base.OnEnterState();
		}
		public override void OnExitState()
		{
			base.OnExitState();
		}
	}

	private class ItemSelectTop : StateBase<BattleMain>
	{
		public ItemSelectTop(BattleMain _machine) : base(_machine)
		{
		}
	}
}















