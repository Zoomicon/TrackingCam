//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: FoscamVideoPlugin.cs
//Version: 20151204

using Camera;
using Camera.Foscam;

using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Windows;

namespace TrackingCam.Plugins.Video
{
  //MEF
  [Export("Video", typeof(IVideo))]
  [Export("Video.Foscam", typeof(IVideo))]
  [ExportMetadata("Description", "Foscam IP Camera Video")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class FoscamVideoPlugin : IVideo, IInitializable
  {

    #region --- Constants ---

    public const string SETTING_CAMERA_FOSCAM_HD = "FoscamHD";
    public const string SETTING_CAMERA_FOSCAM_MJPEG = "FoscamMJPEG";

    public readonly string[] DEFAULT_VLC_OPTIONS = new string[]
    {
      "-I dummy",
      "--ignore-config",
      "--no-video-title",
      "--file-logging",
      "--logfile=log.txt",
      "--verbose=2",
      "--no-sub-autodetect-file",
      //"--rtsp-tcp", //needed to pass RTSP through a VPN
      //"--rtsp-frame-buffer-size=500000", //needed to avoid Live555 error when using --rtsp-tcp (RTCPInstance error: Hit limit when reading incoming packet over TCP. Increase "maxRTCPPacketSize")
      "--network-caching=500" //caching value for network resources in msec (needed for low frame lag - if broken frames need to increase it)
    };

    protected const FoscamCameraType DEFAULT_CAMERA_TYPE = FoscamCameraType.FoscamHD;
    protected const string DEFAULT_USERNAME = "admin";
    protected const string DEFAULT_PASSWORD = "admin";

    #endregion

    #region --- Fields ---

    protected IVideoController _video;

    #endregion

    #region --- Initialization ---

    public FoscamVideoPlugin()
    {
    }

    public void Initialize(SettingsBase settings) //throws Exception
    {
      //Get the Foscam camera type (MJPEG or HD)
      FoscamCameraType cameraType;
      string cameraTypeStr = (string)settings[VideoSettings.SETTING_CAMERA_TYPE];
      if (cameraTypeStr == null || cameraTypeStr == "")
        cameraType = DEFAULT_CAMERA_TYPE;
      else
        if (string.Equals(cameraTypeStr, SETTING_CAMERA_FOSCAM_HD, StringComparison.OrdinalIgnoreCase))
        cameraType = FoscamCameraType.FoscamHD;
      else if (string.Equals(cameraTypeStr, SETTING_CAMERA_FOSCAM_MJPEG, StringComparison.OrdinalIgnoreCase))
        cameraType = FoscamCameraType.FoscamMJPEG;
      else
        throw new ArgumentNullException(VideoSettings.SETTING_CAMERA_TYPE);

      //Get the camera URL
      string url = (string)settings[VideoSettings.SETTING_CAMERA_URL];
      if (url == null || url == "")
        throw new ArgumentNullException(VideoSettings.SETTING_CAMERA_URL);

      //Get the username
      string username = (string)settings[VideoSettings.SETTING_CAMERA_USERNAME] ?? DEFAULT_USERNAME;

      //Get the password
      string password = (string)settings[VideoSettings.SETTING_CAMERA_PASSWORD] ?? DEFAULT_PASSWORD;

      //Create video controller
      _video = FoscamVideo.CreateFoscamVideoController(cameraType, url, username, password, DEFAULT_VLC_OPTIONS);
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
        return (_video != null) ? _video.VideoDisplay : null;
      }
    }

    #endregion

    #region --- Methods ---

    public void Start()
    {
      if (_video != null)
        _video.StartVideo();
    }

    public void Stop()
    {
      if (_video != null)
        _video.StopVideo();
    }

    #endregion
  }

}
