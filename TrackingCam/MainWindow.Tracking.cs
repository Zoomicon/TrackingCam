//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.Tracking.cs
//Version: 20151202

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using TrackingCam.Plugins;
using TrackingCam.Plugins.Tracking;
using TrackingCam.Properties;

namespace TrackingCam
{

  public partial class MainWindow : Window
  {

    #region --- Fields ---

    protected ITracker trackerKinectAudio;
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

    public void LoadUbisenseTrackingPlugin()
    {
      trackerUbisense = LoadTrackingPlugin("Tracking.Ubisense");
    }

    public void LoadTrackingPlugins()
    {
      LoadKinectAudioTrackingPlugin();
      LoadUbisenseTrackingPlugin();
    }

    #region Presenter Tracking

    public void StartTrackingPresenter()
    {
      if (TrackingPresenter) return; //check if already tracking the presenter and do nothing

      presenterTracker = new BackgroundWorker();
      presenterTracker.DoWork += (s, e) =>
      {
        while (!e.Cancel)
          LookToPresenter();
      };
      presenterTracker.RunWorkerAsync();
    }

    public void StopTrackingPresenter()
    {
      presenterTracker.CancelAsync();
    }

    #endregion

    #endregion

  }
}
