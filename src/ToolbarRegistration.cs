using UnityEngine;
using ToolbarControl_NS;
using KSP_Log;

namespace Trajectories
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {

        static public KSP_Log.Log LGG_Log = null;
        void Start()
        {
            ToolbarControl.RegisterMod(AppLauncherButton.MODID, AppLauncherButton.MODNAME);

#if DEBUG
            LGG_Log = new KSP_Log.Log("Trajectories", KSP_Log.Log.LEVEL.INFO);
#else
            LGG_Log = new KSP_Log.Log("Trajectories", KSP_Log.Log.LEVEL.ERROR);
#endif
        }
    }
}
