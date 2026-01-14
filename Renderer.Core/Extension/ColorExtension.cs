using System;
using System.Drawing;

namespace Renderer.Core.Extension
{
    public static class ColorExtension
    {
        extension(Color c)
        {
            public ConsoleColor ToConsoleColor()
            {
                int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0;
                index |= (c.R > 64) ? 4 : 0;
                index |= (c.G > 64) ? 2 : 0;
                index |= (c.B > 64) ? 1 : 0;
                return (ConsoleColor)index;
            }
        }
    }
}
