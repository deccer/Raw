using Raw.Native.OpenGL;
using Serilog;

namespace Raw.Example;

internal sealed class RawExampleApplication : Application
{
    public RawExampleApplication(ILogger logger)
        : base(logger)
    {
    }

    protected override bool Load()
    {
        if (!base.Load())
        {
            return false;
        }
        
        GL.ClearColor(0.2f, 0.4f, 0.6f, 1.0f);

        return true;
    }

    protected override void Render()
    {
        GL.Clear(GL.ClearBufferMask.ColorBufferBit | GL.ClearBufferMask.DepthBufferBit);
    }
}