//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: UbisenseTrackingPlugin.cs
//Version: 20151124

using System.Collections.Generic;
using System.ComponentModel.Composition;

using Ubisense.Positioning;
using Ubisense.UBase;

namespace TrackingCam.Plugins.Tracking
{
  //MEF
  [Export("Tracking", typeof(ITracker))]
  [Export("Tracking.Ubisense", typeof(ITracker))]
  [ExportMetadata("Description", "Ubisense Tracking")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class UbisenseTrackingPlugin : ITracker, IInitializable
  {

    #region --- Fields ---

    protected UbisensePositioning _positioning; //=null
    protected string _key; //=null

    #endregion

    #region --- Initialization ---

    public UbisenseTrackingPlugin()
    {
    }

    public void Initialize(Dictionary<string, string> settings) //throws Exception
    {
      settings.TryGetValue(TrackingSettings.SETTING_TRACKING_OBJECT_KEY, out _key);
      InitializeAsync();
    }

    private void InitializeAsync()
    {
      _positioning = new UbisensePositioning();
      _positioning.GetObjectsCompleted += UbisensePositioning_GetObjectsCompleted;
      _positioning.GetObjectsAsync(); //getting objects may take some time
    }

    #endregion

    #region --- Properties ---

    public double PositionHorizontal
    {
      get
      {
        Position? pos = _positioning.GetPosition();
        return (pos.HasValue) ? pos.Value.P.X : 0;
      }
    }

    public double PositionVertical
    {
      get
      {
        Position? pos = _positioning.GetPosition();
        return (pos.HasValue) ? pos.Value.P.Y : 0;
      }
    }

    public double PositionDepth
    {
      get
      {
        Position? pos = _positioning.GetPosition();
        return (pos.HasValue) ? pos.Value.P.Z : 0;
      }
    }

    #endregion

    #region --- Events ---

    private void UbisensePositioning_GetObjectsCompleted(object sender, SortedDictionary<string, UObject> objects)
    {
      foreach (var o in objects)
        if (o.Key == _key)
          _positioning.SelectedObject = o.Value;
    }

    #endregion

  }

}