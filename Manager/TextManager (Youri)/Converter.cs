using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shapes = System.Windows.Shapes;
using m = System.Windows.Media;
using drawing = System.Drawing;
using System.Windows;
using FillControler;


namespace TextManager
{
    public static class Converter
    {
        public static m.PathGeometry ListWinShapesPolyLineToPathGeometry(List<shapes.Polyline> toConvert)
        {
            m.PathGeometry myPathGeometry = new m.PathGeometry();


            foreach (shapes.Polyline polyline in toConvert)
            {
                m.PathFigure pf = new m.PathFigure();

                m.PolyLineSegment pls = new m.PolyLineSegment();
                pls.Points = polyline.Points;

                pf.Segments.Add(pls);
                pf.StartPoint = polyline.Points[0];
                myPathGeometry.Figures.Add(pf);
            }

            return myPathGeometry;
        }

        public static m.PathGeometry ListWinShapesPolyLineToPathGeometryOLD(List<shapes.Polyline> toConvert)
        {
            m.PathGeometry myPathGeometry = new m.PathGeometry();


            foreach (shapes.Polyline polyline in toConvert)
            {
                m.PathFigure pf = new m.PathFigure();

                m.PolyLineSegment pls = new m.PolyLineSegment();
                pls.Points = polyline.Points;

                pf.Segments.Add(pls);
                pf.StartPoint = polyline.Points[0];
                myPathGeometry.Figures.Add(pf);
            }

            return myPathGeometry;
        }

        /// <summary>
        /// Extracts the the Coordinates out of a PathGeometry and puts them into a PolyLineSegment.
        /// </summary>
        /// <param name="allPointsContainer">The PathGeometry that Contains the all the Coordinates you want to convert</param>
        /// <returns></returns>
        public static List<m.PolyLineSegment> PathGeometryToPlsListOld(m.PathGeometry allPointsContainer)
        {
            allPointsContainer = allPointsContainer.GetFlattenedPathGeometry(1.0, m.ToleranceType.Absolute);

            return
                allPointsContainer.Figures.Select(
                        figures =>
                                figures.Segments.SelectMany(segments => ((m.PolyLineSegment) segments).Points).ToList())
                    .Select(allPoints => new m.PolyLineSegment(allPoints, true))
                    .ToList();
        }

        public static List<m.PolyLineSegment> PathGeometryToPlsList(m.PathGeometry allPointsContainer)
        {
            List<m.PolyLineSegment> toReturn = new List<m.PolyLineSegment>();

            foreach (m.PathFigure figures in allPointsContainer.Figures) // figure = geschlossene umrisslinie
            {
                m.PolyLineSegment singlePls = new m.PolyLineSegment();
                singlePls.Points.Add(figures.StartPoint);

                foreach (m.PathSegment segment in figures.Segments)
                    //segment = offener Linien Typ (z.B. Bezier, PolyLineSegment etc)
                    
                    if (segment is m.PolyLineSegment)
                    {
                        foreach (Point point in ((m.PolyLineSegment) segment).Points)
                            singlePls.Points.Add(point);
                        Point tmp = ((m.PolyLineSegment) segment).Points[0];
                        singlePls.Points.Add(tmp); // fügt startpunkt hinzu damit der kreis geschlossen ist
                    }
                    else if (segment is m.LineSegment)
                    {
                        Point tmp = ((m.LineSegment) segment).Point;
                        singlePls.Points.Add(tmp); // fügt startpunkt hinzu damit der kreis geschlossen ist
                    }
                    else
                    {
                        throw new Exception(
                            "zu konvertierende PathGeometry beinhaltet weder PolyLineSegments noch LineSegments");
                    }
                toReturn.Add(singlePls);
            }
            return toReturn;
        }


        public static IEnumerable<List<drawing.Point>> GeometryToListListPointsDrawing(m.Geometry toConvert, double resolution = 20, bool flattening = false)
        {
            m.PathGeometry allPointsContainer = GeometryToPathGeometry(toConvert, resolution, flattening);

            List<List<drawing.Point>> allPoints = new List<List<drawing.Point>>();

            foreach (m.PathFigure figures in allPointsContainer.Figures)
            {
                List<drawing.Point> allSegmentPoints = new List<drawing.Point>();

                foreach (m.PathSegment segment in figures.Segments)
                {
                    foreach (Point point in ((m.PolyLineSegment)segment).Points)
                    {
                        int x = Convert.ToInt32(Math.Round(point.X));
                        int y = Convert.ToInt32(Math.Round(point.Y));
                        drawing.Point np = new drawing.Point(x,y);

                        allSegmentPoints.Add(np);
                    }

                    Point tmp = ((m.PolyLineSegment)segment).Points[0];
                    //allSegmentPoints.Add(tmp); // fügt startpunkt hinzu damit der kreis geschlossen ist
                }
                allPoints.Add(allSegmentPoints);
            }

            return allPoints;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toConvert">your Geometry (ie TtfObject.TextAsGeometry)</param>
        /// <param name="resolution">not even sure if this value matters (ie 30)</param>
        /// <param name="flattening"></param>
        /// <returns>Connecting all points of the inner list results in proper rendering</returns>
        public static IEnumerable<List<Point>> GeometryToListListPoints(m.Geometry toConvert, double resolution = 20,bool flattening = false)
        {
            m.PathGeometry allPointsContainer = GeometryToPathGeometry(toConvert, resolution, flattening);

            List<List<Point>> allPoints = new List<List<Point>>();

            foreach (m.PathFigure figures in allPointsContainer.Figures)
            {
                List<Point> allSegmentPoints = new List<Point>();

                foreach (m.PathSegment segment in figures.Segments)
                {
                    foreach (Point point in ((m.PolyLineSegment) segment).Points)
                    {
                        allSegmentPoints.Add(point);
                    }
                    Point tmp = ((m.PolyLineSegment) segment).Points[0];
                    //allSegmentPoints.Add(tmp); // fügt startpunkt hinzu damit der kreis geschlossen ist
                }
                allPoints.Add(allSegmentPoints);
            }

            return allPoints;
        }

        /// <summary>
        /// Returns the pure PathGeometry to easily display at the screen.
        /// </summary>
        /// <returns></returns>
        public static m.PathGeometry GeometryToPathGeometry(m.Geometry toConvert, double resolution = 20, bool wannaFlat = false)
        {
            m.PathGeometry myPathGeometry = toConvert.GetOutlinedPathGeometry(resolution,
                m.ToleranceType.Relative);

            if (wannaFlat)
                myPathGeometry = myPathGeometry.GetFlattenedPathGeometry(resolution, m.ToleranceType.Relative);

            return myPathGeometry;
        }

        public static m.PathGeometry TranslatePathGeometry(m.PathGeometry toTranslate, double xOffset, double yOffset, bool textElement = false)
        {
            if (textElement)
            {
                yOffset *= -1;
            }

            toTranslate = toTranslate.GetFlattenedPathGeometry(1, m.ToleranceType.Absolute);

            m.PathGeometry toReturn = new m.PathGeometry();

            foreach (m.PathFigure oldPf in toTranslate.Figures)
            {
                m.PathFigure newPf = new m.PathFigure();

                Point newStartPoint = oldPf.StartPoint;

                newStartPoint.X = newStartPoint.X + xOffset;
                newStartPoint.Y = newStartPoint.Y + yOffset;

                newPf.StartPoint = newStartPoint;

                foreach (m.PathSegment oldPs in oldPf.Segments)
                {
                    m.PathSegment newPs = new m.PolyLineSegment();

                    foreach (Point point in ((m.PolyLineSegment) oldPs).Points)
                    {
                        Point tmp = point;
                        tmp.X = tmp.X + xOffset;
                        tmp.Y = tmp.Y + yOffset;

                        ((m.PolyLineSegment) newPs).Points.Add(tmp);
                    }
                    newPf.Segments.Add(newPs);
                }
                toReturn.Figures.Add(newPf);
            }
            return toReturn;
        }

        public static m.PathGeometry ScalingPathGeometryOld(m.PathGeometry toDownsize, double scaling)
        {
            toDownsize = toDownsize.GetFlattenedPathGeometry(1, m.ToleranceType.Absolute);

            m.PathGeometry toReturn = new m.PathGeometry();

            foreach (m.PathFigure olfPf in toDownsize.Figures)
            {
                m.PathFigure newPf = new m.PathFigure();

                Point newStartPoint = olfPf.StartPoint;

                newStartPoint.X *= scaling;
                newStartPoint.Y *= scaling;

                newPf.StartPoint = newStartPoint;

                foreach (m.PathSegment oldPs in olfPf.Segments)
                {
                    m.PathSegment newPs = new m.PolyLineSegment();

                    foreach (Point point in ((m.PolyLineSegment) oldPs).Points)
                    {
                        Point tmp = point;
                        tmp.X *= scaling;
                        tmp.Y *= scaling;

                        ((m.PolyLineSegment) newPs).Points.Add(tmp);
                    }
                    newPf.Segments.Add(newPs);
                }
                toReturn.Figures.Add(newPf);
            }
            return toReturn;
        }

        public static Rect GetMinimalMaxRect(m.PathGeometry pg, double rahmen)
        {
            pg = pg.GetFlattenedPathGeometry(1, m.ToleranceType.Absolute);
            m.PathSegment tmp = pg.Figures[0].Segments[0];
            Point firstPoint = ((m.PolyLineSegment) tmp).Points[0];

            Point max = firstPoint;
            Point min = firstPoint;

            foreach (m.PathFigure pathFigure in pg.Figures)
            {
                foreach (m.PathSegment segments in pathFigure.Segments)
                {
                    foreach (Point point in ((m.PolyLineSegment) segments).Points)
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

        public static m.PathGeometry ScalingPathGeometry(m.PathGeometry toScale, double scaling, bool scaleAtZero = false)
        {
            toScale = toScale.GetFlattenedPathGeometry(1, m.ToleranceType.Absolute);

            Point pointToScale = new Point();

            if (scaleAtZero)
            {
                pointToScale = new Point(0,0);
            }
            else
            {
                Rect rect = GetMinimalMaxRect(toScale, 0);
                pointToScale = rect.BottomLeft;
            }


            m.PathGeometry toReturn = new m.PathGeometry();

            foreach (m.PathFigure olfPf in toScale.Figures)
            {
                m.PathFigure newPf = new m.PathFigure();

                Point newStartPoint = olfPf.StartPoint;

                newStartPoint.X = (newStartPoint.X - pointToScale.X) * scaling + pointToScale.X;
                newStartPoint.Y = (newStartPoint.Y - pointToScale.Y) * scaling + pointToScale.Y;

                newPf.StartPoint = newStartPoint;

                foreach (m.PathSegment oldPs in olfPf.Segments)
                {
                    m.PathSegment newPs = new m.PolyLineSegment();

                    foreach (Point point in ((m.PolyLineSegment)oldPs).Points)
                    {
                        Point tmp = point;
                        tmp.X = (tmp.X - pointToScale.X) * scaling + pointToScale.X;
                        tmp.Y = (tmp.Y - pointToScale.Y) * scaling + pointToScale.Y;

                        ((m.PolyLineSegment)newPs).Points.Add(tmp);
                    }
                    newPf.Segments.Add(newPs);
                }
                toReturn.Figures.Add(newPf);
            }
            return toReturn;
        }

        public static m.PathGeometry CloseAllPathGeometries(m.PathGeometry toClose)
        {
            toClose = toClose.GetFlattenedPathGeometry(1, m.ToleranceType.Absolute);

            m.PathGeometry toReturn = new m.PathGeometry();

            foreach (m.PathFigure oldPf in toClose.Figures)
            {
                m.PathFigure newPf = new m.PathFigure {StartPoint = oldPf.StartPoint};

                foreach (m.PathSegment oldPs in oldPf.Segments)
                {
                    m.PathSegment newPs = new m.PolyLineSegment();

                    foreach (Point point in ((m.PolyLineSegment) oldPs).Points)
                    {
                        ((m.PolyLineSegment)newPs).Points.Add(point);
                    }

                    ((m.PolyLineSegment) newPs).Points.Add(((m.PolyLineSegment) newPs).Points[0]);
                    newPf.Segments.Add(newPs);
                }
                toReturn.Figures.Add(newPf);
            }
            return toReturn;
        }

        public static m.PathGeometry InvertHorizontally(m.PathGeometry toInvert, double maxHeight)
        {
            toInvert = toInvert.GetFlattenedPathGeometry(1, m.ToleranceType.Absolute);

            m.PathGeometry toReturn = new m.PathGeometry();

            foreach (m.PathFigure oldPf in toInvert.Figures)
            {
                m.PathFigure newPf = new m.PathFigure();

                Point newStartPoint = oldPf.StartPoint;

                newStartPoint.Y = maxHeight - newStartPoint.Y;
                newStartPoint.X = newStartPoint.X;

                newPf.StartPoint = newStartPoint;

                foreach (m.PathSegment oldPs in oldPf.Segments)
                {
                    m.PathSegment newPs = new m.PolyLineSegment();

                    foreach (Point point in ((m.PolyLineSegment) oldPs).Points)
                    {
                        Point tmp = point;
                        tmp.Y = maxHeight - tmp.Y;

                        ((m.PolyLineSegment) newPs).Points.Add(tmp);
                    }
                    newPf.Segments.Add(newPs);
                }
                toReturn.Figures.Add(newPf);
            }
            return toReturn;
        }
    }
}
