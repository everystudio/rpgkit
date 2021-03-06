using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csvutility;

namespace rpgkit
{
	public class DataItemParam : CsvDataParam
	{
		public int item_id;
		public int item_serial;
		public int order_number;

	}
	public class DataItem : CsvData<DataItemParam>
	{
		private int GetNewSerial()
		{
			int iRet = 1;
			foreach( DataItemParam data in list)
			{
				if( iRet < data.item_serial)
				{
					iRet = data.item_serial + 1;
				}
			}
			return iRet;
		}
		public void AddItem(MasterItemParam _master)
		{
			DataItemParam item = new DataItemParam();
			item.item_id = _master.item_id;
			item.item_serial = GetNewSerial();
			list.Add(item);
		}


	}
}



