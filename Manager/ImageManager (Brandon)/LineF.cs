using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace Image_Symbol_Management
{
   public class LineF
    {
       public PointF Point1
        {
            get;
            private set;
        }
      public  PointF Point2
        {
            get;
            private set;
        }
      public  LineF(PointF point1, PointF point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
        public void DatenAendern(PointF point1, PointF point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
    }
}
