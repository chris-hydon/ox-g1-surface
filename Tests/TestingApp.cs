using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SurfaceTower;
using SurfaceTower.Controller;
using SurfaceTower.Model;

namespace Tests
{
  public class TestingApp : IApp
  {
    protected BaseModel model;
    protected ContactParser controller;
    protected GraphicsDevice graphics;

    public TestingApp()
    {
      App.Instance = this;
      
      PresentationParameters pp = new PresentationParameters();
      graphics = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, DeviceType.NullReference, System.IntPtr.Zero, pp);

      model = new GameModel();
      model.Music.TimeSignature = new TimeSignature(4, 4);
      model.Music.Tempo = 60;
      model.Music.ClicksPerBeat = 4;

      controller = new ContactParser();
    }

    public BaseModel Model
    {
      get { return model; }
    }

    public ContactParser Controller
    {
      get { return controller; }
    }

    public bool ApplicationActivated
    {
      get { return true; }
    }

    public GraphicsDevice GraphicsDevice
    {
      get { return graphics; }
    }

    public ContentManager Content
    {
      get { throw new System.NotImplementedException(); }
    }

    public bool onScreen(Vector2 pos)
    {
      return (pos.X >= 0 && pos.X <= 1024 && pos.Y >= 0 && pos.Y <= 768);
    }

    public void Restart()
    {
      Model.Restart();
      Controller.Restart();
    }
  }
}
