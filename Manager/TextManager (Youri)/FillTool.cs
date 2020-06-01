using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using Point = System.Windows.Point;


namespace FillControler
{
    public static class FillTool
    {
        /// <summary>
        /// Malt eine PathFigure mit linien aus (gestrichelt)
        /// </summary>
        /// <param name="pGeometry">Die auszumalende pGeometry</param>
        /// <param name="abstandZwischenLinien">Der Abstand zwischen den Linien (z.B. 5-20)</param>
        /// <param name="winkel">Der Winkel der angeordneten Linien (z.B. 0 für Vertikale linien, 90 für Horizontale oder 45 für schräge)</param>
        /// <returns></returns>
        public static PathGeometry GetFillingLines(PathGeometry pGeometry, int abstandZwischenLinien, double winkel, bool isImportedDxf = false)
        {
            pGeometry = pGeometry.GetFlattenedPathGeometry(1, ToleranceType.Absolute);

            Rect boundary = GetMaximumRect(pGeometry);
            PathGeometry allLines = new PathGeometry();

            for (double abstandZurErstenLinie = 0; abstandZurErstenLinie < (boundary.Width + boundary.Height * Math.Tan(winkel * Math.PI / 180)); abstandZurErstenLinie += abstandZwischenLinien)
            {
                List<Point> allPoints = GetAllIntersections(boundary, pGeometry, abstandZurErstenLinie, winkel);
                if (allPoints.Count > 0)
                {
                    if (!isImportedDxf)
                    {
                        allPoints = SortIntersectionPoints(allPoints);
                    }

                    allLines.AddGeometry(MakeLines(allPoints));
                }
            }

            return allLines;
        }

        public static Rect GetMaximumRect(PathGeometry pg, double rahmen = 0)
        {
            pg = pg.GetFlattenedPathGeometry(1, ToleranceType.Absolute);
            PathSegment tmp = pg.Figures[0].Segments[0];
            Point firstPoint = ((PolyLineSegment)tmp).Points[0];

            Point max = firstPoint;
            Point min = firstPoint;

            foreach (PathFigure pathFigure in pg.Figures)
            {
                foreach (PathSegment segments in pathFigure.Segments)
                {
                    foreach (Point point in ((PolyLineSegment)segments).Points)
                    {
                        if (point.X > max.X)
                        {
                            max.X = point.X;
                        }
                        if (point.X < min.X)
                        {
                            min.X = point.X;
                        }
                        if (point.Y > max.Y)
                        {
                            max.Y = point.Y;
                        }
                        if (point.Y < min.Y)
                        {
                            min.Y = point.Y;
                        }
                    }
                }
            }

            min.X -= rahmen;
            min.Y -= rahmen;

            max.X += rahmen;
            max.Y += rahmen;

            return new Rect(min, max);
        }

        // intersect the intersection point may be stored in the Point I.
        public static bool GetSingleLineIntersection(Line a, Line b, ref Point interSectionPoint)
        {
            Point s1 = new Point();
            Point s2 = new Point();

            s1.X = a.X2 - a.X1; s1.Y = a.Y2 - a.Y1;
            s2.X = b.X2 - b.X1; s2.Y = b.Y2 - b.Y1;

            double s, t;
            s = (-s1.Y * (a.X1 - b.X1) + s1.X * (a.Y1 - b.Y1)) / (-s2.X * s1.Y + s1.X * s2.Y);
            t = (s2.X * (a.Y1 - b.Y1) - s2.Y * (a.X1 - b.X1)) / (-s2.X * s1.Y + s1.X * s2.Y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1) //collision detection
            {
                interSectionPoint.X = a.X1 + (t * s1.X);
                interSectionPoint.Y = a.Y1 + (t * s1.Y);
                return true;
            }
            return false; // No collision
        }

        public static List<Point> GetAllIntersections(Rect boundary, PathGeometry g, double abstandZurErstenLinie, double winkel)
        {
            Line fillingLine = new Line
            {
                X1 = boundary.TopLeft.X,
                Y1 = boundary.TopLeft.Y + abstandZurErstenLinie,
                X2 = boundary.TopLeft.X + abstandZurErstenLinie * Math.Tan(winkel * Math.PI / 180),
                Y2 = boundary.TopLeft.Y
            };

            List<Point> allPoints = new List<Point>();
            Point intersectionPoint = new Point(0, 0);
            Point start;
            Point end;

            foreach (PathFigure f in g.Figures) //durchläuft alle Figuren in der Geometry
            {
                end = f.StartPoint;

                foreach (PathSegment s in f.Segments) //durchläuft alle Segments der Figures
                {
                    foreach (Point currentPoint in ((PolyLineSegment)s).Points) //durchläuft alle Punkte der Segments
                    {
                        start = end;
                        end = currentPoint;
                        if (start == end) continue; //damit start && stop nicht gleich sind (z.B. am Anfang (ende?))
                        Line textBorder = new Line { X1 = start.X, Y1 = start.Y, X2 = end.X, Y2 = end.Y };
                        if (GetSingleLineIntersection(textBorder, fillingLine, ref intersectionPoint))
                        //geht rein falls es einen überschneidungspunkt giebt. Speichert diesen dann direkt in intersectionPoint
                        {
                            allPoints.Add(intersectionPoint);
                        }
                    }
                }
            }
            return allPoints;

        }

        public static List<Point> SortIntersectionPoints(List<Point> allPoints)
        {

            IOrderedEnumerable<Point> sorted = from element in allPoints
                                               orderby element.X
                                               select element;

            return sorted.ToList();
        }

        public static PathGeometry MakeLines(List<Point> allPoints)
        {
            PathGeometry myPathGeometry = new PathGeometry();
            bool nextIsStartpoint = true;
            Point startPoint = new Point();
            PathFigure pFigure = new PathFigure();
            foreach (Point point in allPoints)
            {
                if (nextIsStartpoint)
                {
                    startPoint = point;
                }
                else
                {
                    //pFigure.Segments.Add(new LineSegment(point, true));
                    List<PathSegment> endPoint = new List<PathSegment>();
                    PolyLineSegment myPls = new PolyLineSegment();
                    myPls.Points.Add(point);
                    endPoint.Add(myPls);

                    myPathGeometry.Figures.Add(new PathFigure(startPoint, endPoint, false));
                    //line.Segments.RemoveAt(0);
                }

                nextIsStartpoint = !nextIsStartpoint;
            }

            return myPathGeometry;
        }


        ///********************************************* ///
        ///                                              ///
        ///                 TESTING AREA                 ///
        ///                                              ///
        ///********************************************* ///

        /// <summary>
        /// final version to add Single Lines
        /// </summary>
        /// <returns></returns>
        private static PathGeometry addSingleLines_tester()
        {
            PathGeometry myPathGeometry = new PathGeometry();

            for (int i = 0; i < 100; i = i + 10)
            {
                PathFigure pathFigure1 = new PathFigure();
                pathFigure1.StartPoint = new Point(10 + i, 50 + i);
                pathFigure1.Segments.Add(new LineSegment(new Point(400 + i, 100 + i), true));
                myPathGeometry.Figures.Add(pathFigure1);
            }

            return myPathGeometry;
        }

        private static PathGeometry addExampleLetter_tester()
        {
            PathGeometry myPathGeometry = new PathGeometry();

            PathFigure pathFigure1 = new PathFigure();
            pathFigure1.StartPoint = new Point(70, 60);
            pathFigure1.Segments.Add(new LineSegment(new Point(80, 60), true));
            pathFigure1.Segments.Add(new LineSegment(new Point(80, 90), true));
            pathFigure1.Segments.Add(new LineSegment(new Point(70, 90), true));
            pathFigure1.Segments.Add(new LineSegment(new Point(70, 60), true));
            myPathGeometry.Figures.Add(pathFigure1);

            return myPathGeometry;
        }

        public static void SchnittpunktFinder_Tester()
        {
            Line lineA = new Line();
            lineA.X1 = lineA.Y2 = 0;
            lineA.X2 = lineA.Y1 = 50;

            Line lineB = new Line();
            lineB.X1 = lineB.Y1 = 0;
            lineB.X2 = lineB.Y2 = 50;

            Point interSectionPoint = new Point(0, 0);

            bool isIntersection = GetSingleLineIntersection(lineA, lineB, ref interSectionPoint);

            MessageBox.Show(isIntersection.ToString() + " at Point " + interSectionPoint.ToString());

            //lineA
        }
    }
}
