//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Speech.cs
//Version: 20151210

using System;
using System.Windows;

using SpeechLib.Models;
using TrackingCam.Plugins;
using System.Linq;
using System.IO;
using System.Reflection;
using SpeechLib.Recognition.KinectV1;

namespace TrackingCam
{

  public partial class MainWindow : Window
  {

    #region --- Constants ---

    private const double DEFAULT_SPEECH_RECOGNITION_CONFIDENCE_THRESHOLD = 0.7;

    #endregion

    #region --- Fields ---

    public ISpeechSynthesis speechSynthesis;
    public ISpeechRecognition speechRecognition;
    public ISpeechRecognitionKinect speechRecognitionKinect;

    private double SpeechRecognitionConfidenceThreshold = DEFAULT_SPEECH_RECOGNITION_CONFIDENCE_THRESHOLD;

    #endregion

    #region --- Methods ---

    public void LoadSpeechSynthesisPlugin()
    {
      Lazy<ISpeechSynthesis> plugin = PluginsCatalog.mefContainer.GetExports<ISpeechSynthesis>("SpeechLib.Synthesis").FirstOrDefault();
      speechSynthesis = (plugin != null) ? plugin.Value : null;
    }

    public void LoadSpeechRecognitionPlugin()
    {
      Lazy<ISpeechRecognitionKinect> plugin1 = PluginsCatalog.mefContainer.GetExports<ISpeechRecognitionKinect>("SpeechLib.Recognition.KinectV1").FirstOrDefault();
      speechRecognition = speechRecognitionKinect = (plugin1 != null) ? plugin1.Value : null;

      if (speechRecognition == null) //SpeechRecognitionKinect plugin couldn't be loaded, try to fallback to the SpeechRecognition one (which uses the default audio source as input)
      {
        Lazy<ISpeechRecognition> plugin2 = PluginsCatalog.mefContainer.GetExports<ISpeechRecognition>("SpeechLib.Recognition").FirstOrDefault();
        speechRecognition = (plugin2 != null) ? plugin2.Value : null;
      }

      if (speechRecognition != null)
        StartSpeechRecognition();
    }

    private void StartSpeechRecognition()
    {
      if (speechRecognition == null)
        return;

      try
      {
        string grammarsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Grammars", "SRGS");
        speechRecognition.LoadGrammar(new FileStream(Path.Combine(grammarsFolder, "TrackingCam_en.xml"), FileMode.Open), "TrackingCam");

        speechRecognition.Recognized += SpeechRecognition_Recognized;
        speechRecognition.NotRecognized += SpeechRecognition_NotRecognized;

        /*
        //For long recognition sessions (a few hours or more), it may be beneficial to turn off adaptation of the acoustic model.
        //This will prevent recognition accuracy from degrading over time.
        speechRecognition.AcousticModelAdaptation = false;
        */

        speechRecognition.Start();
      }
      catch(Exception e)
      {
        speechRecognitionKinect = null;
        speechRecognition = null;
        MessageBox.Show(e.Message);
      }
    }

    #endregion

    #region --- Events ---

    private void SpeechRecognition_Recognized(object sender, SpeechRecognitionEventArgs e)
    {
      if ((e.confidence >= SpeechRecognitionConfidenceThreshold))
        switch (e.command)
        {
          case Commands.ZOOM_IN:
            if (ptz != null)
              ptz.ZoomLevel = 1;
            break;
          case Commands.ZOOM_OUT:
            if (ptz != null)
              ptz.ZoomLevel = 0;
            break;
          case Commands.TRACK_ON:
            StartTrackingPresenter();
            break;
          case Commands.TRACK_OFF:
            StopTrackingPresenter();
            break;
        }
    }

    private void SpeechRecognition_NotRecognized(object sender, EventArgs e)
    {
      //TODO: maybe show some hint about which are the supported voice commands
    }

    #endregion

  }
}
