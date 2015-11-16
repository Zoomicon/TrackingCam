//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: KinectV1Video.cs
//Version: 20151117

using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace TrackingCam.Plugins.Video
{
  //MEF
  [Export("Video", typeof(IVideo))]
  [ExportMetadata("Description", "Kinect v1")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class KinectV1Video : IVideo
  {

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
        throw new NotImplementedException();
      }
    }

    #endregion

    #region --- Methods ---

    public void Start()
    {
      throw new NotImplementedException();
    }

    public void Stop()
    {
      throw new NotImplementedException();
    }

    #endregion
  }

}
