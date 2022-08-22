using System;
using System.Collections.Generic;
using System.Text;

namespace HalfLifeAlyxEventDetector
{
    class HalfLifeAlyx_Autoexec
    {
        Dictionary<string, int> CheatTable = new Dictionary<string, int>();
        /// <summary>
        /// Bottomless mag. Guns need no ammo or mags to fire.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="IsOn">Turn On/Off this feature</param>
        public HalfLifeAlyx_Autoexec BottomlessMagazine(bool IsOn)
        {
            CheatTable["sv_infinite_ammo"] = IsOn ? 1 : 0;
            return this;
        }

        /// <summary>
        /// Grab unlimited mags and shells out of your backpack. Grabbed mags use no ammo from your inventory.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="IsOn">Turn On/Off this feature</param>
        public HalfLifeAlyx_Autoexec UnlimitedMagazineInBag(bool IsOn)
        {
            CheatTable["sv_infinite_clips"] = IsOn ? 1 : 0;
            return this;
        }
        /// <summary>
        /// Gives you all weapons, 30 pistol ammo, 30 smg ammo, 6 shotgun shells, 20 resin, gravity gloves, multi-tool, and skips the tutorial prompts.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="IsOn">Turn On/Off this feature</param>
        public HalfLifeAlyx_Autoexec GiveBasicWeapons(bool IsOn)
        {
            CheatTable["impulse"] = 101;
            return this;
        }
        /// <summary>
        /// Automatically unlocks all gun upgrades.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="IsOn">Turn On/Off this feature</param>
        public HalfLifeAlyx_Autoexec GiveAllUnlockedWeapons(bool IsOn)
        {
            CheatTable["impulse"] = 102;
            return this;
        }
        /// <summary>
        /// Enables/Disables live lights. (Such as the flashlight).
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="IsOn">Turn On/Off this feature</param>
        public HalfLifeAlyx_Autoexec VREnableLight(bool IsOn)
        {
            CheatTable["vr_enable_lights"] = IsOn ? 1 : 0;
            return this;
        }
        /// <summary>
        /// Enables/Disables the Skybox. Makes the sky black in most areas.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="IsOn">Turn On/Off this feature</param>
        public HalfLifeAlyx_Autoexec DrawSkybox(bool IsOn)
        {
            CheatTable["r_drawskybox"] = IsOn ? 1 : 0;
            return this;
        }
        /// <summary>
        /// Shows an FPS Counter on the desktop.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="IsOn">Turn On/Off this feature</param>
        public HalfLifeAlyx_Autoexec ShowFPS(bool IsOn)
        {
            CheatTable["cl_showfps"] = IsOn ? 1 : 0;
            return this;
        }
        /// <summary>
        /// Enables/Disables volumetric fog.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="IsOn">Turn On/Off this feature</param>
        public HalfLifeAlyx_Autoexec EnableVolumeFog(bool IsOn)
        {
            CheatTable["vr_enable_volume_fog"] = IsOn ? 1 : 0;
            return this;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("sv_cheats 1\ncl_net_showevents 1\n");
            foreach (var KeyName in CheatTable.Keys)
            {
                stringBuilder.Append($"{KeyName} {CheatTable[KeyName]}\n");
            }
            return stringBuilder.ToString();
        }

    }
}
