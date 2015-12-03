//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: ActionEvents.cs
//Version: 20151203

using System;

namespace TrackingCam.Plugins.Actions
{

  public static class ActionEvents
  {

    #region --- Events ---

    public delegate void ActionOccuredEventHandler(object sender, string id, string action);

    #endregion
  }

}
