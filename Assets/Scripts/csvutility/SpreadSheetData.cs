using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace csvutility
{
	public class DataCopyUtil
	{
		static public bool checkContainsValue(IDictionary _iDict, string _strKey, bool _bLog)
		{
			if (_iDict.Contains(_strKey) && !(_iDict[_strKey] == null))
			{
				return true;
			}
			if (_bLog)
			{
				//string error_msg = "error key name :[" + _strKey + "]";
				////Debug.Log( error_msg );
			}
			Debug.Log("ダメな処理のキー洗い出し" + _strKey);
			return false;
		}

		static public bool copyLongToInt(ref int _lData, IDictionary _iDict, string _strKey, bool _bLog = true)
		{
			if (DataCopyUtil.checkContainsValue(_iDict, _strKey, _bLog))
			{
				_lData = (int)(long)_iDict[_strKey];
				return true;
			}
			return false;
		}

		static public bool copyString(ref string _strData, IDictionary _iDict, string _strKey, bool _bLog = true)
		{
			if (DataCopyUtil.checkContainsValue(_iDict, _strKey, _bLog))
			{
				_strData = (string)_iDict[_strKey];
				return true;
			}
			return false;
		}

		static public bool copyLong(ref long _strData, IDictionary _iDict, string _strKey, bool _bLog = true)
		{
			if (DataCopyUtil.checkContainsValue(_iDict, _strKey, _bLog))
			{
				_strData = (long)_iDict[_strKey];
				return true;
			}
			return false;
		}

		static public bool copyFloat(ref float _strData, IDictionary _iDict, string _strKey, bool _bLog = true)
		{
			if (DataCopyUtil.checkContainsValue(_iDict, _strKey, _bLog))
			{
				_strData = (float)_iDict[_strKey];
				return true;
			}
			return false;
		}

		static public bool copyFloatV(ref float _strData, IDictionary _iDict, string _strKey, bool _bLog = true)
		{
			if (DataCopyUtil.checkContainsValue(_iDict, _strKey, _bLog))
			{
				var temp = _iDict[_strKey];
				_strData = float.Parse(temp.ToString());
				return true;
			}
			return false;
		}

		static public bool copyBool(ref bool _strData, IDictionary _iDict, string _strKey, bool _bLog = true)
		{
			if (DataCopyUtil.checkContainsValue(_iDict, _strKey, _bLog))
			{
				_strData = (bool)_iDict[_strKey];
				return true;
			}
			return false;
		}
	}


	[System.Serializable]
	public class SpreadSheetData
	{

		public int index;
		public int row;
		public int col;
		public string param;

		public bool IsRow(int _iRow)
		{
			if (row == _iRow)
			{
				return true;
			}
			return false;
		}

		public bool IsCol(int _iCol)
		{
			if (col == _iCol)
			{
				return true;
			}
			return false;
		}

		public bool Equal(int _iRow, int _iCol)
		{
			if (IsRow(_iRow) && IsCol(_iCol))
			{
				return true;
			}
			return false;
		}

		static public SpreadSheetData GetSpreadSheet(List<SpreadSheetData> _list, int _iRow, int _iCol)
		{
			foreach (SpreadSheetData data in _list)
			{
				if (data.Equal(_iRow, _iCol))
				{
					return data;
				}
			}
			return null;
		}

		// SpreadSheetデータを取得する
		static private void search(IDictionary _dict, ref bool _bRecord, ref SpreadSheetData _data, ref List<SpreadSheetData> _list)
		{
			foreach (var key in _dict.Keys)
			{
				if (_bRecord)
				{
					if (key.Equals("row") == true)
					{
						//Debug.Log (key);
						//Debug.Log (_dict [key]);
						//Debug.Log (_dict [key].GetType());
						_data.row = int.Parse((string)_dict[key]);
						//_data.row = int.Parse((string)_dict [key]);
					}
					else if (key.Equals("col") == true)
					{
						//DataCopyUtil.copyLongToInt (ref _data.col, _dict, key.ToString());
						_data.col = int.Parse((string)_dict[key]);
						//_data.col = int.Parse(_dict [key]);
					}
					else if (key.Equals("$t") == true)
					{
						DataCopyUtil.copyString(ref _data.param, _dict, key.ToString());
					}
					else
					{
						// むしろエラー
					}
				}
				if (key.Equals("gs$cell") == true)
				{
					_data = new SpreadSheetData();
					_bRecord = true;
				}

				if (_dict[key] is IDictionary)
				{
					//Debug.Log ("idict");
					search((IDictionary)_dict[key], ref _bRecord, ref _data, ref _list);

				}
				else if (_dict[key] is IList)
				{
					IList use_list = (IList)_dict[key];
					//Debug.Log (use_list.Count);
					foreach (IDictionary indict in use_list)
					{
						search((IDictionary)indict, ref _bRecord, ref _data, ref _list);
					}
				}
				else
				{
					;// nocalled
				}

				if (key.Equals("gs$cell") == true)
				{
					_list.Add(_data);
					_bRecord = false;
				}

			}
		}

		static public List<SpreadSheetData> ConvertSpreadSheetData(IDictionary _dict)
		{
			List<SpreadSheetData> ret = new List<SpreadSheetData>();
			SpreadSheetData data = new SpreadSheetData();
			bool bRecord = false;
			search(_dict, ref bRecord, ref data, ref ret);
			return ret;
		}
		/*
		 * ほかのシートの番号を知りたい場合はここにアクセスすればわかるってさ
		 * https://spreadsheets.google.com/feeds/worksheets/[key]/public/basic
		 * 
		 * 
		*/
	}
}


