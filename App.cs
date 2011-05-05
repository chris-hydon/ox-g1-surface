using System;
using Microsoft.Xna.Framework;
using Microsoft.Surface.Core;

using SurfaceTower.Model;
using SurfaceTower.VideoEngine;
using SurfaceTower.Controller;
using SurfaceTower.TowerAudio;

namespace SurfaceTower
{
  public class App : SurfaceApp
  {
    //For static access - should not be changed.
    public static App Instance;

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

    #endregion

    protected override void Initialize()
    {
      base.Initialize();

      App.Instance = this;
      //Initialize the game model
      gameModel = new TestingModel();
      //Initialize AudioEngine
      towerAudioEngine = new TowerAudioEngine(gameModel);
      //Initialize the UI component
      gameView = new SimpleView(gameModel, graphics, Content);
      contactParser = new ContactParser();
    }

    #region Game Events

    protected override void DoDraw(GameTime gameTime)
    {
      gameView.draw(gameTime);
    }

    protected override void DoRotate(GameTime gameTime, bool inverted)
    {
    }

    protected override void DoUpdate(GameTime gameTime, bool activated)
    {
      gameModel.OnUpdate(gameTime);
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
