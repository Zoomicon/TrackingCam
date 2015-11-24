//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: FoscamPTZPlugin.cs
//Version: 20151124

using Camera;
using Camera.Foscam;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace TrackingCam.Plugins.PTZ
{
  //MEF
  [Export("PTZ", typeof(IPTZ))]
  [Export("PTZ.Foscam", typeof(IPTZ))]
  [ExportMetadata("Description", "Foscam IP Camera PTZ")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class FoscamPTZPlugin : IPTZ, IInitializable
  {

    #region --- Constants ---

    public const string SETTING_CAMERA_FOSCAM_HD = "FoscamHD";
    public const string SETTING_CAMERA_FOSCAM_MJPEG = "FoscamMJPEG";

    protected const FoscamCameraType DEFAULT_CAMERA_TYPE = FoscamCameraType.FoscamHD;
    protected const string DEFAULT_USERNAME = "admin";
    protected const string DEFAULT_PASSWORD = "admin";

    #endregion

    #region --- Fields ---

    protected IMotionController _motion;
    protected IZoomController _zoom;
    protected double _panAngle, _tiltAngle;
    protected double _zoomLevel = 1.0;

    #endregion

    #region --- Initialization ---

    public FoscamPTZPlugin()
    {
    }

    public void Initialize(Dictionary<string, string> settings) //throws Exception
    {
      FoscamCameraType cameraType;
      string _cameraType;
      if (!settings.TryGetValue(PTZSettings.SETTING_CAMERA_TYPE, out _cameraType))
        cameraType = DEFAULT_CAMERA_TYPE;
      else
        if (string.Equals(_cameraType, SETTING_CAMERA_FOSCAM_HD, StringComparison.OrdinalIgnoreCase))
        cameraType = FoscamCameraType.FoscamHD;
      else if (string.Equals(_cameraType, SETTING_CAMERA_FOSCAM_MJPEG, StringComparison.OrdinalIgnoreCase))
        cameraType = FoscamCameraType.FoscamMJPEG;
      else
        throw new ArgumentNullException(PTZSettings.SETTING_CAMERA_TYPE);

      string url;
      if (!settings.TryGetValue(PTZSettings.SETTING_CAMERA_URL, out url))
        throw new ArgumentNullException(PTZSettings.SETTING_CAMERA_URL);

      string username;
      if (!settings.TryGetValue(PTZSettings.SETTING_CAMERA_USERNAME, out username))
        username = DEFAULT_USERNAME;

      string password;
      if (!settings.TryGetValue(PTZSettings.SETTING_CAMERA_PASSWORD, out password))
        username = DEFAULT_PASSWORD;

      _motion = FoscamMotion.CreateFoscamMotionController(cameraType, url, username, password);
      _zoom = FoscamZoom.CreateFoscamZoomController(cameraType, url, username, password);
    }

    #endregion

    #region --- Properties ---

    public double PanAngle
    {
      get
      {
        return _panAngle;
      }

      set
      {
        throw new NotImplementedException();
      }
    }

    public double TiltAngle
    {
      get
      {
        return _tiltAngle;
      }

      set
      {
        throw new NotImplementedException();
      }
    }

    public double ZoomLevel
    {
      get
      {
        return _zoomLevel;
      }

      set
      {
        throw new NotImplementedException();
      }
    }

    #endregion

  }

}
