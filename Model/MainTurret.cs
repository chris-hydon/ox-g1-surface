namespace SurfaceTower.Model
{
  public class MainTurret
  {
    private bool active = false;
    private float orientation;
    private int playerId;

    public MainTurret(int playerId)
    {
      this.playerId = playerId;
    }

    #region Properties

    public float Orientation
    {
      get { return orientation; }
    }

    public int PlayerId
    {
      get { return playerId; }
    }

    #endregion
  }
}
