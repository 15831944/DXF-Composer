using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents.DocumentStructures;
using FillControler;
using TextManager;
using XmlSystem;
using Path = System.Windows.Shapes.Path;


namespace _20161029_WPF_TTF_Testfile
{

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public RectangleGeometry MyRectangleGeometry;

        private TtfManager TtfObject; //the oneAndOnly!

        private PathGeometry globalPathGeometry;

        public MainWindow()
        {
            InitializeComponent();
            ClassToXml();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            AddTextGeometry();
            globalPathGeometry = TtfObject.TextAsPathGeometry;
            //AddFilling();

            //ClassToXml();

        }

        public void ClassToXml()
        {
            string startupPath = System.IO.Path.GetFullPath(@"..\..\..\..\");
            HardwareConfig m_list = XmlHandler.XmlReader<HardwareConfig>(startupPath + @"HardwareConfig.xml");
            SoftwareConfig mySoftwareConfig = XmlHandler.XmlReader<SoftwareConfig>(startupPath + @"SoftwareConfig.xml");


            Dictionary<string, string> myTestDictionary = new Dictionary<string, string>();

            foreach (SoftwareConfigPathsFile item in mySoftwareConfig.paths.installedFonts)
            {
                myTestDictionary.Add(item.type, item.Value);
            }

        }

        void AddFilling()
        {
            PathGeometry pGeometry = TtfObject.TextAsPathGeometry;
            DrawThisGeometry(FillTool.GetFillingLines(pGeometry, Convert.ToInt32(tb_desity.Text), Convert.ToInt32(tb_degree.Text)));
        }

        void AddTextGeometry()
        {
            // Ini-Werte:
            string ttfPath = @"C:\Windows\Fonts\ARIAL.TTF";
            double charSize = Convert.ToDouble(tb_size.Text);
            double resolution = Convert.ToDouble(tb_flattingTol.Text);
            Point offset = new Point(Convert.ToInt32(tb_xOffset.Text), Convert.ToInt32(tb_yOffset.Text));

            // TtfObjekt erstellen
            TtfObject = new TtfManager(ttfPath, tb_text.Text, charSize, resolution, false, offset);

            // Zum zeichnen
            PathGeometry wholeGeometry = TtfObject.TextAsPathGeometry; //Benutzt pures/Orignal Objekt (kein runden, enthält auch kreise Splines etc)
            
            // Pure Punkte (für DxfWriter)
            IEnumerable<List<Point>> allPoints = Converter.GeometryToListListPoints(TtfObject.TextAsPathGeometry, resolution, true);

            wholeGeometry = Converter.InvertHorizontally(wholeGeometry, 400);
            DrawThisGeometry(wholeGeometry);// Display the PathGeometry. 
        }

        void DrawThisGeometry(Geometry toDraw)
        {
            Path drawingPath = new Path
            {
                Fill = Brushes.DarkRed,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Data = toDraw
            };
            Drawing_Grid.Children.Add(drawingPath);
        }

        void DrawThisGeometry(Rect toDraw)
        {
            RectangleGeometry test = new RectangleGeometry(toDraw);

            Path drawingPath = new Path
            {
                Fill = null,
                Stroke = Brushes.Blue,
                StrokeThickness = 1,
                Data = test
            };
            Drawing_Grid.Children.Add(drawingPath);
        }

        void toExcelReadyFile(List<List<Point>> letters)
        {

            using (TextWriter writer = File.CreateText(@"C:\Users\Youri\Desktop\tmpProg\forExcelAllPoints.txt"))
            {
                foreach (List<Point> polyLine in letters)
                {
                    foreach (Point point in polyLine)
                    {
                        writer.WriteLine(point.X + "\t" + point.Y);
                    }
                    writer.WriteLine("");
                }
            }
        }
        
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            Drawing_Grid.Children.Clear();
        }

        private void btn_AddBoundingBbox_Click(object sender, RoutedEventArgs e)
        {
            //Draw Rectangle aka BoundaryBox
            //MyRectangleGeometry = new RectangleGeometry(TtfObject.BoundaryBox);
            //DrawThisGeometry(MyRectangleGeometry);
            //PathGeometry tmp = Converter.CloseAllPathGeometries(globalPathGeometry);
            DrawThisGeometry(FillTool.GetMaximumRect(TtfObject.TextAsPathGeometry));
        }

        private void btn_addFilling_Click(object sender, RoutedEventArgs e)
        {
            AddFilling();
        }

        private void btn_Move_Click(object sender, RoutedEventArgs e)
        {
            //TranslateTransform test = new TranslateTransform(10,10);

            Drawing_Grid.Children.Clear();
            globalPathGeometry = Converter.ScalingPathGeometry(globalPathGeometry, 1.1, true);

            DrawThisGeometry(globalPathGeometry);
        }
    }
}