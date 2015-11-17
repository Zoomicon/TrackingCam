//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Video.cs
//Version: 20151117

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

    public IVideo video;

    #endregion

    #region --- Methods ---

    public void LoadVideoPlugin()
    {
      Lazy<IVideo> plugin = PluginsCatalog.mefContainer.GetExports<IVideo>("Video").FirstOrDefault(); //TODO: change this to select from app settings which video plugin to use
      video = plugin.Value;
      //(video as IInitializable)?.Initialize(...); //TODO: initialize this from app settings
    }

    #endregion

  }
}
