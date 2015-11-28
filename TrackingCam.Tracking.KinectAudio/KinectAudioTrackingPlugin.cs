//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: KinectAudioTrackingPlugin.cs
//Version: 20151128

using KinectAudioPositioning.WPF;

using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Windows;

namespace TrackingCam.Plugins.Tracking
{
  //MEF
  [Export("Tracking", typeof(ITracker))]
  [Export("Tracking.KinectAudio", typeof(ITracker))]
  [ExportMetadata("Description", "Kinect Audio Tracking")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class KinectAudioTrackingPlugin : ITracker, IInitializable, IDisplayable
  {

    #region --- Constants ---

    public const double DEFAULT_DISTANCE = 4.0; //4m is the Kinect sensor physical limit (3.5 is the useful region limit) in default (not near) mode

    #endregion

    #region --- Fields ---

    protected KinectAudioPositioningUI _positioning; //=null
    protected double _distance = DEFAULT_DISTANCE;

    #endregion

    #region --- Initialization ---

    public KinectAudioTrackingPlugin()
    {
    }

    public void Initialize(SettingsBase settings) //throws Exception
    {
      _distance = (double?)settings[TrackingSettings.SETTING_TRACKING_DISTANCE] ?? DEFAULT_DISTANCE;

      _positioning = new KinectAudioPositioningUI();
    }

    #endregion

    #region --- Properties ---

    public UIElement Display {
      get { return _positioning;  }
    }

    public double PositionHorizontal
    {
      get { return Math.Tan(_positioning.KinectMicArray.SourceAngle) * _distance; }
    }

    public double PositionVertical
    {
      get { return 0; }
    }

    public double PositionDepth
    {
      get { return _distance; }
    }

    public double PositionAngle
    {
      get { return _positioning.KinectMicArray.SourceAngle;  }
    }

    #endregion

  }

}