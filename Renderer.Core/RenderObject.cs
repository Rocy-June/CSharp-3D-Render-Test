using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Renderer.Core
{
    public struct RenderObject
    {
        public Vector3[] ObjectPointFs { get; set; }
        public int[][] Faces { get; set; }

        public RenderObject Clone() 
        {
            var newObj = new RenderObject
            {
                ObjectPointFs = new Vector3[ObjectPointFs.Length],
                Faces = new int[Faces.Length][]
            };
            Array.Copy(ObjectPointFs, newObj.ObjectPointFs, ObjectPointFs.Length);
            for (var i = 0; i < Faces.Length; i++)
            {
                newObj.Faces[i] = new int[Faces[i].Length];
                Array.Copy(Faces[i], newObj.Faces[i], Faces[i].Length);
            }
            return newObj;
        }
    }
}
