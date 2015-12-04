//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: KinectV1DepthTrackingPlugin.cs
//Version: 20151204

using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Windows;

using TrackingCam.Plugins.Actions;
using LightBuzz.Vitruvius.Controls;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.WPF;
using System.Linq;

namespace TrackingCam.Plugins.Tracking
{
  //MEF
  [Export("Tracking", typeof(ITracker))]
  [Export("Tracking.KinectV1Depth", typeof(ITracker))]
  [ExportMetadata("Description", "Kinect v1 Depth Tracking")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class KinectV1DepthTrackingPlugin : ITracker, IInitializable, IDisplayable, IActionable
  {

    #region --- Constants ---

    public const string ACTIONABLE_ID = "KinectGestureRecognizer";

    #endregion

    #region --- Fields ---

    protected KinectSensor _kinectSensor;
    protected KinectViewer _kinectViewer;
    protected GestureController _gestureController;
    protected SkeletonPoint position; //= new SkeletonPoint() { X = 0, Y = 0, Z = 0 };

    #endregion

    #region --- Initialization ---

    public KinectV1DepthTrackingPlugin()
    {
    }

    public void Initialize(SettingsBase settings) //throws Exception
    {
      _kinectSensor = SensorExtensions.Default();

      if (_kinectSensor != null)
      {
        _kinectSensor.EnableAllStreams();
        _kinectSensor.ColorFrameReady += Sensor_ColorFrameReady;
        //sensor.DepthFrameReady += Sensor_DepthFrameReady;
        _kinectSensor.SkeletonFrameReady += Sensor_SkeletonFrameReady;

        _gestureController = new GestureController(GestureType.All);
        _gestureController.GestureRecognized += GestureController_GestureRecognized;

        _kinectSensor.Start();
      }
    }

    #endregion

    #region --- Properties ---

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
            MessageBox.Show(e.Message, "Kinect Depth");
          }
        return _kinectViewer;
      }
    }

    public double PositionHorizontal
    {
      get
      {
        return position.X;
      }
    }

    public double PositionVertical
    {
      get
      {
        return position.Y;
      }
    }

    public double PositionDepth
    {
      get
      {
        return position.Z;
      }
    }

    public double PositionAngle
    {
      get
      {
        return RadToDeg(Math.Atan2(PositionHorizontal, PositionDepth));
      }
    }

    #endregion

    #region --- Methods ---

    public static double RadToDeg(double radians)
    {
      return radians * (180 / Math.PI);
    }

    #endregion

    #region --- Events ---

    public event ActionEvents.ActionOccuredEventHandler ActionOccured;

    private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
    {
      if (_kinectViewer == null) return;

      using (var frame = e.OpenColorImageFrame())
        if (frame != null)
          _kinectViewer.Update(frame.ToBitmap());
    }

    private void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
    {
      using (var frame = e.OpenSkeletonFrame())
        if (frame != null)
        {
          if (_kinectViewer != null)
            _kinectViewer.Clear();

          var skeletons = frame.Skeletons().Where(s => s.TrackingState == SkeletonTrackingState.Tracked);

          foreach (var skeleton in skeletons)
            if (skeleton != null)
            {
              // Update skeleton gestures
              _gestureController.Update(skeleton);

              // Draw skeleton
              if (_kinectViewer != null)
                _kinectViewer.DrawBody(skeleton);

              position = skeleton.Position;
            }
        }
    }

    private void GestureController_GestureRecognized(object sender, GestureEventArgs e)
    {
      ActionOccured(this, ACTIONABLE_ID, e.Name);
    }

    #endregion

  }

}