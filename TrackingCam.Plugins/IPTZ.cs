//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IPTZ.cs
//Version: 20151116

namespace TrackingCam.Plugins.PTZ
{

  public interface IPTZ
  {

    #region --- Properties ---

    double PanAngle { get; set; }
    double TiltAngle { get; set; }

    double ZoomLevel { get; set; }

    #endregion
  }

}
