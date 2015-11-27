//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: FoscamVideoPlugin.cs
//Version: 20151127

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
      _video = FoscamVideo.CreateFoscamVideoController(cameraType, url, username, password);
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
