//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: KinectV1VideoPlugin.cs
//Version: 20151204

//TODO: maybe implement IDisposable

using System;
using System.ComponentModel.Composition;
using System.Windows;

using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Controls;
using Microsoft.Kinect;
using LightBuzz.Vitruvius.WPF;
using System.Configuration;

namespace TrackingCam.Plugins.Video
{
  //MEF
  [Export("Video", typeof(IVideo))]
  [Export("Video.KinectV1", typeof(IVideo))]
  [ExportMetadata("Description", "Kinect v1")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class KinectV1VideoPlugin : IVideo, IInitializable
  {

    #region --- Fields ---

    protected KinectSensor _kinectSensor;
    protected KinectViewer _kinectViewer;

    #endregion

    #region --- Initialization ---

    public KinectV1VideoPlugin()
    {
    }

    public void Initialize(SettingsBase settings) //throws Exception
    {
      _kinectSensor = SensorExtensions.Default();

      if (_kinectSensor != null)
      {
        _kinectSensor.ColorStream.Enable();
        _kinectSensor.ColorFrameReady += Sensor_ColorFrameReady;
      }
    }

    #endregion

    #region --- Properties ---

    public bool Paused
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public UIElement Display
    {
      get
      {
        if (_kinectViewer == null)
          try
          {
            _kinectViewer = new KinectViewer() {
              FrameType = VisualizationMode.Color,
              FlippedHorizontally = true
            }; //TODO: see what throws exception here when Kinect is disconnected and fix to have better message
          }
          catch (Exception e)
          {
            MessageBox.Show(e.Message, "Kinect Video");
          }
        return _kinectViewer;
      }
    }

    #endregion

    #region --- Methods ---

    public void Start()
    {
      _kinectSensor?.Start(); //note: if it takes too long to start/stop, could start ones and use Enable/Disable method on ColorStream (or even attach/detach the ColorFrameReady event handler, or use a _started class field to ignore the frame)
    }

    public void Stop()
    {
      _kinectSensor?.Stop();
    }

    #endregion

    #region --- Events ---

    private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
    {
      if (_kinectViewer == null) return;

      using (var frame = e.OpenColorImageFrame())
        if (frame != null)
          _kinectViewer.Update(frame.ToBitmap());
    }

    #endregion

  }

}
