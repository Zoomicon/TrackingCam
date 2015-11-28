//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IVideo.cs
//Version: 20151128

namespace TrackingCam.Plugins.Video
{

  public interface IVideo : IDisplayable
  {

    #region --- Properties ---

    bool Paused { get; }

    #endregion

    #region --- Methods ---

    void Start();
    void Stop();

    #endregion
  }

}
