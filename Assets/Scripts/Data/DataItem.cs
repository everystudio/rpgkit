using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csvutility;

namespace rpgkit
{
	public class DataItemParam : CsvDataParam
	{
		public int id;
		public int serial;
		public int order_number;

	}
	public class DataItem : CsvData<DataItemParam>
	{
		private int GetNewSerial()
		{
			int iRet = 1;
			foreach( DataItemParam data in list)
			{
				if( iRet < data.serial)
				{
					iRet = data.serial + 1;
				}
			}
			return iRet;
		}
		public void AddItem(MasterItemParam _master)
		{
			DataItemParam item = new DataItemParam();
			item.id = _master.id;
			item.serial = GetNewSerial();
			list.Add(item);
		}


	}
}



