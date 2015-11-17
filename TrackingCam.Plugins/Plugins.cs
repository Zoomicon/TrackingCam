﻿//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: Plugins.cs
//Version: 20151117

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace TrackingCam.Plugins.CameraPTZ
{

  public class Plugins
  {

    #region --- Fields ---

    protected static CompositionContainer mefContainer;

    #endregion

    #region --- Methods ---

    public void InitPluginsCatalog()
    {
      AggregateCatalog partsCatalog = new AggregateCatalog();

      //TODO: replace the following code to load plugins from a subfolder, remove specific plugin references from application project and set plugin projects to copy their DLL to a "Plugins" subfolder under the folder where the executable of the app is built
      string[] assemblies = new string[]
      {
        "TrackingCam.Video.Foscam",
        "TrackingCam.Video.KinectV1",
        //
        "TrackingCam.CameraPTZ.Foscam",
        //
        "TrackingCam.Tracking.KinectAudio",
        "TrackingCam.Tracking.Ubisense"
      };

      foreach (string s in assemblies)
        partsCatalog.Catalogs.Add(new AssemblyCatalog(s));

      mefContainer = new CompositionContainer(partsCatalog);
      mefContainer.SatisfyImportsOnce(this);
      //CompositionInitializer.SatisfyImports(this);
    }

    #endregion
  }

}