//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: IActionable.cs
//Version: 20151203

namespace TrackingCam.Plugins.Actions
{

  public interface IActionable
  {

    #region --- Events ---

    event ActionEvents.ActionOccuredEventHandler ActionOccured;

    #endregion
  }

}
