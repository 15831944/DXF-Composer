using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using drawing = System.Drawing;
using point = System.Windows.Point;
using Shapes = System.Windows.Shapes;

//manager:
using DXF_Management;
using ElementManager;
using Microsoft.Win32;
using netDxf;
using TextManager;
using ToolsetFunctions;
using XmlSystem;
using Image_Symbol_Management;


namespace GUI_4._0
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string startupPath = Path.GetFullPath(@"..\..\..\..\");
        public static string fontsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

        //Manager uses:
        private ObservableCollection<creativeElement> elementCollection = new ObservableCollection<creativeElement>();
        private creativeElement actualElement;
        private TtfManager actualTtfObject;
        private workpiece myWorkpiece = new workpiece();

        private DXF_Reader dxfReader;
        private DXF_Writer dxfWriter;
        public string dxfSavePath;
        public string htwLogoPath;

        private HardwareConfig myHardwareConfig;
        private SoftwareConfig mySoftwareConfig;

        Image_Class imageElement;
        drawing.Image loadedImageFile;

        //Font variables
        private readonly Dictionary<string, string> fontDictionary = new Dictionary<string, string>();
        private double fontSize = 8;

        private Grid invertedGrid = new Grid();
        private Grid limitingGrid = new Grid();
        private int currentGridHeight;
        private int currentGridWidth;
        private int maxGridHeight = 510;
        private int maxGridWidth = 620;
        private int maxLimitingGridHeight = 510;
        private int maxLimitingGridWidth = 620;
        private int scalingFactor = 1;
        private bool dontScale = false;
        private int einspannungsGrenzen = 20;   //in mm

        //Variablen für TextManager:
        private int numberOfTextElements;
        private Point standardOffset = new Point(60, 240);

        //other stuff
        private bool readyForDrawing;
        private bool isAPathGeometryToBig;

        public MainWindow()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            InitializeComponent();
            InitializeStandardValues();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Own Methods:
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        private void InitializeStandardValues()
        {
            myHardwareConfig = XmlHandler.XmlReader<HardwareConfig>(startupPath + @"HardwareConfig.xml");
            mySoftwareConfig = XmlHandler.XmlReader<SoftwareConfig>(startupPath + @"SoftwareConfig.xml");

            einspannungsGrenzen = myHardwareConfig.einspannungsgrenze;

            //fonts in das dictionary einlesen
            foreach (var font in mySoftwareConfig.paths.installedFonts)
            {
                fontDictionary.Add(font.type, fontsFolderPath + font.Value);
            }
            foreach (var font in mySoftwareConfig.paths.additionalFonts)
            {
                fontDictionary.Add(font.type, startupPath + font.Value);
            }
            cB_fontFamily.ItemsSource = fontDictionary.Keys;

            dxfSavePath = startupPath + mySoftwareConfig.paths.TestDxf;
            htwLogoPath = startupPath + mySoftwareConfig.paths.HtwLogoOskar;

            foreach (var werkzeug in myHardwareConfig.werkzeuge)
            {
                cB_tools.Items.Add(werkzeug.id);
            }

            //set standardvalues
            cB_elementChoose.SelectedIndex = 0;
            //tB_incomingText.Text = mySoftwareConfig.standardValues.text.tB_incomingText;
            cB_fontFamily.Text = mySoftwareConfig.standardValues.text.cB_fontFamily;
            cB_fontSize.Text = mySoftwareConfig.standardValues.text.cB_fontSize.ToString();
            tB_depth.Text = "0";
            //cB_tools.SelectedIndex = 1;
            cB_abstandFillLinien.SelectedIndex = 0;
            cB_value.SelectedIndex = 2;
            cB_scalingForResizing.SelectedIndex = 1;

        }

        private void UpdateWpfElements()
        {
            if (myWorkpiece.height < (4 * einspannungsGrenzen) || myWorkpiece.width < (4 * einspannungsGrenzen))
            {
                MessageBox.Show("Bitte geben sie für Höhe und Breite des Werkstückes nur Werte über 80 an.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                tB_height.Text = 100.ToString();
                tB_width.Text = 100.ToString();
                RefreshWorkpieceData();
                return;
            }
            //delete old invertedGrid in sizingGrid
            sizingGrid.Children.Clear();

            //we are putting the elements on the inverted grid, the limitingGrid is to check for borders to write at dobot
            //the grid gets inverted so we have all elements ready to write into the dxf file and dont need to invert them there
            //we invert because the point of origin at software is at the upper left corner, we need every data from the bottom left corner
            //so we invert the y-axle
            invertedGrid = new Grid();
            limitingGrid = new Grid();

            ScaleTransform myScaleTransform = new ScaleTransform(1, -1);
            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(myScaleTransform);
            invertedGrid.LayoutTransform = myTransformGroup;

            if (myWorkpiece.height != 0 || myWorkpiece.width != 0)
            {
                if (dontScale)
                {
                    //keep scalingFActor
                    dontScale = true;
                }
                else
                {
                    UpdateScalingFactor();
                }


                //resize grid
                currentGridWidth = scalingFactor * Convert.ToInt32(myWorkpiece.width);
                currentGridHeight = scalingFactor * Convert.ToInt32(myWorkpiece.height);
                invertedGrid.Width = Convert.ToDouble(currentGridWidth);
                invertedGrid.Height = Convert.ToDouble(currentGridHeight);
                limitingGrid.Width = Convert.ToDouble(currentGridWidth) - (40 * scalingFactor);
                limitingGrid.Height = Convert.ToDouble(currentGridHeight) - (40 * scalingFactor);
                maxLimitingGridWidth = Convert.ToInt32(limitingGrid.Width);
                maxLimitingGridHeight = Convert.ToInt32(limitingGrid.Height);

                //create border around
                var myBorderForSizingGrid = GetBorder(Brushes.Gray);
                var myBorderForInvertedGrid = GetBorder(Brushes.Black);
                var myBorderForLimitingGrid = GetBorder(Brushes.Red);

                sizingGrid.Children.Add(myBorderForSizingGrid);
                invertedGrid.Children.Add(myBorderForInvertedGrid);
                limitingGrid.Children.Add(myBorderForLimitingGrid);

                sizingGrid.Children.Add(invertedGrid);
                sizingGrid.Children.Add(limitingGrid);

                GetTextElementForWorkpieceSize();
                cB_scalingFactor.IsEnabled = true;
            }


            //update lB_elements data
            lB_elements.Items.Clear();
            //activate or deactivate Buttons to edit elements
            CheckForMovingButtonsActivation();

            
            if (elementCollection.Count != 0)
            {
                foreach (var creativeElement in elementCollection)
                    lB_elements.Items.Add(creativeElement);
                DrawAllGeometries();
                lB_elements.SelectedItem = actualElement;
            }
                
        }

        private void CheckForMovingButtonsActivation()
        {
            if (elementCollection.Count > 0)
            {
                bt_moveLeft.IsEnabled = true;
                bt_moveDown.IsEnabled = true;
                bt_moveUp.IsEnabled = true;
                bt_moveRight.IsEnabled = true;

                bt_increaseFontSize.IsEnabled = true;
                bt_decreaseFontSize.IsEnabled = true;

                //bt_addHTWLogo.IsEnabled = true;
                bt_create.IsEnabled = true;
            }
            else
            {
                bt_moveLeft.IsEnabled = false;
                bt_moveDown.IsEnabled = false;
                bt_moveUp.IsEnabled = false;
                bt_moveRight.IsEnabled = false;

                bt_increaseFontSize.IsEnabled = false;
                bt_decreaseFontSize.IsEnabled = false;

                //bt_addHTWLogo.IsEnabled = false;
                bt_create.IsEnabled = false;
            }      
        }

        private void CheckForActivation()
        {
            if (readyForDrawing)
            {
                bool workpieceDataEntered = (cB_workpiece.SelectedIndex > -1 && cB_workpiece.SelectedIndex != 6 ||
                                             cB_workpiece.SelectedIndex == 6 && tB_height.Text != "" &&
                                             tB_width.Text != "")
                    ? true
                    : false;

                // all information are given
                if (cB_tools.SelectedIndex > -1 && workpieceDataEntered && tB_depth.Text != "")
                    tabItem_elements.IsEnabled = true;
                else
                    tabItem_elements.IsEnabled = false;
            }
        }

        private void DrawThisGeometry(creativeElement actualCreativeElement)
        {
            var drawingPath = new System.Windows.Shapes.Path();
            drawingPath.Stroke = Brushes.Black;
            drawingPath.StrokeThickness = 1;
            drawingPath.Data = actualCreativeElement.pathGeometry;
            invertedGrid.Children.Add(drawingPath);

            /*
            //draw a rectangle around geometry to check 
            Rect rect = Converter.GetMinimalMaxRect(actualCreativeElement.pathGeometry, 0);
            RectangleGeometry rectGeo = new RectangleGeometry(rect);
            var drawingPath3 = new System.Windows.Shapes.Path();
            drawingPath3.Stroke = Brushes.Black;
            drawingPath3.StrokeThickness = 1;
            drawingPath3.Data = rectGeo;
            invertedGrid.Children.Add(drawingPath3);
            */


            if (actualCreativeElement.filled)
            {
                var drawingPath2 = new System.Windows.Shapes.Path();
                drawingPath2.Stroke = Brushes.Black;
                drawingPath2.StrokeThickness = 1;
                drawingPath2.Data = actualCreativeElement.fillingPathGeometry;
                invertedGrid.Children.Add(drawingPath2);
            }
        }

        private void DrawAllGeometries()
        {
            if (elementCollection.Count == 0)
            {
            }
            else
            {
                foreach (var creativeElement in elementCollection)
                {
                    DrawThisGeometry(creativeElement);

                }
            }
        }

        private void RefreshWorkpieceData()
        {
            //updateScalingFactor();
            double height;
            double width;
            
            var try1 = Double.TryParse(tB_height.Text, out height);
            var try2 = Double.TryParse(tB_width.Text, out width);
            var try3 = Double.TryParse(tB_depth.Text, out myWorkpiece.depth);

                myWorkpiece.height = height;
                myWorkpiece.width = width;
                var messageBox = false;

                if (cB_tools.SelectedItem == null)
                {
                    MessageBox.Show("Bitte wählen sie ein Werkzeug aus, mit dem gearbeitet werden soll.", "Hinweis",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var name = Convert.ToString(cB_tools.SelectedItem);
                    myWorkpiece.werkzeugName = name;
                }


                if (try1 == false)
                {
                    messageBox = true;
                    tB_height.Text = "1";
                    //tB_height.Focus();
                }
                else if (try2 == false)
                {
                    messageBox = true;
                    tB_width.Text = "1";
                    //tB_width.Focus();
                }
                else if (try3 == false)
                {
                    messageBox = true;
                    tB_depth.Text = "1";
                    //tB_depth.Focus();
                }

                if (messageBox && readyForDrawing)
                {
                    MessageBox.Show("Bitte geben Sie nur positive Zahlen in die Felder für das Werkstück ein.", "Hinweis",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    tabItem_tools.Focus();
                }
            


        }

        private void CreateTextElement()
        {
            var name = tB_name.Text;
            var fill = (bool)checkBox_fill.IsChecked;
            var text = tB_incomingText.Text;
            var fontName = cB_fontFamily.Text;
            string ttfPath = fontDictionary[fontName];
            var charSize = fontSize * 5;
            var abstandFillLinien = Int32.Parse(cB_abstandFillLinien.Text);

            if (tB_incomingText.Text == "")
            {
                MessageBox.Show("Bitte geben Sie einen Text ein, der angezeigt werden soll.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (tB_name.Text == "")
            {
                MessageBox.Show("Bitte geben Sie Ihrem Element einen Namen.", "Hinweis", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else if (cB_fontFamily.Text == "")
            {
                MessageBox.Show("Bitte wählen sie eine Schriftart (Font) für ihr Textelement aus.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (cB_fontSize.Text == "")
            {
                MessageBox.Show("Bitte wählen sie eine Schriftgröße für ihr Textelement aus.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                actualTtfObject = new TtfManager(ttfPath, text, charSize, 1, false, standardOffset);
                PathGeometry invertedPathGeometry = Converter.InvertHorizontally(actualTtfObject.TextAsPathGeometry, maxGridHeight);

                //create new element and add it to the collection
                actualElement = new creativeElement(fill, name, true, name, abstandFillLinien, 45.0, scalingFactor, standardOffset, invertedPathGeometry);
                elementCollection.Add(actualElement);
                UpdateWpfElements();
                lB_elements.SelectedItem = actualElement;

                //set everything for new TextElement/reset
                numberOfTextElements++;
                tB_name.Text = "TextElement" + (numberOfTextElements + 1);
                tB_incomingText.Text = "";
            }
        }

        private void CreateDxfElement(bool imageObject)
        {
            var name = tB_name.Text;
            var fill = (bool)checkBox_fill.IsChecked;
            var path = tB_path.Text;
            var abstandFillLinien = Int32.Parse(cB_abstandFillLinien.Text);

            if (tB_name.Text == "")
            {
                MessageBox.Show("Bitte geben Sie Ihrem Element einen Namen.", "Hinweis", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else if (tB_path.Text == "")
            {
                MessageBox.Show("Bitte wählen Sie einen gültigen Pfad aus.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //alles für dxf dokumente
                dxfReader = new DXF_Reader(path);

                List<Shapes.Polyline> list = dxfReader.GiveShapePolylineList();
                PathGeometry actualPathGeometry = Converter.ListWinShapesPolyLineToPathGeometry(list);

                //when converting an image to dxf it is inverted, so i invert it again.
                if (imageObject)
                {
                    actualPathGeometry = Converter.InvertHorizontally(actualPathGeometry, 0);
                    //because of inverting the element it gets translated out of border, this is a bit of a fix
                    actualPathGeometry = Converter.TranslatePathGeometry(actualPathGeometry, 30, 200);
                    fill = false;
                }

                if (actualPathGeometry.IsEmpty())
                {
                    MessageBox.Show("Wir konnten diese Datei leider nicht einlesen. Das tut uns leid.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    //i move the dxf element so the point of origin is at the limitingGrid
                    actualPathGeometry = Converter.TranslatePathGeometry(actualPathGeometry, einspannungsGrenzen * scalingFactor,
                        einspannungsGrenzen * scalingFactor);

                    actualElement = new creativeElement(fill, name, false, null, abstandFillLinien, 45.0, scalingFactor, standardOffset, actualPathGeometry);
                    elementCollection.Add(actualElement);
                    UpdateWpfElements();
                    lB_elements.SelectedItem = actualElement;
                }

                //reset
                tB_path.Text = "";
                tB_name.Text = "";
            }
        }

        private void CreateImageElement()
        {
            string tmpDxfFilePath = startupPath + @"tmp.dxf";

            if (tB_name.Text == "")
            {
                MessageBox.Show("Bitte geben Sie Ihrem Element einen Namen.", "Hinweis", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else if (tB_path.Text == "")
            {
                MessageBox.Show("Bitte wählen Sie einen gültigen Pfad aus.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                loadedImageFile = drawing.Image.FromFile(tB_path.Text);
                imageElement = new Image_Class(loadedImageFile);
                imageElement.Set_and_GetDXF_File(tmpDxfFilePath);

                //set path so the now converted image file to dxf gets now converted into polylines and then to geometry
                tB_path.Text = tmpDxfFilePath;
                CreateDxfElement(true);
            }
        }

        private void UpdateScalingFactor()
        {
            //get maxGridSize
            maxGridHeight = Convert.ToInt32(sizingGrid.ActualHeight);
            maxGridWidth = Convert.ToInt32(sizingGrid.ActualWidth);

            //get scaling factor for grid
            int scalingWidth = 1;
            int scalingHeight = 1;
            var doIt = true;
            while (doIt)
                if ((scalingWidth + 1) * myWorkpiece.width <= maxGridWidth)
                    scalingWidth++;
                else
                    doIt = false;
            doIt = true;
            while (doIt)
                if ((scalingHeight + 1) * myWorkpiece.height <= maxGridHeight)
                    scalingHeight++;
                else
                    doIt = false;

            scalingFactor = (scalingHeight <= scalingWidth) ? scalingHeight : scalingWidth;
            myWorkpiece.scalingFactor = scalingFactor;

            switch (scalingFactor)
            {
                case 1:
                    cB_fontSize.Text = "11";
                    break;
                case 2:
                    cB_fontSize.Text = "16";
                    break;
                case 3:
                    cB_fontSize.Text = "20";
                    break;
                case 4:
                    cB_fontSize.Text = "28";
                    break;
                default:
                    cB_fontSize.Text = "11";
                    break;
            }
            double.TryParse(cB_fontSize.Text, out fontSize);
            bool a4 = (cB_workpiece.SelectedIndex == 0) ? true : false;
            GenerateGoodOffsetForScaling(a4);

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // WPF-triggered Events:
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        private void bt_browse_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Symbol/Bild-Auswahl"; 
            openFileDialog.InitialDirectory = startupPath;

            switch (cB_elementChoose.SelectedIndex)
            {
                case 1: //htw logo
                    openFileDialog.DefaultExt = "dxf";
                    openFileDialog.Filter = "DXF files (*.dxf)|*.dxf";
                    break;
                case 2: //esp datei
                    openFileDialog.DefaultExt = "dxf";
                    openFileDialog.Filter = "DXF files (*.dxf)|*.dxf";
                    break;
                case 3: //jpg/png datei
                    openFileDialog.DefaultExt = "jpg";
                    openFileDialog.Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png";
                    break;
                case 4: //png/jpg datei
                    openFileDialog.DefaultExt = "png";
                    openFileDialog.Filter = "PNG files (*.png)|*.png|JPG files (*.jpg)|*.jpg";
                    break;
                default:
                    break;
            }
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                tB_name.Text = openFileDialog.SafeFileName;
                tB_path.Text = openFileDialog.FileName;
            }
        }

        private void bt_add_Click(object sender, RoutedEventArgs e)
        {

            RefreshWorkpieceData();

            switch (cB_elementChoose.SelectedIndex + 1)
            {
                case 1: //textElement
                    CreateTextElement();
                    break;
                case 2: //htw logo
                    CreateHtwLogoElement();           
                    break;
                case 3: //dxf-File
                    CreateDxfElement(false);

                    break;
                case 4: //jpg/png einlesen
                    CreateImageElement();                   
                    break;
                case 5:  //png/jpg einlesen
                    if (tB_name.Text == "")
                    {
                        MessageBox.Show("Bitte geben Sie Ihrem Element einen Namen.", "Hinweis", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else if (tB_path.Text == "")
                    {
                        MessageBox.Show("Bitte wählen Sie einen gültigen Pfad aus.", "Hinweis",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        CreateImageElement();
                    }
                    break;
                default:
                    break;
            }

            //textelement
            if (cB_elementChoose.SelectedIndex == 0)
            {
                
            }
            else //dxf, jpg etc. files
            {
                
            }
        }

        private void bt_deleteElement_Click(object sender, RoutedEventArgs e)
        {
            if (lB_elements.HasItems)
            {
                var actualElement = (creativeElement) lB_elements.SelectedItem;
                elementCollection.Remove(actualElement);
                if (actualElement.textElement)
                    numberOfTextElements--;
                UpdateWpfElements();
            }
            else
            {
                MessageBox.Show("Sie können keine Elemente löschen, wenn keine vorhanden sind.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void bt_deleteAllElements_Click(object sender, RoutedEventArgs e)
        {
            if (lB_elements.HasItems)
            {
                var result = MessageBox.Show("Sind Sie sicher, dass sie alle Elemente löschen wollen?", "Achtung",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (MessageBoxResult.Yes == result)
                {
                    elementCollection.Clear();
                    lB_elements.Items.Clear();
                    numberOfTextElements = 0;
                    DrawAllGeometries();
                    UpdateWpfElements();
                }

            }
            else
            {
                MessageBox.Show("Sie können keine Elemente löschen, wenn keine vorhanden sind.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void bt_create_Click(object sender, RoutedEventArgs e)
        {
            // fusion of all the stuff and destroy the universe!
            RefreshWorkpieceData();

            //DXFWriterAlternative.writeDxfFile(elementCollection, myWorkpiece);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "dxf";
            saveFileDialog.Filter ="DXF files (*.dxf)|*.dxf";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var result = saveFileDialog.ShowDialog();
            dxfSavePath = saveFileDialog.FileName;


            //save file
            if (elementCollection.Count != 0 && result == true)
            {
                //resize geometries because of stretching for display purposes we need to decrease size with scalingFactor
                //create new collection with method that is downscaled
                ObservableCollection<creativeElement> newElementCollection = CreateDownScaledElementCollection(elementCollection);

                if (isAPathGeometryToBig)   //dont save
                {
                    MessageBox.Show("Bitte achten Sie darauf, dass sich alle Ihre erstellten Objekte innerhalb des roten Rahmens befinden.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else   //save
                {
                    /*
                    //create border in dxf (can be removed when ready)
                    creativeElement borderElement = new creativeElement(false, "Border", false, null, 10, 45, 1, standardOffset, CreateBorderAround());
                    newElementCollection.Add(borderElement);

                    //create inner border in dxf (can be removed when ready)
                    creativeElement innerBorderElement = new creativeElement(false, "innerBorder", false, null, 10, 45, 1, standardOffset, CreateInnerBorderAround());
                    newElementCollection.Add(innerBorderElement);
                    */
                    //save
                    dxfWriter = new DXF_Writer(newElementCollection, myWorkpiece);
                    dxfWriter.Set_Dxf_File(dxfSavePath);

                    //alternative save
                    //List<List<drawing.Point>> liste = CreateMegaList(newElementCollection);
                    //dxfWriter.saveDxfFromListListPoints(dxfSavePath, liste);

                    MessageBox.Show("Ihre \"dxf\" - Datei wurde erstellt. \nSie befindet sich unter:" + dxfSavePath, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void cB_elementChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Texteingabe
            if (cB_elementChoose.SelectedIndex == 0)
            {
                tB_path.Text = "";
                tB_name.Text = "";
                tB_incomingText.Visibility = Visibility.Visible;
                label_text.Visibility = Visibility.Visible;
                cB_fontFamily.Visibility = Visibility.Visible;
                cB_fontSize.Visibility = Visibility.Visible;
                label_fontFamily.Visibility = Visibility.Visible;
                label_fontSize.Visibility = Visibility.Visible;

                bt_browse.Visibility = Visibility.Hidden;
                tB_path.Visibility = Visibility.Hidden;
                label_path.Visibility = Visibility.Hidden;
                checkBox_fill.Visibility = Visibility.Visible;
                cB_abstandFillLinien.Visibility = Visibility.Visible;
                label9.Visibility = Visibility.Visible;

                tB_name.Text = "TextElement" + (numberOfTextElements + 1);
            }
            else
            {
                tB_incomingText.Visibility = Visibility.Hidden;
                label_text.Visibility = Visibility.Hidden;
                cB_fontFamily.Visibility = Visibility.Hidden;
                cB_fontSize.Visibility = Visibility.Hidden;
                label_fontFamily.Visibility = Visibility.Hidden;
                label_fontSize.Visibility = Visibility.Hidden;

                tB_path.Visibility = Visibility.Visible;
                label_path.Visibility = Visibility.Visible;
                checkBox_fill.Visibility = Visibility.Visible;
                cB_abstandFillLinien.Visibility = Visibility.Visible;
                label9.Visibility = Visibility.Visible;

                if (cB_elementChoose.SelectedIndex == 1)    //htw logo einlesen
                {
                    bt_browse.Visibility = Visibility.Hidden;
                    tB_path.Text = htwLogoPath;
                    tB_name.Text = "HTW-Logo";
                }
                else if (cB_elementChoose.SelectedIndex == 2)                           //dxf einlesen
                {
                    bt_browse.Visibility = Visibility.Visible;
                    tB_path.Text = "";
                    tB_name.Text = "";
                }
                else   //jpg, png files einlesen
                {
                    bt_browse.Visibility = Visibility.Visible;
                    checkBox_fill.Visibility = Visibility.Hidden;
                    cB_abstandFillLinien.Visibility = Visibility.Hidden;
                    label9.Visibility = Visibility.Hidden;
                    tB_path.Text = "";
                    tB_name.Text = "";
                }
            }
        }

        private void cB_tools_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckForActivation();
            if (cB_tools.SelectedItem.ToString() == "Stift")
            {
                tB_depth.IsEnabled = false;
                tB_depth.Text = 0.ToString();
            }
            else
            {
                tB_depth.IsEnabled = true;
                tB_depth.Text = 1.ToString();
            }

        }

        private void cB_workpiece_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            tB_height.IsEnabled = false;
            tB_width.IsEnabled = false;


            // A4 - 210mm x 297mm
            if (cB_workpiece.SelectedIndex == 0)
            {
                tB_height.Text = "210";
                tB_width.Text = "297";
            }
            // A5 - 148mm x 210mm
            else if (cB_workpiece.SelectedIndex == 1)
            {
                tB_height.Text = "148";
                tB_width.Text = "210";
            }
            // A6 - 105mm x 148mm
            else if (cB_workpiece.SelectedIndex == 2)
            {
                tB_height.Text = "105";
                tB_width.Text = "148";
            }
            // manual input
            else if (cB_workpiece.SelectedIndex == 3)
            {
                tB_height.IsEnabled = true;
                tB_width.IsEnabled = true;
                if (tB_height.Text == "" || tB_width.Text == "")
                {
                    tB_height.Text = "100";
                    tB_width.Text = "100";
                }
            }

            readyForDrawing = true;
            CheckForActivation();
            RefreshWorkpieceData();
            UpdateWpfElements();
        }

        private void tB_depth_LostFocus(object sender, RoutedEventArgs e)
        {
            var tryConvert = Double.TryParse(tB_depth.Text, out myWorkpiece.depth);
            if (!tryConvert)
            {
                tB_depth.Text = "1";
                MessageBox.Show(
                    "Bitte geben Sie nur positive, ganze Zahlen für die Tiefe des Werkstückes ein (in mm).", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                CheckForActivation();
                UpdateWpfElements();
            }
        }

        private void tB_height_LostFocus(object sender, RoutedEventArgs e)
        {
            var tryConvert = Double.TryParse(tB_height.Text, out myWorkpiece.height);
            if (!tryConvert)
            {
                tB_height.Text = "1";
                MessageBox.Show("Bitte geben Sie nur positive, ganze Zahlen für die Höhe des Werkstückes ein (in mm).",
                    "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                CheckForActivation();
                UpdateWpfElements();
            }
        }

        private void tB_width_LostFocus(object sender, RoutedEventArgs e)
        {
            var tryConvert = Double.TryParse(tB_width.Text, out myWorkpiece.width);
            if (!tryConvert)
            {
                tB_width.Text = "1";
                MessageBox.Show(
                    "Bitte geben Sie nur positive, ganze Zahlen für die Tiefe des Werkstückes ein (in mm).", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                CheckForActivation();
                UpdateWpfElements();
            }
        }

        private void tB_incomingText_LostFocus(object sender, RoutedEventArgs e)
        {
            tB_name.Text = "";
            tB_name.Text = "TextElement" + (numberOfTextElements + 1);
            // + ": \"" + tB_incomingText.Text + "\""
        }

        private void cB_fontSize_LostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(cB_fontSize.Text, out fontSize) == false)
            {
                MessageBox.Show("Bitte geben Sie nur positive Zahlen in das Feld für die Schriftgröße ein.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                cB_fontSize.Text = "11";
            }
        }

        private void bt_moveUp_Click(object sender, RoutedEventArgs e)
        {
            if (lB_elements.SelectedItem == null)
            {
                MessageBox.Show("Bitte markieren Sie zuerst ein Element aus der Listbox links neben Ihnen.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                actualElement = (creativeElement) lB_elements.SelectedItem;
                var oldOffset = actualElement.offset;
                var index = elementCollection.IndexOf(actualElement);
                double verschiebung;
                if (Double.TryParse(cB_value.Text, out verschiebung))
                {
                    actualElement = Toolset.moveObject(actualElement, "up", currentGridHeight, currentGridWidth, verschiebung, scalingFactor, einspannungsGrenzen);

                    elementCollection[index] = actualElement;
                    UpdateWpfElements();
                }
                else
                {
                    MessageBox.Show("Bitte geben Sie einen gültigen Wert für die Verschiebung ein.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void bt_moveLeft_Click(object sender, RoutedEventArgs e)
        {
            if (lB_elements.SelectedItem == null)
            {
                MessageBox.Show("Bitte markieren Sie zuerst ein Element aus der Listbox links neben Ihnen.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                actualElement = (creativeElement)lB_elements.SelectedItem;
                var oldOffset = actualElement.offset;
                var index = elementCollection.IndexOf(actualElement);
                double verschiebung;
                if (Double.TryParse(cB_value.Text, out verschiebung))
                {
                    actualElement = Toolset.moveObject(actualElement, "left", currentGridHeight, currentGridWidth, verschiebung, scalingFactor, einspannungsGrenzen);

                    elementCollection[index] = actualElement;
                    UpdateWpfElements();
                }
                else
                {
                    MessageBox.Show("Bitte geben Sie einen gültigen Wert für die Verschiebung ein.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void bt_moveRight_Click(object sender, RoutedEventArgs e)
        {
            if (lB_elements.SelectedItem == null)
            {
                MessageBox.Show("Bitte markieren Sie zuerst ein Element aus der Listbox links neben Ihnen.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                actualElement = (creativeElement)lB_elements.SelectedItem;
                var oldOffset = actualElement.offset;
                var index = elementCollection.IndexOf(actualElement);
                double verschiebung;
                if (Double.TryParse(cB_value.Text, out verschiebung))
                {
                    actualElement = Toolset.moveObject(actualElement, "right", currentGridHeight, currentGridWidth, verschiebung, scalingFactor, einspannungsGrenzen);

                    elementCollection[index] = actualElement;
                    UpdateWpfElements();
                }
                else
                {
                    MessageBox.Show("Bitte geben Sie einen gültigen Wert für die Verschiebung ein.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void bt_moveDown_Click(object sender, RoutedEventArgs e)
        {
            if (lB_elements.SelectedItem == null)
            {
                MessageBox.Show("Bitte markieren Sie zuerst ein Element aus der Listbox links neben Ihnen.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                actualElement = (creativeElement)lB_elements.SelectedItem;
                var oldOffset = actualElement.offset;
                var index = elementCollection.IndexOf(actualElement);
                double verschiebung;
                if (Double.TryParse(cB_value.Text, out verschiebung))
                {
                    actualElement = Toolset.moveObject(actualElement, "down", currentGridHeight, currentGridWidth, verschiebung, scalingFactor, einspannungsGrenzen);

                    elementCollection[index] = actualElement;
                    UpdateWpfElements();
                }
                else
                {
                    MessageBox.Show("Bitte geben Sie einen gültigen Wert für die Verschiebung ein.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void bt_increaseFontSize_Click(object sender, RoutedEventArgs e)
        {
            if (lB_elements.SelectedItem == null)
            {
                MessageBox.Show("Bitte markieren Sie zuerst ein Element aus der Listbox links neben Ihnen.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                actualElement = (creativeElement) lB_elements.SelectedItem;
                var index = elementCollection.IndexOf(actualElement);
                double increaseFactor;
                if (Double.TryParse(cB_scalingForResizing.Text, out increaseFactor))
                {
                    actualElement = Toolset.changeSize(actualElement, true, maxLimitingGridHeight, maxLimitingGridWidth, increaseFactor, scalingFactor, einspannungsGrenzen);

                    elementCollection[index] = actualElement;
                    UpdateWpfElements();
                }
                else
                {
                    MessageBox.Show("Bitte geben Sie einen gültigen Wert für die Verschiebung ein.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void bt_decreaseFontSize_Click(object sender, RoutedEventArgs e)
        {
            if (lB_elements.SelectedItem == null)
            {
                MessageBox.Show("Bitte markieren Sie zuerst ein Element aus der Listbox links neben Ihnen.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                actualElement = (creativeElement)lB_elements.SelectedItem;
                var index = elementCollection.IndexOf(actualElement);
                double decreaseFactor;
                if (Double.TryParse(cB_scalingForResizing.Text, out decreaseFactor))
                {
                    actualElement = Toolset.changeSize(actualElement, false, maxLimitingGridHeight, maxLimitingGridWidth, decreaseFactor, scalingFactor, einspannungsGrenzen);

                    elementCollection[index] = actualElement;
                    UpdateWpfElements();
                }
                else
                {
                    MessageBox.Show("Bitte geben Sie einen gültigen Wert für die Verschiebung ein.", "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void GUI_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (readyForDrawing)
                UpdateWpfElements();
            dontScale = false;
        }

        private PathGeometry CreateBorderAround()
        {
            point point1 = new point(0.0, 0.0);
            point point2 = new point(0.0, myWorkpiece.height);
            point point3 = new point(myWorkpiece.width, myWorkpiece.height);
            point point4 = new point(myWorkpiece.width, 0.0);

            List<System.Windows.Point> pointList = new List<System.Windows.Point>();
            pointList.Add(point1);
            pointList.Add(point2);
            pointList.Add(point3);
            pointList.Add(point4);
            IEnumerable<point> points = pointList;
            PointCollection pointColl = new PointCollection(points);


            PolyLineSegment newPolyLineSegment = new PolyLineSegment(points, false);

            List<Shapes.Polyline> polylineList = new List<Shapes.Polyline>();
            Shapes.Polyline polyline1 = new Shapes.Polyline();
            polyline1.Points = pointColl;
            polylineList.Add(polyline1);


            PathGeometry newPathGeometry = Converter.ListWinShapesPolyLineToPathGeometry(polylineList);
            newPathGeometry = Converter.CloseAllPathGeometries(newPathGeometry);
            return newPathGeometry;
        }

        private PathGeometry CreateInnerBorderAround()
        {
            double einspannung = Double.Parse(einspannungsGrenzen.ToString());
            point point1 = new point(einspannung, einspannung);
            point point2 = new point(einspannung, myWorkpiece.height - einspannung);
            point point3 = new point(myWorkpiece.width - einspannung, myWorkpiece.height - einspannung);
            point point4 = new point(myWorkpiece.width - einspannung, einspannung);

            List<System.Windows.Point> pointList = new List<System.Windows.Point>();
            pointList.Add(point1);
            pointList.Add(point2);
            pointList.Add(point3);
            pointList.Add(point4);
            IEnumerable<point> points = pointList;
            PointCollection pointColl = new PointCollection(points);


            PolyLineSegment newPolyLineSegment = new PolyLineSegment(points, false);

            List<Shapes.Polyline> polylineList = new List<Shapes.Polyline>();
            Shapes.Polyline polyline1 = new Shapes.Polyline();
            polyline1.Points = pointColl;
            polylineList.Add(polyline1);


            PathGeometry newPathGeometry = Converter.ListWinShapesPolyLineToPathGeometry(polylineList);
            newPathGeometry = Converter.CloseAllPathGeometries(newPathGeometry);
            return newPathGeometry;
        }

        private PathGeometry DownScaleStretchedGeometry(PathGeometry toDownScale)
        {
            PathGeometry downScaledPathGeometry = new PathGeometry();
            //only these 4 scalingFactors are possible
            switch (scalingFactor)
            {
                case 1:
                    downScaledPathGeometry = toDownScale;
                    break;
                case 2:
                    downScaledPathGeometry = Converter.ScalingPathGeometry(toDownScale, 0.5, true);
                    break;
                case 3:
                    downScaledPathGeometry = Converter.ScalingPathGeometry(toDownScale, 0.33, true);
                    break;
                case 4:
                    downScaledPathGeometry = Converter.ScalingPathGeometry(toDownScale, 0.25, true);
                    break;
            }
            return downScaledPathGeometry;
        }

        private Border GetBorder(Brush brushColor)
        {
            var myBorder = new Border();
            myBorder.BorderBrush = brushColor;
            myBorder.BorderThickness = new Thickness(mySoftwareConfig.standardValues.drawing.strokeThickness);
            return myBorder;
        }

        private void bt_HowTo_Click(object sender, RoutedEventArgs e)
        {
            string message1 = "Hier sind einige Informationen, wie man die Oberfläche bedienen sollte." + "\n";
            string message2 = "\n" + "Wählen Sie zuerst ein Werkzeug und Maße für ihr zu bearbeitendes Werkstück aus.";
            string message3 = "\n" + "Danach erhalten Sie im nebenstehenden Grid verschiedene Rahmen. Der schwarze Rahmen enthält die Maße Ihres Werkstücks." + "\n";
            string message4 = "\n" + "Bitte legen Sie spätestens jetzt fest, ob sie im Vollbildmodus arbeiten wollen, oder wie groß die Applikation sein soll. Späteres Verändern der Größe verschiebt Ihre angeordneten Elemente.";
            string message5 = "\n" + "Platzieren Sie bitte ihre Elemente nur in dem roten Bereich. Zur Sicherheit wird etwas Platz gelassen, sodass der Roboter nicht außerhalb des Werkstückes arbeitet." + "\n";
            string message6 = "\n" + "Wenn sie dann links den Tab wechseln, können Sie verschiedene Elemente platzieren. In der Ansicht darunter sind Ihre Elemente aufgelistet.";
            string message7 = "\n" + "Dort können Sie Ihre Objekte auswählen und mit den verschiedenen Elementen und Knöpfen unter der Arbeitsfläche modifizieren.";
            string message8 = "\n" + "Wenn Sie alles Ihren Wünschen entsprechend arrangiert haben, können Sie eine dxf-Datei erstellen, die dem DoBot-Roboter übergeben werden kann." + "\n";
            string message9 = "\n" + "Das Einlesen von Dateien funktioniert leider nicht in hundert Prozent der Fälle, da wir die Elemente aus den Dateien approximieren und in Polylinien umwandeln, um damit arbeiten zu können.";
            string message10 = "\n" + "Bitte haben Sie dafür Verständnis.";
            string message11 = "\n" + "";
            string message12 = "\n" + "Wenn sie jpg/png-Dateien einlesen";
            string message13 = "\n" + "";

            MessageBox.Show(message1 + message2 + message3 + message4 + message5 + message6 + message7 + message8 + message9 + message10 + message11 + message12 + message13, "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void bt_about_Click(object sender, RoutedEventArgs e)
        {
            string message1 = "Softy BOY's - DXF Composer" + "\n";
            string message2 = "version 1.0" + "\n";
            string message3 = "revision 2726" + "\n";
            string message4 = "netDXF version 0.9.3" + "\n";
            string message5 = "" + "\n";
            string message6 = "Dieses Programm wurde im Rahmen der HTW-Berlin, Studiengang Ingenieurinformatik im 3. Semester als Projekt im Modul Programmierung erstellt." + "\n";
            string message7 = "" + "\n";
            string message8 = "BOY = Brandon, Oskar & Youri" + "\n";
            string message9 = "Das Icon wurde von Leo Wegener erstellt."+ "\n";


            MessageBox.Show(message1 + message2 + message3 + message4 + message5 + message6 + message7 + message8 + message9, "Hinweis",
                    MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void cB_scalingFactor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch (cB_scalingFactor.SelectedIndex + 1)
            {
                  case 1:
                    scalingFactor = 1;
                    break;
                case 2:
                    scalingFactor = 2;
                    break;
                case 3:
                    scalingFactor = 3;
                    break;
                case 4:
                    scalingFactor = 4;
                    break;
            }

            switch (scalingFactor)
            {
                case 1:
                    cB_fontSize.Text = "11";
                    break;
                case 2:
                    cB_fontSize.Text = "16";
                    break;
                case 3:
                    cB_fontSize.Text = "20";
                    break;
                case 4:
                    cB_fontSize.Text = "28";
                    break;
                default:
                    cB_fontSize.Text = "11";
                    break;
            }
            double.TryParse(cB_fontSize.Text, out fontSize);

            bool a4 = (cB_workpiece.SelectedIndex == 0) ? true : false;
            GenerateGoodOffsetForScaling(a4);
            dontScale = true;
            UpdateWpfElements();
        }

        private ObservableCollection<creativeElement> CreateDownScaledElementCollection(ObservableCollection<creativeElement> oldElementCollection)
        {
            ObservableCollection<creativeElement> newElementCollection = new ObservableCollection<creativeElement>();
            isAPathGeometryToBig = false;

            foreach (var creativeElement in oldElementCollection)
            {
                //check if a pasted object is to big to save it
                if (Toolset.pathGeometryToBig(creativeElement.pathGeometry, maxLimitingGridHeight, maxLimitingGridWidth, scalingFactor, einspannungsGrenzen))
                {
                    isAPathGeometryToBig = true;
                }
                PathGeometry resultingPathGeometry = creativeElement.pathGeometry;
                PathGeometry newOne = DownScaleStretchedGeometry(resultingPathGeometry);
                resultingPathGeometry = newOne;


                //abstände beim füllen verkleinern wenn man downscaled
                double fillingFactor = 1;
                switch (scalingFactor)
                {
                    case 1:
                        //do nothing
                        break;
                    case 2:
                        fillingFactor = 0.5;
                        break;
                    case 3:
                        fillingFactor = 0.33;
                        break;
                    case 4:
                        fillingFactor = 0.25;
                        break;
                }

                /*
                if (creativeElement.textElement)
                {
                    resultingPathGeometry = Converter.CloseAllPathGeometries(resultingPathGeometry);
                }
                */
                double abstand1 = creativeElement.abstandZwischenLinien * fillingFactor;
                int abstand = Convert.ToInt32(abstand1);

                creativeElement newCreativeElement = new creativeElement(creativeElement.filled, creativeElement.name, creativeElement.textElement, creativeElement.text, abstand, creativeElement.winkel, 1, creativeElement.offset, resultingPathGeometry);
                newElementCollection.Add(newCreativeElement);
            }

            return newElementCollection;
        }

        private void CreateHtwLogoElement()
        {
            var name = tB_name.Text;
            var fill = (bool)checkBox_fill.IsChecked;
            var abstandFillLinien = Int32.Parse(cB_abstandFillLinien.Text);

            dxfReader = new DXF_Reader(htwLogoPath);

            PathGeometry htwLogo = Converter.ListWinShapesPolyLineToPathGeometry(dxfReader.GiveShapePolylineList());

            //i move the dxf element so the point of origin is at the limitingGrid
            PathGeometry newHtwLogo = Converter.TranslatePathGeometry(htwLogo, einspannungsGrenzen * scalingFactor,
                einspannungsGrenzen * scalingFactor);

            actualElement = new creativeElement(fill, name, false, null, abstandFillLinien, 45.0, 1, standardOffset, newHtwLogo);
            elementCollection.Add(actualElement);
            UpdateWpfElements();
        }

        private void GetTextElementForWorkpieceSize()
        {
            string ttfPath = fontDictionary["Arial"];
            double fontSize = 6.0;
            if (scalingFactor == 1)
                fontSize = 2.0;
            double charSize = fontSize * 5;
            point offset = new point(10, 0);

            TtfManager sizeTtfObject = new TtfManager(ttfPath, "A4", charSize, 1, false, offset);

            switch (cB_workpiece.SelectedIndex + 1)
            {
                case 1: //A4 - 210mm x 297mm
                    //already defined above
                    break;
                case 2: //A5 - 148mm x 210mm
                    sizeTtfObject = new TtfManager(ttfPath, "A5", charSize, 1, false, offset);
                    break;
                case 3: //A6 - 105mm x 148mm
                    sizeTtfObject = new TtfManager(ttfPath, "A6", charSize, 1, false, offset);
                    break;
                case 4: //manuelle maße
                    sizeTtfObject = new TtfManager(ttfPath, myWorkpiece.height + "x" + myWorkpiece.width, charSize/2, 1, false, offset);
                    break;
                default:
                    sizeTtfObject = new TtfManager(ttfPath, "nanu", charSize, 1, false, offset);
                    break;
            }

            var drawingPath = new System.Windows.Shapes.Path();
            drawingPath.Stroke = Brushes.Black;
            drawingPath.StrokeThickness = 1;
            drawingPath.Data = Converter.TranslatePathGeometry(Converter.InvertHorizontally(sizeTtfObject.TextAsPathGeometry, 0), 0, 10);
            invertedGrid.Children.Add(drawingPath);

        }

        private void GenerateGoodOffsetForScaling(bool a4)
        {
            //improves usability for workpiece: A4
            int amountOfElements = elementCollection.Count;
            if (a4)
            {
                switch (scalingFactor)
                {
                    case 1:
                        standardOffset = new Point(40, 800);
                        break;
                    case 2:
                        standardOffset = new Point(65, 650);
                        break;
                    case 3:
                        standardOffset = new Point(100, 450);
                        break;
                    case 4:
                        standardOffset = new Point(135, 350);
                        break;
                    default:
                        standardOffset = new Point(200, 400);
                        break;
                }
            }
            else
            {
                standardOffset = new Point(60, 240);
            }

            //wert für die startgröße des windows
            //Height = "653.564" Width = "1000
            if (GUI.ActualHeight <= 655.0 && GUI.ActualWidth <= 1001.0)
            {
                standardOffset = new Point(60, 240);
            }
            
        }

        private List<List<drawing.Point>> CreateMegaList(ObservableCollection<creativeElement> elementCollection)
        {
            List<List<drawing.Point>> allLines = new List<List<drawing.Point>>();

            foreach (var creativeElement in elementCollection)
            {
                List<List<drawing.Point>> partOfAllLines = Converter.GeometryToListListPointsDrawing(creativeElement.pathGeometry).ToList();

                foreach (var listOfOnePolyline in partOfAllLines)
                {
                    allLines.Add(listOfOnePolyline);
                }
            }

            return allLines;
        }
    } // class
} // namespace