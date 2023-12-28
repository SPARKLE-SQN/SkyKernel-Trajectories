/*
  Copyright© (c) 2017-2020 S.Gray, (aka PiezPiedPy).

  This file is part of Trajectories.
  Trajectories is available under the terms of GPL-3.0-or-later.
  See the LICENSE.md file for more details.

  Trajectories is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  Trajectories is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

  You should have received a copy of the GNU General Public License
  along with Trajectories.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.IO;
using KSP.Localization;
using KSP.UI.Screens;
using ToolbarControl_NS;
using UnityEngine;
using static Trajectories.RegisterToolbar;


namespace Trajectories
{
    /// <summary>
    /// Handles the creation, destroying etc of an App start button for either KSP's stock toolbar or Blizzy's toolbar.
    /// </summary>
    internal static class AppLauncherButton
    {
        internal const string MODID = "Trajectories_NS";
        internal const string MODNAME = "Trajectories";

        internal const string TRAJ_TEXTURE_PATH = "Trajectories/PluginData/Textures/";


        /// <summary> Toolbar button icon style</summary>
        internal enum IconStyleType
        {
            NORMAL = 0,
            ACTIVE,
            AUTO
        }
        internal static ToolbarControl toolbarControl;


        private static bool constructed = false;

        /// <summary> Current style of the toolbar button icon </summary>
        internal static IconStyleType IconStyle { get; private set; } = IconStyleType.NORMAL;


        /// <summary> Creates the toolbar button for either a KSP stock toolbar or Blizzy toolbar if available. </summary>
        internal static void Start()
        {
            Util.Log("AppLauncherButton.Start, Settings.DisplayTrajectories: " + Settings.DisplayTrajectories);
            Util.DebugLog(constructed ? "Resetting" : "Constructing");

            if (Settings.DisplayTrajectories)
                IconStyle = IconStyleType.ACTIVE;
            else
                IconStyle = IconStyleType.NORMAL;

            if (toolbarControl == null)
            {
                GameObject g1 = new GameObject("g1");

                toolbarControl = g1.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(null, null,
                    ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW,
                    MODID,
                    "TrajectoriesButton",
                    TRAJ_TEXTURE_PATH + "iconAuto",
                    TRAJ_TEXTURE_PATH + "iconActive",
                    TRAJ_TEXTURE_PATH + "icon-blizzy",
                    TRAJ_TEXTURE_PATH + "icon-blizzy",
                    MODNAME
                );
                toolbarControl.AddLeftRightClickCallbacks(OnLeftToggle, OnRightToggle);
            }
            ChangeIcon(IconStyle);
        }
        /// <summary> Releases held resources. </summary>
        internal static void Destroy()
        {
            Util.DebugLog("");
            DestroyToolbarButton();
        }

        internal static void DestroyToolbarButton()
        {
            Util.DebugLog("");
            IconStyle = IconStyleType.NORMAL;
        }

        internal static void OnLeftToggle()
        {
            LGG_Log.Info("OnLeftToggle");
            if (Settings.SwapLeftRightClicks)
                OnMainGUIToggle();
            else
                OnTrajectoryToggle();
        }
        internal static void OnRightToggle()
        {
            LGG_Log.Info("OnRightToggle");

            if (Settings.SwapLeftRightClicks)
                OnTrajectoryToggle();
            else
                OnMainGUIToggle();
        }
        internal static void OnTrajectoryToggle()
        {
            LGG_Log.Info("OnTrajectoryToggle");
            // check that we have patched conics. If not, apologize to the user and return.
            if (!Util.IsPatchedConicsAvailable)
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_Trajectories_ConicsErr"));
                Settings.DisplayTrajectories = false;
                return;
            }

            Settings.DisplayTrajectories = !Settings.DisplayTrajectories;
            MainGUI.OnButtonClick_DisplayTrajectories(Settings.DisplayTrajectories);

        }
        internal static void OnMainGUIToggle()
        {
            LGG_Log.Info("OnMainGUItoggle");
            Settings.MainGUIEnabled = !Settings.MainGUIEnabled;
        }

        /// <summary> Changes the toolbar button icon </summary>
        internal static void ChangeIcon(IconStyleType iconstyle)
        {
            LGG_Log.Info("ChangeIcon, iconstyle: " + iconstyle.ToString());
            string icon = "";

            switch (iconstyle)
            {
                case IconStyleType.ACTIVE:
                    icon = TRAJ_TEXTURE_PATH + "iconActive";
                    IconStyle = IconStyleType.ACTIVE;
                    break;
                case IconStyleType.AUTO:
                    icon = TRAJ_TEXTURE_PATH + "iconAuto";
                    IconStyle = IconStyleType.AUTO;
                    break;
                default:
                    icon = TRAJ_TEXTURE_PATH + "icon";
                    IconStyle = IconStyleType.NORMAL;
                    break;
            }

            toolbarControl.SetTexture(icon, TRAJ_TEXTURE_PATH + "icon-blizzy");
        }
    }
}
