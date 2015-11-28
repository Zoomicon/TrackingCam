//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.PTZ.cs
//Version: 20151128

using System;
using System.Linq;
using System.Windows;

using TrackingCam.Plugins;
using TrackingCam.Plugins.PTZ;
using TrackingCam.Properties;

namespace TrackingCam
{

  public partial class MainWindow : Window
  {

    #region --- Fields ---

    protected IPTZ ptz;

    #endregion

    #region --- Methods ---

    public void LoadPTZPlugin()
    {
      Lazy<IPTZ> plugin = PluginsCatalog.mefContainer.GetExports<IPTZ>("PTZ").FirstOrDefault(); //TODO: change this to select from app settings which tracking plugin to use instead of just using the 1st one found
      ptz = plugin.Value;
      try
      {
        (ptz as IInitializable)?.Initialize(Settings.Default);
      } catch (Exception e)
      {
        ptz = null;
        MessageBox.Show((e.InnerException ?? e).Message);
      }
    }

    #endregion

  }
}
