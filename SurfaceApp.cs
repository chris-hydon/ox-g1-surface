using System;
using Microsoft.Surface;
using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;
using SurfaceTower.VideoEngine;
using SurfaceTower.Model;


namespace SurfaceTower
{
  /// <summary>
  /// This class provides all of the routine stuff that the Surface requires for every application.
  /// </summary>
  public abstract class SurfaceApp : Game
  {
    protected readonly GraphicsDeviceManager graphics;
    protected ContactTarget contactTarget;
    protected bool applicationLoadCompleteSignalled;

    protected UserOrientation currentOrientation = UserOrientation.Bottom;
    protected Matrix screenTransform = Matrix.Identity;
    protected Matrix inverted;

    // application state: Activated, Previewed, Deactivated,
    // start in Activated state
    protected bool isApplicationActivated = true;
    protected bool isApplicationPreviewed;


    //Holds the game state and logic
    protected BaseModel gameModel;
    //Used to display the game, implements the draw method
    protected View gameView;


    /// <summary>
    /// The graphics device manager for the application.
    /// </summary>
    protected GraphicsDeviceManager Graphics
    {
      get { return graphics; }
    }

    /// <summary>
    /// The target receiving all surface input for the application.
    /// </summary>
    protected ContactTarget ContactTarget
    {
      get { return contactTarget; }
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public SurfaceApp()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    #region Initialization

    /// <summary>
    /// Moves and sizes the window to cover the input surface.
    /// </summary>
    private void SetWindowOnSurface()
    {
      System.Diagnostics.Debug.Assert(Window.Handle != System.IntPtr.Zero,
          "Window initialization must be complete before SetWindowOnSurface is called");
      if (Window.Handle == System.IntPtr.Zero)
        return;

      // We don't want to run in full-screen mode because we need
      // overlapped windows, so instead run in windowed mode
      // and resize to take up the whole surface with no border.

      // Make sure the graphics device has the correct back buffer size.
      InteractiveSurface interactiveSurface = InteractiveSurface.DefaultInteractiveSurface;
      if (interactiveSurface != null)
      {
        graphics.PreferredBackBufferWidth = interactiveSurface.Width;
        graphics.PreferredBackBufferHeight = interactiveSurface.Height;
        graphics.ApplyChanges();

        // Remove the border and position the window.
        Program.RemoveBorder(Window.Handle);
        Program.PositionWindow(Window);
      }
    }

    /// <summary>
    /// Initializes the surface input system. This should be called after any window
    /// initialization is done, and should only be called once.
    /// </summary>
    private void InitializeSurfaceInput()
    {
      System.Diagnostics.Debug.Assert(Window.Handle != System.IntPtr.Zero,
          "Window initialization must be complete before InitializeSurfaceInput is called");
      if (Window.Handle == System.IntPtr.Zero)
        return;
      System.Diagnostics.Debug.Assert(contactTarget == null,
          "Surface input already initialized");
      if (contactTarget != null)
        return;

      // Create a target for surface input.
      contactTarget = new ContactTarget(Window.Handle, EventThreadChoice.OnBackgroundThread);
      contactTarget.EnableInput();
    }

    /// <summary>
    /// Reset the application's orientation and transform based on the current launcher orientation.
    /// </summary>
    private void ResetOrientation()
    {
      UserOrientation newOrientation = ApplicationLauncher.Orientation;

      if (newOrientation == currentOrientation) { return; }

      currentOrientation = newOrientation;

      if (currentOrientation == UserOrientation.Top)
      {
        screenTransform = inverted;
      }
      else
      {
        screenTransform = Matrix.Identity;
      }
    }

    #endregion

    #region Overridden Game Methods

    /// <summary>
    /// Allows the app to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      SetWindowOnSurface();
      InitializeSurfaceInput();

      // Set the application's orientation based on the current launcher orientation
      currentOrientation = ApplicationLauncher.Orientation;

      // Subscribe to surface application activation events
      ApplicationLauncher.ApplicationActivated += OnApplicationActivated;
      ApplicationLauncher.ApplicationPreviewed += OnApplicationPreviewed;
      ApplicationLauncher.ApplicationDeactivated += OnApplicationDeactivated;

      // Setup the UI to transform if the UI is rotated.
      // Create a rotation matrix to orient the screen so it is viewed correctly
      // when the user orientation is 180 degress different.
      inverted = Matrix.CreateRotationZ(MathHelper.ToRadians(180)) *
                 Matrix.CreateTranslation(graphics.GraphicsDevice.Viewport.Width,
                                           graphics.GraphicsDevice.Viewport.Height,
                                           0);

      //Initialize the game model
      gameModel = new BaseModel();  
      //Initialize the UI component
      gameView = new SimpleView(gameModel, graphics);

      if (currentOrientation == UserOrientation.Top)
      {
        screenTransform = inverted;
      }

      base.Initialize();
    }

    /// <summary>
    /// Load your graphics content.
    /// </summary>
    protected override void LoadContent()
    {
    }

    /// <summary>
    /// Unload your graphics content.
    /// </summary>
    protected override void UnloadContent()
    {
      Content.Unload();
    }

    /// <summary>
    /// Allows the app to run logic such as updating the world,
    /// checking for collisions, gathering input and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      if (isApplicationActivated || isApplicationPreviewed)
      {
        if (isApplicationActivated)
        {
          ProcessContacts(gameTime, contactTarget.GetState());
        }

        DoUpdate(gameTime, isApplicationActivated);
      }

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the app should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      if (!applicationLoadCompleteSignalled)
      {
        // Dismiss the loading screen now that we are starting to draw
        ApplicationLauncher.SignalApplicationLoadComplete();
        applicationLoadCompleteSignalled = true;
      }
      DoRotate(gameTime, screenTransform.Equals(inverted));
      gameView.draw(gameTime);
      base.Draw(gameTime);
    }

    #endregion

    #region Application Event Handlers

    /// <summary>
    /// This is called when application has been activated.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnApplicationActivated(object sender, EventArgs e)
    {
      // Update application state.
      isApplicationActivated = true;
      isApplicationPreviewed = false;

      // Orientaton can change between activations.
      ResetOrientation();
    }

    /// <summary>
    /// This is called when application is in preview mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnApplicationPreviewed(object sender, EventArgs e)
    {
      // Update application state.
      isApplicationActivated = false;
      isApplicationPreviewed = true;
    }

    /// <summary>
    ///  This is called when application has been deactivated.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnApplicationDeactivated(object sender, EventArgs e)
    {
      // Update application state.
      isApplicationActivated = false;
      isApplicationPreviewed = false;
    }

    #endregion

    #region IDisposable

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Release managed resources.
        IDisposable graphicsDispose = graphics as IDisposable;
        if (graphicsDispose != null)
        {
          graphicsDispose.Dispose();
        }
        if (contactTarget != null)
        {
          contactTarget.Dispose();
          contactTarget = null;
        }
      }

      // Release unmanaged Resources.

      // Set large objects to null to facilitate garbage collection.

      base.Dispose(disposing);
    }


    #endregion

    #region Methods to override

    /// <summary>
    /// This is called when the app should draw itself, after Surface-specific methods have been called.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected abstract void DoDraw(GameTime gameTime);
    /// <summary>
    /// Called whenever the user interface may need rotating.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    /// <param name="inverted">True if the display should be inverted, false otherwise.</param>
    protected abstract void DoRotate(GameTime gameTime, bool inverted);
    /// <summary>
    /// Allows the app to run logic such as updating the world, checking for collisions, gathering input
    /// and playing audio. Called after the Surface-specific methods have been called.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    /// <param name="activated">True if the application is activated, false if in preview mode.</param>
    protected abstract void DoUpdate(GameTime gameTime, bool activated);
    /// <summary>
    /// Called just before DoUpdate if contacts need to be processed.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    /// <param name="contacts">The collection of contacts that are currently active.</param>
    protected abstract void ProcessContacts(GameTime gameTime, ReadOnlyContactCollection contacts);

    #endregion

  }
}
