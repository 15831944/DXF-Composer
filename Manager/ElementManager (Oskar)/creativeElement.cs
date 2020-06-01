using System.Windows;
using System.Windows.Media;
using FillControler;
using TextManager;

namespace ElementManager
{
    public class creativeElement
    {
        public PathGeometry pathGeometry;
        public PathGeometry fillingPathGeometry;
        public Point offset;

        public bool filled;
        public int abstandZwischenLinien;
        public double winkel;

        public string name;
        public int scalingFactor;


        //textElement specific
        public bool textElement;
        public string text;



        //constructor
        public creativeElement(bool filled, string name, bool textElement, string text, int abstand, double winkel, int scalingFactor, Point offset, PathGeometry pathGeometry)
        {
            this.filled = filled;
            this.name = name;
            this.textElement = textElement;
            this.text = text;
            this.abstandZwischenLinien = abstand;
            this.winkel = winkel;
            this.scalingFactor = scalingFactor;

            this.offset = offset;
            this.pathGeometry = pathGeometry;

            if (filled)
                fill();

        }

        public void fill()
        {
            fillingPathGeometry = FillControler.FillTool.GetFillingLines(pathGeometry, abstandZwischenLinien, winkel);
        }

        public override string ToString()
        {
            return name;
        }
    }
}