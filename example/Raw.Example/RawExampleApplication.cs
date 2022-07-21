using Serilog;

namespace Raw.Example;

internal sealed class RawExampleApplication : Application
{
    public RawExampleApplication(ILogger logger)
        : base(logger)
    {
    }
}