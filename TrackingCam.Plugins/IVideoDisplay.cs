//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IVideoDisplay.cs
//Version: 20151115

using System.Windows;

namespace TrackingCam.Plugins
{

  public interface IVideoDisplay
  {
    UIElement Player { get; }
    void Zoom(double percentX, double percentY, double percentWidth, double percentHeight);
  }

}
