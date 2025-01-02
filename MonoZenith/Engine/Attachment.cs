using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoZenith.Engine;
using MonoZenith.Engine.Support;

// ReSharper disable once CheckNamespace
namespace MonoZenith;

public enum MouseButtons { Left, Middle, Right }

public partial class Game
{
    private static GameFacade _facade;
    private static FadeEffectManager _fadeEffect;
    public static Game Instance { get; private set; }

    public static Color BackgroundColor => _facade.BackgroundColor;
    public static int ScreenWidth => _facade.ScreenWidth;
    public static int ScreenHeight => _facade.ScreenHeight;
    public static bool ScreenResizable => _facade.ScreenResizable;
    public static bool ScreenFullScreen => _facade.ScreenFullScreen;
    public static string WindowTitle => _facade.WindowTitle;
    
    public static bool ControllerConnected => _facade.ControllerConnected;
    public static bool HasLeftStick => _facade.HasLeftStick;
    public static bool HasRightStick => _facade.HasRightStick;
    public static bool HasDPad => _facade.HasDPad;
    public static bool HasLeftTrigger => _facade.HasLeftTrigger;
    public static bool HasRightTrigger => _facade.HasRightTrigger;
    public static bool HasLeftBumper => _facade.HasLeftBumper;
    public static bool HasRightBumper => _facade.HasRightBumper;
    public static bool HasAButton => _facade.HasAButton;
    public static bool HasBButton => _facade.HasBButton;
    public static bool HasXButton => _facade.HasXButton;
    public static bool HasYButton => _facade.HasYButton;

    public static bool IsFadingIn;
    public static bool IsFadingOut;
    public static bool QuitToDesktop = false;
    
    // PlayStation DualSense buttons
    public enum DualSenseButtons
    {
        Cross = Buttons.A,
        Circle = Buttons.B,
        Square = Buttons.X,
        Triangle = Buttons.Y,
        L1 = Buttons.LeftShoulder,
        R1 = Buttons.RightShoulder,
        L2 = Buttons.LeftTrigger,
        R2 = Buttons.RightTrigger,
        Share = Buttons.Back,
        Options = Buttons.Start,
        L3 = Buttons.LeftStick,
        R3 = Buttons.RightStick,
        Ps = Buttons.BigButton,
        Touchpad = Buttons.BigButton, // Assuming Touchpad uses the same button as PS
        Up = Buttons.DPadUp,
        Down = Buttons.DPadDown,
        Left = Buttons.DPadLeft,
        Right = Buttons.DPadRight
    }
    
    // XBOX Controller buttons
    public enum XboxButtons
    {
        A = Buttons.A,
        B = Buttons.B,
        X = Buttons.X,
        Y = Buttons.Y,
        L1 = Buttons.LeftShoulder,
        R1 = Buttons.RightShoulder,
        L2 = Buttons.LeftTrigger,
        R2 = Buttons.RightTrigger,
        Back = Buttons.Back,
        Start = Buttons.Start,
        L3 = Buttons.LeftStick,
        R3 = Buttons.RightStick,
        Xbox = Buttons.BigButton,
        Up = Buttons.DPadUp,
        Down = Buttons.DPadDown,
        Left = Buttons.DPadLeft,
        Right = Buttons.DPadRight
    }
    
    // Nintendo Switch Pro Controller buttons
    public enum SwitchProButtons
    {
        B = Buttons.A,
        A = Buttons.B,
        Y = Buttons.X,
        X = Buttons.Y,
        L = Buttons.LeftShoulder,
        R = Buttons.RightShoulder,
        ZL = Buttons.LeftTrigger,
        ZR = Buttons.RightTrigger,
        Minus = Buttons.Back,
        Plus = Buttons.Start,
        L3 = Buttons.LeftStick,
        R3 = Buttons.RightStick,
        Home = Buttons.BigButton,
        Up = Buttons.DPadUp,
        Down = Buttons.DPadDown,
        Left = Buttons.DPadLeft,
        Right = Buttons.DPadRight
    }
    
    public static void Initialize(GameFacade f)
    {
        Instance = new Game(f);
    }
    
    public Game(GameFacade f)
    {
        _facade = f;
        _fadeEffect = new FadeEffectManager(1, 0.01f);
        IsFadingIn = false;
        IsFadingOut = false;
    }
    
    /// <summary>
    /// Log a message to the console.
    /// </summary>
    /// <param name="msg">Message</param>
    public static void DebugLog(string msg)
    {
        Console.WriteLine(msg);
    }
    
    // Trigger a fade-in effect
    public static void StartFadeIn(Action onFadeInComplete = null)
    {
        IsFadingIn = true;
        _fadeEffect.StartFadeIn(FadeInComplete);
        return;

        void FadeInComplete()
        {
            IsFadingIn = false;
            onFadeInComplete?.Invoke();
        }
    }
    
    // Trigger a fade-out effect
    public static void StartFadeOut(Action onFadeOutComplete = null)
    {
        IsFadingOut = true;
        _fadeEffect.StartFadeOut(FadeOutComplete);
        return;

        void FadeOutComplete()
        {
            IsFadingOut = false;
            onFadeOutComplete?.Invoke();
        }
    }
    
    /// <summary>
    /// Set the background color.
    /// </summary>
    /// <param name="c">Color</param>
    public static void SetBackgroundColor(Color c)
    {
        _facade.SetBackgroundColor(c);
    }
    
    /// <summary>
    /// Set the screen size to the desktop resolution, adjusted to be smaller.
    /// </summary>
    public static void SetScreenSizeAutomatically()
    {
        int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        float aspectRatio = (float)AppSettings.Scaling.OriginalScreenWidth / AppSettings.Scaling.OriginalScreenHeight;
        if ((float)width / height > aspectRatio)
            height = (int)(width / aspectRatio);
        else
            width = (int)(height * aspectRatio);

        _facade.SetScreenSize((int)(width * 0.75), (int)(height * 0.75));
    }
    
    /// <summary>
    /// Set the screen size while maintaining the aspect ratio.
    /// </summary>
    /// <param name="w">Width</param>
    /// <param name="h">Height</param>
    public static void SetScreenSize(int w, int h)
    {
        // Original aspect ratio
        float aspectRatio = (float)AppSettings.Scaling.OriginalScreenWidth / AppSettings.Scaling.OriginalScreenHeight;

        // Calculate new dimensions based on the desired aspect ratio
        if ((float)w / h > aspectRatio)
        {
            // Width is too wide, adjust height
            h = (int)(w / aspectRatio);
        }
        else
        {
            // Height is too tall, adjust width
            w = (int)(h * aspectRatio);
        }

        _facade.SetScreenSize(w, h);
    }
    
    /// <summary>
    /// Set the screen full screen.
    /// </summary>
    /// <param name="fullScreen">Whether the screen is full screen.</param>
    public static void SetScreenFullScreen(bool fullScreen)
    {
        _facade.SetScreenFullScreen(fullScreen);
    }
    
    /// <summary>
    /// Set the screen resizable.
    /// </summary>
    /// <param name="resizable">Whether the screen is resizable.</param>
    public static void SetScreenResizable(bool resizable)
    {
        _facade.SetScreenResizable(resizable);
    }

    /// <summary>
    /// Set the window title.
    /// </summary>
    /// <param name="t">The window title.</param>
    public static void SetWindowTitle(string t)
    {
        _facade.SetWindowTitle(t);
    }

    /// <summary>
    /// Get whether a keyboard key is pressed.
    /// </summary>
    /// <param name="key">The key that is checked.</param>
    /// <returns>Whether the provided key is pressed.</returns>
    public static bool GetKeyDown(Keys key)
    {
        return _facade.GetKeyDown(key);
    }

    /// <summary>
    /// Get whether a mouse button is pressed.
    /// </summary>
    /// <param name="button">The button that is checked.</param>
    /// <returns>Whether a mouse button is pressed.</returns>
    public static bool GetMouseButtonDown(MouseButtons button)
    {
        return _facade.GetMouseButtonDown(button);
    }

    /// <summary>
    /// Get the mouse position.
    /// </summary>
    /// <returns>Position of the mouse pointer.</returns>
    public static Point GetMousePosition()
    {
        return _facade.GetMousePosition();
    }

    /// <summary>
    /// Get the mouse wheel value.
    /// </summary>
    /// <returns>The value of the mouse wheel.</returns>
    public static int GetMouseWheelValue()
    {
        return _facade.GetMouseWheelValue();
    }

    /// <summary>
    /// Load a font.
    /// </summary>
    /// <param name="font">Name of the font to be loaded.</param>
    /// <param name="scale">Scale of the font.</param>
    /// <returns>The SpriteFont of the requested font.</returns>
    public static SpriteFont LoadFont(string font, float scale)
    {
        return _facade.LoadFont(font, scale);
    }

    /// <summary>
    /// Draw text to the screen.
    /// </summary>
    /// <param name="content">Content</param>
    /// <param name="pos">Position</param>
    /// <param name="font">Font</param>
    /// <param name="c">Color</param>
    /// <param name="scale">Scale</param>
    /// <param name="angle">Rotational angle</param>
    public static void DrawText(string content, Vector2 pos, SpriteFont font, Color c, float scale=1, float angle=0)
    {
        _facade.DrawText(content, pos, font, c, scale, angle);
    }

    /* Source: https://community.monogame.net/t/loading-png-jpg-etc-directly/7403 */
    /// <summary>
    /// Load an image.
    /// </summary>
    /// <param name="filepath">Filepath</param>
    /// <returns>Texture of the requested image.</returns>
    public static Texture2D LoadImage(string filepath)
    {
        return _facade.LoadImage(filepath);
    }
    
    /* Source: https://www.industrian.net/tutorials/texture2d-and-drawing-sprites/ */
    /// <summary>
    /// Draw an image to the screen.
    /// </summary>
    /// <param name="texture">Image texture</param>
    /// <param name="pos">Position</param>
    /// <param name="scale">Scale</param>
    /// <param name="angle">Rotational angle</param>
    /// <param name="flipped">Horizontally flipped</param>
    /// <param name="alpha">Alpha</param>
    public static void DrawImage(
        Texture2D texture, 
        Vector2 pos, 
        float scale = 1, 
        float angle = 0, 
        bool flipped = false, 
        float alpha = 1.0f)
    {
        _facade.DrawImage(texture, pos, scale, angle, flipped, alpha);
    }
    
    /// <summary>
    /// Draw a rectangle to the screen.
    /// </summary>
    /// <param name="color">Color</param>
    /// <param name="pos">Position</param>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    public static void DrawRectangle(Color color, Vector2 pos, int width, int height)
    {
        _facade.DrawRectangle(color, pos, width, height);
    }

    /// <summary>
    /// Load an audio file.
    /// </summary>
    /// <param name="filePath">Filepath to the audio file to be loaded.</param>
    /// <returns>SoundEffectInstance of the audio file.</returns>
    public static SoundEffect LoadAudio(string filePath)
    {
        return _facade.LoadAudio(filePath);
    }

    public static void LogControllerSupportProperties()
    {
        DebugLog("===== Controller support properties =====");
        DebugLog($"Controller connected: {ControllerConnected}");
        DebugLog($"Has left stick: {HasLeftStick}");
        DebugLog($"Has right stick: {HasRightStick}");
        DebugLog($"Has DPad: {HasDPad}");
        DebugLog($"Has left trigger: {HasLeftTrigger}");
        DebugLog($"Has right trigger: {HasRightTrigger}");
        DebugLog($"Has left bumper: {HasLeftBumper}");
        DebugLog($"Has right bumper: {HasRightBumper}");
        DebugLog($"Has A button: {HasAButton}");
        DebugLog($"Has B button: {HasBButton}");
        DebugLog($"Has X button: {HasXButton}");
        DebugLog($"Has Y button: {HasYButton}");
        DebugLog("========================================");
    }
    
    /// <summary>
    /// Get the gamepad state.
    /// </summary>
    /// <returns>Gamepad state.</returns>
    public static GamePadState GetGamePadState()
    {
        return GamePad.GetState(PlayerIndex.One);
    }

    /// <summary>
    /// Log the pressed DualSense buttons.
    /// </summary>
    private static void LogPressedDualSenseButton()
    {
        foreach (DualSenseButtons button in Enum.GetValues(typeof(DualSenseButtons)))
        {
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown((Buttons)button))
            {
                Console.WriteLine("Pressed: " + button.ToString());
            }
        }
    }

    /// <summary>
    /// Vibrate the controller. 
    /// </summary>
    /// <param name="leftMotor">Left motor in controller</param>
    /// <param name="rightMotor">Right motor in controller</param>
    public static void VibrateController(float leftMotor, float rightMotor)
    {
        if (!ControllerConnected)
        {
            DebugLog("Controller not connected.");
            return;
        }
        
        GamePad.SetVibration(PlayerIndex.One, leftMotor, rightMotor);
    }
}