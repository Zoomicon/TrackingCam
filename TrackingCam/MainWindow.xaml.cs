//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.xaml.cs
//Version: 20151127

using System.Windows;
using System.Windows.Controls;
using TrackingCam.Plugins;
using TrackingCam.Plugins.Video;

namespace TrackingCam
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    #region --- Initialization ---

    public MainWindow()
    {
      InitializeComponent();
      PluginsCatalog.Init(this);
      LoadPlugins();
    }

    #endregion

    #region --- Methods ---

    public void AddVideo(IVideo video, Grid container)
    {
      if (video == null) return;

      UIElement display = video.Display;
      if (display != null)
      {
        container.Children.Add(display);
        video.Start();
      }
    }

    #endregion

    #region --- Events ---

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      AddVideo(videoFoscam, ZoomVideoArea);
      AddVideo(videoKinect, TrackerVideoArea);
    }

    #endregion

  }

}
