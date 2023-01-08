using System;

using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;

//Haptic gun imports
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Linq;

namespace GunClubVR_bhaptics
{
    public class GunClubVR_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;
        public static bool isRightHanded = true;

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
            if (tcpclntRight.Connected)
            {
                Stream stm = (tcpclntRight.GetStream());
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes("a");
                stm.Write(ba, 0, ba.Length);
            }
        }

        public static void createGunHapticFeedbackLeft()
        {
            if (tcpclntLeft.Connected)
            {
                Stream stm = (tcpclntLeft.GetStream());
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes("a");
                stm.Write(ba, 0, ba.Length);
            }
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

                //hapticGun feedback
                if (isRightHanded)
                {
                    GunClubVR_bhaptics.createGunHapticFeedbackRight();
                }
                else
                {
                    GunClubVR_bhaptics.createGunHapticFeedbackLeft();
                }

                //bHaptics feedback
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


        [HarmonyPatch(typeof(MenuInterface), "SetLeftHanded", new Type[] { })]
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
