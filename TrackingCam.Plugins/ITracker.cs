//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: ITracker.cs
//Version: 20151124

namespace TrackingCam.Plugins.Tracking
{

  public interface ITracker
  {

    #region --- Properties ---

    double PositionDepth { get; }
    double PositionHorizontal { get; }
    double PositionVertical { get; }

    double PositionAngle { get; }

    #endregion
  }

}
