using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using System.IO;
using netDxf.Entities;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Media;
using shapes = System.Windows.Shapes;

namespace DXF_Management
{
    public class DXF_Reader
    {

        string DXF_path;
        public static string startupPath = System.IO.Path.GetFullPath(@"..\..\..\..\");

        List<netDxf.Entities.Polyline> listConvertedPolyline = new List<netDxf.Entities.Polyline>();
        List<netDxf.Entities.LwPolyline> listConvertedLwPolyline = new List<netDxf.Entities.LwPolyline>();
        List<System.Windows.Shapes.Polyline> listConvertedShapePolylines = new List<System.Windows.Shapes.Polyline>();
        List<System.Windows.Media.PolyLineSegment> listConvertedPolylineSegments = new List<PolyLineSegment>();
        PathGeometry pathGeo = new PathGeometry();
        bool schon_konvertiert = false;

        #region public field
        public netDxf.DxfDocument loaded_DXF
        {
            get
            {
                return DxfDocument.Load(DXF_path);
            }
            private set
            {
                loaded_DXF = value;
            }
        }
        #endregion
        #region Konstruktor
        /// <summary>
        /// Konstruktor 
        /// </summary>
        /// <param name="dxf_path"> Pfad der DXF Datei der ausgelesen werden soll </param>
        public DXF_Reader(string dxf_path)
        {
            if (File.Exists(dxf_path))
            {
                FileInfo file = new FileInfo(dxf_path);
                if (file.Extension == ".dxf")
                {
                    DXF_path = dxf_path;
                    ConvertArcsToPolylines();
                }
                else
                {
                    throw new Exception("The file you gave is not a dxf file");
                }
            }
            else
            {
                throw new Exception("The path you entered is not corrrect");
            }

        }
        #endregion
        #region private Methods
        /// <summary>
        /// Hier versucht die Funktion alle Figuren in Polylinien zu konvertieren. Sachen, die nicht umgewandelt werden können, werden gelöscht.
        /// </summary>
        void ConvertArcsToPolylines()
        {
            try
            {
                List<netDxf.Entities.Arc> listArc = loaded_DXF.Arcs.ToList();
                foreach (netDxf.Entities.Arc item in listArc)
                {
                    listConvertedLwPolyline.Add(item.ToPolyline(20));
                    loaded_DXF.RemoveEntity(item);
                }
            }
            catch
            {
            }

            try
            {
                List<netDxf.Entities.Circle> listCircle = loaded_DXF.Circles.ToList();
                foreach (netDxf.Entities.Circle item in listCircle)
                {
                    listConvertedLwPolyline.Add(item.ToPolyline(20));
                    loaded_DXF.RemoveEntity(item);
                }
            }
            catch
            {
            }
         
            try
            {
                List<netDxf.Entities.Ellipse> listEllipse = loaded_DXF.Ellipses.ToList();
                foreach (netDxf.Entities.Ellipse item in listEllipse)
                {
                    listConvertedLwPolyline.Add(item.ToPolyline(20));
                    loaded_DXF.RemoveEntity(item);
                }
            }
            catch
            {
            }
            try
            {
                List<netDxf.Entities.Face3d> listFace3D = loaded_DXF.Faces3d.ToList();
                foreach (netDxf.Entities.Face3d item in listFace3D)
                {
                    loaded_DXF.RemoveEntity(item);
                }
            }
            catch
            {
            }
            try
            {
                List<netDxf.Entities.Image> listImage = loaded_DXF.Images.ToList();
                foreach (netDxf.Entities.Image item in listImage)
                {
                    loaded_DXF.RemoveEntity(item);
                }
            }
            catch
            {
            }
          
            try
            {
                List<netDxf.Entities.LwPolyline> listLwPolyLine = loaded_DXF.LwPolylines.ToList();
                foreach (netDxf.Entities.LwPolyline item in listLwPolyLine)
                {
                    listConvertedLwPolyline.Add(item);
                }
            }
            catch
            {
            }
         
            try
            {
                List<netDxf.Entities.MLine> listMLine = loaded_DXF.MLines.ToList();
                foreach (netDxf.Entities.MLine item in listMLine)
                {

                    loaded_DXF.RemoveEntity(item);
                }
            }
            catch
            {

            }
          
            try
            {
                List<netDxf.Entities.MText> listMTexts = loaded_DXF.MTexts.ToList();
                foreach (netDxf.Entities.MText item in listMTexts)
                {

                    loaded_DXF.RemoveEntity(item);
                }
            }
            catch
            {

            }
            
            try
            {
                List<netDxf.Entities.XLine> listXLine = loaded_DXF.XLines.ToList();
                foreach (netDxf.Entities.XLine item in listXLine)
                {

                    loaded_DXF.RemoveEntity(item);
                }
            }
            catch
            {

            }
            
            try
            {
                List<netDxf.Entities.Spline> listSpline = loaded_DXF.Splines.ToList();
                foreach (netDxf.Entities.Spline item in listSpline)
                {
                    listConvertedPolyline.Add(item.ToPolyline(20));
                    loaded_DXF.RemoveEntity(item);

                }
            }
            catch
            {

            }

            try
            {
                List<netDxf.Entities.Line> listLine = loaded_DXF.Lines.ToList();

            }
            catch
            {

            }
        
            try
            {
                List<netDxf.Entities.Point> listPoints = loaded_DXF.Points.ToList();
                foreach (netDxf.Entities.Point item in listPoints)
                {

                    loaded_DXF.RemoveEntity(item);
                }
            }
            catch
            {

            }

            //loaded_DXF.Save("tmp");
        }
        /// <summary>
        /// Convert polylines to Polylines from shape
        /// </summary>
        void convertLwPolylinesToShapePolylines()
        {
            foreach (LwPolyline lwPolyline in listConvertedLwPolyline)
            {
                shapes.Polyline shapePolyline = new shapes.Polyline();

                PointCollection listOfPoints = new PointCollection();

                List<System.Windows.Point> ListPoi = listOfPoints.ToList();
                
                foreach (LwPolylineVertex lwPolylineVertex in lwPolyline.Vertexes.ToList())
                {
                    System.Windows.Point actualPoint = new System.Windows.Point(lwPolylineVertex.Location.X, lwPolylineVertex.Location.Y);
                    listOfPoints.Add(actualPoint);

                }

                shapePolyline.Points = listOfPoints;

                listConvertedShapePolylines.Add(shapePolyline);
            }
        }
        /// <summary>
        /// Erstellt eine Liste von PolySegmenten von allen Geomitrien, die übergeben wurdenn
        /// </summary>
        void convertToPolylineSegments()
        {
            List<PolyLineSegment> ListPolySeg = new List<PolyLineSegment>();
            foreach (LwPolyline item in listConvertedLwPolyline)
            {
            PointCollection listPoi = new PointCollection();
            PolyLineSegment neuPolyline = new PolyLineSegment();
           
                foreach (LwPolylineVertex item2 in item.Vertexes)
                {
                    System.Windows.Point neuPoint = new System.Windows.Point(item2.Location.X, item2.Location.Y);
                    listPoi.Add(neuPoint);
                 }
                
                neuPolyline.Points = listPoi;
           
                ListPolySeg.Add(neuPolyline);
            }
            ListPolySeg = listConvertedPolylineSegments;
            
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Remove all Entity Oject from the dxf File
        /// </summary>
        /// <param name="listObj"> List of Entity that should be removed</param>
        public void RemoveListEntities(params List<EntityObject>[] listObj)
        {
            foreach (List<netDxf.Entities.EntityObject> item in listObj)
            {
                foreach (netDxf.Entities.EntityObject item1 in item)
                {
                    loaded_DXF.RemoveEntity(item1);
                    this.loaded_DXF.Save(this.DXF_path);
                }

            }

        }
        /// <summary>
        /// Prüft alles in der Dxf Datei und macht alles in Polylinien
        /// </summary>
        public void Rebuild_DXf()
        {
            ConvertArcsToPolylines();

        }
        /// <summary>
        /// Gibt alle aus der Dxf Datei extrahierten LwPolylinien 2.Grad zurück
        /// </summary>
        /// <returns> List von LwPolylinien </returns>
        public List<LwPolyline> GiveLwPolylineList()
        {
            Rebuild_DXf();
            return this.listConvertedLwPolyline;

        }
        /// <summary>
        /// Gibt alle aus der Dxf Datei extrahierten Polylinien 3.Grad zurück
        /// </summary>
        /// <returns> List von Polylinien </returns>
        public List<netDxf.Entities.Polyline> GivePolylineList()
        {
            Rebuild_DXf();
            return this.listConvertedPolyline;

        }
        /// <summary>
        /// Convert and give all Polyline in System.Windows.Shape Format
        /// </summary>
        /// <returns>List of Polylines in Shape Format </returns>
        public List<shapes.Polyline> GiveShapePolylineList()
        {
            //Rebuild_DXf();
            convertLwPolylinesToShapePolylines();
            return this.listConvertedShapePolylines;
        }


        /// <summary>
        /// Give the List of all Polysegments
        /// </summary>
        /// <returns> The List of all pathsegments </returns>
        public List<PolyLineSegment> GivePolylineSegmentsList()
        {
            Rebuild_DXf();
            convertToPolylineSegments();
            return this.listConvertedPolylineSegments;

        }
        public static DxfDocument GetDXF_Document(string Path)
        {
            return DxfDocument.Load(Path);
        }

        #endregion

    }
}
