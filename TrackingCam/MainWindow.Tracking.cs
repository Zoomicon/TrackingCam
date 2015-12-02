//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Tracking.cs
//Version: 20151201

using System;
using System.Linq;
using System.Windows;

using TrackingCam.Plugins;
using TrackingCam.Plugins.Tracking;
using TrackingCam.Properties;

namespace TrackingCam
{

  public partial class MainWindow : Window
  {

    #region --- Fields ---

    protected ITracker trackerKinectAudio;
    protected ITracker trackerUbisense;

    #endregion

    #region --- Methods ---

    public ITracker LoadTrackingPlugin(string protocol)
    {
      Lazy<ITracker> plugin = PluginsCatalog.mefContainer.GetExports<ITracker>(protocol).FirstOrDefault(); //TODO: change this to select from app settings which tracking plugin to use instead of just using the 1st one found
      ITracker tracker = (plugin != null) ? plugin.Value : null;
      try
      {
        (tracker as IInitializable)?.Initialize(Settings.Default);
      }
      catch (Exception e)
      {
        tracker = null;
        MessageBox.Show((e.InnerException ?? e).Message);
      }
      return tracker;
    }

    public void LoadKinectAudioTrackingPlugin()
    {
      trackerKinectAudio = LoadTrackingPlugin("Tracking.KinectAudio");
    }

    public void LoadUbisenseTrackingPlugin()
    {
      trackerUbisense = LoadTrackingPlugin("Tracking.Ubisense");
    }

    public void LoadTrackingPlugins()
    {
      LoadKinectAudioTrackingPlugin();
      LoadUbisenseTrackingPlugin();
    }

    #endregion

  }
}
