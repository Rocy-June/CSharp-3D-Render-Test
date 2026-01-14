using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Renderer.Core.Extension
{
    public static class Vector3Extension
    {
        extension(Vector3 v3) 
        {
            public PointF ToProjectPoint() 
            {
                return new PointF(
                    v3.X / v3.Z,
                    v3.Y / v3.Z);
            }

            public PointF ToScreenPoint(Size screenSize)
            {
                return ToProjectPoint(v3).ToScreenPoint(screenSize);
            }
        }
    }
}
