//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: KinectAudioTrackingPlugin.cs
//Version: 20151124

using KinectAudioPositioning;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

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

    public void Initialize(Dictionary<string, string> settings) //throws Exception
    {
      string distanceStr;
      settings.TryGetValue(TrackingSettings.SETTING_TRACKING_OBJECT_KEY, out distanceStr);
      if (!double.TryParse(distanceStr, out _distance))
        _distance = DEFAULT_DISTANCE;
      _positioning = new KinectMicArray();
    }

    #endregion

    #region --- Properties ---

    public double PositionHorizontal
    {
      get
      {
        return Math.Tan(_positioning.SourceAngle) * _distance;
      }
    }

    public double PositionVertical
    {
      get
      {
        return 0;
      }
    }

    public double PositionDepth
    {
      get
      {
        return 0;
      }
    }

    #endregion

  }

}