using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Renderer.Core.Extension
{
    public static class PointF_Extension
    {
        extension(PointF p)
        {
            public PointF ToScreenPoint(Size screenSize)
            {
                return new PointF(
                    (p.X + 1) / 2 * (screenSize.Width - 1),
                    (1 - (p.Y + 1) / 2) * (screenSize.Height - 1));
            }
        }
    }
}
