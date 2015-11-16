//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: ICameraPTZ.cs
//Version: 20151116

namespace TrackingCam.Plugins.CameraPTZ
{

  public interface ICameraPTZ
  {

    #region --- Properties ---

    double PanAngle { get; }
    double TiltAngle { get; }

    double ZoomLevel { get; }

    #endregion
  }

}
