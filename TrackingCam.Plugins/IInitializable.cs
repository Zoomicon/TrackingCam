//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IInitializable.cs
//Version: 20151127

using System.Configuration;

namespace TrackingCam.Plugins
{

  public interface IInitializable
  {

    #region --- Methods ---

    void Initialize(SettingsBase settings); //throws Exception

    #endregion
  }

}
