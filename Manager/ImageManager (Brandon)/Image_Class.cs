using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsPotrace;
using System.Drawing;
using System.Collections;
using DXF_Management;
using netDxf;
using Ent = netDxf.Entities;
using System.IO;

namespace Image_Symbol_Management
{
    public class Image_Class
    {
        #region Private Felder
        List<LineF> ListOfLine;
         List<List<object>> ListElement_Image
        {
            get;
             set;
        }
        bool[,] Matrix
        {
            get;

             set;

        }
        Image loaded_Image;
        int tresHold = 249;
        double curve_tolerance = 0.2;
      bool Curveoptimizing = true;
      double AlphaMax = 1;
        int ignore_area_pixel = 2;
        bool gesetzt = false;
        Bitmap Bitmap;
        #region Arbeitsbereich
        const double X_max = 277;
        const double Y_max = 190;
        const double Y_min = 20;
        const double X_min = 20;
        #endregion
        double verschiebung_schritt ;
         double skalierungsfaktor ;
   

        double max_X = 2000, min_X = 2000, max_Y = 2000, min_Y = 2000;
      

        #endregion
        #region Public Felder
        public ArrayList ListOfCurveArray;
        public List<Ent.LwPolyline> allePolylinien;
        #endregion
        #region Konstruktoren
        /// <summary>
        /// Das zu Vektor zu konvertierende Bild als Übergabeparameter
        /// </summary>
        /// <param name="img">Das zu Vektor zu konvertierende Bild</param>
        public Image_Class(Image img)
        {
            if (img == null)
            {
                throw new Exception("Error : NTB_ImageManagement_100000  on the given image");
            }
            loaded_Image = img;
            gesetzt = true;
           
        }
        /// <summary>
        /// Pfad des zu Vektor zu konvertierenden Bildes als Übergabeparameter
        /// </summary>
        /// <param name="Path">Pfad des zu Vektor zu konvertierenden Bildes </param>
        public Image_Class(string Path)
        {

            if (!(new FileInfo(Path)).Exists)
            {
                throw new Exception("Error : NTB_ImageManagement_100001  the given path is not correct");
            }
            else
            {
                try
                {
                    loaded_Image = new Bitmap(Path);
                    gesetzt = true;
                }
                catch (Exception e)
                {

                    throw new Exception("Error : NTB_ImageManagement_100002 " + e.Message);
                }
            }
           

        }
        #endregion
        #region private Methoden
        /// <summary>
        /// Passt alle Punkte an dem Arbeitsbereich
        /// </summary>
        /// <param name="alle">Liste von Polylinien. Hier erzeugt <see cref="All_To_Polyline"/>
        void RahmenPassend(ref List<Ent.LwPolyline> alle)
        {
            if (min_X<X_min)
            {
                verschiebung_schritt = Math.Abs(X_min - min_X);
                foreach (Ent.LwPolyline Pl in alle)
                {
                    foreach (Ent.LwPolylineVertex Vert in Pl.Vertexes)
                    {
                        Vert.Location = new Vector2(Vert.Location.X + verschiebung_schritt, Vert.Location.Y);
                    }
                }
                Set_Max_und_Min(alle);
               RahmenPassend(ref alle);
            }
            if (min_Y<Y_min)
            {
                verschiebung_schritt = Math.Abs(Y_min - min_Y);
                foreach (Ent.LwPolyline Pl in alle)
                {
                    foreach (Ent.LwPolylineVertex Vert in Pl.Vertexes)
                    {
                        Vert.Location = new Vector2(Vert.Location.X , Vert.Location.Y + verschiebung_schritt);
                    }
                }
                Set_Max_und_Min(alle);
               RahmenPassend(ref alle);
            }
            if (max_X>X_max)
            {
                skalierungsfaktor = max_X / X_max;
                foreach (Ent.LwPolyline Pl in alle)
                {
                    foreach (Ent.LwPolylineVertex Vert in Pl.Vertexes)
                    {
                        Vert.Location = new Vector2(Vert.Location.X/skalierungsfaktor, Vert.Location.Y /skalierungsfaktor);
                    }
                }
                Set_Max_und_Min(alle);
              RahmenPassend(ref alle);
            }
            if (max_Y > Y_max)
            {
                skalierungsfaktor = max_Y / Y_max;
                foreach (Ent.LwPolyline Pl in alle)
                {
                    foreach (Ent.LwPolylineVertex Vert in Pl.Vertexes)
                    {
                        Vert.Location = new Vector2(Vert.Location.X / skalierungsfaktor, Vert.Location.Y / skalierungsfaktor);
                    }
                }
                Set_Max_und_Min(alle);
                RahmenPassend(ref alle);
            }
        }
        /// <summary>
        /// Setzt das Maximum und Minimum von allen Punkten
        /// </summary>
        /// <param name="alle"></param>
        void Set_Max_und_Min (List<Ent.LwPolyline> alle)
        {
         
            double x_min_temp = 2000, x_max_temp = 2000, y_min_temp = 2000, y_max_temp = 2000;
            foreach (Ent.LwPolyline Pl in alle)
            {
                foreach (Ent.LwPolylineVertex Vert in Pl.Vertexes)
                {
                    if (x_min_temp == 2000)
                    {
                        x_max_temp = x_min_temp = Vert.Location.X;
                        y_max_temp = y_min_temp = Vert.Location.Y;
                    }
                    else
                    {
                        x_min_temp = Min(x_min_temp, Vert.Location.X);
                        y_min_temp = Min(y_min_temp, Vert.Location.Y);
                        x_max_temp = Max(x_max_temp, Vert.Location.X);
                        y_max_temp = Min(y_max_temp, Vert.Location.Y);
                    }
                   
                }
            }
            max_X = x_max_temp;
            max_Y = y_max_temp;
            min_X = x_min_temp;
            min_Y = y_min_temp;
        }
        double Max(double a , double b)
        {
            return a < b ? b : a > b ? a : b;
        }
        double Min(double a, double b)
        {
            return a > b ? b : a < b ? a : b;
        }

      

        /// <summary>
        /// Vectorize the image
        /// </summary>
       void Vectorize()

        {
            Bitmap = new Bitmap(loaded_Image);
            ListOfCurveArray = new ArrayList();
            Potrace.turdsize = ignore_area_pixel;
            Potrace.alphamax = AlphaMax;
            Potrace.opttolerance = curve_tolerance;

            Potrace.curveoptimizing = Curveoptimizing;
            Matrix = Potrace.BitMapToBinary(Bitmap, tresHold);
            Potrace.potrace_trace(Matrix, ListOfCurveArray);
            ListElement_Image = new List<List<object>>();
            ListOfLine = new List<LineF>();

        }
        /// <summary>
        /// Erzeugt alle Punkte, die zum Nachmalen des Bildes wichtig sind
        /// </summary>
        /// <param name="anzahl">Anzahl der der Segmente pro Bezier-Kurve</param>
    private void TakeAllPoints(double anzahl)

        {
            if (ListOfCurveArray == null) return;
            Image tt = loaded_Image;

            for (int i = 0; i < ListOfCurveArray.Count; i++)
            {
                

                ArrayList CurveArray = (ArrayList)ListOfCurveArray[i];
                for (int j = 0; j < CurveArray.Count; j++)
                {
                    List<object> listElement_Image = new List<object>();
                    Potrace.Curve[] Curves = (Potrace.Curve[])CurveArray[j];
                   

                    for (int k = 0; k < Curves.Length; k++)
                    {
                        if (Curves[k].Kind == Potrace.CurveKind.Bezier)
                        {

                            PointF A = new PointF(Curves[k].A.X, Curves[k].A.Y, new ControlPoint(Curves[k].ControlPointA.X, Curves[k].ControlPointA.Y));
                            PointF B = new PointF(Curves[k].B.X, Curves[k].B.Y, new ControlPoint(Curves[k].ControlPointB.X, Curves[k].ControlPointB.Y));
                            Beziercurve newCurve = new Beziercurve(A, A.ControlPt, B, B.ControlPt);
                            newCurve.List_Of_All_Points = newCurve.BezierToDoublePoint(anzahl);
                            listElement_Image.Add(newCurve);

                        }
                        else
                        {
                            PointF Point1 = new PointF(Curves[k].A.X, Curves[k].A.Y, null);
                            PointF Point2 = new PointF(Curves[k].B.X, Curves[k].B.Y, null);
                            LineF newLine = new LineF(Point1, Point2);

                            listElement_Image.Add(newLine);
                        }


                    }
                    ListElement_Image.Add(listElement_Image);
                }
            

            }


        }
        /// <summary>
        /// Konvertiert alles zu Polylinien
        /// </summary>
        /// <returns> Erzeugte Polylinien</returns>
      private  List<Ent.LwPolyline> All_To_Polyline()
        {
            List<Ent.LwPolyline> list_all_Path = new List<Ent.LwPolyline>();
            foreach (List<object> Element in ListElement_Image)
            {

               
                List<Ent.LwPolylineVertex> list_all_vertex = new List<Ent.LwPolylineVertex>();
                foreach (object element in Element)
                {
                    Type type = element.GetType();
                    if (type == typeof(Beziercurve))
                    {
                        Beziercurve newCurve = element as Beziercurve;
                        foreach (Potrace.DoublePoint pt in newCurve.List_Of_All_Points)
                        {
                            list_all_vertex.Add(new Ent.LwPolylineVertex((double)pt.X, (double)pt.Y));

                        }
                    }
                    else
                    {
                        LineF newLine = element as LineF;
                        list_all_vertex.Add(new Ent.LwPolylineVertex((double)newLine.Point1.x, (double)newLine.Point1.Y));
                        list_all_vertex.Add(new Ent.LwPolylineVertex((double)newLine.Point2.x, (double)newLine.Point2.Y));

                    }
                }
                Ent.LwPolyline newPolyline = new Ent.LwPolyline(list_all_vertex);
              
                list_all_Path.Add(newPolyline);

            }
            Set_Max_und_Min(list_all_Path);
            RahmenPassend(ref list_all_Path);
            return allePolylinien = list_all_Path ;
        }
        #endregion
        #region Public Methoden
        /// <summary>
        /// Erzeugt die entsprechende DXF Datei
        /// </summary>
        /// <param name="Path">Pfad der zu erzeugenden DXF File</param>
        /// <returns></returns>
        public netDxf.DxfDocument Set_and_GetDXF_File(string Path)
        {
            if (gesetzt)
            {
                netDxf.DxfDocument newDocument = new DxfDocument(netDxf.Header.DxfVersion.AutoCad2013);
                Vectorize();
                TakeAllPoints(10);
             
                foreach (Ent.LwPolyline PolyLine in All_To_Polyline())
                {
                    newDocument.AddEntity(PolyLine);
                }
                newDocument.Save(Path);

                return newDocument;
            }
            else
            {
                throw new Exception("Error : NTB_ImageManagement_100003 Please you have first to create the object");
            }
            
        }
        #endregion
    }
}
