using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_Symbol_Management
{
   public   class PointF 
    {
        double x_;
        double Y_;
        ControlPoint ControlP;

        public ControlPoint ControlPt
        {
            get
            {
                return ControlP;
            }
            private set
            {
                value = ControlP;
            }
        }
        public double x
        {
            get
            {
                return x_;
            }
          private  set
            {
                x_ = value;
            }
        }
        public double Y
        {
            get
            {
                return Y_;
            }
           private set
            {
                Y_ = value;
            }
        }
       public  PointF(double x,double y,ControlPoint controlP)
        {
            x_ = x;
            Y_ = y;

            ControlP = controlP;
        }
       public void ChangeDaten(double x, double y, ControlPoint controlP)
        {
            x_ = x;
            Y_ = y;
            ControlP = controlP;
        }
    }
}
