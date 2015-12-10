//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.PTZ.cs
//Version: 20151210

using System;
//using System.Diagnostics;
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

    protected IPTZ ptz, ptzFoscam;

    #endregion

    #region --- Methods ---

    public IPTZ LoadPTZPlugin(string protocol)
    {
      Lazy<IPTZ> plugin = PluginsCatalog.mefContainer.GetExports<IPTZ>(protocol).FirstOrDefault(); //TODO: change this to select from app settings which tracking plugin to use instead of just using the 1st one found
      IPTZ ptz = (plugin != null) ? plugin.Value : null;
      try
      {
        (ptz as IInitializable)?.Initialize(Settings.Default);
      } catch (Exception e)
      {
        ptz = null;
        MessageBox.Show((e.InnerException ?? e).Message);
      }
      return ptz;
    }

    public void LoadPTZPlugins()
    {
      ptzFoscam = LoadPTZPlugin("PTZ.Foscam");
      ptz = ptzFoscam;
    }

    public void LookTo(double angle)
    {
      //Debug.WriteLine("LookTo: " + angle);
      if (ptzFoscam!= null)
        ptzFoscam.PanAngle = angle;
    }

    #endregion

  }
}
