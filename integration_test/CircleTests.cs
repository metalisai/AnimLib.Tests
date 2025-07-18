using Xunit.Abstractions;
using Xunit.Sdk;
using AnimLib;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;
using System.IO;

namespace AnimLib.Tests;

public class CircleTests : IDisposable
{
    AnimationSettings settings = new AnimationSettings()
    {
        FPS = 60,
        Width = 800,
        Height = 600,
        MaxLength = 60.0f
    };
    TestingPlatform platform;
    private bool disposedValue;

    class SimpleCircleScene : AnimationBehaviour
    {
        public async Task Animation(World world, Animator animator)
        {
            world.ActiveCamera!.ClearColor = Color.BLACK;
            var circle = new Circle(300.0f);
            circle.Color = Color.RED;
            world.CreateInstantly(circle);
            world.Marker("CircleCreated");
            await Time.WaitSeconds(1.0);
        }

        public void Init(AnimationSettings settings)
        {
            settings.FPS = 60;
            settings.Width = 1920;
            settings.Height = 1080;
            settings.MaxLength = 60.0f;
        }
    }

    public CircleTests()
    {
        platform = new TestingPlatform();
        platform.Init();
    }

    [LinuxOnlyFact]
    public void CreateCircle_Circle_Rendered()
    {
        platform.ClearMarkers();
        platform.AddScreenshotMarker("CircleCreated");
        TestingPlatform.SSDelegate onScreenshot = (id, ss) =>
        {
            var image = ss.ToImage();
            image.Save("test.png", new PngEncoder
            {
                BitDepth = PngBitDepth.Bit16,
                ColorType = PngColorType.Rgb
            });
        };
        platform.ScreenshotCaptured += onScreenshot;
        platform.Run(new SimpleCircleScene());
        platform.ScreenshotCaptured -= onScreenshot;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                platform.Close();
                platform.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~CircleTests()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}