
using MelonLoader;
using HarmonyLib;

using MyBhapticsTactsuit;

//Haptic gun imports
using System.Net.Sockets;
using System;
using System.Text;
using System.IO;

namespace PistolWhip_bhaptics
{
    public class PistolWhip_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;
        public static bool rightGunHasAmmo = true;
        public static bool leftGunHasAmmo = true;
        public static bool reloadHip = true;
        public static bool reloadShoulder = false;
        public static bool reloadTrigger = false;
        public static bool justKilled = false;

        public static TcpClient tcpclnt;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");

            //Haptic Gun connect to Wifi
            try
            {
                tcpclnt = new TcpClient();

                tcpclnt.Connect("192.168.50.236", 23); //23 is your port number. Change this to match the port number you specified in the esp32 code

                if (tcpclnt.Connected)
                {
                    Stream stm = (tcpclnt.GetStream());
                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] ba = asen.GetBytes("0");
                    stm.Write(ba, 0, ba.Length);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Error..... " + err.StackTrace);
            }
        }

        private static void setAmmo(bool hasAmmo, bool isRight)
        {
            if (isRight) { rightGunHasAmmo = hasAmmo; }
            else { leftGunHasAmmo = hasAmmo; }
        }


        private static bool checkIfRightHand(string controllerName)
        {
            if (controllerName.Contains("Right") | controllerName.Contains("right"))
            {
                return true;
            }
            else { return false; }
        }
     
        [HarmonyPatch(typeof(MeleeWeapon), "ProcessHit")]
        public class bhaptics_MeleeHit
        {
            [HarmonyPostfix]
            public static void Postfix(MeleeWeapon __instance)
            {
                bool isRightHand;
                if (checkIfRightHand(__instance.hand.name))
                {
                    isRightHand = true;
                    if (!rightGunHasAmmo) { return; }
                }
                else
                {
                    isRightHand = false;
                    if (!leftGunHasAmmo) { return; }
                }
                tactsuitVr.MeleeRecoil(isRightHand);
            }
        }


        [HarmonyPatch(typeof(Gun), "Fire")]
        public class bhaptics_GunFired
        {
            [HarmonyPostfix]
            public static void Postfix(Gun __instance)
            {
                bool isRightHand;
                if (checkIfRightHand(__instance.hand.name))
                {
                    isRightHand = true;
                    if (!rightGunHasAmmo) { return; }
                }
                else
                {
                    isRightHand = false;
                    if (!leftGunHasAmmo) { return; }
                }

                if (tcpclnt.Connected)
                {
                    Stream stm = (tcpclnt.GetStream());
                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] ba = asen.GetBytes("0");
                    stm.Write(ba, 0, ba.Length);
                }

                if (__instance.gunType == 3) { tactsuitVr.ShotgunRecoil(isRightHand); }
                else { tactsuitVr.GunRecoil(isRightHand); }
            }
        }

        [HarmonyPatch(typeof(Reloader), "SetReloadMethod")]
        public class bhaptics_ReloadMethod
        {
            [HarmonyPostfix]
            public static void Postfix(Reloader __instance, Reloader.ReloadMethod method)
            {
                try
                {
                    if (method.ToString() == "Gesture") { reloadTrigger = false; }
                    else { reloadTrigger = true; }
                }
                catch { return; }
            }
        }


        [HarmonyPatch(typeof(Gun), "Reload")]
        public class bhaptics_GunReload
        {
            [HarmonyPostfix]
            public static void Postfix(Gun __instance, bool triggeredByMelee)
            {
                try
                {
                    if (!__instance.reloadTriggered) { return; }
                    if (triggeredByMelee) { return; }
                }
                catch { return; }
                if (__instance.reloadGestureTypeVar.Value == ESettings_ReloadType.DOWN) { reloadHip = true; reloadShoulder = false; }
                if (__instance.reloadGestureTypeVar.Value == ESettings_ReloadType.UP) { reloadHip = false; reloadShoulder = true; }
                if (__instance.reloadGestureTypeVar.Value == ESettings_ReloadType.BOTH)
                {
                    if ((__instance.player.head.position.y - __instance.hand.position.y) >= 0.3f) { reloadHip = true; reloadShoulder = false; }
                    else { reloadHip = false; reloadShoulder = true; }
                }
                //if (__instance.nextReload >= 5.0f) { return; }
                bool isRightHand;
                if (checkIfRightHand(__instance.hand.name)) { isRightHand = true; }
                else { isRightHand = false; }
                tactsuitVr.GunReload(isRightHand, reloadHip, reloadShoulder, reloadTrigger);
            }
        }


        [HarmonyPatch(typeof(GunAmmoDisplay), "Update")]
        public class bhaptics_GunHasAmmo
        {
            [HarmonyPostfix]
            public static void Postfix(GunAmmoDisplay __instance)
            {
                bool isRightHand;
                bool hasAmmo = true;
                string handName = "";
                int numberBullets = 0;
                try { handName = __instance.gun.hand.name; numberBullets = __instance.currentBulletCount; }
                catch { return; }
                if (checkIfRightHand(handName)) { isRightHand = true; }
                else { isRightHand = false; }
                if (numberBullets == 0) { hasAmmo = false; }
                setAmmo(hasAmmo, isRightHand);
            }
        }


        [HarmonyPatch(typeof(Projectile), "ShowPlayerHitEffects")]
        public class bhaptics_MyPlayerHit
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.HeadShot();
            }
        }

        [HarmonyPatch(typeof(Player), "ProcessKillerHit")]
        public class bhaptics_PlayerKilled
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("ExplosionFace");
                tactsuitVr.PlaybackHaptics("ExplosionFaceFace");
                tactsuitVr.PlaybackHaptics("ExplosionFaceArms");
                tactsuitVr.PlaybackHaptics("ExplosionFaceFeet");
            }
        }

        [HarmonyPatch(typeof(PlayerHUD), "OnArmorLost")]
        public class bhaptics_LoseArmor
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerHUD __instance)
            {
                bool hasArmor = true;
                try { hasArmor = __instance.hasArmor; } catch { return; }
                if (!hasArmor) { tactsuitVr.StartHeartBeat(); justKilled = true; }
                else { tactsuitVr.StopHeartBeat(); }
            }
        }
/*
        [HarmonyPatch(typeof(PlayerHUD), "playArmorGainedEffect")]
        public class bhaptics_GainArmor
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.PlaybackHaptics("Healing");
                justKilled = false;
            }
        }
*/
        [HarmonyPatch(typeof(PlayerHUD), "OnPlayerDeath")]
        public class bhaptics_PlayerDeath
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                if (justKilled)
                {
                    tactsuitVr.PlaybackHaptics("Dying");
                    tactsuitVr.PlaybackHaptics("DyingArms");
                    tactsuitVr.PlaybackHaptics("DyingFeet");
                    justKilled = false;
                }
            }
        }
    }
}
