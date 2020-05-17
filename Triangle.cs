using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
///     Класс для треугольника
/// </summary>
public class Triangle {
    /// <summary>
    ///     Координаты треугольника
    /// </summary>
    public Vector3 a, b, c;

    /// <summary>
    ///     Конструктор треугольника
    /// </summary>
    public Triangle() { }
    /// <summary>
    ///     Конструктор треугольника
    /// </summary>
    /// <param name="a">Первая точка</param>
    /// <param name="b">Вторая точка</param>
    /// <param name="c">Третья точка</param>
    public Triangle(Vector3 a, Vector3 b, Vector3 c) {
        this.a = a;
        this.b = b;
        this.c = c;
    }
    
    /// <summary>
    ///     Получает случайную точку в треугольнике
    /// </summary>
    /// <returns>Случаная точка</returns>
    public Vector3 RandomPoint() {
        float r1 = Random.value;
        float r2 = Random.value;
        return (1 - Mathf.Sqrt(r1)) * a + (Mathf.Sqrt(r1) * (1 - r2)) * b + (Mathf.Sqrt(r1) * r2) * c;
    }
    
    /// <summary>
    ///     Вычисляет площадь треугольника
    /// </summary>
    public float Area => Vector3.Cross(a - b, a - c).magnitude / 2;
}

/// <summary>
///     Класс, представляющий выпуклый многоугольник, составленный из треугольников
/// </summary>
public class TrianglePolygon {
    /// <summary>
    ///     Список треугольников
    /// </summary>
    private List<Triangle> triangles;
    /// <summary>
    ///     Суммарная площадь всех треугольников
    /// </summary>
    private float areaSum;
    /// <summary>
    ///     Конструктор
    /// </summary>
    /// <param name="points">Список точек</param>
    public TrianglePolygon(List<Vector3> points) {
        triangles = new List<Triangle>();
        for (int i = 2; i < points.Count; i++) {
            triangles.Add(new Triangle(points[0], points[i - 1], points[i]));
        }

        areaSum = 0;
        foreach (var triangle in triangles) {
            areaSum += triangle.Area;
        }
    }

    /// <summary>
    ///     Находит случайную точку внутри многоугольника
    /// </summary>
    /// <returns>Случайную точку</returns>
    public Vector3 RandomPoint() {
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