using UnityEngine;
using System.Collections;
using System.IO;

namespace csvutility
{
	public class EditDirectory : MonoBehaviour
	{

		/*
		 * フォルダ系のコピー削除英は終わったら		
		 * AssetDatabase.Refresh ();
		 * こいつを呼び出してください
		 * 
		*/
		public static void AddCopy(string _strSourcePath, string _strCopyPath, string _strTail = "")
		{

			MakeDirectory(_strCopyPath, Application.dataPath + "/..");

			//ファイルをコピー
			foreach (var file in Directory.GetFiles(_strSourcePath))
			{
				string filename = file + _strTail;
				string fullpath = System.IO.Path.Combine(_strCopyPath, Path.GetFileName(filename));
				if (File.Exists(fullpath) == true)
				{
					File.Delete(fullpath);
				}
				Debug.Log(file);
				Debug.Log(Path.Combine(_strCopyPath, Path.GetFileName(filename)));
				File.Copy(file, Path.Combine(_strCopyPath, Path.GetFileName(filename)));
			}

			//ディレクトリの中のディレクトリも再帰的にコピー
			foreach (var dir in Directory.GetDirectories(_strSourcePath))
			{
				AddCopy(dir, Path.Combine(_strCopyPath, Path.GetFileName(dir)), _strTail);
			}
			return;
		}



		/// <summary>
		/// ディレクトリとその中身を上書きコピー
		/// </summary>
		public static void CopyAndReplace(string sourcePath, string copyPath)
		{
			//既にディレクトリがある場合は削除し、新たにディレクトリ作成
			Delete(copyPath);
			Directory.CreateDirectory(copyPath);

			//ファイルをコピー
			foreach (var file in Directory.GetFiles(sourcePath))
			{
				File.Copy(file, Path.Combine(copyPath, Path.GetFileName(file)));
			}

			//ディレクトリの中のディレクトリも再帰的にコピー
			foreach (var dir in Directory.GetDirectories(sourcePath))
			{
				CopyAndReplace(dir, Path.Combine(copyPath, Path.GetFileName(dir)));
			}
		}

		//Assetsディレクトリ以下にあるTestディレクトリを削除
		/// <summary>
		/// 指定したディレクトリとその中身を全て削除する
		/// </summary>
		public static void Delete(string targetDirectoryPath)
		{
			if (!Directory.Exists(targetDirectoryPath))
			{
				return;
			}

			//ディレクトリ以外の全ファイルを削除
			string[] filePaths = Directory.GetFiles(targetDirectoryPath);
			foreach (string filePath in filePaths)
			{
				File.SetAttributes(filePath, FileAttributes.Normal);
				File.Delete(filePath);
			}

			//ディレクトリの中のディレクトリも再帰的に削除
			string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
			foreach (string directoryPath in directoryPaths)
			{
				Delete(directoryPath);
			}
#if !UNITY_WEBPLAYER
			//中が空になったらディレクトリ自身も削除
			Directory.Delete(targetDirectoryPath, false);
#endif
		}

		public static void MakeDirectory(string _strDirectories, string _strRoot = "")
		{

			string[] elements = _strDirectories.Split('/');

			string strCurrentPath = _strRoot;
			if (strCurrentPath.Equals("") == true)
			{
				strCurrentPath = Application.persistentDataPath;
			}

			int iCount = 0;
			foreach (string subDirectory in elements)
			{
				iCount += 1;
				if (iCount == elements.Length)
				{
					if (subDirectory.Contains(".") == true)
					{
						break;
					}
				}
				strCurrentPath = Path.Combine(strCurrentPath, subDirectory);
				//Debug.Log (strCurrentPath);
				if (!Directory.Exists(strCurrentPath))
				{
					Debug.LogWarning(string.Format("create directory:{0}", strCurrentPath));
					Directory.CreateDirectory(strCurrentPath);
				}
			}
			return;
		}
	}
}


