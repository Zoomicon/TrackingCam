//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.xaml.cs
//Version: 20151204

//using SilverFlow.Controls;
using System;
using System.Windows;
//using System.Windows.Controls;

using SilverFlow.Controls;

using TrackingCam.Plugins;
using TrackingCam.Plugins.Video;

namespace TrackingCam
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    #region --- Constants ---

    public const string SPEECH_GREETING = "You can use speech commands Track, Stop, Zoom and Unzoom";

    #endregion

    #region --- Initialization ---

    public MainWindow()
    {
      InitializeComponent();
      PluginsCatalog.Init(this);
      LoadPlugins();
    }

    public void InitializeUI()
    {
      AddDisplayable(videoFoscam, "Video - Foscam IP Camera", new Rect(0, 0, 600, 600)); //IVideo interface extends from IDisplayable
      //AddDisplayable(videoKinect, "Video - Kinect Color Camera", new Rect(0, 600, 1000, 150)); //IVideo interface extends from IDisplayable

      AddDisplayable(ptz as IDisplayable, "PTZ - Foscam IP Camera", new Rect(600, 400, 200, 200)); //AddDisplayable will ignore the call if null (that is if the tracker isn't an IDisplayable)

      AddDisplayable(trackerKinectAudio as IDisplayable, "Tracking - Kinect Microphone Array", new Rect(600, 200, 200, 200)); //AddDisplayable will ignore the call if null (that is if the tracker isn't an IDisplayable)
      AddDisplayable(trackerKinectDepth as IDisplayable, "Tracking - Kinect Depth", new Rect(800, 200, 400, 400));
      AddDisplayable(trackerUbisense as IDisplayable, "Tracking - Ubisense", new Rect(600, 0, 450, 200)); //AddDisplayable will ignore the call if null (that is if the tracker isn't an IDisplayable)
    }

    #endregion

    #region --- Cleanup ---

    public void Cleanup()
    {
      UnloadPlugins();
    }

    #endregion

    #region --- Methods ---

    /* //TEMP FIX
    public void AddDisplay(UIElement display, string title = "", Rect? bounds = null) //TODO: REMOVE TEMP FIX
    {
      FrameworkElement f = (display as FrameworkElement);
      if (f == null) return;

      Rect r = bounds.HasValue ? bounds.Value : new Rect(0,0,1000,1000);

      f.SetValue(Canvas.LeftProperty, bounds.Value.X);
      f.SetValue(Canvas.TopProperty, bounds.Value.Y);
      f.Width = r.Width;
      f.Height = r.Height;

      canvas.Children.Add(f);
    }
    */

    public void AddDisplay(UIElement display, string title="", Rect? bounds = null)
    {
      FloatingWindow window = new FloatingWindow()
      {
        Content = display,
        Title = title,
        IconText = title
      };

      host.Add(window);

      if (bounds.HasValue)
      {
        window.Width = bounds.Value.Width;
        window.Height = bounds.Value.Height;
        window.Show(bounds.Value.TopLeft.X, bounds.Value.TopLeft.Y);
      }
      else
        window.Show(100, 100);
    }

    public void AddDisplayable(IDisplayable displayable, string title="", Rect? bounds = null)
    {
      if (displayable == null) return;

      UIElement display = displayable.Display;
      if (display != null)
      {
        AddDisplay(display, title, bounds);
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
      InitializeUI();
      if (ptz != null)
        speechSynthesis?.Speak(SPEECH_GREETING);

      TrackingPresenter = true; //start tracking the presenter
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      Cleanup();
    }

    #endregion

  }

}
