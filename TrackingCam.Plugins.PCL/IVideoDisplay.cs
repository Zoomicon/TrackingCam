//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IVideoDisplay.cs
//Version: 20151021

namespace TrackingCam.Plugins
{

  public interface IVideoDisplay
  {
    bool Paused { get; }
    void Zoom(double percentX, double percentY, double percentWidth, double percentHeight);
  }

}
