using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace miaGame.Painter
{
    public class PaintInfo
    {
        private double mWorld2ScreenXee;
        private double mWorld2ScreenYps;

        public double WorldSizeXee { get; private set; }
        public double WorldSizeYps { get; private set; }

        public double ScreenSizeXee { get; private set; }
        public double ScreenSizeYps { get; private set; }

        public DrawingContext Context { get; private set; }
        public Canvas Canvas { get; private set; }

        public PaintInfo(double worldWidth, double worldHeight, double screenWidth, double screenHeight, Canvas canvas, DrawingContext context)
        {
            WorldSizeXee = worldWidth;
            WorldSizeYps = worldHeight;

            ScreenSizeXee = screenWidth;
            ScreenSizeYps = screenHeight;

            Context = context;
            Canvas = canvas;

            mWorld2ScreenXee = screenWidth / worldWidth;
            mWorld2ScreenYps = screenHeight / worldHeight;
        }

        public double World2ScreenXee(double worldXee)
        {
            return mWorld2ScreenXee * worldXee;
        }

        public double World2ScreenYps(double worldYps)
        {
            return mWorld2ScreenYps * worldYps;
        }

    }
}
