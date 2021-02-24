using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csvutility;

namespace rpgkit
{
	public class MasterItemParam : CsvDataParam
	{
		public int id;
		public string name;

	}
	public class MasterItem : CsvData<MasterItemParam>
	{
	}
}




