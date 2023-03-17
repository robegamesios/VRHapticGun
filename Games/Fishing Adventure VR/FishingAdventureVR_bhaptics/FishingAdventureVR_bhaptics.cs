using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;
using System.IO;
using System.Net.Sockets;

namespace FishingAdventureVR_bhaptics
{
    public class FishingAdventureVR_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;
        public static bool footStepRight = true;

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

        [HarmonyPatch(typeof(MainPlayer), "ThrowRod", new Type[] { })]
        public class bhaptics_ThrowRod
        {
            [HarmonyPostfix]
            public static void Postfix(MainPlayer __instance)
            {
                if (__instance.handRight == true)
                {
                    tactsuitVr.PlaybackHaptics("Healing");
                }

            }
        }

        [HarmonyPatch(typeof(MainPlayer), "StartThrowRod", new Type[] { typeof(float) })]
        public class bhaptics_StartThrowRod
        {
            [HarmonyPostfix]
            public static void Postfix(MainPlayer __instance)
            {
                if (__instance.handRight == true)
                {
                    tactsuitVr.PlaybackHaptics("Healing");
                }

            }
        }
    }
}
