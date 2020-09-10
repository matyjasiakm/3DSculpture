using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace _3DSculpture
{
    public static class ExstensionMethod
    {
        /// <summary>
        /// Find and return triangle with provided vertex and remove it from list. If does not exist return null.
        /// </summary>
        /// <param name="list">Model triangles list</param>
        /// <param name="p1">First vertex</param>
        /// <param name="p2">Second vertex</param>
        /// <param name="p3">Third vertex</param>
        /// <returns>Triangle with vertex if exsist or null</returns>
        public static Triangle GetTriangle(this List<Triangle> list, Point3D p1, Point3D p2, Point3D p3)
        {
            Triangle triangle = null;
            foreach(var elem in list)
            {
                if( (elem.p1==p1 && elem.p2==p2 && elem.p3==p3)||
                    (elem.p1 == p2 && elem.p2 == p3 && elem.p3 == p1)||
                    (elem.p1 == p3 && elem.p2 == p1 && elem.p3 == p2)||
                    (elem.p1 == p1 && elem.p3 == p2 && elem.p2 == p3)||
                    (elem.p1 == p2 && elem.p2 == p1 && elem.p3 == p3)||
                    (elem.p1 == p3 && elem.p2 == p2 && elem.p3 == p1))
                {
                    triangle = elem;
                    break;

                }

            }

            if (list.Remove(triangle) == false) ;
                
            return triangle;

        }

        /// <summary>
        /// Find triangle that edge correspondign with provided edge point
        /// </summary>
        /// <param name="list">Model triangles list</param>
        /// <param name="x">Edge first vertex</param>
        /// <param name="y">Edge second vertex</param>
        /// <returns>Triangle with coresponding edge and corresponding edge vertex numbers</returns>
        public static (Triangle triangle,int a , int b) GetTriangleWithCorrespondentEdge(this List<Triangle> list, Point3D x, Point3D y)
        {
            foreach(var elem in list)
            {
                if(elem.p1==x && elem.p2==y)
                {
                    list.Remove(elem);
                    return (elem, 1, 2);
                }
                else if(elem.p2 == x && elem.p3 == y)
                {
                    list.Remove(elem);
                    return (elem, 2, 3);
                }
                else if(elem.p3 == x && elem.p1 == y)
                {
                    list.Remove(elem);
                    return (elem, 3, 1);
                }
                else if(elem.p2 == x && elem.p1 == y)
                {
                    list.Remove(elem);
                    return (elem, 2,1);
                }
                else if(elem.p3 == x && elem.p2 == y)
                {
                    list.Remove(elem);
                    return (elem,3,2);
                }
                else if (elem.p1 == x && elem.p3 == y)
                {
                    list.Remove(elem);
                    return (elem,1,3);
                }


            }

            return (null,-1,-1);

        }
        /// <summary>
        /// Translate triangle with triangle normal vector multiplied by multiplier
        /// </summary>
        /// <param name="listT">Model triangle list</param>
        /// <param name="triangle">Triangle to move</param>
        /// <param name="multiplier">Normal vector multiplier</param>
        public static void MoveTriangles(this List<Triangle> listT, Triangle triangle, double multiplier = 1)
        {
            Vector3D normal = triangle.GetNormal();
            normal.Negate();
            listT.MoveAllTriangleWithMatchingVertex(triangle.p1, normal * multiplier);
            listT.MoveAllTriangleWithMatchingVertex(triangle.p2, normal * multiplier);
            listT.MoveAllTriangleWithMatchingVertex(triangle.p3, normal * multiplier);
            triangle.p1 = triangle.p1 + normal * multiplier;
            triangle.p2 = triangle.p2 + normal * multiplier;
            triangle.p3 = triangle.p3 + normal * multiplier;
            listT.Add(triangle);

        }
        /// <summary>
        /// Translate all matching vertex in triangle lists with normal vector multiplied by multiplyer.
        /// </summary>
        /// <param name="list">Model triangle list</param>
        /// <param name="x">Vertex</param>
        /// <param name="vector">Translate vector</param>
        /// <param name="multiplier">Translate vector multiplier</param>
        public static void MoveAllTriangleWithMatchingVertex(this List<Triangle> list, Point3D x, Vector3D vector, double multiplier = 1)
        {
            foreach (var elem in list)
            {
                if (elem.p1 == x)
                {
                    elem.MovePoint3D(1, vector * multiplier);
                }
                else if (elem.p2 == x)
                {
                    elem.MovePoint3D(2, vector * multiplier);
                }
                else if (elem.p3 == x)
                {
                    elem.MovePoint3D(3, vector *multiplier);
                }


            }
            

        }




    }
}
