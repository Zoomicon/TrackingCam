//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Tracking.cs
//Version: 20151210

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

    protected IActionable actionableKinectGestures; //=null
    protected IActionable actionableUbisense; //=null

    protected ITracker tracker; //=null
    protected ITracker trackerKinectAudio; //=null
    protected ITracker trackerKinectDepth; //=null
    protected ITracker trackerUbisense; //=null

    protected BackgroundWorker presenterTracker; //=null

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

      if (!Settings.Default.TrackingPresenter) //must check this to avoid infinite loop
        Settings.Default.TrackingPresenter = true;

      presenterTracker = new BackgroundWorker() { WorkerSupportsCancellation = true };
      presenterTracker.DoWork += (s, e) =>
      {
        while (!e.Cancel)
          if (tracker != null)
            LookTo(tracker.PositionAngle); //look to presenter
      };
      presenterTracker.RunWorkerAsync();
    }

    public void StopTrackingPresenter()
    {
      if (TrackingPresenter)
      {
        if (Settings.Default.TrackingPresenter) //must check this to avoid infinite loop
          Settings.Default.TrackingPresenter = false;
        presenterTracker.CancelAsync();
      }
    }

    #endregion

    #region Settings

    public void ApplySettings_Tracking()
    {
      SetTrackerFromSettings(); //do first
      SetTrackingPresenterFromSettings();
    }

    public void SetTrackerFromSettings()
    {
      switch (Settings.Default.Tracker)
      {
        case TrackingSettings.SETTING_TRACKER_VALUE_KINECT_DEPTH:
          tracker = trackerKinectDepth;
          break;
        case TrackingSettings.SETTING_TRACKER_VALUE_KINECT_AUDIO:
          tracker = trackerKinectAudio;
          break;
        case TrackingSettings.SETTING_TRACKER_VALUE_UBISENSE:
          tracker = trackerUbisense;
          break;
        default:
          tracker = null;
          break;
      }
    }

    public void SetTrackingPresenterFromSettings()
    {
      TrackingPresenter = Settings.Default.TrackingPresenter;
    }

    #endregion

    #endregion

    #region --- Events ---

    private void ActionableKinectGestures_ActionOccured(object sender, string id, string action)
    {
      switch (action)
      {
        case KinectV1DepthTrackingPlugin.ACTION_ZOOM_IN:
          if (ptz != null)
            ptz.ZoomLevel = 1;
          break;
        case KinectV1DepthTrackingPlugin.ACTION_ZOOM_OUT:
          if (ptz != null)
            ptz.ZoomLevel = 0;
          break;
        case KinectV1DepthTrackingPlugin.ACTION_SWIPE_LEFT:
          if (ptz != null)
            ptz.PanAngle -= 10;
          break;
        case KinectV1DepthTrackingPlugin.ACTION_SWIPE_RIGHT:
          if (ptz != null)
            ptz.PanAngle += 10;
          break;
      }
    }

    private void ActionableUbisense_ActionOccured(object sender, string id, string action)
    {
      switch (action)
      {
        case UbisenseTrackingPlugin.ACTION_BUTTON_1:
          if (ptz != null)
            ptz.ZoomLevel = 1; //Zoom in
          break;
        case UbisenseTrackingPlugin.ACTION_BUTTON_2:
          if (ptz != null)
            ptz.ZoomLevel = 0; //Zoom out
          break;
      }
    }

    #endregion

  }
}
