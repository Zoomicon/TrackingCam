//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: FoscamPTZPlugin.cs
//Version: 20151127

using Camera;
using Camera.Foscam;

using System;
using System.ComponentModel.Composition;
using System.Configuration;

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

    public void Initialize(SettingsBase settings) //throws Exception
    {
      //Get the Foscam camera type (MJPEG or HD)
      FoscamCameraType cameraType;
      string _cameraType = (string)settings[PTZSettings.SETTING_CAMERA_TYPE];
      if (_cameraType == null || _cameraType == "")
        cameraType = DEFAULT_CAMERA_TYPE;
      else
        if (string.Equals(_cameraType, SETTING_CAMERA_FOSCAM_HD, StringComparison.OrdinalIgnoreCase))
        cameraType = FoscamCameraType.FoscamHD;
      else if (string.Equals(_cameraType, SETTING_CAMERA_FOSCAM_MJPEG, StringComparison.OrdinalIgnoreCase))
        cameraType = FoscamCameraType.FoscamMJPEG;
      else
        throw new ArgumentNullException(PTZSettings.SETTING_CAMERA_TYPE);

      //Get the camera URL
      string url = (string)settings[PTZSettings.SETTING_CAMERA_URL];
      if (url == null || url == "")
        throw new ArgumentNullException(PTZSettings.SETTING_CAMERA_URL);

      //Get the username
      string username = (string)settings[PTZSettings.SETTING_CAMERA_USERNAME] ?? DEFAULT_USERNAME;

      //Get the password
      string password = (string)settings[PTZSettings.SETTING_CAMERA_PASSWORD] ?? DEFAULT_PASSWORD;

      //Create motion and zoom controllers
      _motion = FoscamMotion.CreateFoscamMotionController(cameraType, url, username, password);
      _zoom = FoscamZoom.CreateFoscamZoomController(cameraType, url, username, password);

      //Unzoom
      ZoomLevel = 0;
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
        //TODO: go to nearest horizontal,vertical preset point (and then set _panAngle to its real value). Maybe need some param with the preset point names given in horizontalAngle:verticalAngle string format
        _panAngle = value;
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
        //TODO: go to nearest horizontal,vertical preset point (and then set _tiltAngle to its real value). Maybe need some param with the preset point names given in horizontalAngle:verticalAngle string format
        _tiltAngle = value;
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
        if (value == 0)
          _zoom.ZoomOut();
        else
          _zoom.ZoomIn();

        _zoomLevel = value;
      }
    }

    #endregion

  }

}
