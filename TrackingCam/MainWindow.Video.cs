//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Video.cs
//Version: 20151204

using System;
using System.Linq;
using System.Windows;

using TrackingCam.Plugins;
using TrackingCam.Plugins.Video;
using TrackingCam.Properties;

namespace TrackingCam
{

  public partial class MainWindow : Window
  {

    #region --- Fields ---

    protected IVideo videoKinect;
    protected IVideo videoFoscam;

    #endregion

    #region --- Methods ---

    public IVideo LoadVideoPlugin(string protocol)
    {
      Lazy<IVideo> plugin = PluginsCatalog.mefContainer.GetExports<IVideo>(protocol).FirstOrDefault(); //TODO: change this to select from app settings which video plugin to use instead of just using the 1st one found
      IVideo video = (plugin != null) ? plugin.Value : null;
      try
      {
        (video as IInitializable)?.Initialize(Settings.Default);
      }
      catch (Exception e)
      {
        video = null;
        MessageBox.Show((e.InnerException ?? e).Message);
      }
      return video;
    }

    public void LoadKinectVideoPlugin()
    {
      videoKinect = LoadVideoPlugin("Video.KinectV1");
    }

    public void LoadFoscamVideoPlugin()
    {
      videoFoscam = LoadVideoPlugin("Video.Foscam");
    }

    public void LoadVideoPlugins()
    {
      //LoadKinectVideoPlugin();
      LoadFoscamVideoPlugin();
    }

    #endregion

  }
}
