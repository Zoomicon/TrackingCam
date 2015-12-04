//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Tracking.cs
//Version: 20151204

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using TrackingCam.Plugins;
using TrackingCam.Plugins.Actions;
using TrackingCam.Plugins.Tracking;
using TrackingCam.Properties;

namespace TrackingCam
{

  public partial class MainWindow : Window
  {

    #region --- Fields ---

    protected IActionable actionableKinectGestures;
    protected IActionable actionableUbisense;

    protected ITracker trackerKinectAudio;
    protected ITracker trackerKinectDepth;
    protected ITracker trackerUbisense;

    protected BackgroundWorker presenterTracker;

    #endregion

    #region --- Properties

    public bool TrackingPresenter
    {
      get { return (presenterTracker != null) && !presenterTracker.IsBusy; }
      set { if (value) StartTrackingPresenter(); else StopTrackingPresenter(); } //StartTrackingPresenter will check if it is already tracking the presenter and do nothing
    }


    #endregion

    #region --- Methods ---

    public ITracker LoadTrackingPlugin(string protocol)
    {
      Lazy<ITracker> plugin = PluginsCatalog.mefContainer.GetExports<ITracker>(protocol).FirstOrDefault(); //TODO: change this to select from app settings which tracking plugin to use instead of just using the 1st one found
      ITracker tracker = (plugin != null) ? plugin.Value : null;

      try
      {
        (tracker as IInitializable)?.Initialize(Settings.Default);
      }
      catch (Exception e)
      {
        tracker = null;
        MessageBox.Show((e.InnerException ?? e).Message);
      }
      return tracker;
    }

    public void LoadKinectAudioTrackingPlugin()
    {
      trackerKinectAudio = LoadTrackingPlugin("Tracking.KinectAudio");
    }

    public void LoadKinectDepthTrackingPlugin()
    {
      trackerKinectDepth = LoadTrackingPlugin("Tracking.KinectV1Depth");

      actionableKinectGestures = trackerKinectDepth as IActionable;
      if (actionableKinectGestures != null)
        actionableKinectGestures.ActionOccured += ActionableKinectGestures_ActionOccured;
    }

    public void LoadUbisenseTrackingPlugin()
    {
      trackerUbisense = LoadTrackingPlugin("Tracking.Ubisense");

      actionableUbisense = trackerUbisense as IActionable;
      if (actionableUbisense != null)
        actionableUbisense.ActionOccured += ActionableUbisense_ActionOccured;
    }

    public void LoadTrackingPlugins()
    {
      LoadKinectAudioTrackingPlugin();
      LoadKinectDepthTrackingPlugin();
      LoadUbisenseTrackingPlugin();
    }

    #region Presenter Tracking

    public void StartTrackingPresenter()
    {
      if (TrackingPresenter) return; //check if already tracking the presenter and do nothing

      presenterTracker = new BackgroundWorker() { WorkerSupportsCancellation = true };
      presenterTracker.DoWork += (s, e) =>
      {
        while (!e.Cancel)
        {
          ITracker tracker = null;

          if (trackerKinectDepth != null)
            tracker = trackerKinectDepth;
          else if (trackerKinectAudio != null)
            tracker = trackerKinectAudio;
          else if (trackerUbisense != null)
            tracker = trackerUbisense;

          if (tracker != null)
            LookTo(tracker.PositionAngle); //look to presenter
        }
      };
      presenterTracker.RunWorkerAsync();
    }

    public void StopTrackingPresenter()
    {
      presenterTracker.CancelAsync();
    }

    #endregion

    #endregion

    #region --- Events ---

    private void ActionableKinectGestures_ActionOccured(object sender, string id, string action)
    {
      switch (action)
      {
        case "ZoomIn":
          if (ptz != null)
            ptz.ZoomLevel = 1;
          break;
        case "ZoomOut":
          if (ptz != null)
            ptz.ZoomLevel = 0;
          break;
        case "SwipeLeft":
          if (ptz != null)
            ptz.PanAngle -= 10;
          break;
        case "SwipeRight":
          if (ptz != null)
            ptz.PanAngle += 10;
          break;
      }
    }

    private void ActionableUbisense_ActionOccured(object sender, string id, string action)
    {
      switch (action)
      {
        case "1":
          if (ptz != null)
            ptz.ZoomLevel = 1;
          break;
        case "2":
          if (ptz != null)
            ptz.ZoomLevel = 0;
          break;
      }
    }

    #endregion

  }
}
