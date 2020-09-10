using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace _3DSculpture
{
    public class Triangle
    {
        //Triangle vertex
        public Point3D p1;
        public Point3D p2;
        public Point3D p3;

        //Triangle normal vector
        public Vector3D n1;

        public Triangle(Point3D _p1, Point3D _p2, Point3D _p3)
        {
            p1 = _p1;
            p2 = _p2;
            p3 = _p3;

            ///Compute triangle normal vector 
            Vector3D normal = Vector3D.CrossProduct(GetVectorFromPoints(p1, p2), GetVectorFromPoints(p1, p3));
            normal.Normalize();
            n1 = normal;
        }
        public Triangle(Point3D _p1, Point3D _p2, Point3D _p3, Vector3D _n1)
        {
            p1 = _p1;
            p2 = _p2;
            p3 = _p3;
            n1 = _n1;


        }
        /// <summary>
        /// Get triangle normal vector
        /// </summary>
        /// <returns>Normal vector</returns>
        public Vector3D GetNormal()
        {
            return n1;
        }

        /// <summary>
        /// Generate triangles list, that build a 3D cube.
        /// </summary>
        /// <param name="length">Triangle edge length</param>
        /// <returns>Triangles list, that build a 3D cube.</returns>
        public static List<Triangle> GenerateCube(double length = 1)
        {

            List<Triangle> list = new List<Triangle>();
            list.Add(new Triangle(new Point3D(-length / 2, -length / 2, -length / 2), new Point3D(-length / 2, length / 2, -length / 2), new Point3D(length / 2, length / 2, -length / 2)));
            list.Add(new Triangle(new Point3D(-length / 2, -length / 2, -length / 2), new Point3D(length / 2, length / 2, -length / 2), new Point3D(length / 2, -length / 2, -length / 2)));

            list.Add(new Triangle(new Point3D(-length / 2, -length / 2, length / 2), new Point3D(length / 2, length / 2, length / 2), new Point3D(-length / 2, length / 2, length / 2)));
            list.Add(new Triangle(new Point3D(-length / 2, -length / 2, length / 2), new Point3D(length / 2, -length / 2, length / 2), new Point3D(length / 2, length / 2, length / 2)));

            list.Add(new Triangle(new Point3D(-length / 2, length / 2, -length / 2), new Point3D(-length / 2, length / 2, length / 2), new Point3D(length / 2, length / 2, length / 2)));
            list.Add(new Triangle(new Point3D(-length / 2, length / 2, -length / 2), new Point3D(length / 2, length / 2, length / 2), new Point3D(length / 2, length / 2, -length / 2)));

            list.Add(new Triangle(new Point3D(-length / 2, -length / 2, -length / 2), new Point3D(length / 2, -length / 2, length / 2), new Point3D(-length / 2, -length / 2, length / 2)));
            list.Add(new Triangle(new Point3D(-length / 2, -length / 2, -length / 2), new Point3D(length / 2, -length / 2, -length / 2), new Point3D(length / 2, -length / 2, length / 2)));

            list.Add(new Triangle(new Point3D(length / 2, -length / 2, -length / 2), new Point3D(length / 2, length / 2, -length / 2), new Point3D(length / 2, length / 2, length / 2)));
            list.Add(new Triangle(new Point3D(length / 2, -length / 2, -length / 2), new Point3D(length / 2, length / 2, length / 2), new Point3D(length / 2, -length / 2, length / 2)));

            list.Add(new Triangle(new Point3D(-length / 2, -length / 2, -length / 2), new Point3D(-length / 2, length / 2, length / 2), new Point3D(-length / 2, length / 2, -length / 2)));
            list.Add(new Triangle(new Point3D(-length / 2, -length / 2, -length / 2), new Point3D(-length / 2, -length / 2, length / 2), new Point3D(-length / 2, length / 2, length / 2)));

            return list;


        }
        /// <summary>
        /// Compute 3D vector from two points.
        /// </summary>
        /// <param name="from">Start vertex</param>
        /// <param name="to">End vertex</param>
        /// <returns>Computed vector from two points</returns>
        public static Vector3D GetVectorFromPoints(Point3D from, Point3D to)
        {
            double x, y, z;
            x = to.X - from.X;
            y = to.Y - from.Y;
            z = to.Z - from.Z;

            return new Vector3D(x, y, z);

        }

        /// <summary>
        /// Returns vertex with provided number
        /// </summary>
        /// <param name="i">Vertex number</param>
        /// <returns>Vertex with the given number</returns>
        public Point3D GetVertex(int i)
        {
            switch (i)
            {
                case 1: { return p1; }
                case 2: { return p2; }
                case 3: { return p3; }
                default: { throw new InvalidOperationException(); }

            }

        }

        /// <summary>
        /// Compute triangle area.
        /// </summary>
        /// <returns>Triangle area</returns>
        public double GetTriangleArea()
        {
            Vector3D a = GetVectorFromPoints(p1, p2);
            Vector3D b = GetVectorFromPoints(p1, p3);
            Vector3D pom = Vector3D.CrossProduct(a, b);
            return Vector3D.CrossProduct(a, b).Length;
        }

        /// <summary>
        /// Translate specified vertex with provided vector
        /// </summary>
        /// <param name="i">Vertex number</param>
        /// <param name="vector">Transalte vector</param>
        public void MovePoint3D(int i, Vector3D vector)
        {
            switch (i)
            {
                case 1: { p1 = p1 + vector; break; }
                case 2: { p2 = p2 + vector; break; }
                case 3: { p3 = p3 + vector; break; }
                default: { throw new InvalidOperationException(); }

            }
            //Calcualte new normal vector
            Vector3D normal = Vector3D.CrossProduct(GetVectorFromPoints(p1, p2), GetVectorFromPoints(p1, p3));
            normal.Normalize();
            n1 = normal;
        }

        /// <summary>
        /// If triangle is not small enough, divide triangle in smaller triangles. Push triangles inside model.
        /// </summary>
        /// <param name="listT">Model triangle list</param>
        /// <param name="triangle">Triangle to push</param>
        /// <param name="point3D">Hit point on triangle to push</param>
        /// <param name="multiplier">Normal vector multiplier</param>
        /// <returns>New triangles list after divided</returns>
        public static List<Triangle> ConcentrateTriangleInPoint(List<Triangle> listT, Triangle triangle, Point3D point3D, double multiplier = 4)
        {
            double vectorLength = 0.7;
            List<Triangle> list = new List<Triangle>();
            //Triangle divided to smaller triangle
            Vector3D v1 = GetVectorFromPoints(triangle.p1, point3D);
            Vector3D v2 = GetVectorFromPoints(triangle.p2, point3D);
            Vector3D v3 = GetVectorFromPoints(triangle.p3, point3D);

            Vector3D v4 = GetVectorFromPoints(triangle.p1, triangle.p2) * 0.5;
            Vector3D v5 = GetVectorFromPoints(triangle.p2, triangle.p3) * 0.5;
            Vector3D v6 = GetVectorFromPoints(triangle.p3, triangle.p1) * 0.5;

            Point3D m1 = triangle.p1 + v4;
            Point3D m2 = triangle.p2 + v5;
            Point3D m3 = triangle.p3 + v6;

            Point3D p1 = triangle.p1 + v1 * vectorLength;
            Point3D p2 = triangle.p2 + v2 * vectorLength;
            Point3D p3 = triangle.p3 + v3 * vectorLength;

            Vector3D AB = GetVectorFromPoints(triangle.p1, triangle.p2);
            Vector3D AC = GetVectorFromPoints(triangle.p1, triangle.p3);

            Vector3D move = (triangle.GetNormal() * multiplier);
            move.Negate();

            //New triangles
            list.Add(new Triangle(p1 + move, p2 + move, p3 + move));
            list.Add(new Triangle(p1 + move, m1 + move * 0.5, p2 + move));
            list.Add(new Triangle(p3 + move, p2 + move, m2 + move * 0.5));
            list.Add(new Triangle(p1 + move, p3 + move, m3 + move * 0.5));
            list.Add(new Triangle(triangle.p1 + move * 0.5, m1 + move * 0.5, p1 + move));
            list.Add(new Triangle(m1 + move * 0.5, triangle.p2 + move * 0.5, p2 + move));
            list.Add(new Triangle(triangle.p2 + move * 0.5, m2 + move * 0.5, p2 + move));
            list.Add(new Triangle(triangle.p3 + move * 0.5, p3 + move, m2 + move * 0.5));
            list.Add(new Triangle(triangle.p3 + move * 0.5, m3 + move * 0.5, p3 + move));
            list.Add(new Triangle(m3 + move * 0.5, triangle.p1 + move * 0.5, p1 + move));

            Triangle t;
            int a, b, c;
            (t, a, b) = listT.GetTriangleWithCorrespondentEdge(triangle.p1, triangle.p2);
            c = CalculateThirdVertex(a, b);
            listT.Add(new Triangle(t.GetVertex(c), m1, t.GetVertex(a)));
            listT.Add(new Triangle(t.GetVertex(b), m1, t.GetVertex(c)));

            (t, a, b) = listT.GetTriangleWithCorrespondentEdge(triangle.p2, triangle.p3);
            c = CalculateThirdVertex(a, b);
            listT.Add(new Triangle(t.GetVertex(c), m2, t.GetVertex(a)));
            listT.Add(new Triangle(t.GetVertex(b), m2, t.GetVertex(c)));

            (t, a, b) = listT.GetTriangleWithCorrespondentEdge(triangle.p3, triangle.p1);
            c = CalculateThirdVertex(a, b);
            listT.Add(new Triangle(t.GetVertex(c), m3, t.GetVertex(a)));
            listT.Add(new Triangle(t.GetVertex(b), m3, t.GetVertex(c)));

            listT.MoveAllTriangleWithMatchingVertex(triangle.p1, move * 0.5);
            listT.MoveAllTriangleWithMatchingVertex(triangle.p2, move * 0.5);
            listT.MoveAllTriangleWithMatchingVertex(triangle.p3, move * 0.5);

            listT.MoveAllTriangleWithMatchingVertex(m1, move * 0.5);
            listT.MoveAllTriangleWithMatchingVertex(m2, move * 0.5);
            listT.MoveAllTriangleWithMatchingVertex(m3, move * 0.5);

            return list;
        }

        /// <summary>
        /// Transalte provided trangle vertex by normal vector multiply by multiplier
        /// </summary>
        /// <param name="listT">Model triangl list</param>
        /// <param name="triangle">Triangle to move</param>
        /// <param name="multiplier">Normal vector mutiplier</param>
        public static void MoveTriangles(List<Triangle> listT, Triangle triangle, double multiplier = 1)
        {
            Vector3D normal = triangle.GetNormal();
            listT.MoveAllTriangleWithMatchingVertex(triangle.p1, normal * multiplier);
            listT.MoveAllTriangleWithMatchingVertex(triangle.p2, normal * multiplier);
            listT.MoveAllTriangleWithMatchingVertex(triangle.p3, normal * multiplier);

        }

        /// <summary>
        /// Calculate vertex number that was not provided
        /// </summary>
        /// <param name="a">Vertex number</param>
        /// <param name="b">Vertex number</param>
        /// <returns>Not provided vertex number</returns>
        public static int CalculateThirdVertex(int a, int b)
        {
            int c = -1;
            if ((a == 1 && b == 2) || (a == 2 && b == 1))
                c = 3;
            if ((a == 2 && b == 3) || (a == 3 && b == 2))
                c = 1;
            if ((a == 3 && b == 1) || (a == 1 && b == 3))
                c = 2;
            return c;
        }

        /// <summary>
        /// Pull start triangle and neighbours triangles outside the model provided by triangle list
        /// </summary>
        /// <param name="listT">Model triangles list</param>
        /// <param name="triangle">Start triangle</param>
        /// <param name="multiplier">Pull length</param>
        /// <param name="neighb">Number of neighbour to propagate pull operation</param>
        public static void PullTriangle(List<Triangle> listT, Triangle triangle, double multiplier = 4, double neighb = 2)
        {
            List<Point3D> activeVertex = new List<Point3D>();
            List<Point3D> newActiveVertex = new List<Point3D>();
            activeVertex.Add(triangle.GetVertex(1));
            activeVertex.Add(triangle.GetVertex(2));
            activeVertex.Add(triangle.GetVertex(3));
            Vector3D vector = triangle.GetNormal();

            //A step in which further vertices will be pulled by a smaller vector
            double step = multiplier / (neighb + 1);

            int match = 0;

            List<int> matchingVertex = new List<int>();
            List<int> allVertexNumber = new List<int>();

            allVertexNumber.Add(1);
            allVertexNumber.Add(2);
            allVertexNumber.Add(3);

            triangle.MovePoint3D(1, vector * multiplier);
            triangle.MovePoint3D(2, vector * multiplier);
            triangle.MovePoint3D(3, vector * multiplier);
            listT.Add(triangle);

            //next neighbours
            for (int i = 0; i <= neighb; i++)
            {
                foreach (var actualTriangle in listT)
                {
                    foreach (var vertex in activeVertex)
                    {
                        //Checking matching vertex in actual triangle with vertex in active list
                        for (int j = 1; j <= 3; j++)
                        {
                            if (actualTriangle.GetVertex(j) == vertex)
                            {
                                match++;
                                if (!matchingVertex.Contains(j))
                                    matchingVertex.Add(j);
                            }
                        }

                    }

                    if (match != 0)
                    {
                        List<int> newNeighbour = allVertexNumber.Except(matchingVertex).ToList<int>();
                        foreach (var elem3 in matchingVertex)
                        {
                            actualTriangle.MovePoint3D(elem3, vector * (multiplier - step * i));
                        }

                        foreach (var elem3 in newNeighbour)
                        {
                            if (!newActiveVertex.Contains(actualTriangle.GetVertex(elem3)))
                                newActiveVertex.Add(actualTriangle.GetVertex(elem3));
                        }
                        matchingVertex.Clear();
                        match = 0;
                    }
                }
                activeVertex.Clear();
                activeVertex.AddRange(newActiveVertex);
                newActiveVertex.Clear();

            }


        }
    }
}
