using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;

namespace InfiniteNotches
{
    /*
     * This is an example Hollow Knight mod.
     * Mods must be made using .NET Framework 3.5, and should output a (.DLL) class library.
     * Mods must reference the Modding API. If you have already installed the API, this is the Assembly-CSharp file
     * Then, place "using Modding;" in the header to begin your mod.
    */

    public class InfiniteNotches : Mod, ITogglableMod
    {
        internal static InfiniteNotches instance;

        /*
         * Every mod needs an override Initialize() method. This is what the API will call when it loads the mod.
        */
        public override void Initialize()
        {
            instance = this;

            instance.Log("Initializing");

            ModHooks.Instance.GetPlayerIntHook += OverrideNotchCount;
        }

        /*
         * The override GetVersion() method is how the API retrieves the version number it displays for your mod.
        */
        public override string GetVersion()
        {
            return "2000";
        }

        /*
         * This is an example of a method which is used in a hook.
         * We hook the method in Initialize()
         * Then, whenever the game tries to retrieve an int from the player's save data,
         * we can intercept that call and replace it with our own int.
         * (Save data can be accessed in PlayerData.instance)
         * Here, when the game checks the number of charm notches, we always return 2000.
         * Otherwise, we send the call forwards with the GetIntInternal method, which prevents the hook from being called again immediately.
        */
        private static int OverrideNotchCount(string target)
        {
            if (target == nameof(PlayerData.instance.charmSlots))
            {
                return 2000;
            }
            return PlayerData.instance.GetIntInternal(target);
        }

        /*
         * The Unload() method is used by the IToggleableMod interface.
         * When the mod is toggled off in the menu, Unload() is called.
         * We use it here to unhook OverrideNotchCount, so the game can now access the number of notches in the save data.
         * When the mod is toggled on later, it will call Initialize() again, and rehook OverrideNotchCount.
        */

        public void Unload()
        {
            ModHooks.Instance.GetPlayerIntHook -= OverrideNotchCount;
        }

    }
}
