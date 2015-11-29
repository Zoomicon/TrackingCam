//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.xaml.cs
//Version: 20151128

using SilverFlow.Controls;
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

    public void AddDisplay(UIElement display, string title="")
    {
      FloatingWindow window = new FloatingWindow()
      {
        Content = display,
        Title = title,
        IconText = title
      };

      host.Add(window);
      window.Show(100, 100, true);
    }

    public void AddDisplayable(IDisplayable displayable, string title="")
    {
      if (displayable == null) return;

      UIElement display = displayable.Display;
      if (display != null)
      {
        AddDisplay(display, title);
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
      AddDisplayable(videoFoscam, "Video - Foscam IP Camera");
      AddDisplayable(videoKinect, "Video - Kinect Color Camera");
      AddDisplayable(tracker as IDisplayable, "Tracking - Kinect Microphone Array"); //AddDisplayable will ignore the call if null (that is the tracker isn't an IDisplayable)
    }

    #endregion

  }

}
