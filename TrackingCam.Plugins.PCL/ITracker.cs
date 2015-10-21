//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: ITracker.cs
//Version: 20151021

namespace TrackingCam.Plugins
{

  public interface ITracker
  {
    double PositionHorizontal { get; }
    double PositionVertical { get; }
    double PositionDepth { get; }

    double SizeWidth { get; }
    double SizeHeight { get; }
    double SizeDepth { get; }
  }

}
