//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.xaml.cs
//Version: 20151128

using System;
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

    public void AddDisplayable(IDisplayable displayable, Grid container)
    {
      if (displayable == null) return;

      UIElement display = displayable.Display;
      if (display != null)
      {
        container.Children.Add(display);
        try
        {
          (displayable as IVideo)?.Start();
        }
        catch (Exception e)
        {
          MessageBox.Show((e.InnerException ?? e).Message);
        }
      }
    }

    #endregion

    #region --- Events ---

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      AddDisplayable(videoFoscam, ZoomVideoArea);
      AddDisplayable(videoKinect, TrackerVideoArea);
      AddDisplayable(tracker as IDisplayable, TrackerInfoArea); //AddDisplayable will ignore the call if null (that is the tracker isn't an IDisplayable)
    }

    #endregion

  }

}
