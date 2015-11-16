//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: FoscamVideo.cs
//Version: 20151117

using Camera;
using Camera.Foscam;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace TrackingCam.Plugins.Video
{
  //MEF
  [Export("Video", typeof(IVideo))]
  [ExportMetadata("Description", "Foscam IP Cameras")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class FoscamVideo : IVideo, IInitializable
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

    public FoscamVideo()
    {
    }

    public void Initialize(Dictionary<string, string> settings) //throws Exception
    {
      FoscamCameraType cameraType;
      string _cameraType;
      if (!settings.TryGetValue(VideoSettings.SETTING_CAMERA_TYPE, out _cameraType))
        cameraType = DEFAULT_CAMERA_TYPE;
      else
        if (string.Equals(_cameraType, SETTING_CAMERA_FOSCAM_HD, StringComparison.OrdinalIgnoreCase))
        cameraType = FoscamCameraType.FoscamHD;
      else if (string.Equals(_cameraType, SETTING_CAMERA_FOSCAM_MJPEG, StringComparison.OrdinalIgnoreCase))
        cameraType = FoscamCameraType.FoscamMJPEG;
      else
        throw new ArgumentNullException(VideoSettings.SETTING_CAMERA_TYPE);

      string url;
      if (!settings.TryGetValue(VideoSettings.SETTING_CAMERA_URL, out url))
        throw new ArgumentNullException(VideoSettings.SETTING_CAMERA_URL);

      string username;
      if (!settings.TryGetValue(VideoSettings.SETTING_CAMERA_USERNAME, out username))
        username = DEFAULT_USERNAME;

      string password;
      if (!settings.TryGetValue(VideoSettings.SETTING_CAMERA_PASSWORD, out password))
        username = DEFAULT_PASSWORD;

      _video = Camera.Foscam.FoscamVideo.CreateFoscamVideoController(cameraType, url, username, password);
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
        return _video.VideoDisplay;
      }
    }

    #endregion

    #region --- Methods ---

    public void Start()
    {
      _video.StartVideo();
    }

    public void Stop()
    {
      _video.StopVideo();
    }

    #endregion
  }

}
