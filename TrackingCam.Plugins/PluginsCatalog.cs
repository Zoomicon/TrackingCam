//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: PluginsCatalog.cs
//Version: 20151203

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace TrackingCam.Plugins
{

  public static class PluginsCatalog
  {

    #region --- Fields ---

    public static CompositionContainer mefContainer;

    #endregion

    #region --- Methods ---

    public static void Init(object attributedPart)
    {
      if (mefContainer != null) return;

      AggregateCatalog partsCatalog = new AggregateCatalog();

      //TODO: replace the following code to load plugins from a subfolder, remove specific plugin references from application project and set plugin projects to copy their DLL to a "Plugins" subfolder under the folder where the executable of the app is built
      string[] assemblies = new string[]
      {
        "TrackingCam.Video.KinectV1.dll",
        "TrackingCam.Video.Foscam.dll",
        //
        "TrackingCam.PTZ.Foscam.dll",
        //
        "TrackingCam.Tracking.KinectAudio.dll",
        "TrackingCam.Tracking.Ubisense.dll",
        //
        "SpeechLib.Synthesis.dll",
        "SpeechLib.Recognition.dll",
        "SpeechLib.Recognition.KinectV1.dll"
      };

      foreach (string s in assemblies)
        partsCatalog.Catalogs.Add(new AssemblyCatalog(s));

      mefContainer = new CompositionContainer(partsCatalog);
      mefContainer.SatisfyImportsOnce(attributedPart);
      //CompositionInitializer.SatisfyImports(attributedPart);
    }

    #endregion
  }

}
