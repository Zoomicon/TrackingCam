//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Plugins.cs
//Version: 20151203

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
      LoadPTZPlugin();
      LoadTrackingPlugins();
      LoadSpeechSynthesisPlugin();
      LoadSpeechRecognitionPlugin();
    }

    public void UnloadPlugins()
    {
      if (videoFoscam != null) videoFoscam.Stop();
      if (videoKinect != null) videoKinect.Stop();

      object[] plugins = new object[] { videoFoscam, videoKinect, trackerUbisense, trackerKinectAudio, ptz };
      foreach (object plugin in plugins)
        (plugin as IDisposable)?.Dispose();

      videoFoscam = null;
      videoKinect = null;
      trackerUbisense = null;
      trackerKinectAudio = null;
      ptz = null;
    }

    #endregion

  }
}
