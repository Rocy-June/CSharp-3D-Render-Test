using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Renderer.Core;
using Renderer.Core.Extension;
using DIPixelFormat = System.Drawing.Imaging.PixelFormat;
using DPoint = System.Drawing.Point;
using DSize = System.Drawing.Size;

namespace Renderer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RendererBase Renderer { get; set; }
        private Stopwatch Watch { get; } = Stopwatch.StartNew();
        private long LastElapsedMilliseconds { get; set; } = 0;

        RenderObject RenderObj = new()
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

        public MainWindow()
        {
            InitializeComponent();

            Renderer = new RendererBase(new DSize((int)CanvasBox.Width, (int)CanvasBox.Height));

            CompositionTarget.Rendering += CompositionTarget_Rendering;

            LastElapsedMilliseconds = Watch.ElapsedMilliseconds;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Renderer = new RendererBase(new DSize((int)CanvasBox.ActualWidth, (int)CanvasBox.ActualHeight));
        }

        float angle = 0f;
        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            var dt = Watch.ElapsedMilliseconds - LastElapsedMilliseconds;
            LastElapsedMilliseconds = Watch.ElapsedMilliseconds;

            angle += (float)Math.PI * dt / 1000f;

            var tmpObj = RenderObj.RotateXZ(angle).TranslateZ(1f);

            var frame = Renderer.Render([tmpObj]);

            Canvas.Source = PixelInfo2Bitmap(frame);

            FPS_TB.Text = (1000 / dt).ToString();
        }

        WriteableBitmap wb = new(1, 1, 96, 96, PixelFormats.Bgra32, null);
        byte[] buffer = [];
        private WriteableBitmap PixelInfo2Bitmap(PixelInfo[][] pixelInfos)
        {
            var width = Renderer.Resolution.Width;
            var height = Renderer.Resolution.Height;

            if (width <= 0 || height <= 0) return new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgra32, null);

            if (wb.Width != width || wb.Height != height)
            {
                wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            }

            var bytesPerPixel = 4;
            var stride = width * bytesPerPixel;

            var bufferLength = height * stride;
            if (buffer.Length != bufferLength)
            {
                buffer = new byte[bufferLength];
            }

            var i = 0;
            for (var y = 0; y < height; y++)
            {
                var row = pixelInfos[y];
                for (var x = 0; x < width; x++)
                {
                    var pi = row[x];

                    buffer[i++] = pi.Color.B;
                    buffer[i++] = pi.Color.G;
                    buffer[i++] = pi.Color.R;
                    buffer[i++] = pi.Color.A;
                }
            }

            wb.WritePixels(new Int32Rect(0, 0, width, height), buffer, stride, 0);
            return wb;
        }

    }
}