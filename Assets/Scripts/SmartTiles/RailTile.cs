using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace rpgkit
{
	[CreateAssetMenu( fileName="RailTile" ,menuName = "SmartTile/RailTile")]
	public class RailTile : TileBase
	{
		public Sprite[] m_Sprites;

		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
			int mask = (HasRail(position + Vector3Int.up, tilemap) ? 1 : 0)
					+ (HasRail(position + Vector3Int.right, tilemap) ? 2 : 0)
					+ (HasRail(position + Vector3Int.down, tilemap) ? 4 : 0)
					+ (HasRail(position + Vector3Int.left, tilemap) ? 8 : 0);

			tileData.sprite = m_Sprites[mask];
			tileData.flags = TileFlags.LockTransform;
			tileData.colliderType = Tile.ColliderType.Sprite;
		}

		public bool HasRail(Vector3Int position, ITilemap tilemap)
		{
			return tilemap.GetTile(position) == this;
		}

		public override void RefreshTile(Vector3Int position, ITilemap tilemap)
		{
			foreach (var p in new BoundsInt(-1, -1, 0, 3, 3, 1).allPositionsWithin)
			{
				tilemap.RefreshTile(position + p);

			}
		}
	}


#if UNITY_EDITOR
	[CustomEditor(typeof(RailTile))]
	public class RailTileEditor : Editor
	{
		private RailTile tile { get { return (target as RailTile); } }

		public void OnEnable()
		{
			if (tile.m_Sprites == null || tile.m_Sprites.Length != 16)
			{
				tile.m_Sprites = new Sprite[16];
				EditorUtility.SetDirty(tile);
			}
		}


		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("Place sprites shown based on the contents of the sprite.");
			EditorGUILayout.Space();

			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 210;

			EditorGUI.BeginChangeCheck();

			//tile.m_layerIndex = EditorGUILayout.IntField("Layer", tile.m_layerIndex);

			tile.m_Sprites[0] = (Sprite)EditorGUILayout.ObjectField("Single", tile.m_Sprites[0], typeof(Sprite), false, null);
			tile.m_Sprites[1] = (Sprite)EditorGUILayout.ObjectField("Up", tile.m_Sprites[1], typeof(Sprite), false, null);
			tile.m_Sprites[2] = (Sprite)EditorGUILayout.ObjectField("Right", tile.m_Sprites[2], typeof(Sprite), false, null);
			tile.m_Sprites[3] = (Sprite)EditorGUILayout.ObjectField("Up-Right", tile.m_Sprites[3], typeof(Sprite), false, null);
			tile.m_Sprites[4] = (Sprite)EditorGUILayout.ObjectField("Down", tile.m_Sprites[4], typeof(Sprite), false, null);
			tile.m_Sprites[5] = (Sprite)EditorGUILayout.ObjectField("Up-Down", tile.m_Sprites[5], typeof(Sprite), false, null);
			tile.m_Sprites[6] = (Sprite)EditorGUILayout.ObjectField("Right-Down", tile.m_Sprites[6], typeof(Sprite), false, null);
			tile.m_Sprites[7] = (Sprite)EditorGUILayout.ObjectField("Up-Right-Down", tile.m_Sprites[7], typeof(Sprite), false, null);
			tile.m_Sprites[8] = (Sprite)EditorGUILayout.ObjectField("Left", tile.m_Sprites[8], typeof(Sprite), false, null);
			tile.m_Sprites[9] = (Sprite)EditorGUILayout.ObjectField("Up-Left", tile.m_Sprites[9], typeof(Sprite), false, null);
			tile.m_Sprites[10] = (Sprite)EditorGUILayout.ObjectField("Right-Left", tile.m_Sprites[10], typeof(Sprite), false, null);
			tile.m_Sprites[11] = (Sprite)EditorGUILayout.ObjectField("Up-Right-Left", tile.m_Sprites[11], typeof(Sprite), false, null);
			tile.m_Sprites[12] = (Sprite)EditorGUILayout.ObjectField("Down-Left", tile.m_Sprites[12], typeof(Sprite), false, null);
			tile.m_Sprites[13] = (Sprite)EditorGUILayout.ObjectField("Up-Down-Left", tile.m_Sprites[13], typeof(Sprite), false, null);
			tile.m_Sprites[14] = (Sprite)EditorGUILayout.ObjectField("Right-Down-Left", tile.m_Sprites[14], typeof(Sprite), false, null);
			tile.m_Sprites[15] = (Sprite)EditorGUILayout.ObjectField("All", tile.m_Sprites[15], typeof(Sprite), false, null);

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(tile);

			EditorGUIUtility.labelWidth = oldLabelWidth;
		}
	}
#endif
}

