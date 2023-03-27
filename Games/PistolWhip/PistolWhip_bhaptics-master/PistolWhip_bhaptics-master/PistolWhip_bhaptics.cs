
using MelonLoader;
using HarmonyLib;

using MyBhapticsTactsuit;

//Haptic gun imports
using System.Net.Sockets;
using System;
using System.Text;
using System.IO;
using System.Linq;

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

        public static TcpClient tcpclntRight;
        public static TcpClient tcpclntLeft;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");

            //Read the hapticGunConfig.txt file to get the ip address and port number settings
            string basePath = MelonUtils.BaseDirectory;
            string path = basePath + "\\Mods\\hapticGunConfig.txt";

            //Right haptic gun
            string ipAddressRight = File.ReadLines(path).First();

            //Port number
            string port = File.ReadLines(path).ElementAt(1);
            int portNumber = Int32.Parse(port);

            //Left haptic gun
            string ipAddressLeft = File.ReadLines(path).Last();

            //Haptic Gun connect to Wifi
            if (ipAddressRight != null)
            {
                try
                {
                    tcpclntRight = new TcpClient();

                    tcpclntRight.Connect(ipAddressRight, portNumber); //23 is your port number. Change this to match the port number you specified in the esp32 code

                    if (tcpclntRight.Connected)
                    {
                        Console.WriteLine("Right Haptic Gun Connected to: " + path + " " + ipAddressRight + " " + portNumber);
                        createGunHapticFeedbackRight();
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine("Error Right Haptic Gun..... " + err.StackTrace);
                }
            }

            if (ipAddressLeft != null)
            {
                try
                {
                    tcpclntLeft = new TcpClient();

                    tcpclntLeft.Connect(ipAddressLeft, portNumber); //23 is your port number. Change this to match the port number you specified in the esp32 code

                    if (tcpclntLeft.Connected)
                    {
                        Console.WriteLine("Left Haptic Gun Connected to: " + path + " " + ipAddressLeft + " " + portNumber);
                        createGunHapticFeedbackLeft();
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine("Error Left Haptic Gun..... " + err.StackTrace);
                }
            }
        }

        //hapticGun feedback
        public static void createGunHapticFeedbackRight()
        {
            if (tcpclntRight != null && tcpclntRight.Connected)
            {
                Stream stm = (tcpclntRight.GetStream());
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes("a");
                stm.Write(ba, 0, ba.Length);
            }
        }

        public static void createGunHapticFeedbackLeft()
        {
            if (tcpclntLeft != null && tcpclntLeft.Connected)
            {
                Stream stm = (tcpclntLeft.GetStream());
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes("a");
                stm.Write(ba, 0, ba.Length);
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
                    if (!rightGunHasAmmo) 
                    { 
                        return; 
                    } else 
                    {
                        PistolWhip_bhaptics.createGunHapticFeedbackRight();
                    }
                }
                else
                {
                    isRightHand = false;
                    if (!leftGunHasAmmo) { 
                        return; 
                    } else
                    {
                        PistolWhip_bhaptics.createGunHapticFeedbackLeft();
                    }
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
