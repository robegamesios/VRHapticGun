using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;

namespace GunClubVR_bhaptics
{
    public class GunClubVR_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;
        public static bool isRightHanded = true;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }

        [HarmonyPatch(typeof(TBMVRDevice), "FireHaptics", new Type[] { typeof(Side), typeof(VibrationForce) })]
        public class bhaptics_FireHaptics
        {
            [HarmonyPostfix]
            public static void Postfix(TBMVRDevice __instance, Side handSide, VibrationForce force)
            {
                if ((force == VibrationForce.Light) | (force == VibrationForce.None)) return;
                float intensity = 1.0f;
                if (force == VibrationForce.Medium) { intensity = 0.7f; }
                tactsuitVr.Recoil("Pistol", (handSide == Side.Right), intensity, isRightHanded);
            }
        }
        
        [HarmonyPatch(typeof(DestructableHandler), "DamageSurroundings", new Type[] { typeof(SpawnedRangeHandler), typeof(UnityEngine.Transform), typeof(int), typeof(int) })]
        public class bhaptics_ExplosionDamage
        {
            [HarmonyPostfix]
            public static void Postfix(DestructableHandler __instance)
            {
                tactsuitVr.PlaybackHaptics("ExplosionBelly");
            }
        }

        
        [HarmonyPatch(typeof(MenuInterface), "SetLeftHanded", new Type[] {  })]
        public class bhaptics_LeftHanded
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                isRightHanded = false;
            }
        }

        [HarmonyPatch(typeof(MenuInterface), "SetRightHanded", new Type[] { })]
        public class bhaptics_RightHanded
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                isRightHanded = true;
            }
        }

        [HarmonyPatch(typeof(WeaponHandednessSwapper), "SetTargetHandedness", new Type[] { typeof(bool) })]
        public class bhaptics_SwapHandedness
        {
            [HarmonyPostfix]
            public static void Postfix(bool rightHanded)
            {
                isRightHanded = rightHanded;
            }
        }

        [HarmonyPatch(typeof(AudioHandler), "PlaySoundAtPoint", new Type[] { typeof(UnityEngine.Vector3), typeof(UnityEngine.AudioClip) })]
        public class bhaptics_PlaySound6
        {
            [HarmonyPostfix]
            public static void Postfix(UnityEngine.AudioClip soundClip)
            {
                // aUILevelUpExplosion
                // 
                if (soundClip.name.Contains("Hurt")) tactsuitVr.PlaybackHaptics("Impact");
                //tactsuitVr.LOG("Audio6 " + soundClip.name);
            }
        }
        
    }
}
