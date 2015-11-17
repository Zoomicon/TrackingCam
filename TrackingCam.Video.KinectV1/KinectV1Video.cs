//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: KinectV1Video.cs
//Version: 20151117

using System;
using System.ComponentModel.Composition;
using System.Windows;

using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Controls;

namespace TrackingCam.Plugins.Video
{
  //MEF
  [Export("Video", typeof(IVideo))]
  [ExportMetadata("Description", "Kinect v1")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class KinectV1Video : IVideo
  {

    #region --- Fields ---

    protected KinectViewer _kinectViewer;

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
        if (_kinectViewer == null)
          _kinectViewer = new KinectViewer() { FrameType = VisualizationMode.Color };

        return _kinectViewer;
      }
    }

    #endregion

    #region --- Methods ---

    public void Start()
    {
      //NOP
    }

    public void Stop()
    {
      //NOP
    }

    #endregion
  }

}
