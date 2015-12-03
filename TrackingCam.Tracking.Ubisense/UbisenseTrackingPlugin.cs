//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: UbisenseTrackingPlugin.cs
//Version: 20151203

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Windows;

using Ubisense.Positioning.WPF;
using Ubisense.UBase;

using TrackingCam.Plugins.Actions;

namespace TrackingCam.Plugins.Tracking
{
  //MEF
  [Export("Tracking", typeof(ITracker))]
  [Export("Tracking.Ubisense", typeof(ITracker))]
  [ExportMetadata("Description", "Ubisense Tracking")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class UbisenseTrackingPlugin : ITracker, IInitializable, IDisplayable, IActionable
  {

    #region --- Constants ---

    public const double DEFAULT_DISTANCE = 0; //if set to 0, will use Z value from ubisence as distance at PositionAngle calculation

    public const double DEFAULT_MIN_X = 0;
    public const double DEFAULT_MAX_X = 6;

    public const double DEFAULT_CAMERA_X = 3;
    public const double DEFAULT_CAMERA_Y = 6;
    public const double DEFAULT_CAMERA_Z = 1;

    #endregion

    #region --- Fields ---

    protected UbisensePositioningUI _positioning; //=null
    protected string _key; //=null
    protected double _distance = DEFAULT_DISTANCE;

    protected double _cameraX = DEFAULT_CAMERA_X;
    protected double _cameraY = DEFAULT_CAMERA_Y;
    protected double _cameraZ = DEFAULT_CAMERA_Z;

    #endregion

    #region --- Initialization ---

    public UbisenseTrackingPlugin()
    {
    }

    public void Initialize(SettingsBase settings) //throws Exception
    {
      _distance = (double?)settings[TrackingSettings.SETTING_TRACKING_DISTANCE] ?? DEFAULT_DISTANCE;

      _key = (string)settings[TrackingSettings.SETTING_TRACKING_OBJECT];

      _cameraX = (double?)settings[TrackingSettings.SETTING_TRACKING_CAMERA_X] ?? DEFAULT_CAMERA_X;
      _cameraY = (double?)settings[TrackingSettings.SETTING_TRACKING_CAMERA_Y] ?? DEFAULT_CAMERA_Y;
      _cameraZ = (double?)settings[TrackingSettings.SETTING_TRACKING_CAMERA_Z] ?? DEFAULT_CAMERA_Z;

      InitializeAsync();
    }

    private void InitializeAsync()
    {
      _positioning = new UbisensePositioningUI();
      _positioning.Positioning.GetObjectsCompleted += UbisensePositioning_GetObjectsCompleted;
      _positioning.Positioning.ButtonPressed += Positioning_ButtonPressed;
      //do not do _positioning.Positioning.GetObjectsAsync(), since UbisensePositioningUI also calls it (would result in objects getting displayed twice at UbisensePositioningUI's list)
    }

    #endregion

    #region --- Properties ---

    public UIElement Display
    {
      get { return _positioning; }
    }

    public double PositionHorizontal
    {
      get
      {
        Position? pos = _positioning.Positioning.GetPosition();
        return (pos.HasValue) ? pos.Value.P.X : 0;
      }
    }

    public double PositionVertical
    {
      get
      {
        Position? pos = _positioning.Positioning.GetPosition();
        return (pos.HasValue) ? pos.Value.P.Y : 0;
      }
    }

    public double PositionDepth
    {
      get
      {
        Position? pos = _positioning.Positioning.GetPosition();
        return (pos.HasValue) ? pos.Value.P.Z : 0;
      }
    }

    public double PositionAngle
    {
      get
      {
        return Math.Atan2(PositionHorizontal - _cameraX, PositionVertical - _cameraY);
      }
    }

    #endregion

    #region --- Events ---

    public event ActionEvents.ActionableEventHandler ActionOccured;

    private void UbisensePositioning_GetObjectsCompleted(object sender, SortedDictionary<string, UObject> objects)
    {
      foreach (var o in objects)
        if (o.Key == _key)
        {
          _positioning.Positioning.SelectedObject = o.Value;
          return;
        }
    }

    private void Positioning_ButtonPressed(object sender, Ubisense.UData.Data.ObjectButtonPressed.RowType? oldRow, Ubisense.UData.Data.ObjectButtonPressed.RowType newRow)
    {
      if (ActionOccured != null)
        ActionOccured(this, newRow.object_.Id.ToString(), newRow.button_.ToString());
    }

    #endregion

  }

}