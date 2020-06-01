using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TextManager;

namespace ElementManager
{
    public static class DXFWriterAlternative
    {
        public static string startupPath = Path.GetFullPath(@"..\..\..\..\");
        public static void writeDxfFile(ObservableCollection<creativeElement> elementCollection, workpiece myWorkpiece)
        {
            //startupPath += "GUI 4.0";
            using (StreamWriter writer = new StreamWriter(startupPath + @"\DXF_testdatei.dxf"))
            {
                //layer schreiben - werkzeugname
                writer.WriteLine("AcDbLayerTableRecord");
                writer.WriteLine("junk");
                writer.WriteLine(myWorkpiece.werkzeugName);
                writer.WriteLine(myWorkpiece.height);
                writer.WriteLine(myWorkpiece.width);        
                writer.WriteLine(myWorkpiece.depth);        //z-Koordinate für alle Punkte


                //werkstück informationen schreiben

                //polylinien schreiben
                foreach (var creativeElement in elementCollection)
                {
                    
                    foreach (var polyLineSegment in Converter.PathGeometryToPlsList(creativeElement.pathGeometry))
                    {
                        writer.WriteLine("AcDbPolyline");   //start jeder polylinie
                        PointCollection pointCollection = polyLineSegment.Points;

                        foreach (var point in pointCollection)
                        {
                            double xWert = point.X / myWorkpiece.scalingFactor;
                            double yWert = point.Y / myWorkpiece.scalingFactor;

                            //schreibe x
                            writer.WriteLine(" 10");                 
                            writer.WriteLine(xWert.ToString());
                            //y-invertieren
                            yWert = myWorkpiece.height - yWert;
                            //schreibe y
                            writer.WriteLine(" 20");  
                            writer.WriteLine(yWert.ToString());
                           
                        }
                        writer.WriteLine("ENDSEC");
                        
                    }

                }

            }

        }


    }
}
