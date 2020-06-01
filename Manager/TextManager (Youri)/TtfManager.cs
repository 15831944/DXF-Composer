/*H**********************************************************************
* FILENAME :        TtfManager
*
* DESCRIPTION :
*       Transforms Text into Glyphs, Geometries and PolyLines 
* 
* AUTHOR :    Youri Seichter     START DATE :    30 Okt 2016
* 
*H*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace TextManager
{
    /// <summary>
    /// A class that provides tools that helps to make Textgeometries out of strings.
    /// </summary>
    public class TtfManager
    {
        /// <summary>
        /// The constructor that for the initial build of the string.
        /// </summary>
        /// <param name="ttfPath">The path of the TrueType-Font file.</param>
        /// <param name="text">The text you want to convert.</param>
        /// <param name="hintingEmSize">Equals the Point Size.</param>
        /// <param name="resolution">the max distance between Points in the poly-Aproxximation
        ///  of the Geometry. smaler values result in slower execution. If the res is smaler 
        /// than 0,000001, 0,000001 will be used</param>
        /// <param name="wannaFlat">set True of you want to strongly aproximate your Points so that you get less points.</param>
        /// <param name="offset">Where you Want to place your Text.</param>
        public TtfManager(string ttfPath, string text, double hintingEmSize, double resolution, bool wannaFlat, Point offset)
        {
            TtfPath = ttfPath;
            Text = text;
            HintingEmSize = hintingEmSize;
            Resolution = resolution;
            WannaFlat = wannaFlat;
            Offset = offset;

            BuildGeometry();
        }

        #region regionInitialValues
        /// <summary>
        /// Full Path to the .tff (TrueTypeFont) file you want to convert.
        /// </summary>
        private readonly string TtfPath;

        /// <summary>
        /// The text you want to convert.
        /// </summary>
        private readonly string Text;

        /// <summary>
        /// The size of the Geometry. Value between 12-72 is normal.
        /// </summary>
        private readonly double HintingEmSize;

        /// <summary>
        /// The max distance between Points in the poly-Aproxximation
        /// of the Geometry. Smaler values result in slower execution. If the res is smaler 
        /// than 0,000001, 0,000001 will be used. Use 1.0 if you dont know better.
        /// </summary>
        private readonly double Resolution;

        /// <summary>
        /// Set True if you want to strongly Aproximate the Geometry. Results in fewer Lines.
        /// </summary>
        private bool WannaFlat;

        /// <summary>
        /// The Point where you want to place your Geometry
        /// </summary>
        private readonly Point Offset;

        #endregion

        #region generatedProperties
        /// <summary>
        /// Gives you the text as PathFigure --> contains all PolyLineSegments!
        /// </summary>
        public PathFigure TextAsPathFigure
        {
            get
            {
                PathFigure wholePathFigure = new PathFigure();
                foreach (PolyLineSegment segment in TextAsPolyLineSegments)
                {
                    wholePathFigure.Segments.Add(segment);
                }
                return wholePathFigure;
            }
        }

        /// <summary>
        /// Main Object - contains all Lines
        /// </summary>
        public Geometry TextAsGeometry;

        /// <summary>
        /// Lets you grab the Text as Path Geometry. (Windows.Media)
        /// </summary>
        public PathGeometry TextAsPathGeometry
        {
            get
            {
                return TextAsGeometry.GetOutlinedPathGeometry(Resolution, ToleranceType.Relative);
            }
        }

        /// <summary>
        /// Lets you take the Text as PolyLineSegments (Windows.Media)
        /// </summary>
        public List<PolyLineSegment> TextAsPolyLineSegments => Converter.PathGeometryToPlsList(TextAsPathGeometry);

        /// <summary>
        /// A Windows.Media "Rect" that contains the boundaries of the Text including width, height, points etc.
        /// </summary>
        public Rect BoundaryBox => TextAsPathGeometry.Bounds;

        #endregion

        #region Functions
        /// <summary>
        /// Calculates / Renders the Main Geometry object
        /// </summary>
        /// <returns></returns>
        private void BuildGeometry()
        {
            Uri ttfPathUri = new Uri(TtfPath);
            GlyphTypeface glyphTypeface = new GlyphTypeface(ttfPathUri);
            ushort[] glyphIndexes = new ushort[Text.Length];
            double[] advanceWidths = new double[Text.Length];

            for (int n = 0; n < Text.Length; n++)
            {
                ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[Text[n]];
                glyphIndexes[n] = glyphIndex;

                double width = glyphTypeface.AdvanceWidths[glyphIndex] * HintingEmSize;
                advanceWidths[n] = width;
            }

            GlyphRun wholeString = new GlyphRun(
                glyphTypeface,//glyphTypeface
                0, //bidiLevel Specifies the bidirectional layout level. Even-numbered and zero values imply left-to-right layout; odd-numbered values imply right-to-left layout.
                false,//isSideways
                HintingEmSize, //renderingEmSize
                glyphIndexes,//glyphIndices (IList<ushort>)
                Offset,//Point baselineOrigin
                advanceWidths, //IList<double> advancedWiths
                null,//IList<Point> glyphOffsets
                null,//IList<char> characters
                null,//string deviceFontName
                null,//IList<ushort> clustermap
                null,//IList<bool>caretStops
                null //System.Windows.MarkupLanguage
                );

            TextAsGeometry = wholeString.BuildGeometry();
        }

        /// <summary>
        /// Altes original, sicherheitshalber noch behalten (zurzeit ungenutzt)
        /// </summary>
        /// <param name="allPointsContainer"></param>
        /// <returns></returns>
        private List<PolyLineSegment> OldPathGeometryToPlsList(PathGeometry allPointsContainer)
        {
            allPointsContainer = allPointsContainer.GetFlattenedPathGeometry(1.0, ToleranceType.Absolute);

            List<PolyLineSegment> allPolyLineSegments = new List<PolyLineSegment>();
            allPointsContainer = allPointsContainer.GetFlattenedPathGeometry(1.0, ToleranceType.Absolute);

            foreach (PathFigure figures in allPointsContainer.Figures)
            {
                List<Point> allPoints = new List<Point>();
                //allPoints.Add(figures.StartPoint);
                //isFirst = false;
                PolyLineSegment segment = (PolyLineSegment) figures.Segments[0];
                allPoints.Add(segment.Points[0]);

                foreach (PathSegment segments in figures.Segments)
                {
                    foreach (Point point in ((PolyLineSegment)segments).Points)
                    {
                        allPoints.Add(point);
                    }
                }
                allPoints.Add(segment.Points[0]); // fügt startpunkt hinzu damit der kreis geschlossen ist
                allPolyLineSegments.Add(new PolyLineSegment(allPoints, true));
            }
            
            return allPolyLineSegments;
        }

        /// <summary>
        /// Ignore that. Thats just for me.
        /// </summary>
        /// <param name="pGeometry"></param>
        private void TestFunctionByYouri(PathGeometry pGeometry)
        {
            StringBuilder sb = new StringBuilder();
            foreach (PathFigure figure in pGeometry.Figures)
            {
                Console.WriteLine(figure.StartPoint);
                foreach (PathSegment segment in figure.Segments)
                {
                    ScaleTransform test = new ScaleTransform(0.1,0.1);
                    foreach (Point point in ((PolyLineSegment)segment).Points)
                    {
                        sb.Append(point);
                    }
                }
            }
        }

        

        #endregion
    }
}
