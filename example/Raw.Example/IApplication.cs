using System;

namespace Raw.Example;

public interface IApplication : IDisposable
{
    void Run();
}