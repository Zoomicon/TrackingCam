//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.xaml.cs
//Version: 20151126

using System.Windows;
using TrackingCam.Plugins;

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

    public void AddVideo()
    {
      if (video == null) return;

      UIElement display = video.Display;
      if (display != null)
      {
        LayoutRoot.Children.Add(video.Display);
        video.Start();
      }
    }

    #endregion

    #region --- Events ---

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      AddVideo();
    }

    #endregion

  }

}
