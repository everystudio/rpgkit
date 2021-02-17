using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Events;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace csvutility
{
	[Serializable]
	public class JsonDataItem
	{
		public string key;
		public string value;
	}

	[System.Serializable]
	public class CsvDataParam
	{

		public void Load(Dictionary<string, string> param)
		{
			foreach (string key in param.Keys)
			{
				SetField(key.Replace("\"", ""), param[key]);
			}
		}

		public void SetField(string key, string value)
		{
			if (value == null)
			{
				value = "";
			}
			FieldInfo fieldInfo = this.GetType().GetField(key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo == null)
			{
				return;
			}
			//Debug.Log(string.Format("key:{0} value:{1}", key, value));
			if (fieldInfo.FieldType == typeof(int)) fieldInfo.SetValue(this, int.Parse(value));
			else if (fieldInfo.FieldType == typeof(long)) fieldInfo.SetValue(this, long.Parse(value));
			else if (fieldInfo.FieldType == typeof(string))
			{
				string strValue = value;
				if (value.Contains(':') && value.Contains('{') && value.Contains('}'))
				{
					// jsonデータだと思うので"はのけない
					// そもそもなんなの、この処理
				}
				else
				{
					strValue = value.Replace("\"", "");
				}
				fieldInfo.SetValue(this, strValue);
			}
			else if (fieldInfo.FieldType == typeof(float)) fieldInfo.SetValue(this, float.Parse(value));
			else if (fieldInfo.FieldType == typeof(double)) fieldInfo.SetValue(this, double.Parse(value));
			else if (fieldInfo.FieldType == typeof(bool)) fieldInfo.SetValue(this, bool.Parse(value));
			// 他の型にも対応させたいときには適当にここに。enumとかもどうにかなりそう。
		}
		public string GetString(string _strKey)
		{
			FieldInfo fieldInfo = this.GetType().GetField(_strKey, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				if (fieldInfo.FieldType == typeof(string))
				{
					return (string)fieldInfo.GetValue(this);
					//return propertyInfo.GetValue(this, null).ToString();//.Replace("\n", "\r\n");
				}
				else
				{
					return fieldInfo.GetValue(this).ToString();
				}
			}
			return "";
		}

		public void Set(Dictionary<string, string> _dict)
		{
			foreach (string key in _dict.Keys)
			{
				FieldInfo fieldInfo = GetType().GetField(key);
				if (fieldInfo.FieldType == typeof(int))
				{
					int iValue = int.Parse(_dict[key]);
					fieldInfo.SetValue(this, iValue);
				}
				else if (fieldInfo.FieldType== typeof(string))
				{
					fieldInfo.SetValue(this, _dict[key].Replace("\"", ""));
				}
				else if (fieldInfo.FieldType == typeof(double))
				{
					fieldInfo.SetValue(this, double.Parse(_dict[key]));
				}
				else if (fieldInfo.FieldType == typeof(float))
				{
					fieldInfo.SetValue(this, float.Parse(_dict[key]));
				}
				else if (fieldInfo.FieldType == typeof(bool))
				{
					fieldInfo.SetValue(this, bool.Parse(_dict[key]));
				}
				else
				{
					Debug.LogError("error type unknown");
				}
			}
		}

		public bool Equals(string _strWhere)
		{
			string[] div_array = _strWhere.Trim().Split(' ');
			bool bRet = true;
			for (int i = 0; i < div_array.Length; i += 4)
			{
				FieldInfo fieldInfo = GetType().GetField(div_array[i]);
				if (fieldInfo.FieldType == typeof(int))
				{
					int intparam = (int)fieldInfo.GetValue(this);
					string strJudge = div_array[i + 1];
					int intcheck = int.Parse(div_array[i + 2]);
					if (strJudge.Equals("="))
					{
						if (intparam != intcheck)
						{
							bRet = false;
						}
					}
					else if (strJudge.Equals("!="))
					{
						if (intparam == intcheck)
						{
							bRet = false;
						}
					}
					else
					{
					}
				}
			}
			return bRet;
		}
	}

	//public abstract class CsvData<T> : MonoBehaviour where T : CsvDataParam, new() {
	public abstract class CsvData<T> where T : CsvDataParam, new()
	{
		// 作る必要はないんだけど一応ね
		public List<T> list = new List<T>();
		public List<T> All { get { return list; } }

		private bool m_bLoadedActionFinished;
		private void LoadedAction()
		{
			if (m_bLoadedActionFinished == false)
			{
				loadedAction();
				m_bLoadedActionFinished = true;
			}
		}
		virtual protected void loadedAction()
		{

		}

		virtual public bool Load(string _strFilename, string _strPath)
		{
			m_bLoadedActionFinished = false;

			bool bRet = false;
			list = new List<T>();

			string file = string.Format("{0}.csv", _strFilename);
			string fullpath = System.IO.Path.Combine(_strPath, file);

			if (System.IO.File.Exists(fullpath) == false)
			{
				//Debug.LogError ("file not exists:" + fullpath );
				return false;
			}

#if !UNITY_WEBPLAYER
			FileInfo fi = new FileInfo(fullpath);

			StreamReader sr = new StreamReader(fi.OpenRead());

			try
			{
				//int iLoop = 0;
				string strFirst = sr.ReadLine();
				var headerElements = strFirst.Split(',');

				while (sr.Peek() != -1)
				{
					string strLine = sr.ReadLine();
					ParseLine(strLine, headerElements);
				}
				sr.Close();
				bRet = true;
			}
			catch (System.Exception ex)
			{
				Debug.LogError(ex);
				bRet = false;
			}
#endif

			if (bRet)
			{
				LoadedAction();
			}
			return bRet;
		}
		virtual public bool Load()
		{
			return Load(m_saveFilename);
		}
		virtual public bool Load(string _strFilename)
		{
			return Load(_strFilename, Application.persistentDataPath);
		}

		virtual public bool LoadMulti()
		{
			return LoadMulti(m_saveFilename);
		}

		virtual public bool LoadMulti(string _strFilename)
		{
			bool bRet = true;
			// Loadの中で解除してくれるのでそれを利用
			//m_bLoadedActionFinished = false;
			if (Load(_strFilename) == false)
			{
				bRet = LoadResources(_strFilename);

				if (bRet)
				{
					LoadedAction();
				}
			}
			return bRet;
		}
		/*
		virtual public bool Load(){
			return Load (typeof(T).ToString ());
		}
		*/

		virtual public bool Load(TextAsset _textAsset)
		{
			bool bRet = false;
			string text = _textAsset.text;
			text = text.Trim().Replace("\r", "") + "\n";
			var lines = text.Split('\n').ToList();

			// header
			var headerElements = lines[0].Split(',');
			lines.RemoveAt(0); // header
			foreach( string key in headerElements)
			{
				//Debug.Log(key);
			}
			// body
			list = new List<T>();
			foreach (var line in lines) ParseLine(line, headerElements);
			bRet = true;
			if (bRet)
			{
				LoadedAction();
			}
			return bRet;
		}

		virtual public bool LoadResources(string _strFilename)
		{
			bool bRet;
			string path = _strFilename;
			//Debug.Log ( path );
			try
			{
				TextAsset textAsset = ((TextAsset)Resources.Load(path, typeof(TextAsset)));
				bRet = Load(textAsset);
			}
			catch (System.Exception ex)
			{
				if (ex != null)
				{
					Debug.LogError(_strFilename);
					Debug.LogError(ex);
				}
				bRet = false;
			}
			return bRet;
		}

		private void ParseLine(string line, string[] headerElements)
		{
			List<string> replace_items = new List<string>();
			int iSubStartIndex = 0;
			int iBucketNum = 0;
			bool bCollect = false;
			for (int i = 0; i < line.Length; i++)
			{
				if (line[i].Equals('{'))
				{
					iBucketNum += 1;
					if (bCollect == false)
					{
						iSubStartIndex = i;
						bCollect = true;
					}
				}
				if (line[i].Equals('}'))
				{
					iBucketNum -= 1;
					if (bCollect == true)
					{
						if (iBucketNum == 0)
						{
							bCollect = false;
							string sub = line.Substring(iSubStartIndex, i - iSubStartIndex);
							line = line.Remove(iSubStartIndex, i - iSubStartIndex);
							string strReplaceKey = string.Format("replace_{0}", replace_items.Count);
							line = line.Insert(iSubStartIndex, strReplaceKey);
							replace_items.Add(sub);
							//Debug.LogWarning(sub);
							i = iSubStartIndex;
						}
					}
				}
			}

			var elements = line.Split(',');

			if (elements.Length == 1) return;
			if (elements.Length != headerElements.Length)
			{
				Debug.LogWarning(string.Format("can't load: {0}", line));
				return;
			}

			if (0 < replace_items.Count)
			{
				for (int i = 0; i < replace_items.Count; i++)
				{
					string strReplaceKey = string.Format("replace_{0}", i);

					for (int j = 0; j < elements.Length; j++)
					{
						if (elements[j].Contains(strReplaceKey))
						{
							elements[j] = elements[j].Replace(strReplaceKey, replace_items[i]);
						}
					}
				}
			}

			var param = new Dictionary<string, string>();
			for (int i = 0; i < elements.Length; ++i)
			{
				param.Add(headerElements[i], elements[i]);
			}
			var master = new T();
			master.Load(param);
			list.Add(master);
		}

		// ここ、デフォルト引数にあまり頼らないようにしてください
		public string m_saveFilename;
		public void SetSaveFilename(string _strFilename)
		{
			m_saveFilename = _strFilename;
		}
		public void Save()
		{
			try
			{
				if (m_saveFilename.Length == 0)
				{
					throw new System.Exception("no set saveFilename");
				}
				Save(m_saveFilename);
			}
			catch
			{
				;// 特に何をするわけでもない
				Debug.LogError("no set savefilename");
			}
		}
		public void Save(string _strFilename)
		{
			// 保存前に処理をしたい場合実装する
			preSave();
			if (_strFilename.Equals("") == true)
			{
				_strFilename = typeof(T).ToString();
			}
			save(_strFilename);
		}
		public void SaveEditor(string _strPath, string _strFilename)
		{
			save_editor(_strPath, _strFilename);
		}

		// 同じディレクトリにある名前違いのファイルを移動させる
		private void fileMove(string _strPath, string _strSource, string _strDest)
		{
			// 例外は上でとってください
			System.IO.File.Copy(
				System.IO.Path.Combine(_strPath, _strSource),
				System.IO.Path.Combine(_strPath, _strDest),
				true);

			System.IO.File.Delete(System.IO.Path.Combine(_strPath, _strSource));
		}

		private bool WritableField(FieldInfo _info)
		{
			bool bRet = false;

			if (_info.FieldType == typeof(int))
			{
				bRet = true;
			}
			else if (_info.FieldType == typeof(float))
			{
				bRet = true;
			}
			else if (_info.FieldType == typeof(string))
			{
				bRet = true;
			}
			else if (_info.FieldType == typeof(bool))
			{
				bRet = true;
			}
			else
			{
				//Debug.LogError(_info.PropertyType);
			}
			return bRet;
		}

		/*
		 * 特に指定が無い場合は自動書き込み
		 * 独自実装をしたい場合は個別にoverrideしてください
		 * */
		virtual protected void save(string _strFilename)
		{
			//Debug.LogWarning (string.Format( "kvs.save {0}" , list.Count));
			//int test = 0;
			//Debug.Log(test++);
			StreamWriter sw;
			try
			{
				string strTempFilename = string.Format("{0}.csv.tmp", _strFilename);
				EditDirectory.MakeDirectory(strTempFilename);
				sw = Textreader.Open(Application.persistentDataPath, strTempFilename);
				//Debug.Log(test++);
				T dummy = new T();
				FieldInfo[] infoArray = dummy.GetType().GetFields();
				//Debug.Log(test++);
				bool bIsFirst = true;
				string strHead = "";
				foreach (FieldInfo info in infoArray)
				{
					if (!WritableField(info))
					{
						continue;
					}
					if (bIsFirst == true)
					{
						bIsFirst = false;
					}
					else
					{
						strHead += ",";
					}
					strHead += info.Name;
				}
				//Debug.Log(test++);

				Textreader.Write(sw, strHead);
				//Debug.Log(test++);
				foreach (T data in list)
				{
					bIsFirst = true;
					string strData = "";
					foreach (FieldInfo info in infoArray)
					{
						//Debug.Log(test++);
						if (!WritableField(info))
						{
							//Debug.Log(test++);
							continue;
						}

						if (bIsFirst == true)
						{
							bIsFirst = false;
							//Debug.Log(test++);
						}
						else
						{
							strData += ",";
							//Debug.Log(test++);
						}
						//Debug.Log(info.Name);
						//Debug.Log(data);
						//Debug.Log(data.GetString(info.Name));
						strData += data.GetString(info.Name);
						//Debug.Log(strData);
					}
					//Debug.Log(strData);
					Textreader.Write(sw, strData);
				}
				Textreader.Close(sw);

				fileMove(
					Application.persistentDataPath,
					string.Format("{0}.csv.tmp", _strFilename),
					string.Format("{0}.csv", _strFilename));
			}
			catch (Exception ex)
			{
				Debug.LogError(_strFilename);
				Debug.LogError(ex);
				return;
			}
			return;
		}

		virtual protected void save_editor(string _strPath, string _strFilename)
		{
			StreamWriter sw;
			try
			{
				string strLocalFilename = Path.Combine(_strPath, _strFilename);
				string strTempFileName = string.Format("{0}.csv.temp", strLocalFilename);
				EditDirectory.MakeDirectory(_strPath, Application.dataPath);
				sw = Textreader.Open(Application.dataPath, strTempFileName);
				//Debug.Log(test++);
				T dummy = new T();
				FieldInfo[] infoArray = dummy.GetType().GetFields();
				//Debug.Log(test++);
				bool bIsFirst = true;
				string strHead = "";
				foreach (FieldInfo info in infoArray)
				{
					if (!WritableField(info))
					{
						continue;
					}
					if (bIsFirst == true)
					{
						bIsFirst = false;
					}
					else
					{
						strHead += ",";
					}
					strHead += info.Name;
				}

				Textreader.Write(sw, strHead);
				foreach (T data in list)
				{
					bIsFirst = true;
					string strData = "";
					foreach (FieldInfo info in infoArray)
					{
						if (!WritableField(info))
						{
							continue;
						}

						if (bIsFirst == true)
						{
							bIsFirst = false;
						}
						else
						{
							strData += ",";
						}
						string temp = data.GetString(info.Name);
						//Debug.Log(string.Format("info.Name{0} value={1}", info.Name, temp));
						//temp = temp.Replace("\n", "\\n");
						/*
						if (temp.Contains("\n"))
						{
							//Debug.Log(temp);
						}
						*/

						strData += temp;


					}
					strData = strData.Replace("\n", "\\n");
					Textreader.Write(sw, strData);
				}
				Textreader.Close(sw);

				fileMove(
					Application.dataPath,
					string.Format("{0}", strTempFileName),
					string.Format("{0}.csv", strLocalFilename));
			}
			catch (Exception ex)
			{
				Debug.LogError(_strFilename);
				Debug.LogError(ex);
				return;
			}
			return;
		}


		protected virtual void preSave()
		{
			return;
		}

		public virtual List<T> Select(string _strWhere)
		{
			List<T> ret_list = new List<T>();
			foreach (T param in list)
			{
				if (param.Equals(_strWhere))
				{
					ret_list.Add(param);
				}
			}
			return ret_list;
		}

		public virtual T SelectOne(string _strWhere)
		{
			List<T> ret_list = new List<T>();
			foreach (T param in list)
			{
				if (param.Equals(_strWhere))
				{
					ret_list.Add(param);
				}
			}
			if (0 < ret_list.Count)
			{
				return ret_list[0];
			}
			return new T();
		}

		virtual public int Update(Dictionary<string, string> _dictUpdate, string _strWhere)
		{
			List<T> update_list = new List<T>();
			update_list = Select(_strWhere);
			foreach (T param in update_list)
			{
				param.Set(_dictUpdate);
			}
			return update_list.Count;
		}

		virtual protected T makeParam(List<SpreadSheetData> _list, int _iSerial, int _iRow, string[] _keyArr)
		{
			T retParam = new T();

			// このあたりは共通化を頼る
			// 並びとか変わると残念な結果になりますので！
			int iIndex = 1;
			//PropertyInfo[] infoArray = retParam.GetType().GetProperties();

			foreach (string key in _keyArr)
			{
				SpreadSheetData ssd = SpreadSheetData.GetSpreadSheet(_list, _iRow, iIndex++);
				if (ssd != null)
				{
					retParam.SetField(key, ssd.param);
				}
			}
			return retParam;
		}

		private string[] GetKeys(List<SpreadSheetData> _list)
		{
			List<string> ret = new List<string>();
			foreach (SpreadSheetData data in _list)
			{
				if (data.IsRow(1))
				{
					ret.Add(data.param);
				}
			}
			return ret.ToArray();
		}

		public void Input(List<SpreadSheetData> _list)
		{
			int iSerial = 0;
			list.Clear();
			// 先頭は除外する

			string[] key_arr = GetKeys(_list);
			_list.RemoveAt(0);
			foreach (SpreadSheetData data in _list)
			{
				if (data.IsCol(1) && !data.param.Contains("#"))
				{
					list.Add(makeParam(_list, iSerial, data.row, key_arr));
					iSerial += 1;
				}
			}
			return;
		}
		public class EventBool : UnityEvent<bool> { }
		public EventBool OnRecievedResultEvent = new EventBool();

		public class DataListEvent : UnityEvent<List<T>>
		{
		}
		public DataListEvent OnRecieveData = new DataListEvent();
		public delegate void OnRecievedResult(bool _bResult);       // まぁ基本真しか返さんけど
		
		protected string m_strSpreadSheet;
		protected string m_strSheetName;

		public IEnumerator SpreadSheet(string _strSheetId, string _strSheetName, Action _onRecieved)
		{
			string sheet_id = "";
			{
				string strUrl = string.Format("https://spreadsheets.google.com/feeds/worksheets/{0}/public/basic", _strSheetId);

				UnityWebRequest req = UnityWebRequest.Get(strUrl);
				yield return req.SendWebRequest();
				while (req.downloadProgress < 1f)
				{
					if (req.downloadProgress > 0)
					{
					}
					yield return null;
				}
				if (req.isNetworkError)
				{
					Debug.LogError("error:" + strUrl);
					yield break;
				}
				//Debug.LogWarning(req.downloadHandler.text);
				sheet_id = ParseSpreadSheetSerial(_strSheetId, _strSheetName, req.downloadHandler.text);

				//Debug.LogWarning(sheet_id);
			}


			{
				string strUrlSub = string.Format("https://spreadsheets.google.com/feeds/cells/{0}/{1}/public/values?alt=json", _strSheetId, sheet_id);
				UnityWebRequest req = UnityWebRequest.Get(strUrlSub);
				yield return req.SendWebRequest();
				while (req.downloadProgress < 1f)
				{
					if (req.downloadProgress > 0)
					{
					}
					yield return null;
				}
				if (req.isNetworkError)
				{
					Debug.LogError("error:" + strUrlSub);
					yield break;
				}

				IDictionary dictRecieveData = null;
#if USE_MINIJSON
				dictRecieveData = (IDictionary)MiniJSON.Json.Deserialize(req.downloadHandler.text);
#endif
				if (dictRecieveData != null)
				{
					Input(SpreadSheetData.ConvertSpreadSheetData(dictRecieveData));
				}
			}

			_onRecieved.Invoke();

		}

		/*
		public void onRecievedWorksheet(TNetworkData _networkData)
		{
			string sheet_id = CommonNetwork.ParseSpreadSheetSerial(m_strSpreadSheet, m_strSheetName, _networkData.m_strData);
			LoadSpreadSheet(m_strSpreadSheet, sheet_id);
		}
		*/


		// エディター用機能
		public void MakeMasterData(string _strSpreadSheet, string _strSheetName, string _strLocalPath, string _strFilename, List<IEnumerator> _taskList)
		{
			_taskList.Add(SpreadSheet(_strSpreadSheet, _strSheetName, () =>
			{
				SaveEditor(_strLocalPath, _strFilename);
#if UNITY_EDITOR
			Debug.Log(string.Format("Assets/{0}/{1}.csv", _strLocalPath, _strFilename));
				AssetImporter importer = AssetImporter.GetAtPath(string.Format("Assets/{0}/{1}.csv", _strLocalPath, _strFilename));

				importer.assetBundleName = "master_data";
#endif
			}));
		}

		public string ParseSpreadSheetSerial(string _strSpreadSheetId, string _strSheetName, string _strSource)
		{

			int index = _strSource.IndexOf(string.Format(">{0}<", _strSheetName));
			if (index < 0)
			{
				Debug.Log(_strSheetName);
			}
			int index2 = _strSource.IndexOf(string.Format("{0}/", _strSpreadSheetId), index);
			int index2End = index2 + string.Format("{0}/", _strSpreadSheetId).Length;
			int index3 = _strSource.IndexOf("/", index2End);
			return _strSource.Substring(index2End, index3 - index2End);
		}

	}


}






