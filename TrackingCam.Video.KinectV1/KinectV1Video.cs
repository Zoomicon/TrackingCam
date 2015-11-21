//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: KinectV1Video.cs
//Version: 20151122

using System;
using System.ComponentModel.Composition;
using System.Windows;

using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Controls;
using Microsoft.Kinect;
using LightBuzz.Vitruvius.WPF;

namespace TrackingCam.Plugins.Video
{
  //MEF
  [Export("Video", typeof(IVideo))]
  [ExportMetadata("Description", "Kinect v1")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class KinectV1Video : IVideo
  {

    #region --- Fields ---

    protected KinectSensor _kinectSensor;
    protected KinectViewer _kinectViewer;

    #endregion

    #region --- Initialization ---

    public KinectV1Video()
    {
      _kinectSensor = SensorExtensions.Default();

      if (_kinectSensor != null)
      {
        _kinectSensor.ColorStream.Enable();
        _kinectSensor.ColorFrameReady += Sensor_ColorFrameReady;
      }
    }

    private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
    {
      using (var frame = e.OpenColorImageFrame())
        if (frame != null)
          _kinectViewer.Update(frame.ToBitmap());
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
          _kinectViewer = new KinectViewer() { FrameType = VisualizationMode.Color };

        return _kinectViewer;
      }
    }

#endregion

    #region --- Methods ---

    public void Start()
    {
      _kinectSensor.Start(); //note: if it takes too long to start/stop, could start ones and use Enable/Disable method on ColorStream (or even attach/detach the ColorFrameReady event handler, or use a _started class field to ignore the frame)
    }

    public void Stop()
    {
      _kinectSensor.Stop();
    }

    #endregion
  }

}
