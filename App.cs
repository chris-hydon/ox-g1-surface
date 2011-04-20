using System;
using Microsoft.Xna.Framework;
using Microsoft.Surface.Core;

using SurfaceTower.Model;
using SurfaceTower.VideoEngine;

namespace SurfaceTower
{
  public class App : SurfaceApp
  {
    //For static access - should not be changed.
    public static App Instance;

    //Holds the game state and logic
    protected BaseModel gameModel;
    //Used to display the game, implements the draw method
    protected View gameView;

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
      gameModel = new BaseModel();
      //Initialize the UI component
      gameView = new SimpleView(gameModel, graphics);

      //For toy version, put an enemy on the stage
      //TO-DO: For the toy version, manually add enemy to Stage.INSTANCE.Living. Commented-out below gives error
      Enemy enemy = new Enemy(20, 30, 10);
      enemy.MoveTo(new Vector2(100, 150));
      //Stage.INSTANCE.Living.Add(enemy);

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
