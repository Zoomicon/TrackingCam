//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: ITracker.cs
//Version: 20151116

namespace TrackingCam.Plugins.Tracking
{

  public interface ITracker
  {

    #region --- Properties ---

    double PositionHorizontal { get; }
    double PositionVertical { get; }
    double PositionDepth { get; }

    #endregion
  }

}
