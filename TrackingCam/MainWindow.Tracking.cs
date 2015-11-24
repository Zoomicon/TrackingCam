//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Tracking.cs
//Version: 20151124

using System;
using System.Linq;
using System.Windows;

using TrackingCam.Plugins;
using TrackingCam.Plugins.Tracking;

namespace TrackingCam
{

  public partial class MainWindow : Window
  {

    #region --- Fields ---

    protected ITracker tracker;

    #endregion

    #region --- Methods ---

    public void LoadTrackingPlugin()
    {
      Lazy<ITracker> plugin = PluginsCatalog.mefContainer.GetExports<ITracker>("Tracking").FirstOrDefault(); //TODO: change this to select from app settings which tracking plugin to use instead of just using the 1st one found
      tracker = plugin.Value;
      //(tracker as IInitializable)?.Initialize(...); //TODO: initialize this from app settings
    }

    #endregion

  }
}
