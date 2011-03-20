using System;
using Microsoft.Xna.Framework;
using Microsoft.Surface.Core;

using SurfaceTower.Model;

namespace SurfaceTower
{
  public class App : SurfaceApp
  {
    private BaseModel model = new BaseModel();

    #region Properties

    public BaseModel Model
    {
      get { return model; }
    }

    #endregion

    protected override void Initialize()
    {
      base.Initialize();
    }

    #region Game Events

    protected override void DoDraw(GameTime gameTime)
    {
    }

    protected override void DoRotate(GameTime gameTime, bool inverted)
    {
    }

    protected override void DoUpdate(GameTime gameTime, bool activated)
    {
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
