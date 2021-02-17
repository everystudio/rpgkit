using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csvutility;
using UnityEngine.Events;

namespace rpgkit
{
	public class DataUnitParamEvent : UnityEvent<DataUnitParam>
	{
		public DataUnitParamEvent() { }
	}

	public class DataUnitParam : CsvDataParam
	{
		public int unit_id;
		public string unit_name;
		public int level;
		public int hp_current;
		public int hp_max;
		public int tp_current;
		public int tp_max;
	}
	public class DataUnit : CsvData<DataUnitParam>
	{
	}
}


