using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csvutility;

namespace rpgkit
{
	public class DataItemParam : CsvDataParam
	{
		public int item_serial;
		public int order_number;
		public string item_name;

	}
	public class DataItem : CsvData<DataItemParam>
	{
	}
}



