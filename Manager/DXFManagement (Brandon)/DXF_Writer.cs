using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Drawing;
using netDxf;
using netDxf.Entities;
using System.IO;
using TextManager;
using shapes = System.Windows.Shapes;
using System.Collections.ObjectModel;
using ElementManager;

//ich (oskar) habe die version geändert, verschieden konstrukturen und methoden verändert oder rausgeschmissen, weil 
//ich sie nicht benötige und ich mir hier drin mehr übersicht verschaffen musste

namespace DXF_Management
{

    public class DXF_Writer
    {
        #region private Variablen
        //Liste von Listen von Punkten, wo eine innere Liste einer Polylinie entspricht
        List<List<System.Drawing.Point>> ListAddedElements_as_List_Of_Point;
        //Gesamte Geometrie
        PathGeometry pathGeo;

        //Endgültige DXF Datei
        DxfDocument dxf_File;

        //Liste von Polyilinien 2.Grades
        List<LwPolyline> listToLwPolyline;

        //Liste von PolyineSegmenten
        List<PolyLineSegment> PolyLineSegments;

        //
        List<shapes.Polyline> listToPolyline;

        //Liste von all hinzugefügten Elementen
        List<creativeElement> ListAddedElements;

        //Liste von von all hinzugefügten Elementen als Polylinien Segmente
        List<List<PolyLineSegment>> ListAddedElements_as_PLS;

        //Liste von von all hinzugefügten Elementen als Polylinien 
        List<List<shapes.Polyline>> ListAddedElements_as_PL;

        List<List<LwPolyline>> allLists_LwPolyline;

        //Höhe des Werkzeugs
        double Height;

        // Breite des Werkzeugs
        double Width;

        // Name des Werkzeugs
        string Werkzeug = "Fraeser";

        // Tiefe des Werkzeugs
        double Depth;


        #endregion
        public DXF_Writer(ObservableCollection<creativeElement> listAddedElements, workpiece workPiece)
        {

            if ((listAddedElements.Count == 0 || listAddedElements == null) || workPiece == null)
            {
                throw new Exception("Error : NTB_DXF_100000 : No avalaible parameter");
            }
            else
            {
                Werkzeug = workPiece.werkzeugName;
                Height = workPiece.height;
                Width = workPiece.width;
                Depth = workPiece.depth;
                ListAddedElements = listAddedElements.ToList();

                foreach (var creativeElement in ListAddedElements)
                {
                    
                }
            }

        }

        #region private Methoden
        /// <summary>
        /// versucht, eine Liste von Polylinien segmenten aus einer Pathgeometrie  zu gewinnen
        /// </summary>
        /// <param name="pathGeo"> die Geometrie</param>
        /// <returns>Liste von Polylinien Segmenten</returns>
        List<PolyLineSegment> TryParseToPolySegment(PathGeometry pathGeo)
        {
            bool parsable = false;
            List<PolyLineSegment> allPolyLineSegments = new List<PolyLineSegment>();
            if (pathGeo != null)
            {
                foreach (var figure in pathGeo.Figures)
                {
                    string allPointsAsString = figure.ToString();
                    StringBuilder coordinate = new StringBuilder();
                    double temporaryXCoordinate = 0;
                    List<System.Windows.Point> allPoints = new List<System.Windows.Point>();
                    string validForDouble = ",0123456789";

                    pathGeo.Transform = Transform.Parse("1,0,0,1,50,50"); //1) x-Stretch, 2) rotate 3) neigung 4) y-stretch 5) x-Verschiebung 6) yVerschiebung

                    foreach (var varChar in allPointsAsString)
                        if (varChar == ';')
                        {
                            temporaryXCoordinate = Convert.ToDouble(coordinate.ToString());
                            coordinate.Clear();
                        }
                        else if (validForDouble.Contains(varChar))
                        {
                            coordinate.Append(varChar);
                        }
                        else if (coordinate.Length != 0)
                        {
                            allPoints.Add(new System.Windows.Point(temporaryXCoordinate, Convert.ToDouble(coordinate.ToString())));
                            coordinate.Clear();
                        }

                    IEnumerable<System.Windows.Point> myIEnumerable = allPoints;
                    allPolyLineSegments.Add(new PolyLineSegment(myIEnumerable, true));

                }
                parsable = true;
            }
            else
                parsable = false;

            PolyLineSegments = allPolyLineSegments;

            return allPolyLineSegments;
        }
        /// <summary>
        /// versucht, eine Liste von LwPolylinien (Polylinien von netDxf) Segmenten aus einer Liste von Polylinien Segmenten  zu gewinnen
        /// </summary>
        /// <param name="polyLineSegments"> Liste von Polylinien Segmenten</param>
        /// <returns>Liste von LwPolylinien</returns>
        List<LwPolyline> TryToLwPolyline(List<PolyLineSegment> polyLineSegments)
        {
            List<LwPolyline> ListToLwPolyline = new List<LwPolyline>();
            if (polyLineSegments != null)
            {
                foreach (PolyLineSegment item in polyLineSegments)
                {
                    List<LwPolylineVertex> listV = new List<LwPolylineVertex>();


                    for (int i = 0; i < item.Points.Count; i++)
                    {
                        if ((item.Points[i].X <= 277 && item.Points[i].X >= 0) && (item.Points[i].Y <= 170 && item.Points[i].Y >= 0))
                        {
                            listV.Add(new LwPolylineVertex(item.Points[i].X, item.Points[i].Y));
                        }
                        else
                        {
                            throw new Exception("Error NTB_DXF_200001: on one polyline. Coordinates out of accepted range");
                        }


                    }
                    //dxf_File.AddEntity(new LwPolyline(listV));
                    ListToLwPolyline.Add(new LwPolyline(listV));



                }
                return ListToLwPolyline;
            }
            return null;

        }
        /// <summary>
        /// versucht, eine Liste von Liste von Polylinien segmenten aus der Liste von hinzugefügten Elementen  zu gewinnen
        /// </summary>
        /// <param name="listAddedElements"> Liste hinzugefügter Elemente</param>
        /// <returns> Liste Polylinien Segmente </returns>
        List<List<PolyLineSegment>> ToPolylineSegment(List<creativeElement> listAddedElements)
        {
            
            List<List<PolyLineSegment>> listListPolyLineSegments = new List<List<PolyLineSegment>>();
            
            //using youris converter method (edited by oskar)
            foreach (creativeElement actualElement in listAddedElements)
            {
                listListPolyLineSegments.Add(Converter.PathGeometryToPlsList(actualElement.pathGeometry));
                if (actualElement.filled)
                {
                    listListPolyLineSegments.Add(Converter.PathGeometryToPlsList(actualElement.fillingPathGeometry));
                }
            }

            /*
            //brandons version
            foreach (creativeElement item in listAddedElements)
            {
                PathGeometry newPG = item.pathGeometry;
                listListPolyLineSegments.Add(TryParseToPolySegment(newPG));
            }
            */
            return listListPolyLineSegments;
        }
        /// <summary>
        /// versucht, eine Liste von Polylinien  aus einer Liste von Listen von PolylinienSegmenten  zu gewinnen
        /// </summary>
        /// <param name="PolyLineSegmentsListList"> Liste Polylininen Segmente</param>
        /// <returns>Liste Polylinien</returns>
        List<List<shapes.Polyline>> ConvertPolyLineSegmentsToPolyline(List<List<PolyLineSegment>> PolyLineSegmentsListList)
        {
            List<List<shapes.Polyline>> shapePolylineList = new List<List<System.Windows.Shapes.Polyline>>();
            foreach (List<PolyLineSegment> polyLineSegmentsList in PolyLineSegmentsListList)
            {
                List<shapes.Polyline> ListPL = new List<System.Windows.Shapes.Polyline>();
                foreach (PolyLineSegment PLS in polyLineSegmentsList)
                {
                    shapes.Polyline PL = new System.Windows.Shapes.Polyline();
                    foreach (System.Windows.Point Pt in PLS.Points)
                    {
                        PL.Points.Add(Pt);
                    }
                    ListPL.Add(PL);
                }
                shapePolylineList.Add(ListPL);
            }
            return shapePolylineList;
        }
        /// <summary>
        /// versucht, Liste von Listen von polylinien zu LwPolylinien umzuwandeln
        /// </summary>
        /// <param name="listPLs">List Polylinien</param>
        /// <returns>Liste Lw Polylinien</returns>
        List<List<LwPolyline>> ToLwPolyline2(List<List<shapes.Polyline>> listPLs)
        {
            List<List<LwPolyline>> all_Lists_LWPL = new List<List<LwPolyline>>();
            foreach (List<shapes.Polyline> listPL in listPLs)
            {
                List<LwPolyline> new_List_LwPL = new List<LwPolyline>();
                foreach (shapes.Polyline PL in listPL)
                {
                    LwPolyline new_LWPL = new LwPolyline();
                    List<LwPolylineVertex> new_LWPL_Vertex = new List<LwPolylineVertex>();
                    foreach (System.Windows.Point Pt in PL.Points)
                    {
                        new_LWPL.Vertexes.Add(new LwPolylineVertex(Pt.X, Pt.Y));
                    }

                    new_List_LwPL.Add(new_LWPL);
                }
                all_Lists_LWPL.Add(new_List_LwPL);

            }


            return all_Lists_LWPL;

        }
        /// <summary>
        /// Versucht eine Liste von Listen von Punkten zu einer Liste von Shape Polylinien umzuwandeln, wobei jede innere Liste einer Polylinie entspricht
        /// </summary>
        /// <param name="listOfPoints">Liste von der Liste von den Punkten, die konvertiert werden müssen</param>
        /// <returns>Liste von Shape Polylinien</returns>
        List<shapes.Polyline> ToPolyLine2(List<List<System.Drawing.Point>> listOfPoints)
        {
            List<shapes.Polyline> listPls = new List<System.Windows.Shapes.Polyline>();
            foreach (List<System.Drawing.Point> polyLine_as_List_Of_Point in listOfPoints)
            {
                shapes.Polyline neuShPolyLine = new System.Windows.Shapes.Polyline();
                foreach (System.Drawing.Point pt in polyLine_as_List_Of_Point)
                {
                    neuShPolyLine.Points.Add(new System.Windows.Point(pt.X, pt.Y));
                }
                listPls.Add(neuShPolyLine);
            }


            return listPls;
        }

        #endregion


        #region public Methoden
        /// <summary>
        /// Erstellt die letzendliche DXF Datei und speichert die ab
        /// </summary>
        /// <param name="File_path_saving"> Der Pfad , wo gespeichert wird</param>
        public void Set_Dxf_File(string File_path_saving)
        {

            dxf_File = new DxfDocument(netDxf.Header.DxfVersion.AutoCad2013);

                    ListAddedElements_as_PLS = ToPolylineSegment(ListAddedElements);
                    ListAddedElements_as_PL = ConvertPolyLineSegmentsToPolyline(ListAddedElements_as_PLS);
                    allLists_LwPolyline = ToLwPolyline2(ListAddedElements_as_PL);

                    foreach (List<LwPolyline> list_LwPL in allLists_LwPolyline)
                    {
                        foreach (LwPolyline LwPL in list_LwPL)
                        {
                            LwPL.Layer = new netDxf.Tables.Layer(Werkzeug);
                            dxf_File.AddEntity(LwPL);
                        }
                    }

            netDxf.Entities.Point newPt = new netDxf.Entities.Point(Width, Height, Depth);
            dxf_File.AddEntity(newPt);

            dxf_File.Save(File_path_saving);
        }

        public void saveDxfFromListListPoints(string savePath, List<List<System.Drawing.Point>> listListPoints)
        {
            ListAddedElements_as_List_Of_Point = listListPoints;
            dxf_File = new DxfDocument(netDxf.Header.DxfVersion.AutoCad2013);

            listToPolyline = ToPolyLine2(ListAddedElements_as_List_Of_Point);
            ListAddedElements_as_PL = new List<List<shapes.Polyline>>();
            ListAddedElements_as_PL.Add(listToPolyline);
            allLists_LwPolyline = ToLwPolyline2(ListAddedElements_as_PL);


            foreach (List<LwPolyline> List_LwPL in allLists_LwPolyline)
            {
                foreach (LwPolyline LwPL in List_LwPL)
                {
                    LwPL.Layer = new netDxf.Tables.Layer(Werkzeug);
                    dxf_File.AddEntity(LwPL);
                }

            }

            netDxf.Entities.Point newPt = new netDxf.Entities.Point(Width, Height, Depth);
            dxf_File.AddEntity(newPt);

            dxf_File.Save(savePath);
        }

        /// <summary>
        /// Fügt Entity Objekte in einer bereits vorhandenen DXF Datei hinzu.  
        /// </summary>
        /// <param name="dxf_path"> Pfad der DXF Datei, wo hinzugefügt werden soll</param>
        /// <param name="listObj"> Liste der hinzuzufügenden Entity Objekte</param>
        static public void JoinToDxF(string dxf_path, ElementManager.workpiece workPiece, params EntityObject[] listObj)
        {
            DxfDocument doc;
            if (File.Exists(dxf_path))
            {
                FileInfo file = new FileInfo(dxf_path);
                if (file.Extension == ".dxf")
                {
                    doc = DxfDocument.Load(dxf_path);
                    foreach (EntityObject item in listObj)
                    {
                        item.Layer = new netDxf.Tables.Layer(workPiece.werkzeugName);

                        doc.AddEntity(item);

                    }
                    netDxf.Entities.Point newPt = new netDxf.Entities.Point(workPiece.height, workPiece.width, workPiece.depth);
                    doc.AddEntity(newPt);
                    doc.Save(dxf_path);

                }
                else
                {
                    throw new Exception(" Error NTB_DXF_200003 : The file you gave is not a dxf file");
                }
            }
            else
            {
                throw new Exception(" Error NTB_DXF_200004 : The path you entered is not corrrect");
            }
        }
        /// <summary>
        /// Fügt Entity Objekte in einer bereits vorhandenen DXF Datei hinzu.
        /// </summary>
        /// <param name="dxf_path">Pfad des DXF Documents, wo es hingespeichert werden soll</param>
        /// <param name="doc">DXF Document wo es hinzugefügt werden soll</param>
        /// <param name="workPiece">Entsprechende Arbeitseigenschaften</param>
        /// <param name="listObj">Liste der hinzuzufügenden Entity Objekte</param>
        static public void JoinToDxF(string dxf_path, ref DxfDocument doc, ElementManager.workpiece workPiece, params EntityObject[] listObj)
        {


            foreach (EntityObject item in listObj)
            {
                item.Layer = new netDxf.Tables.Layer(workPiece.werkzeugName);

                doc.AddEntity(item);

            }
            netDxf.Entities.Point newPt = new netDxf.Entities.Point(workPiece.height, workPiece.width, workPiece.depth);
            doc.AddEntity(newPt);
            doc.Save(dxf_path);

        }
        /// <summary>
        /// Fügt Entity Objekte in einer bereits vorhandenen DXF Datei hinzu.
        /// </summary>
        /// <param name="dxf_path">Pfad des DXF Documents, wo es hingespeichert werden soll</param>
        /// <param name="doc">DXF Document wo es hinzugefügt werden soll</param>
        /// <param name="listObj">Liste der hinzuzufügenden Entity Objekte</param>
        static public void JoinToDxF(string dxf_path,ref DxfDocument doc, params EntityObject[] listObj)
        {
            foreach (EntityObject item in listObj)
            {
                doc.AddEntity(item);
            }

            doc.Save(dxf_path);
        }
        /// <summary>
        /// Fügt Arbeitseigenschaften in einer bereits vorhandenen DXF Datei hinzu.
        /// </summary>
        /// <param name="dxf_path">Pfad des DXF Documents, wo es hingespeichert werden soll</param>
        /// <param name="doc">DXF Document wo es hinzugefügt werden soll</param>
        /// <param name="workPiece">Entsprechende Arbeitseigenschaften</param>
        static public void JoinToDxF(string dxf_path,ref DxfDocument doc, ElementManager.workpiece workPiece)
        {


            foreach (LwPolyline item in doc.LwPolylines)
            {
                item.Layer = new netDxf.Tables.Layer(workPiece.werkzeugName);
            }
            netDxf.Entities.Point newPt = new netDxf.Entities.Point(workPiece.height, workPiece.width, workPiece.depth);
            doc.AddEntity(newPt);
            doc.Save(dxf_path);

        }

        /// <summary>
        /// Fügt Entity Objekte in dem aktuellen DXF Objekt hinzu.  
        /// </summary>
        /// <param name="listObj"> Liste der hinzuzufügenden Entity Objekte</param>
        public void JoinToDxF(params EntityObject[] listObj)
        {

            foreach (EntityObject item in listObj)
            {
                dxf_File.AddEntity(item);
                dxf_File.Save(dxf_File.Name);
            }


        }

        #endregion

    }
}
