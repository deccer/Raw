﻿using System;
using Raw.Native.Glfw;
using Serilog;

namespace Raw.Example;

internal class Application : IApplication
{
    private readonly ILogger _logger; 
    private IntPtr _windowHandle;

    private Glfw.KeyCallback? _keyCallback;
    private Glfw.MouseButtonCallback? _mouseButtonCallback;
    private Glfw.CursorEnterCallback? _cursorEnterCallback;
    private Glfw.CursorPositionCallback? _cursorPositionCallback;

    protected Application(ILogger logger)
    {
        _logger = logger.ForContext<Application>();
        _windowHandle = IntPtr.Zero;

        _keyCallback = null;
        _mouseButtonCallback = null;
        _cursorEnterCallback = null;
        _cursorPositionCallback = null;
    }

    public void Run()
    {
        if (!Initialize())
        {
            return;
        }

        _logger.Debug("Application Initialized.");

        if (!Load())
        {
        }

        _logger.Debug("Application Loaded");

        while (!Glfw.ShouldWindowClose(_windowHandle))
        {
            Glfw.PollEvents();

            Update();
            Render();

            Glfw.SwapBuffers(_windowHandle);
        }
    }

    public void Dispose()
    {
        Cleanup();
    }

    protected virtual bool Initialize()
    {
        if (!Glfw.Init())
        {
            _logger.Error("{Category}: Failed to Initialize", "GLFW");
        }

        Glfw.WindowHint(Glfw.WindowInitHint.ClientApi, Glfw.ClientApi.OpenGL);
        Glfw.WindowHint(Glfw.WindowInitHint.IsResizeable, true);

        Glfw.WindowHint(Glfw.WindowOpenGLContextHint.VersionMajor, 4);
        Glfw.WindowHint(Glfw.WindowOpenGLContextHint.VersionMinor, 6);
        Glfw.WindowHint(Glfw.WindowOpenGLContextHint.DebugContext, true);
        Glfw.WindowHint(Glfw.WindowOpenGLContextHint.Profile, Glfw.OpenGLProfile.Core);

        var monitorHandle = Glfw.GetPrimaryMonitor();
        var videoMode = Glfw.GetVideoMode(monitorHandle);
        var screenWidth = videoMode.Width;
        var screenHeight = videoMode.Height;
        var windowWidth = (int)(screenWidth * 0.8f);
        var windowHeight = (int)(screenHeight * 0.8f);

        _windowHandle = Glfw.CreateWindow(windowWidth, windowHeight, "Hello", IntPtr.Zero, IntPtr.Zero);
        if (_windowHandle == IntPtr.Zero)
        {
            _logger.Error("{Category}: Failed to create window", "GLFW");
            return false;
        }

        Glfw.SetWindowPos(_windowHandle, screenWidth / 2 - windowWidth / 2, screenHeight / 2 - windowHeight / 2);
        Glfw.MakeContextCurrent(_windowHandle);

        BindCallbacks();

        return true;
    }

    protected virtual bool Load()
    {
        return true;
    }

    protected virtual void Cleanup()
    {
        UnbindCallbacks();
        Glfw.DestroyWindow(_windowHandle);
        Glfw.Terminate();
    }

    protected virtual void Update()
    {
    }

    protected virtual void Render()
    {
    }

    private void BindCallbacks()
    {
        _keyCallback = OnKey;
        _cursorPositionCallback = OnMousePosition;
        _cursorEnterCallback = OnMouseEnter;
        _mouseButtonCallback = OnMouseButton;

        Glfw.SetKeyCallback(_windowHandle, _keyCallback);
        Glfw.SetCursorPositionCallback(_windowHandle, _cursorPositionCallback);
        Glfw.SetCursorEnterCallback(_windowHandle, _cursorEnterCallback);
        Glfw.SetMouseButtonCallback(_windowHandle, _mouseButtonCallback);
    }

    private void UnbindCallbacks()
    {
        Glfw.SetKeyCallback(_windowHandle, null);
        Glfw.SetCursorEnterCallback(_windowHandle, null);
        Glfw.SetCursorPositionCallback(_windowHandle, null);
        Glfw.SetMouseButtonCallback(_windowHandle, null);
    }

    private void OnKey(
        IntPtr windowHandle,
        Glfw.Key key,
        Glfw.Scancode scanCode,
        Glfw.KeyAction action,
        Glfw.KeyModifiers modifiers)
    {
        _logger.Debug("key: {Key} scancode: {ScanCode} action: {Action} modifiers: {Modifiers}",
            key,
            scanCode,
            action,
            modifiers);
    }

    private void OnMousePosition(
        IntPtr windowHandle,
        double x,
        double y)
    {
        _logger.Debug("x: {X} y: {Y}", x, y);
    }

    private void OnMouseEnter(
        IntPtr windowHandle,
        Glfw.CursorEnterMode cursorEnterMode)
    {
        _logger.Debug("mode: {CursorEnterMode}", cursorEnterMode);
    }

    private void OnMouseButton(
        IntPtr windowHandle,
        Glfw.MouseButton mouseButton,
        Glfw.KeyAction action,
        Glfw.KeyModifiers modifiers)
    {
        _logger.Debug("button: {MouseButton} action: {Action} modifiers: {Modifiers}", mouseButton, action, modifiers);
    }
}