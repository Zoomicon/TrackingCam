//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Plugins.cs
//Version: 20151210

using System;
using System.Windows;

namespace TrackingCam
{

  public partial class MainWindow : Window
  {
    #region --- Methods ---

    static MainWindow()
    {
    }

    public void LoadPlugins()
    {
      LoadVideoPlugins();
      LoadPTZPlugins();
      LoadTrackingPlugins();
      LoadSpeechSynthesisPlugin();
      LoadSpeechRecognitionPlugin();
    }

    public void UnloadPlugins()
    {
      if (videoFoscam != null) videoFoscam.Stop();
      if (videoKinect != null) videoKinect.Stop();

      object[] plugins = new object[] { videoFoscam, videoKinect, trackerKinectDepth, trackerKinectAudio, trackerUbisense, ptzFoscam };
      foreach (object plugin in plugins)
        (plugin as IDisposable)?.Dispose();

      videoFoscam = null;
      videoKinect = null;

      trackerKinectAudio = null;
      trackerKinectDepth = null;
      trackerUbisense = null;

      ptzFoscam = null;
    }

    #endregion

  }
}
