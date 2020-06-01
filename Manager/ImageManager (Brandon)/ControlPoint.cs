using System.Windows;
using SD = System.Drawing;
using CsPotrace;

namespace Image_Symbol_Management
{
  public class ControlPoint 
    {
        double _x, _y;
     //private PointF ptContainer;
     //    public PointF pointContainer
     //   {
     //       get
     //       {
     //           return ptContainer;
     //       }
     //       private set
     //       {
     //           ptContainer = value;
     //       }
     //   }
        public double X
        {
            get
            {
                return _x;
            }
            private set
            {
                _x = value;
            }
        }
        public double Y
        {
            get
            {
                return _y;
            }
            private set
            {
                _y = value;
            }
        }

        public  ControlPoint (double x , double y/*, PointF ptf*/)
        {
            _x = x;
            _y = y;
            //ptContainer = ptf;
        }
        void DatenAendern(double x, double y/*, PointF ptf*/)
        {
            _x = x;
            _y = y;
            //ptContainer = ptf;
        }


    }
}
