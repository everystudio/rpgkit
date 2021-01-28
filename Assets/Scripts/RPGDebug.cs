using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
	public class RPGDebug : MonoBehaviour
	{
		private const string _editorPrefsDebugLogs = "DebugLogsEnabled";
		private const string _editorPrefsDebugDraws = "DebugDrawsEnabled";

		/// <summary>
		/// whether or not debug draws should be executed
		/// </summary>
		public static bool DebugDrawEnabled
		{
			get
			{
				if (PlayerPrefs.HasKey(_editorPrefsDebugDraws))
				{
					return (PlayerPrefs.GetInt(_editorPrefsDebugDraws) == 0) ? false : true;
				}
				else
				{
					return true;
				}
			}
			private set { }
		}


		/// <summary>
		/// Draws a gizmo arrow going from the origin position and along the direction Vector3
		/// </summary>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="color">Color.</param>
		public static void DrawGizmoArrow(Vector3 origin, Vector3 direction, Color color, float arrowHeadLength = 3f, float arrowHeadAngle = 25f)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Gizmos.color = color;
			Gizmos.DrawRay(origin, direction);

			DrawArrowEnd(true, origin, direction, color, arrowHeadLength, arrowHeadAngle);
		}

		/// <summary>
		/// Draws a debug arrow going from the origin position and along the direction Vector3
		/// </summary>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="color">Color.</param>
		public static void DebugDrawArrow(Vector3 origin, Vector3 direction, Color color, float arrowHeadLength = 0.2f, float arrowHeadAngle = 35f)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Debug.DrawRay(origin, direction, color);

			DrawArrowEnd(false, origin, direction, color, arrowHeadLength, arrowHeadAngle);
		}

		/// <summary>
		/// Draws a debug arrow going from the origin position and along the direction Vector3
		/// </summary>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="color">Color.</param>
		/// <param name="arrowLength">Arrow length.</param>
		/// <param name="arrowHeadLength">Arrow head length.</param>
		/// <param name="arrowHeadAngle">Arrow head angle.</param>
		public static void DebugDrawArrow(Vector3 origin, Vector3 direction, Color color, float arrowLength, float arrowHeadLength = 0.20f, float arrowHeadAngle = 35.0f)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Debug.DrawRay(origin, direction * arrowLength, color);

			DrawArrowEnd(false, origin, direction * arrowLength, color, arrowHeadLength, arrowHeadAngle);
		}

		/// <summary>
		/// Draws a debug cross of the specified size and color at the specified point
		/// </summary>
		/// <param name="spot">Spot.</param>
		/// <param name="crossSize">Cross size.</param>
		/// <param name="color">Color.</param>
		public static void DebugDrawCross(Vector3 spot, float crossSize, Color color)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Vector3 tempOrigin = Vector3.zero;
			Vector3 tempDirection = Vector3.zero;

			tempOrigin.x = spot.x - crossSize / 2;
			tempOrigin.y = spot.y - crossSize / 2;
			tempOrigin.z = spot.z;
			tempDirection.x = 1;
			tempDirection.y = 1;
			tempDirection.z = 0;
			Debug.DrawRay(tempOrigin, tempDirection * crossSize, color);

			tempOrigin.x = spot.x - crossSize / 2;
			tempOrigin.y = spot.y + crossSize / 2;
			tempOrigin.z = spot.z;
			tempDirection.x = 1;
			tempDirection.y = -1;
			tempDirection.z = 0;
			Debug.DrawRay(tempOrigin, tempDirection * crossSize, color);
		}

		/// <summary>
		/// Draws the arrow end for DebugDrawArrow
		/// </summary>
		/// <param name="drawGizmos">If set to <c>true</c> draw gizmos.</param>
		/// <param name="arrowEndPosition">Arrow end position.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="color">Color.</param>
		/// <param name="arrowHeadLength">Arrow head length.</param>
		/// <param name="arrowHeadAngle">Arrow head angle.</param>
		private static void DrawArrowEnd(bool drawGizmos, Vector3 arrowEndPosition, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 40.0f)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			if (direction == Vector3.zero)
			{
				return;
			}
			Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back;
			Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back;
			Vector3 up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
			Vector3 down = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;
			if (drawGizmos)
			{
				Gizmos.color = color;
				Gizmos.DrawRay(arrowEndPosition + direction, right * arrowHeadLength);
				Gizmos.DrawRay(arrowEndPosition + direction, left * arrowHeadLength);
				Gizmos.DrawRay(arrowEndPosition + direction, up * arrowHeadLength);
				Gizmos.DrawRay(arrowEndPosition + direction, down * arrowHeadLength);
			}
			else
			{
				Debug.DrawRay(arrowEndPosition + direction, right * arrowHeadLength, color);
				Debug.DrawRay(arrowEndPosition + direction, left * arrowHeadLength, color);
				Debug.DrawRay(arrowEndPosition + direction, up * arrowHeadLength, color);
				Debug.DrawRay(arrowEndPosition + direction, down * arrowHeadLength, color);
			}
		}

		/// <summary>
		/// Draws a gizmo sphere of the specified size and color at a position
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="size">Size.</param>
		/// <param name="color">Color.</param>
		public static void DrawGizmoPoint(Vector3 position, float size, Color color)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}
			Gizmos.color = color;
			Gizmos.DrawWireSphere(position, size);
		}

		/// <summary>
		/// Draws a cube at the specified position, and of the specified color and size
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="color">Color.</param>
		/// <param name="size">Size.</param>
		public static void DrawCube(Vector3 position, Color color, Vector3 size)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Vector3 halfSize = size / 2f;

			Vector3[] points = new Vector3[]
			{
				position + new Vector3(halfSize.x,halfSize.y,halfSize.z),
				position + new Vector3(-halfSize.x,halfSize.y,halfSize.z),
				position + new Vector3(-halfSize.x,-halfSize.y,halfSize.z),
				position + new Vector3(halfSize.x,-halfSize.y,halfSize.z),
				position + new Vector3(halfSize.x,halfSize.y,-halfSize.z),
				position + new Vector3(-halfSize.x,halfSize.y,-halfSize.z),
				position + new Vector3(-halfSize.x,-halfSize.y,-halfSize.z),
				position + new Vector3(halfSize.x,-halfSize.y,-halfSize.z),
			};

			Debug.DrawLine(points[0], points[1], color);
			Debug.DrawLine(points[1], points[2], color);
			Debug.DrawLine(points[2], points[3], color);
			Debug.DrawLine(points[3], points[0], color);
		}

		/// <summary>
		/// Draws a cube at the specified position, offset, and of the specified size
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="offset"></param>
		/// <param name="cubeSize"></param>
		/// <param name="wireOnly"></param>
		public static void DrawGizmoCube(Transform transform, Vector3 offset, Vector3 cubeSize, bool wireOnly)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Matrix4x4 rotationMatrix = transform.localToWorldMatrix;
			Gizmos.matrix = rotationMatrix;
			if (wireOnly)
			{
				Gizmos.DrawWireCube(offset, cubeSize);
			}
			else
			{
				Gizmos.DrawCube(offset, cubeSize);
			}
		}

		/// <summary>
		/// Draws a gizmo rectangle
		/// </summary>
		/// <param name="center">Center.</param>
		/// <param name="size">Size.</param>
		/// <param name="color">Color.</param>
		public static void DrawGizmoRectangle(Vector2 center, Vector2 size, Color color)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Gizmos.color = color;

			Vector3 v3TopLeft = new Vector3(center.x - size.x / 2, center.y + size.y / 2, 0);
			Vector3 v3TopRight = new Vector3(center.x + size.x / 2, center.y + size.y / 2, 0); ;
			Vector3 v3BottomRight = new Vector3(center.x + size.x / 2, center.y - size.y / 2, 0); ;
			Vector3 v3BottomLeft = new Vector3(center.x - size.x / 2, center.y - size.y / 2, 0); ;

			Gizmos.DrawLine(v3TopLeft, v3TopRight);
			Gizmos.DrawLine(v3TopRight, v3BottomRight);
			Gizmos.DrawLine(v3BottomRight, v3BottomLeft);
			Gizmos.DrawLine(v3BottomLeft, v3TopLeft);
		}

		/// <summary>
		/// Draws a gizmo rectangle
		/// </summary>
		/// <param name="center">Center.</param>
		/// <param name="size">Size.</param>
		/// <param name="color">Color.</param>
		public static void DrawGizmoRectangle(Vector2 center, Vector2 size, Matrix4x4 rotationMatrix, Color color)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			GL.PushMatrix();

			Gizmos.color = color;

			Vector3 v3TopLeft = rotationMatrix * new Vector3(center.x - size.x / 2, center.y + size.y / 2, 0);
			Vector3 v3TopRight = rotationMatrix * new Vector3(center.x + size.x / 2, center.y + size.y / 2, 0); ;
			Vector3 v3BottomRight = rotationMatrix * new Vector3(center.x + size.x / 2, center.y - size.y / 2, 0); ;
			Vector3 v3BottomLeft = rotationMatrix * new Vector3(center.x - size.x / 2, center.y - size.y / 2, 0); ;


			Gizmos.DrawLine(v3TopLeft, v3TopRight);
			Gizmos.DrawLine(v3TopRight, v3BottomRight);
			Gizmos.DrawLine(v3BottomRight, v3BottomLeft);
			Gizmos.DrawLine(v3BottomLeft, v3TopLeft);
			GL.PopMatrix();
		}

		/// <summary>
		/// Draws a rectangle based on a Rect and color
		/// </summary>
		/// <param name="rectangle">Rectangle.</param>
		/// <param name="color">Color.</param>
		public static void DrawRectangle(Rect rectangle, Color color)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Vector3 pos = new Vector3(rectangle.x + rectangle.width / 2, rectangle.y + rectangle.height / 2, 0.0f);
			Vector3 scale = new Vector3(rectangle.width, rectangle.height, 0.0f);

			RPGDebug.DrawRectangle(pos, color, scale);
		}

		/// <summary>
		/// Draws a rectangle of the specified color and size at the specified position
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="color">Color.</param>
		/// <param name="size">Size.</param>
		public static void DrawRectangle(Vector3 position, Color color, Vector3 size)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Vector3 halfSize = size / 2f;

			Vector3[] points = new Vector3[]
			{
				position + new Vector3(halfSize.x,halfSize.y,halfSize.z),
				position + new Vector3(-halfSize.x,halfSize.y,halfSize.z),
				position + new Vector3(-halfSize.x,-halfSize.y,halfSize.z),
				position + new Vector3(halfSize.x,-halfSize.y,halfSize.z),
			};

			Debug.DrawLine(points[0], points[1], color);
			Debug.DrawLine(points[1], points[2], color);
			Debug.DrawLine(points[2], points[3], color);
			Debug.DrawLine(points[3], points[0], color);
		}

		/// <summary>
		/// Draws a point of the specified color and size at the specified position
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="col">Col.</param>
		/// <param name="scale">Scale.</param>
		public static void DrawPoint(Vector3 position, Color color, float size)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Vector3[] points = new Vector3[]
			{
				position + (Vector3.up * size),
				position - (Vector3.up * size),
				position + (Vector3.right * size),
				position - (Vector3.right * size),
				position + (Vector3.forward * size),
				position - (Vector3.forward * size)
			};

			Debug.DrawLine(points[0], points[1], color);
			Debug.DrawLine(points[2], points[3], color);
			Debug.DrawLine(points[4], points[5], color);
			Debug.DrawLine(points[0], points[2], color);
			Debug.DrawLine(points[0], points[3], color);
			Debug.DrawLine(points[0], points[4], color);
			Debug.DrawLine(points[0], points[5], color);
			Debug.DrawLine(points[1], points[2], color);
			Debug.DrawLine(points[1], points[3], color);
			Debug.DrawLine(points[1], points[4], color);
			Debug.DrawLine(points[1], points[5], color);
			Debug.DrawLine(points[4], points[2], color);
			Debug.DrawLine(points[4], points[3], color);
			Debug.DrawLine(points[5], points[2], color);
			Debug.DrawLine(points[5], points[3], color);
		}

	}
}




