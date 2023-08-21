
using UnityEngine;
using ToolbarControl_NS;

namespace Trajectories
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(AppLauncherButton.MODID, AppLauncherButton.MODNAME);
        }
    }
}
