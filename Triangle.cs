using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Triangle
{
    public Vector3 a, b, c;

    public Triangle()
    {

    }
    public Triangle(Vector3 a, Vector3 b, Vector3 c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }
    
    public Vector3 RandomPoint()
    {
        float r1 = Random.value;
        float r2 = Random.value;
        return (1 - Sqrt(r1)) * a + (Sqrt(r1) * (1 - r2)) * b + (Sqrt(r1) * r2) * c;
    }

    private float Sqrt(float value)
    {
        return (float)Math.Sqrt(value);
    }

    public float Area => Vector3.Cross(a - b, a - c).magnitude / 2;
    
}


public class TrianglePolygon
{
    private List<Triangle> triangles;
    private float areaSum;
    public TrianglePolygon(List<Vector3> points)
    {
        triangles = new List<Triangle>();
        for (int i = 2; i < points.Count; i++)
        {
            triangles.Add(new Triangle(points[0], points[i - 1], points[i]));
        }

        areaSum = 0;
        foreach (var triangle in triangles)
        {
            areaSum += triangle.Area;
        }
    }

    public Vector3 RandomPoint()
    {
        float r = Random.Range(0, areaSum);
        int i = 0;

        while (r >= triangles[i].Area)
        {
            r -= triangles[i].Area;
            i++;
        }

        return triangles[i].RandomPoint();
    }
}