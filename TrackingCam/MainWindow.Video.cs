//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Video.cs
//Version: 20151124

using System;
using System.Linq;
using System.Windows;

using TrackingCam.Plugins;
using TrackingCam.Plugins.Video;

namespace TrackingCam
{

  public partial class MainWindow : Window
  {

    #region --- Fields ---

    protected IVideo video;

    #endregion

    #region --- Methods ---

    public void LoadVideoPlugin()
    {
      Lazy<IVideo> plugin = PluginsCatalog.mefContainer.GetExports<IVideo>("Video").FirstOrDefault(); //TODO: change this to select from app settings which video plugin to use instead of just using the 1st one found
      video = plugin.Value;
      //(video as IInitializable)?.Initialize(...); //TODO: initialize this from app settings
    }

    #endregion

  }
}
