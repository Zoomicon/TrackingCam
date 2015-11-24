//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: ITracker.cs
//Version: 20151116

namespace TrackingCam.Plugins.Tracking
{

  public interface ITracker
  {

    #region --- Properties ---

    double PositionDepth { get; }
    double PositionHorizontal { get; }
    double PositionVertical { get; }

    #endregion
  }

}
