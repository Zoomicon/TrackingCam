//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: FoscamPTZPlugin.cs
//Version: 20151211

using Camera;
using Camera.Foscam;

using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading;
using System.Windows;

namespace TrackingCam.Plugins.PTZ
{
  //MEF
  [Export("PTZ", typeof(IPTZ))]
  [Export("PTZ.Foscam", typeof(IPTZ))]
  [ExportMetadata("Description", "Foscam IP Camera PTZ")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class FoscamPTZPlugin : IPTZ, IInitializable, IDisplayable
  {

    #region --- Constants ---

    public const string SETTING_CAMERA_FOSCAM_HD = "FoscamHD";
    public const string SETTING_CAMERA_FOSCAM_MJPEG = "FoscamMJPEG";

    protected const FoscamCameraType DEFAULT_CAMERA_TYPE = FoscamCameraType.FoscamHD;
    protected const string DEFAULT_USERNAME = "admin";
    protected const string DEFAULT_PASSWORD = "admin";

    protected const double DEFAULT_MIN_PAN_ANGLE = -45.0; //degrees
    protected const double DEFAULT_MAX_PAN_ANGLE = 45.0; //degrees

    protected const string DEFAULT_PRESET_PREFIX = "Prefix";
    protected const int DEFAULT_PRESET_COUNT = 10;

    #endregion

    #region --- Fields ---

    protected PTZControl _ptz;

    protected IMotionController _motion;
    protected IZoomController _zoom;

    protected double _panAngle, _tiltAngle; //=0
    protected double _minPanAngle = DEFAULT_MIN_PAN_ANGLE;
    protected double _maxPanAngle = DEFAULT_MAX_PAN_ANGLE;

    protected double _zoomLevel; //=0;

    protected string _presetPrefix = DEFAULT_PRESET_PREFIX;
    protected int _presetCount = DEFAULT_PRESET_COUNT;
    protected string _presetPoint; //=null

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

      //Create PTZ control UI
      _ptz = new PTZControl() { MotionController = _motion, ZoomController = _zoom };

      _minPanAngle = (double?)settings[PTZSettings.SETTING_MIN_PAN_ANGLE] ?? DEFAULT_MIN_PAN_ANGLE;
      _maxPanAngle = (double?)settings[PTZSettings.SETTING_MAX_PAN_ANGLE] ?? DEFAULT_MAX_PAN_ANGLE;

      _presetCount = (int?)settings[PTZSettings.SETTING_PRESET_COUNT] ?? DEFAULT_PRESET_COUNT;
      _presetPrefix = (string)settings[PTZSettings.SETTING_PRESET_PREFIX] ?? DEFAULT_PRESET_PREFIX;

      //Unzoom
      ZoomLevel = 0;
      _motion.MotionGotoCenter();
    }

    #endregion

    #region --- Properties ---

    public UIElement Display
    {
      get { return _ptz;  }
    }

    protected string PresetPointFromAngles //note: only using the pan angle, since there are only 10 preset points in Foscam cameras
    {
      get
      {
        int presetNum = (int)((_maxPanAngle - _minPanAngle) / (_panAngle - _minPanAngle)  / (double)_presetCount);
        return _presetPrefix + presetNum;
      }
    }

    public double PanAngle
    {
      get
      {
        return _panAngle;
      }

      set
      {
        if (_panAngle == value) return;
        _panAngle = value;
        string newPreset = PresetPointFromAngles;
        if (newPreset != _presetPoint)
        {
          _motion.MotionGotoPreset(newPreset);
          _presetPoint = newPreset;
        }
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
        if (_tiltAngle == value) return;
        _tiltAngle = value;
        string newPreset = PresetPointFromAngles;
        if (newPreset != _presetPoint)
        {
          _motion.MotionGotoPreset(newPreset);
          _presetPoint = newPreset;
        }
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
        {
          _zoom.ZoomIn();
          /**/
          Thread.Sleep(600);
          _zoom.ZoomStop(); //don't zoom in too much //TODO: use some option for this
          /**/
        }

        _zoomLevel = value;
      }
    }

    #endregion

  }

}
