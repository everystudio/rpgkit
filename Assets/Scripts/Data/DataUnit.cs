using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogamelib;
using UnityEngine.Events;
using System.Reflection;

namespace rpgkit
{
	public class DataUnitParamEvent : UnityEvent<DataUnitParam>
	{
		public DataUnitParamEvent() { }
	}

	public class StatusParam : CsvDataParam
	{
		public int hp;
		public int tp;
		public int attack;
		public int defense;
		public int speed;
		public int mind;
		public int wisdom;

		public void Copy(StatusParam _param)
		{
			hp = _param.hp;
			tp = _param.tp;
			attack = _param.attack;
			defense = _param.defense;
			speed = _param.speed;
			mind = _param.mind;
			wisdom = _param.wisdom;
		}
	}

	public class DataUnitParam : StatusParam
	{
		public int unit_id;
		public string unit_name;
		public int level;
		public int position;

		public int hp_current;
		public int tp_current;

		public StatusParam assist_param;

		public int equip1;
		public int equip2;
		public int equip3;
		public int equip4;
		public int equip5;
		public int equip6;

		public int GetStatus( string _strKey)
		{
			int iRet = 0;
			FieldInfo unit_info = this.GetType().GetField(_strKey);
			FieldInfo assist_info = assist_param.GetType().GetField(_strKey);

			if( unit_info == null)
			{
				return iRet;
			}
			//Debug.Log((int)unit_info.GetValue(this));
			//Debug.Log((int)assist_info.GetValue(assist_param));

			iRet = (int)unit_info.GetValue(this) + (int)assist_info.GetValue(assist_param);
			return iRet;
		}

		public void RefreshAssist(List<MasterEquipParam> _master_list , List<DataEquipParam> _data_list)
		{
			int[] equip_arr = new int[]
			{
				equip1,
				equip2,
				equip3,
				equip4,
				equip5,
				equip6,
			};
			assist_param = new StatusParam();

			foreach( int equip_serial in equip_arr)
			{
				DataEquipParam data = _data_list.Find(p => p.equip_serial == equip_serial);
				MasterEquipParam equip = data == null ? null : _master_list.Find(p => p.equip_id == data.equip_id);
				if( equip != null)
				{
					FieldInfo[] infoArr = assist_param.GetType().GetFields();
					foreach (FieldInfo info in infoArr)
					{
						FieldInfo master_info = equip.GetType().GetField(info.Name);
						FieldInfo assist_info = assist_param.GetType().GetField(info.Name);
						int iMasterParam = (int)master_info.GetValue(equip);
						int iAssistParam = (int)assist_info.GetValue(assist_param);
						assist_info.SetValue(assist_param, iMasterParam + iAssistParam);
					}
				}
			}
		}

		public void AllCopy( DataUnitParam _unit)
		{
			Copy(_unit);
			equip1 = _unit.equip1;
			equip2 = _unit.equip2;
			equip3 = _unit.equip3;
			equip4 = _unit.equip4;
			equip5 = _unit.equip5;
			equip6 = _unit.equip6;
			assist_param = new StatusParam();
			assist_param.Copy(_unit.assist_param);
		}

	}
	public class DataUnit : CsvData<DataUnitParam>
	{
	}
}


