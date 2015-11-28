//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IVideo.cs
//Version: 20151128

using System.Windows;

namespace TrackingCam.Plugins
{

  public interface IDisplayable
  {

    #region --- Properties ---

    UIElement Display { get; }

    #endregion

  }

}
