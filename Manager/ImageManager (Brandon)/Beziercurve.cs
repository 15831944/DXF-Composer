using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsPotrace;


namespace Image_Symbol_Management
{
  public class Beziercurve 
    {
       public PointF A
        {
            get;
        }
        
      public ControlPoint ControlPt_A
        {
            get;
        }
      public  PointF B
        {
            get;
        }
     public  ControlPoint ControlPt_B
        {
            get;
        }
       public  List<Potrace.DoublePoint> List_Of_All_Points = new List<Potrace.DoublePoint>();
        public Beziercurve(PointF a , ControlPoint controlA, PointF b, ControlPoint controlB)
        {
            A = a;
            ControlPt_A = controlA;
            B = b;
            ControlPt_B = controlB;
        }

       Potrace.DoublePoint Bezier(double t, Potrace.DoublePoint p0, Potrace.DoublePoint p1, Potrace.DoublePoint p2, Potrace.DoublePoint p3)
        {
            double s = 1 - t;
            double x = s * s * s * p0.X + 3 * (s * s * t) * p1.X + 3 * (t * t * s) * p2.X + t * t * t * p3.X;
            double y = s * s * s * p0.Y + 3 * (s * s * t) * p1.Y + 3 * (t * t * s) * p2.Y + t * t * t * p3.Y;
            return new Potrace.DoublePoint(x, y);
        }
      public  List<Potrace.DoublePoint> BezierToDoublePoint(double anzahl)
        {
            List<Potrace.DoublePoint> listPts = new List<Potrace.DoublePoint>();
            double schrit = 1 / anzahl;
            double t = 0;
            Potrace.DoublePoint a = new Potrace.DoublePoint(this.A.x, this.A.Y);
            Potrace.DoublePoint ctrlA = new Potrace.DoublePoint(this.ControlPt_A.X, this.ControlPt_A.Y);
            Potrace.DoublePoint b = new Potrace.DoublePoint(this.B.x, this.B.Y);
            Potrace.DoublePoint ctrlB = new Potrace.DoublePoint(this.ControlPt_B.X, this.ControlPt_B.Y);
            while (t<=1)
            {
                listPts.Add(Bezier(t,a, ctrlA, b, ctrlB));
                t = t + schrit;
            }
            return listPts;
        }

    }
}
