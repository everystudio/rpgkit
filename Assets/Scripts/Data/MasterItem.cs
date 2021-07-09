using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogamelib;

namespace rpgkit
{
	public class MasterItemParam : CsvDataParam
	{
		public int item_id;
		public string item_name;

		public bool field;
		public bool battle;

		public string item_target;
		public string item_type;
		public int item_param;

		public string description;
		public string flavor;

		public SOItem so_item;
	}
	public class MasterItem : CsvData<MasterItemParam>
	{
	}
}




