//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Plugins.cs
//Version: 20151117

using System.Windows;

namespace TrackingCam
{

  public partial class MainWindow : Window
  {
    #region --- Methods ---

    public void LoadPlugins()
    {
      LoadVideoPlugin();
      //LoadCameraPTZPlugin(); //TODO
      //LoadTrackingPlugin(); //TODO
    }

    #endregion

  }
}
