using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text;
using Renderer.Core;
using Renderer.Core.Extension;

Console.CursorVisible = false;

var prevColor = Console.BackgroundColor;

var consoleSize = new Size(Console.WindowWidth, Console.WindowHeight);
var renderer = new RendererBase(consoleSize);

var sw = Stopwatch.StartNew();

var renderObj = new RenderObject
{
    ObjectPointFs =
    [
        new(0.25f, 0.25f, 0.25f),
        new(-0.25f, 0.25f, 0.25f),
        new(-0.25f, -0.25f, 0.25f),
        new(0.25f, -0.25f, 0.25f),

        new(0.25f, 0.25f, -0.25f),
        new(-0.25f, 0.25f, -0.25f),
        new(-0.25f, -0.25f, -0.25f),
        new(0.25f, -0.25f, -0.25f),
    ],
    Faces =
    [
        [0, 1, 2, 3],
        [4, 5, 6, 7],
        [0, 4],
        [1, 5],
        [2, 6],
        [3, 7],
    ]
};
var dz = 1f;
var angle = 0f;

var lastTime = sw.ElapsedMilliseconds;
while (true)
{
    var dt = sw.ElapsedMilliseconds - lastTime;
    lastTime = sw.ElapsedMilliseconds;

    //dz += dt / 1000f;
    angle += (float)Math.PI * dt / 1000f;

    //var tmp_v3s = new Vector3[v3s.Length];
    //for (var i = 0; i < tmp_v3s.Length; ++i)
    //{
    //    var tmp_v3 = v3s[i];
    //    tmp_v3.Z += dz;
    //    tmp_v3s[i] = tmp_v3;
    //}

    var tmpObj = renderObj.RotateXZ(angle).TranslateZ(dz);

    var frame = renderer.Render([tmpObj]);

    Console.SetCursorPosition(0, 0);

    var fps = 1000f / dt;
    ChangeBackColor(ConsoleColor.Black);
    Console.Write(fps);
    var sb = new StringBuilder();

    for (int x = fps.ToString().Length; x < consoleSize.Width; ++x)
    {
        var flag = CheckBackColor(frame[0][x].Color, out var cc);

        if (!flag)
        {
            Console.Write(sb.ToString());
            sb.Clear();

            ChangeBackColor(cc);
        }

        sb.Append(' ');
    }

    Console.WriteLine(sb.ToString());
    sb.Clear();

    for (int y = 1; y < consoleSize.Height; y++)
    {
        for (int x = 0; x < consoleSize.Width; x++)
        {
            var flag = CheckBackColor(frame[y][x].Color, out var cc);

            if (!flag)
            {
                Console.Write(sb.ToString());
                sb.Clear();

                ChangeBackColor(cc);
            }

            sb.Append(' ');
        }

        Console.WriteLine(sb.ToString());
        sb.Clear();
    }

    Thread.Sleep(0);
}

bool CheckBackColor(Color c, out ConsoleColor cc)
{
    cc = c.ToConsoleColor();
    return cc == prevColor;
}

void ChangeBackColor(ConsoleColor cc)
{
    Console.BackgroundColor = cc;
    prevColor = cc;
}