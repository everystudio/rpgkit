using UnityEngine;
using System.Collections;
using System;
using System.IO;
namespace csvutility
{
	public class Textreader : MonoBehaviour
	{

		void Start()
		{
			Debug.Log(Application.dataPath);
			//write("Hello, world!");
			//read();
		}
		static public StreamWriter Open(string _strPath, string _strFilename)
		{
			string pathDB = System.IO.Path.Combine(_strPath, _strFilename);

			if (System.IO.File.Exists(pathDB))
			{
				System.IO.File.Delete(pathDB);
			}
			FileInfo fi = new FileInfo(pathDB);
			StreamWriter sw = fi.AppendText();
			return sw;
		}

		// Openを使う機能がApplication.persistentDataPathを使うのがほとんどではありますが、
		// あまりよい実装ではないですね。というわけでラッパー扱い
		static public StreamWriter Open(string _strFilename)
		{
			return Open(Application.persistentDataPath, _strFilename);
		}
		static public void Write(StreamWriter _sw, string _strMessage)
		{
			_sw.WriteLine(_strMessage);
		}
		static public void Close(StreamWriter _sw)
		{
			_sw.Flush();
			_sw.Close();
			return;
		}

		static public void SaveWrite(string _strFilename, string _strMessage)
		{
			string pathDB = System.IO.Path.Combine(Application.persistentDataPath, _strFilename);
			FileInfo fi = new FileInfo(pathDB);
			StreamWriter sw = fi.AppendText();
			sw.WriteLine(_strMessage);
			sw.Flush();
			sw.Close();
			return;
		}

		// 書き込み

		static public void Append(string _path, string _text)
		{
			FileInfo fi = new FileInfo(Application.dataPath + _path);
			//write
			StreamWriter sw = fi.AppendText();
			//sw.Write(text);      // 未改行
			sw.WriteLine(_text);        // 改行
			sw.Flush();
			sw.Close();
		}

		/**
		 * ほぼデバッグ用
		 * */
		static public void write(string text)
		{

			FileInfo fi = new FileInfo(Application.dataPath + "/test.txt");
			//write
			StreamWriter sw = fi.AppendText();
			//sw.Write(text);      // 未改行
			sw.WriteLine(text);        // 改行
			sw.Flush();
			sw.Close();
		}

		// 読み込み
		public void read()
		{
#if !UNITY_WEBPLAYER
			FileInfo fi = new FileInfo(Application.dataPath + "/test.txt");
			StreamReader sr = new StreamReader(fi.OpenRead());
			while (sr.Peek() != -1)
			{
				print(sr.ReadLine());
			}
			sr.Close();
#endif
		}
	}
}

