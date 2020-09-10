using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using _3DTools;
using System.Windows.Forms;
using System.IO;

namespace _3DSculpture
{
    
    public partial class MainWindow : Window
    {
        //Model triangle list
        List<Triangle> triangleList;
        Material frontMaterial;
        GeometryModel3D triangleModel;
        //Drawed model triangles
        MeshGeometry3D mesh;
        //Model edges
        ScreenSpaceLines3D wire;
        Viewport3D viewport;
        ModelVisual3D myModelVisual3D;
        GeometryModel3D myGeometryModel;
        Model3DGroup myModel3DGroup;
        Point prevClickedPoint;
        bool drawLines;
        MaterialGroup mg;

        public MainWindow()
        {
            InitializeComponent();
            drawLines = true;
            
            triangleList = new List<Triangle>();
            mg = new MaterialGroup();
            myModel3DGroup = new Model3DGroup();
            myGeometryModel = new GeometryModel3D();
            myModelVisual3D = new ModelVisual3D();


            Trackball Trackball_3DTools = new Trackball();
            
            Trackball_3DTools.EventSource = viewport3D;
            
            viewport3D.Camera.Transform = Trackball_3DTools.Transform;

            viewport = viewport3D;

            mesh = new MeshGeometry3D();           
            wire = new ScreenSpaceLines3D();

            //Generate cube
            triangleList = Triangle.GenerateCube(100);
            InvalidateModel(drawLines);
            //Init edge drawing
            wire.Color = Colors.Black;
            wire.Thickness = 3;
            wire.Transform = new Transform3DGroup();
            //Init model color
            frontMaterial =new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(25,83,83,83)));
            mg.Children.Add(frontMaterial);
            mg.Children.Add(new EmissiveMaterial(new SolidColorBrush(Color.FromArgb(25, 83, 83, 83))));
            model_color_frame.Background = new SolidColorBrush(Color.FromArgb(25, 83, 83, 83));
            //Init triangle model to draw
            triangleModel =new GeometryModel3D(mesh, mg);
            triangleModel.Transform = new Transform3DGroup();
            ModelVisual3D visualModel = new ModelVisual3D();
            visualModel.Content = triangleModel;
           
            //Add wire and model to draw
            viewport.Children.Add(visualModel);
            viewport.Children.Add(wire);

            checkbox.IsChecked = drawLines;


        }
        //Pull triangles, push triangles, and helping camera move
        private void MouseDownGetPointFromModel(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) && e.LeftButton == MouseButtonState.Pressed))
            {
                Point mouse_pos = e.GetPosition(viewport3D);

                // Perform the hit test.
                HitTestResult result =
                    VisualTreeHelper.HitTest(viewport3D, mouse_pos);

                // Display information about the hit.
                RayMeshGeometry3DHitTestResult mesh_result =
                    result as RayMeshGeometry3DHitTestResult;
                if (mesh_result == null) this.Title = "";
                else
                {
                    // Display the name of the model.


                    // Display more detail about the hit.
                    Console.WriteLine("Distance: " +
                        mesh_result.DistanceToRayOrigin);
                    Console.WriteLine("Point hit: (" +
                        mesh_result.PointHit.ToString() + ")");

                    Console.WriteLine("Triangle:");
                    MeshGeometry3D mesh = mesh_result.MeshHit;

                    Console.WriteLine("    (" +
                        mesh.Positions[mesh_result.VertexIndex1].ToString()
                            + ")");
                    Console.WriteLine("    (" +
                        mesh.Positions[mesh_result.VertexIndex2].ToString()
                            + ")");
                    Console.WriteLine("    (" +
                        mesh.Positions[mesh_result.VertexIndex3].ToString()
                            + ")");
                    //Getting clicked triangle
                    Triangle t = triangleList.GetTriangle(mesh.Positions[mesh_result.VertexIndex1], mesh.Positions[mesh_result.VertexIndex2], mesh.Positions[mesh_result.VertexIndex3]);
                    if (t == null) return;
                    //Divide triangle and push or push if triangle is too small
                    if (t.GetTriangleArea() > 100)
                        triangleList.AddRange(Triangle.ConcentrateTriangleInPoint(triangleList, t, mesh_result.PointHit, slider.Value));
                    else
                        triangleList.MoveTriangles(t, slider.Value);

                    InvalidateModel(drawLines);
                }


            }
            //For camera move
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                prevClickedPoint = e.GetPosition(viewport3D);
            }
            else if (((Keyboard.IsKeyDown(Key.LeftShift) && e.LeftButton == MouseButtonState.Pressed)))
            {
                Point mouse_pos = e.GetPosition(viewport3D);

                // Perform the hit test.
                HitTestResult result =
                    VisualTreeHelper.HitTest(viewport3D, mouse_pos);

                // Display information about the hit.
                RayMeshGeometry3DHitTestResult mesh_result =
                    result as RayMeshGeometry3DHitTestResult;
                if (mesh_result == null) this.Title = "";
                else
                {
                    // Display the name of the model.


                    // Display more detail about the hit.
                    Console.WriteLine("Distance: " +
                        mesh_result.DistanceToRayOrigin);
                    Console.WriteLine("Point hit: (" +
                        mesh_result.PointHit.ToString() + ")");

                    Console.WriteLine("Triangle:");
                    MeshGeometry3D mesh = mesh_result.MeshHit;

                    Console.WriteLine("    (" +
                        mesh.Positions[mesh_result.VertexIndex1].ToString()
                            + ")");
                    Console.WriteLine("    (" +
                        mesh.Positions[mesh_result.VertexIndex2].ToString()
                            + ")");
                    Console.WriteLine("    (" +
                        mesh.Positions[mesh_result.VertexIndex3].ToString()
                            + ")");

                    //Getting clicked triangle
                    Triangle t = triangleList.GetTriangle(mesh.Positions[mesh_result.VertexIndex1], mesh.Positions[mesh_result.VertexIndex2], mesh.Positions[mesh_result.VertexIndex3]);
                    if (t == null) return;
                    //Pull triangles
                    Triangle.PullTriangle(triangleList, t, slider.Value,slider2.Value);
                    InvalidateModel(drawLines);
                }

            }
        }    
        //Move camera
        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(e.MiddleButton == MouseButtonState.Pressed)
            {

                Vector3D vector = Triangle.GetVectorFromPoints(new Point3D(0, prevClickedPoint.X, prevClickedPoint.Y), new Point3D(0, e.GetPosition(viewport3D).X, e.GetPosition(viewport3D).Y));
                vector.Normalize();
                
                camMain.Position = new Point3D(camMain.Position.X , camMain.Position.Y +1.5*vector.Z, camMain.Position.Z + 1.5 * vector.Y);
                camMain.LookDirection = new Vector3D(-camMain.Position.X, 0,0);
                prevClickedPoint = e.GetPosition(viewport3D);
            }
        }
        //Draw model
        private void InvalidateModel(bool drawLines)
        {
            mesh.TriangleIndices.Clear();
            mesh.Normals.Clear();
            mesh.Positions.Clear();

            wire.Points.Clear();
            int pom = 0;
            foreach (var elem in triangleList)
            {
                if (drawLines)
                {
                wire.Points.Add(elem.p1);
                wire.Points.Add(elem.p2);
                wire.Points.Add(elem.p2);
                wire.Points.Add(elem.p3);
                wire.Points.Add(elem.p3);
                wire.Points.Add(elem.p1);
                }
                //Setting vertices and their indexes in mesh
                mesh.Positions.Add(elem.p1);
                mesh.TriangleIndices.Add(pom);


                pom++;

                mesh.Positions.Add(elem.p2);
                mesh.TriangleIndices.Add(pom);

               
                pom++;

                mesh.Positions.Add(elem.p3);
                mesh.TriangleIndices.Add(pom);

                
                pom++;
            }

        }
        //Camera zoom in or out with mouse wheel
        private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Vector3D vector = (camMain.LookDirection);
            vector.Normalize();

            if(e.Delta>0)
            {
                camMain.Position +=3* vector;
            }
            else
            {
                camMain.Position -=3* vector;
            }
            
        }
        //Generate new cube
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            triangleList.Clear();
            triangleList.AddRange(Triangle.GenerateCube(100));
            InvalidateModel(drawLines);
        }
        //Export model to text .stl file
        private void Button_Save(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Stl Format|*.stl";
            
            saveFileDialog1.ShowDialog();


            if (saveFileDialog1.FileName != "")
            {

                FileStream fs =
                   (FileStream)saveFileDialog1.OpenFile();

                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine("solid " + saveFileDialog1.FileName);
                foreach (var elem in triangleList)
                {
                    sw.WriteLine("facet normal " + elem.GetNormal().X.ToString("0.0#####").Replace(',', '.') + " " + elem.GetNormal().Y.ToString("0.0#####").Replace(',', '.') + " " + elem.GetNormal().Z.ToString("0.0#####").Replace(',', '.'));
                    sw.WriteLine("outer loop");
                    for(int i=1;i<=3;i++)
                    {
                        sw.WriteLine("vertex " + elem.GetVertex(i).X.ToString("0.0#####").Replace(',', '.') + " " + elem.GetVertex(i).Y.ToString("0.0#####").Replace(',', '.') + " " + elem.GetVertex(i).Z.ToString("0.0#####").Replace(',', '.'));
                    }
                    sw.WriteLine("endloop");
                    sw.WriteLine("endfacet");
                }
                sw.WriteLine("endsolid");
                sw.Flush();
                sw.Close();
                fs.Close();
            }


        }
        //Load .stl text file
        private void Button_Open(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            List<Triangle> list = new List<Triangle>();
            openFileDialog1.Filter = "Stl Format|*.stl";

            


            if (openFileDialog1.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {

                FileStream fs =
                   (FileStream)openFileDialog1.OpenFile();

                StreamReader sw = new StreamReader(fs);

                sw.ReadLine();
                while(!sw.EndOfStream)
                {

                    string[] parse1 = sw.ReadLine().Replace('.', ',').Split(' ');
                    if (parse1[0].ToLower() == "endsolid") break;
                    sw.ReadLine();
                    string[] parse3 = sw.ReadLine().Replace('.',',').Split(' ');
                    string[] parse4 = sw.ReadLine().Replace('.', ',').Split(' ');
                    string[] parse5 = sw.ReadLine().Replace('.', ',').Split(' ');
                    sw.ReadLine();
                    sw.ReadLine();
                    list.Add(new Triangle(new Point3D(double.Parse(parse3[1]), double.Parse(parse3[2]), double.Parse(parse3[3])),
                        new Point3D(double.Parse(parse4[1]), double.Parse(parse4[2]), double.Parse(parse4[3])),
                        new Point3D(double.Parse(parse5[1]), double.Parse(parse5[2]), double.Parse(parse5[3])), new Vector3D(double.Parse(parse1[2]), double.Parse(parse1[3]), double.Parse(parse1[4]))));
                }
                sw.Close();
                fs.Close();

                triangleList.Clear();
                triangleList.AddRange(list);
                InvalidateModel(drawLines);
            }


        }
        //Reset camera view
        private void Camera_Reset(object sender, RoutedEventArgs e)
        {
            camMain.Position = new Point3D(300,0,0);
            camMain.LookDirection = new Vector3D(-camMain.Position.X, 0, 0);
        }
        //Change model color
        private void Change_Color(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                model_color_frame.Background =  new SolidColorBrush( Color.FromArgb( colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                mg.Children.Clear();
                frontMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B)));
                mg.Children.Add(frontMaterial);
                mg.Children.Add(new EmissiveMaterial(new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B))));
                triangleModel.Material = mg;
            }
        }
        //Draw model lines
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            drawLines = true;
            InvalidateModel(drawLines);
        }
        //Do not draw model lines
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            drawLines = false;
            InvalidateModel(drawLines);
        }
    }
}
