﻿using System;

using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SurfaceTower.Model;
using SurfaceTower.VideoEngine;
using SurfaceTower.Controller;
using SurfaceTower.TowerAudio;

namespace SurfaceTower
{
  public interface IApp
  {
    BaseModel Model { get; }
    ContactParser Controller { get; }
    bool ApplicationActivated { get; }
    GraphicsDevice GraphicsDevice { get; }
    ContentManager Content { get; }

    bool onScreen(Vector2 pos);
    void Restart();
  }

  public class App : SurfaceApp, IApp
  {
    //For static access - should not be changed.
    public static IApp Instance;

    //Holds the game state and logic
    protected BaseModel gameModel;
    //Plays the sounds.
    protected TowerAudioEngine towerAudioEngine;
    //Used to display the game, implements the draw method
    protected View gameView;
    //Used to handle input
    protected ContactParser contactParser;

    #region Properties

    public BaseModel Model
    {
      get { return gameModel; }
    }

    public ContactParser Controller
    {
      get { return contactParser; }
    }

    public bool ApplicationActivated
    {
      get { return isApplicationActivated; }
    }

    #endregion

    protected override void Initialize()
    {
      base.Initialize();

      App.Instance = this;
      //Initialize the game model
      gameModel = new GameModel();
      //Initialize AudioEngine
      towerAudioEngine = new TowerAudioEngine(gameModel);
      //Initialize the UI component
      gameView = new SimpleView(gameModel, graphics, Content);
      contactParser = new ContactParser();
    }

    public void Restart()
    {
      Model.Restart();
      gameView.Restart();  
      towerAudioEngine.Restart();
      Controller.Restart();
    }

    public bool onScreen(Vector2 pos)
    {
      //width and height are set to the width and height of the screen
      int width = App.Instance.GraphicsDevice.Viewport.Width;
      int height = App.Instance.GraphicsDevice.Viewport.Height;
      return (pos.X >= 0 && pos.X <= width && pos.Y >= 0 && pos.Y <= height);
    }

    #region Game Events

    protected override void DoDraw(GameTime gameTime)
    {
      if (ApplicationActivated)
      {
        gameView.draw(gameTime);
      }
    }

    protected override void DoRotate(GameTime gameTime, bool inverted)
    {
    }

    protected override void DoUpdate(GameTime gameTime, bool activated)
    {
      if (ApplicationActivated)
      {
        gameModel.OnUpdate(gameTime);
      }
    }
    
    protected override void ProcessContacts(GameTime gameTime, ReadOnlyContactCollection contacts)
    {
      contactParser.ProcessContacts(gameTime, contacts);
    }

    #endregion

    #region Event Handlers

    protected override void OnApplicationActivated(object sender, EventArgs e)
    {
      base.OnApplicationActivated(sender, e);

      //TODO: Enable audio, animations here
      //TODO: Optionally enable raw image here
    }
    
    protected override void OnApplicationPreviewed(object sender, EventArgs e)
    {
      base.OnApplicationPreviewed(sender, e);

      //TODO: Disable audio here if it is enabled
      //TODO: Optionally enable animations here
    }
    
    protected override void OnApplicationDeactivated(object sender, EventArgs e)
    {
      base.OnApplicationDeactivated(sender, e);

      //TODO: Disable audio, animations here
      //TODO: Disable raw image if it's enabled
    }

    #endregion
  }
}
