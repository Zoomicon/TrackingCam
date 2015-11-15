//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IVideoCapture.cs
//Version: 20151021

namespace TrackingCam.Plugins
{

  public interface IVideoCapture
  {
    bool Paused { get; }

    void Start();
    void Stop();
  }

}
