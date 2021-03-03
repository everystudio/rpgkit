using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csvutility;

namespace rpgkit
{
	public class DataEquipParam : CsvDataParam
	{
		public int equip_id;
		public int equip_serial;

		public int equip_unit;      // 0でだれも装備してない
	}

	public class DataEquip : CsvData<DataEquipParam>
	{
		private int getNewSerial()
		{
			int iRet = 0;
			foreach( DataEquipParam data in list)
			{
				iRet = Mathf.Max(iRet, data.equip_serial);
			}
			return iRet + 1;
		}
		public int Add(MasterEquipParam _master)
		{
			DataEquipParam data = new DataEquipParam();
			data.equip_id = _master.equip_id;
			data.equip_serial = getNewSerial();
			data.equip_unit = 0;
			list.Add(data);
			return data.equip_serial;
		}
	}
}





