//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IVideo.cs
//Version: 20151116

using System.Windows;

namespace TrackingCam.Plugins.Video
{

  public interface IVideo
  {

    #region --- Properties ---

    UIElement Display { get; }

    bool Paused { get; }

    #endregion

    #region --- Methods ---

    void Start();
    void Stop();

    #endregion
  }

}
