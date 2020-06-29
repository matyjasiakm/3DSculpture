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
        public Point3D p1;
        public Point3D p2;
        public Point3D p3;
        public Vector3D n1;
        public Vector3D n2;
        public Vector3D n3;

        public Triangle(Point3D _p1,Point3D _p2, Point3D _p3)
        {
            p1 = _p1;
            p2 = _p2;
            p3 = _p3;

            Vector3D normal = Vector3D.CrossProduct(GetVectorFromPoints(p1, p2), GetVectorFromPoints(p1, p3));
            normal.Normalize();
            n1 = normal;
        }
        public Triangle(Point3D _p1, Point3D _p2, Point3D _p3,Vector3D _n1)
        {
            p1 = _p1;
            p2 = _p2;
            p3 = _p3;
            n1 = _n1;
            

        }
        public Vector3D GetNormal()
        {
            return n1;
        }
        public static List<Triangle> GenerateCube(double length=1)
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
        public static Vector3D GetVectorFromPoints(Point3D from, Point3D to)
        {
            double x, y, z;
            x = to.X - from.X;
            y = to.Y - from.Y;
            z = to.Z - from.Z;

            return new Vector3D(x, y, z);

        }
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
        public double GetTriangleArea()
        {
            Vector3D a = GetVectorFromPoints(p1, p2);
            Vector3D b = GetVectorFromPoints(p1, p3);
            Vector3D pom=Vector3D.CrossProduct(a, b);
            return Vector3D.CrossProduct(a, b).Length;
        }
        public void MovePoint3D(int i, Vector3D vector)
        {
            switch (i)
            {
                case 1: { p1 = p1 + vector; break; }
                case 2: { p2 = p2 + vector; break; }
                case 3: { p3 = p3 + vector; break; }
                default: { throw new InvalidOperationException();}

            }
            Vector3D normal = Vector3D.CrossProduct(GetVectorFromPoints(p1, p2), GetVectorFromPoints(p1, p3));
            normal.Normalize();
            n1 = normal;
        }
        public static List<Triangle> ConcentrateTriangleInPoint(List<Triangle> listT,Triangle triangle, Point3D point3D, double multiplier = 4)
        {
            double vectorLength = 0.7;
            List<Triangle> list = new List<Triangle>();
            Vector3D v1 = GetVectorFromPoints(triangle.p1, point3D);
            Vector3D v2 = GetVectorFromPoints(triangle.p2, point3D);
            Vector3D v3 = GetVectorFromPoints(triangle.p3, point3D);

            Vector3D v4 = GetVectorFromPoints(triangle.p1, triangle.p2)*0.5;
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
            Vector3D move = (triangle.GetNormal()* multiplier);
            move.Negate();


            list.Add(new Triangle(p1+move, p2+move, p3+move));
            list.Add(new Triangle(p1+move,m1+move*0.5,p2+move));
            list.Add(new Triangle(p3+move,p2+move,m2+move*0.5));
            list.Add(new Triangle(p1+move,p3+move,m3+move*0.5));
            list.Add(new Triangle(triangle.p1+move*0.5, m1 + move * 0.5, p1 + move));
            list.Add(new Triangle(m1+move*0.5, triangle.p2 + move * 0.5, p2 + move));
            list.Add(new Triangle(triangle.p2 + move * 0.5,m2+move*0.5, p2 + move));
            list.Add(new Triangle(triangle.p3 + move * 0.5, p3 + move, m2 + move * 0.5));
            list.Add(new Triangle(triangle.p3 + move * 0.5, m3 + move * 0.5, p3 + move));
            list.Add(new Triangle( m3 + move * 0.5,triangle.p1 + move * 0.5, p1 + move));

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

        public static void MoveTriangles(List<Triangle> listT, Triangle triangle, double multiplier=1)
        {
            Vector3D normal = triangle.GetNormal();
            listT.MoveAllTriangleWithMatchingVertex(triangle.p1, normal*multiplier);
            listT.MoveAllTriangleWithMatchingVertex(triangle.p2, normal * multiplier);
            listT.MoveAllTriangleWithMatchingVertex(triangle.p3, normal * multiplier);

        }

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


        public static void PullTriangle(List<Triangle> listT, Triangle triangle, double multiplier =4 ,double neighb=2)
        {
            List<Point3D> list = new List<Point3D>();
            List<Point3D> list2 = new List<Point3D>();
            list.Add(triangle.GetVertex(1));
            list.Add(triangle.GetVertex(2));
            list.Add(triangle.GetVertex(3));
            Vector3D vector = triangle.GetNormal();
            double step = multiplier / (neighb+1);
            int match = 0;
            List<int> vert = new List<int>();
            List<int> vert2 = new List<int>();
            vert2.Add(1);
            vert2.Add(2);
            vert2.Add(3);
            triangle.MovePoint3D(1, vector * multiplier);
            triangle.MovePoint3D(2, vector * multiplier);
            triangle.MovePoint3D(3, vector * multiplier);
            listT.Add(triangle);
            for (int i = 0; i <= neighb; i++)
            {
                foreach(var elem  in listT)
                {
                    foreach(var elem2 in list)
                    {
                        for(int j = 1; j <= 3; j++)
                        {
                            if(elem.GetVertex(j)==elem2)
                            {
                                match++;
                                if(!vert.Contains(j))
                                    vert.Add(j);
                            }
                        }

                    }

                    if(match != 0)
                    {
                        List<int> pom = vert2.Except(vert).ToList<int>();
                        foreach (var elem3 in vert)
                        {
                            elem.MovePoint3D(elem3, vector * (multiplier-step*i ));
                        }
                        foreach (var elem3 in pom)
                        {
                            if(!list2.Contains(elem.GetVertex(elem3)))
                                list2.Add(elem.GetVertex(elem3));
                        }
                        vert.Clear();
                        match = 0;
                    }
                }
                list.Clear();
                list.AddRange(list2);/*.Except(list).ToList<Point3D>()*/;                
                list2.Clear();

            }

           
        }
    }
}
