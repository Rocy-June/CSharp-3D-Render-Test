using System;
using System.Collections.Generic;
using System.Text;
using Renderer.Core.Extension;

namespace Renderer.Core.Extension
{
    public static class Transformer
    {
        extension(RenderObject renderObject)
        {
            public RenderObject RotateXZ(float angle)
            {
                var newObject = renderObject.Clone();

                var c = Math.Cos(angle);
                var s = Math.Sin(angle);
                for (var i = 0; i < newObject.ObjectPointFs.Length; ++i)
                {
                    var v3 = newObject.ObjectPointFs[i];
                    var x = v3.X;
                    var z = v3.Z;
                    v3.X = (float)(x * c - z * s);
                    v3.Z = (float)(x * s + z * c);
                    newObject.ObjectPointFs[i] = v3;
                }
                return newObject;
            }

            public RenderObject TranslateZ(float z)
            {
                var newObject = renderObject.Clone();

                for (var i = 0; i < newObject.ObjectPointFs.Length; ++i)
                {
                    var v3 = newObject.ObjectPointFs[i];
                    v3.Z += z;
                    newObject.ObjectPointFs[i] = v3;
                }

                return newObject;
            }
        }
    }
}
