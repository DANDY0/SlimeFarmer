using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AiSensor : MonoBehaviour
{
	[SerializeField] private float distance = 10f;
	[SerializeField] private float angle = 30f;
	[SerializeField] private float height = 1f;
	[SerializeField] private Color m_color = Color.red;
	[SerializeField] private MeshFilter meshFilter;
	[SerializeField] private int segments = 10;
	[SerializeField] private string pathMesh = "Assets/Generate/Sensor.mesh";
	private Mesh mesh;

	#region Editor
	[Button]
	private void SetRefs()
	{
		mesh = CreateWedgeMesh();
		meshFilter.mesh = mesh;
	}

#if UNITY_EDITOR
	[Button]
	private void SaveMeshes()
	{
		AssetDatabase.CreateAsset(mesh, pathMesh);
		AssetDatabase.SaveAssets();
	}
#endif

	#endregion
	private Mesh CreateWedgeMesh()
	{
		Mesh mesh = new Mesh();
	
		int numTriangles = (segments * 4) + 2 + 2;
		int numVertices = numTriangles * 3;

		Vector3[] vertices = new Vector3[numVertices];
		int[] triangles = new int[numVertices];

		Vector3 bottomCenter = Vector3.zero;
		Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
		Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

		Vector3 topCenter = bottomCenter + Vector3.up * height;
		Vector3 topRight = bottomRight + Vector3.up * height;
		Vector3 topLeft = bottomLeft + Vector3.up * height;

		int vert = 0;

		// left side
		vertices[vert++] = bottomCenter;
		vertices[vert++] = bottomLeft;
		vertices[vert++] = topLeft;

		vertices[vert++] = topLeft;
		vertices[vert++] = topCenter;
		vertices[vert++] = bottomCenter;

		//right side
		vertices[vert++] = bottomCenter;
		vertices[vert++] = topCenter;
		vertices[vert++] = topRight;

		vertices[vert++] = topRight;
		vertices[vert++] = bottomRight;
		vertices[vert++] = bottomCenter;

		float currentAngle = -angle;
		float deltaAngle = (angle * 2) / segments;

		for (int i = 0; i < segments; i++)
		{
			bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
			bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

			topRight = bottomRight + Vector3.up * height;
			topLeft = bottomLeft + Vector3.up * height;

			//far side
			vertices[vert++] = bottomLeft;
			vertices[vert++] = bottomRight;
			vertices[vert++] = topRight;

			vertices[vert++] = topRight;
			vertices[vert++] = topLeft;
			vertices[vert++] = bottomLeft;

			//top side
			vertices[vert++] = topCenter;
			vertices[vert++] = topLeft;
			vertices[vert++] = topRight;

			//bottom side
			vertices[vert++] = bottomCenter;
			vertices[vert++] = bottomRight;
			vertices[vert++] = bottomLeft;

			currentAngle += deltaAngle;
		}

		for (int i = 0; i < numVertices; ++i)
		{
			triangles[i] = i;
		}

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		return mesh;
	}
	

#if UNITY_EDITOR
	private void OnValidate()
	{
		mesh = CreateWedgeMesh();
	}

	private void OnDrawGizmos()
	{
		if (mesh)
		{
			Gizmos.color = m_color;
			Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
		}
	}
#endif

}
