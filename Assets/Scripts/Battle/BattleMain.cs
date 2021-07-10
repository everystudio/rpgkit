using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using rpgkit;
using anogamelib;

public class BattleMain : StateMachineBase<BattleMain>
{
	[SerializeField] private GameObject m_goRootBattleLog;
	[SerializeField] private GameObject m_prefBattleLog;

	[SerializeField] private InputAction m_inputDebugBattleStart;
	[SerializeField] private BattleHUD m_battleHUD;

	#region
	[SerializeField] private TextAsset m_taMasterUnit;
	public MasterUnit masterUnit = new MasterUnit();

	[SerializeField] private TextAsset m_taDataUnit;	// テスト用
	public DataUnit dataUnitParty = new DataUnit();
	#endregion


	private int m_iPlayerCommandIndex;
	public struct BattleCommand
	{
		public int player_index;
		public string command;
	}
	private List<BattleCommand> m_battleCommandList = new List<BattleCommand>();
	
	public void ClearBattleInfo()
	{
		m_prefBattleLog.SetActive(false);
		foreach( Transform tf in m_goRootBattleLog.transform.GetComponentsInChildren<Transform>())
		{
			//Debug.Log(tf.name);
			Destroy(tf.gameObject);
		}
		m_battleHUD.Setup();
	}

	private void Start()
	{
		SetState(new BattleMain.Standby(this));
	}

	public void DataSetup()
	{
		masterUnit.Load(m_taMasterUnit);
		dataUnitParty.Load(m_taDataUnit);
	}
	private void OnEnable()
	{
		m_inputDebugBattleStart.Enable();
	}
	private void OnDisable()
	{
		m_inputDebugBattleStart.Disable();
	}

	private class Standby : StateBase<BattleMain>
	{
		public Standby(BattleMain _machine) : base(_machine)
		{
			machine.DataSetup();
			Debug.Log("Standby");
		}
		public override void OnEnterState()
		{
			base.OnEnterState();
			machine.m_inputDebugBattleStart.performed += M_inputDebugBattleStart_performed;

			machine.ClearBattleInfo();
			machine.m_battleHUD.ShowParty(machine.masterUnit.list, machine.dataUnitParty.list);
		}

		public override void OnExitState()
		{
			base.OnExitState();
			machine.m_inputDebugBattleStart.performed -= M_inputDebugBattleStart_performed;
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
			machine.m_battleHUD.AnimIntro();

			UIAssistant.Instance.ShowPage("Battle");
		}
		public override void OnEnterState()
		{
			base.OnEnterState();

		}
		public override void OnUpdateState()
		{
			if (machine.m_inputDebugBattleStart.phase == InputActionPhase.Started)
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
			machine.SetState(new BattleMain.CommandCheck(machine));
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
	private class CommandCheck : StateBase<BattleMain>
	{
		public CommandCheck(BattleMain _machine) : base(_machine)
		{
		}
		public override void OnEnterState()
		{
			base.OnEnterState();

			if( machine.dataUnitParty.list.Count <= machine.m_iPlayerCommandIndex)
			{
				machine.SetState(new BattleMain.CommandEnd(machine));
			}
			else
			{
				machine.SetState(new BattleMain.CommandTop(machine));
			}
		}
	}
	private class CommandTop : StateBase<BattleMain>
	{
		public CommandTop(BattleMain _machine) : base(_machine)
		{
		}
		public override void OnEnterState()
		{
			Debug.Log($"CommandTop:{machine.m_iPlayerCommandIndex}");
			base.OnEnterState();
			machine.m_battleHUD.m_btnAttack.onClick.AddListener(() =>
			{
				machine.m_iPlayerCommandIndex += 1;
				machine.m_battleCommandList.Add(new BattleCommand()
				{
					player_index = machine.m_iPlayerCommandIndex,
					command = "attack"
				});
				machine.SetState(new BattleMain.CommandCheck(machine));
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

	private class CommandEnd : StateBase<BattleMain>
	{
		public CommandEnd(BattleMain _machine) : base(_machine)
		{
			Debug.Log("CommandEnd");
		}
	}
}















