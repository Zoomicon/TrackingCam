//Project: TrackingCam (http://TrackingCam.codeplex.com)
//File: MainWindow.cs
//Version: 20151117

using System.Windows;
using TrackingCam.Plugins;

namespace TrackingCam
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    #region --- Initialization ---

    public MainWindow()
    {
      InitializeComponent();
      PluginsCatalog.Init(this);
      LoadPlugins();
    }

    #endregion
  }

}
