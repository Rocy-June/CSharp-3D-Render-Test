using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Renderer.Core.Extension;

namespace Renderer.Core
{
    public class RendererBase
    {
        public Size Resolution { get; private set; }
        private PixelInfo[][] FrameBuffer { get; set; }

        private Color BackgroundColor = Color.Black;
        private Color ForegroundColor = Color.White;

        public RendererBase(Size resolution)
        {
            Resolution = resolution;
            FrameBuffer = new PixelInfo[Resolution.Height][];
            for (var y = 0; y < Resolution.Height; ++y)
            {
                FrameBuffer[y] = new PixelInfo[Resolution.Width];
            }
        }

        public PixelInfo[][] Render(IEnumerable<RenderObject> objs)
        {
            Clear();
            foreach (var obj in objs)
            {
                foreach (var v3 in obj.ObjectPointFs)
                {
                    Point(v3.ToScreenPoint(Resolution), ForegroundColor);
                }
                foreach (var face in obj.Faces)
                {
                    for (var i = 0; i < face.Length; ++i)
                    {
                        var p1 = obj.ObjectPointFs[face[i]].ToScreenPoint(Resolution);
                        var p2 = obj.ObjectPointFs[face[(i + 1) % face.Length]].ToScreenPoint(Resolution);
                        
                        Line(p1, p2);
                    }
                }
            }

            return FrameBuffer;
        }

        private void Clear()
        {
            for (var y = 0; y < FrameBuffer.Length; ++y)
            {
                var line = FrameBuffer[y];
                for (var x = 0; x < line.Length; ++x)
                {
                    var pxl = new PixelInfo
                    {
                        Color = BackgroundColor
                    };

                    line[x] = pxl;
                }
            }
        }

        private void Point(PointF p, Color c)
        {
            Point((int)p.X, (int)p.Y, c);
        }

        private void Point(Point p, Color c)
        {
            Point(p.X, p.Y, c);
        }

        private void Point(int x, int y, Color c)
        {
            if (x < 0 || y < 0 || x >= Resolution.Width || y >= Resolution.Height) return;
            FrameBuffer[y][x].Color = c;
        }

        private void Line(PointF p1, PointF p2) 
        {
            Line((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y);
        }

        private void Line(int x1, int y1, int x2, int y2) 
        {
            var dx = Math.Abs(x2 - x1);
            var dy = Math.Abs(y2 - y1);
            var sx = x1 < x2 ? 1 : -1;
            var sy = y1 < y2 ? 1 : -1;
            var err = dx - dy;
            while (true)
            {
                Point(x1, y1, ForegroundColor);
                if (x1 == x2 && y1 == y2) break;
                var e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }
    }
}
