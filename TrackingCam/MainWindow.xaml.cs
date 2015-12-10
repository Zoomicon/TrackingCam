//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.xaml.cs
//Version: 20151210

using System;
using System.Windows;

using SilverFlow.Controls;

using TrackingCam.Plugins;
using TrackingCam.Plugins.Tracking;
using TrackingCam.Plugins.Video;
using TrackingCam.Properties;

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

    #region --- Fields ---

    FloatingWindow windowVideoFoscam,
                   //windowVideoKinect,
                   windowPTZFoscam,
                   windowTrackerKinectAudio, windowTrackerKinectDepth, windowTrackerUbisense; //=null

    #endregion

    #region --- Initialization ---

    public MainWindow()
    {
      InitializeComponent();
      PluginsCatalog.Init(this);
      LoadPlugins();
      Settings.Default.PropertyChanged += Settings_PropertyChanged;
    }

    public void InitializeUI()
    {
      windowVideoFoscam = AddDisplayable(videoFoscam, "Video - Foscam IP Camera", new Rect(0, 0, 600, 600)); //IVideo interface extends from IDisplayable
      //windowVideoKinect = AddDisplayable(videoKinect, "Video - Kinect Color Camera", new Rect(0, 600, 1000, 150)); //IVideo interface extends from IDisplayable

      windowPTZFoscam = AddDisplayable(ptzFoscam as IDisplayable, "PTZ - Foscam IP Camera", new Rect(600, 400, 200, 200)); //AddDisplayable will ignore the call if null (that is if the tracker isn't an IDisplayable)

      windowTrackerKinectAudio = AddDisplayable(trackerKinectAudio as IDisplayable, "Tracking - Kinect Microphone Array", new Rect(600, 200, 200, 200)); //AddDisplayable will ignore the call if null (that is if the tracker isn't an IDisplayable)
      windowTrackerKinectDepth = AddDisplayable(trackerKinectDepth as IDisplayable, "Tracking - Kinect Depth", new Rect(800, 200, 400, 400));
      windowTrackerUbisense = AddDisplayable(trackerUbisense as IDisplayable, "Tracking - Ubisense", new Rect(600, 0, 450, 200)); //AddDisplayable will ignore the call if null (that is if the tracker isn't an IDisplayable)
    }

    #endregion

    #region --- Cleanup ---

    public void Cleanup()
    {
      TrackingPresenter = false;
      UnloadPlugins();
    }

    #endregion

    #region --- Methods ---

    #region Settings

    public void ApplySettings()
    {
      ApplySettings_Tracking();
    }

    public void ReloadSettings()
    {
      Settings.Default.Reload();
      ApplySettings();
    }

    public void SaveSettings()
    {
      Settings.Default.Save();
    }

    #endregion

    #region Windows

    public FloatingWindow AddDisplay(UIElement display, string title="", Rect? bounds = null, bool visible = true)
    {
      FloatingWindow window = new FloatingWindow()
      {
        Content = display,
        Title = title,
        IconText = title,
        Visibility = visible ? Visibility.Visible : Visibility.Hidden
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

      return window;
    }

    public FloatingWindow AddDisplayable(IDisplayable displayable, string title="", Rect? bounds = null, bool visible = true)
    {
      if (displayable == null) return null;

      UIElement display = displayable.Display;
      if (display == null) return null;

      FloatingWindow window = AddDisplay(display, title, bounds, visible);
      try
      {
        (displayable as IVideo)?.Start();
      }
      catch (Exception e)
      {
        MessageBox.Show((e.InnerException ?? e).Message);
      }
      return window;
    }

    #endregion

    #endregion

    #region --- Events ---

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      InitializeUI();

      if (ptz != null)
        Speak(SPEECH_GREETING);

      ApplySettings();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      SaveSettings(); //save settings before calling Cleanup
      Cleanup();
    }

    private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case TrackingSettings.SETTING_TRACKING_PRESENTER:
          SetTrackingPresenterFromSettings();
          break;
        case TrackingSettings.SETTING_TRACKER:
          SetTrackerFromSettings();
          break;
      }
    }

    #endregion

  }

}
