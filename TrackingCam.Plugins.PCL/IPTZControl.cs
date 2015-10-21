//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IPTZControl.cs
//Version: 20151021

namespace TrackingCam.Plugins
{

  public interface IPTZControl
  {
    double PanAngle { get; }
    double TiltAngle { get; }

    double ZoomLevel { get; }
  }

}
