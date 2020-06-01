using System;
using ElementManager;
using TextManager;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows;

namespace ToolsetFunctions
{
    public static class Toolset
    {
        public static creativeElement fillObject(creativeElement currentCreativeElement)
        {
            //fill the boy
            //creativeElement newCreativeElement;
            return currentCreativeElement;
        }

        public static creativeElement changeSize(creativeElement currentCreativeElement, bool increase, int height, int width, double increaseDecreaseFactor, int scalingFactor, int einspannungsGrenzen)
        {
            PathGeometry oldPathGeometry = currentCreativeElement.pathGeometry;
            PathGeometry newPathGeometry = oldPathGeometry;
            creativeElement newCreativeElement = new creativeElement(currentCreativeElement.filled, currentCreativeElement.name, currentCreativeElement.textElement, currentCreativeElement.text, currentCreativeElement.abstandZwischenLinien, currentCreativeElement.winkel, currentCreativeElement.scalingFactor, currentCreativeElement.offset, newPathGeometry);

            Rect safetyRect = Converter.GetMinimalMaxRect(oldPathGeometry, 0);
            double top = safetyRect.Bottom;    //inverted because of inverted grid   
            double bottom = safetyRect.Top;     //inverted because of inverted grid   
            double right = safetyRect.Right;
            double left = safetyRect.Left;

            if (increase)
            {

                /*  old stuff
                //1) x-Stretch, 2) rotate 3) neigung 4) y-Stretch 5) x-Verschiebung 6) y-Verschiebung
                //actualPathGeometry.Transform = Transform.Parse("1.1, 0, 0, 1.1, 0, 0");
                transform = new ScaleTransform(1.05, 1.05);
                newPathGeometry = Geometry.Combine(actualPathGeometry, emptyGeometry, GeometryCombineMode.Union, transform);
                */

                newPathGeometry = Converter.ScalingPathGeometry(oldPathGeometry, 1.0 + increaseDecreaseFactor);
                newCreativeElement.pathGeometry = newPathGeometry;

                //check whether new geometry is too big for grid
                if (pathGeometryToBig(newPathGeometry, height, width, scalingFactor, einspannungsGrenzen))
                {
                    return currentCreativeElement;
                }
                else
                {
                    if (newCreativeElement.filled)
                    {
                        newCreativeElement.fill();
                    }
                }

            }
            else //decrease
            {
                newCreativeElement.pathGeometry = Converter.ScalingPathGeometry(currentCreativeElement.pathGeometry, 1.0 - increaseDecreaseFactor);
                if (newCreativeElement.filled)
                {
                    newCreativeElement.fill();
                }
            }



            return newCreativeElement;
        }

        public static creativeElement moveObject(creativeElement currentCreativeElement, string direction,
            int gridHeigth, int gridWidth, double verschiebung, int scalingFactor, int einspannungsGrenzen)
        {
            PathGeometry actualPathGeometry = currentCreativeElement.pathGeometry;
            PathGeometry actualFillingPathGeometry = currentCreativeElement.fillingPathGeometry;

            
            Rect safetyRect = Converter.GetMinimalMaxRect(actualPathGeometry, 0);
            double top = safetyRect.Bottom;    //inverted because of inverted grid   
            double bottom = safetyRect.Top;     //inverted because of inverted grid   
            double right = safetyRect.Right;    
            double left = safetyRect.Left;      

            if (direction == "up" && top + verschiebung <= gridHeigth - (einspannungsGrenzen * scalingFactor))
            {
                actualPathGeometry = Converter.TranslatePathGeometry(actualPathGeometry, 0.0, +verschiebung);
                currentCreativeElement.pathGeometry = actualPathGeometry;
                if (currentCreativeElement.filled)
                {
                    currentCreativeElement.fill();
                }
                
            }
            else if (direction == "down" && bottom - verschiebung >= (einspannungsGrenzen * scalingFactor))
            {
                actualPathGeometry = Converter.TranslatePathGeometry(actualPathGeometry, 0.0, -verschiebung);
                currentCreativeElement.pathGeometry = actualPathGeometry;
                if (currentCreativeElement.filled)
                {
                    currentCreativeElement.fill();
                }

            }
            else if (direction == "left" && left - verschiebung >= (einspannungsGrenzen * scalingFactor))
            {
                actualPathGeometry = Converter.TranslatePathGeometry(actualPathGeometry, -verschiebung, 0.0);
                currentCreativeElement.pathGeometry = actualPathGeometry;
                if (currentCreativeElement.filled)
                {
                    currentCreativeElement.fill();
                }
            }
            else if (direction == "right" && right + verschiebung <= gridWidth - (einspannungsGrenzen * scalingFactor))
            {
                actualPathGeometry = Converter.TranslatePathGeometry(actualPathGeometry, +verschiebung, 0.0);
                currentCreativeElement.pathGeometry = actualPathGeometry;
                if (currentCreativeElement.filled)
                {
                    currentCreativeElement.fill();
                }
            }
            return currentCreativeElement;
        }

        public static bool pathGeometryToBig(PathGeometry pathGeometry, int height, int width, int scalingFactor, int einspannungsGrenzen)
        {
            Rect safetyRect = Converter.GetMinimalMaxRect(pathGeometry, 0);
            double top = safetyRect.Bottom;    //inverted because of inverted grid   
            double bottom = safetyRect.Top;     //inverted because of inverted grid   
            double right = safetyRect.Right;
            double left = safetyRect.Left;

            //weil die geometry nullbasiert ist zum äußeren rand die geometry aber nicht über den inneren rand gehen darf, rechne ich die distanz wieder drauf
            int newWidth = width + (einspannungsGrenzen * scalingFactor);
            int newHeight = height + (einspannungsGrenzen * scalingFactor);
            if (right > newWidth || top > newHeight || bottom < (einspannungsGrenzen * scalingFactor + 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    } //end class
}



/*
                TtfManager newTtfObject;
                var currentTtfObject = currentCreativeElement.actualTtfObject;
                var oldOffset = currentTtfObject.Offset;
                var newOffset = currentTtfObject.Offset;

                //check also for moving out of borders from grid             
                if (direction == "up" && oldOffset.Y - 5 >= 10)
                {
                    newOffset.Y = oldOffset.Y - 5;
                    newTtfObject = new TtfManager(currentTtfObject.TtfPath, currentTtfObject.Text,
                        currentTtfObject.HintingEmSize, currentTtfObject.Resolution, currentTtfObject.WannaFlat,
                        newOffset);
                }
                else if (direction == "down" && oldOffset.Y + 5 <= gridHeigth - 10)
                {
                    newOffset.Y = oldOffset.Y + 5;
                    newTtfObject = new TtfManager(currentTtfObject.TtfPath, currentTtfObject.Text,
                        currentTtfObject.HintingEmSize, currentTtfObject.Resolution, currentTtfObject.WannaFlat,
                        newOffset);
                }
                else if (direction == "left" && oldOffset.X - 5 >= 10)
                {
                    newOffset.X = oldOffset.X - 5;
                    newTtfObject = new TtfManager(currentTtfObject.TtfPath, currentTtfObject.Text,
                        currentTtfObject.HintingEmSize, currentTtfObject.Resolution, currentTtfObject.WannaFlat,
                        newOffset);
                }
                else if (direction == "right" && oldOffset.X + 5 <= gridWidth - 10)
                {
                    newOffset.X = oldOffset.X + 5;
                    newTtfObject = new TtfManager(currentTtfObject.TtfPath, currentTtfObject.Text,
                        currentTtfObject.HintingEmSize, currentTtfObject.Resolution, currentTtfObject.WannaFlat,
                        newOffset);
                }
                else
                {
                    return currentCreativeElement;
                }
                                currentCreativeElement.actualTtfObject = newTtfObject;

    */
