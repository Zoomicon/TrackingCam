//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IInitializable.cs
//Version: 20151116

using System.Collections.Generic;

namespace TrackingCam.Plugins.Video
{

  public interface IInitializable
  {

    #region --- Methods ---

    void Initialize(Dictionary<string, string> settings); //throws Exception

    #endregion
  }

}
