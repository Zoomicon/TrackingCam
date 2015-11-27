//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: KinectAudioTrackingPlugin.cs
//Version: 20151127

using KinectAudioPositioning;

using System;
using System.ComponentModel.Composition;
using System.Configuration;

namespace TrackingCam.Plugins.Tracking
{
  //MEF
  [Export("Tracking", typeof(ITracker))]
  [Export("Tracking.KinectAudio", typeof(ITracker))]
  [ExportMetadata("Description", "Kinect Audio Tracking")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class KinectAudioTrackingPlugin : ITracker, IInitializable
  {

    #region --- Constants ---

    public const double DEFAULT_DISTANCE = 4.0; //4m is the Kinect sensor physical limit (3.5 is the useful region limit) in default (not near) mode

    #endregion

    #region --- Fields ---

    protected KinectMicArray _positioning; //=null
    protected double _distance;

    #endregion

    #region --- Initialization ---

    public KinectAudioTrackingPlugin()
    {
    }

    public void Initialize(SettingsBase settings) //throws Exception
    {
      _distance = (double?)settings[TrackingSettings.SETTING_TRACKING_DISTANCE] ?? DEFAULT_DISTANCE;

      _positioning = new KinectMicArray();
    }

    #endregion

    #region --- Properties ---

    public double PositionHorizontal
    {
      get { return Math.Tan(_positioning.SourceAngle) * _distance; }
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
      get { return _positioning.SourceAngle;  }
    }

    #endregion

  }

}