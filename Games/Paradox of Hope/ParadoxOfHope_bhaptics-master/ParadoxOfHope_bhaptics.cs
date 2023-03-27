using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;
using BNG;
using EmeraldAI;
using UnityEngine;
using Knife.RealBlood.SimpleController;

//Haptic gun imports
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Linq;

namespace ParadoxOfHope_bhaptics
{
    public class ParadoxOfHope_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;
        public static bool isRightHanded = true;

        public static TcpClient tcpclntRight;
        public static TcpClient tcpclntLeft;
 
        public override void OnInitializeMelon()
        {
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

        [HarmonyPatch(typeof(RaycastWeapon), "Shoot", new Type[] {  })]
        public class bhaptics_ShootWeapon
        {
            [HarmonyPostfix]
            public static void Postfix(RaycastWeapon __instance)
            {
                if (!__instance.readyToShoot) return;
                if (!__instance.BulletInChamber) return;

                bool isRight = (__instance.thisGrabber.HandSide == ControllerHand.Right);
 
                //hapticGun feedback
                if (__instance.PistolSecondHand)
                {
                    ParadoxOfHope_bhaptics.createGunHapticFeedbackRight();
                    ParadoxOfHope_bhaptics.createGunHapticFeedbackLeft();
                }
                else if (isRight)
                {
                    ParadoxOfHope_bhaptics.createGunHapticFeedbackRight();
                }
                else
                {
                    ParadoxOfHope_bhaptics.createGunHapticFeedbackLeft();
                }

                //bHaptics feedback
                tactsuitVr.GunRecoil(isRight);
            }
        }

        [HarmonyPatch(typeof(PlayerBase), "DamageTake", new Type[] { typeof(int) })]
        public class bhaptics_TakeDamage
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("Impact");
            }
        }

        [HarmonyPatch(typeof(PlayerBase), "Healing", new Type[] {  })]
        public class bhaptics_Healing
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("Healing");
            }
        }

        [HarmonyPatch(typeof(PlayerBase), "Healing2", new Type[] {  })]
        public class bhaptics_Healing2
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("Healing");
            }
        }

        [HarmonyPatch(typeof(PlayerBase), "AdrenalineShot", new Type[] { })]
        public class bhaptics_AdrenalineShot
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("Healing");
            }
        }

        [HarmonyPatch(typeof(PlayerBase), "StrangeSound", new Type[] { })]
        public class bhaptics_StrangeSound
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("NeckTingle");
            }
        }


        private static (float, float) getAngleAndShift(Transform player, Vector3 hit)
        {
            Vector3 patternOrigin = new Vector3(0f, 0f, 1f);
            // y is "up", z is "forward" in local coordinates
            Vector3 hitPosition = hit - player.position;
            Quaternion PlayerRotation = player.rotation;
            Vector3 playerDir = PlayerRotation.eulerAngles;
            // We only want rotation correction in y direction (left-right), top-bottom and yaw we can leave
            Vector3 flattenedHit = new Vector3(hitPosition.x, 0f, hitPosition.z);
            float earlyhitAngle = Vector3.Angle(flattenedHit, patternOrigin);
            Vector3 earlycrossProduct = Vector3.Cross(flattenedHit, patternOrigin);
            if (earlycrossProduct.y > 0f) { earlyhitAngle *= -1f; }
            float myRotation = earlyhitAngle - playerDir.y;
            myRotation *= -1f;
            if (myRotation < 0f) { myRotation = 360f + myRotation; }

            float hitShift = hitPosition.y;
            tactsuitVr.LOG("hitShift: " + hitShift.ToString());
            float upperBound = 0.0f;
            float lowerBound = -0.5f;
            if (hitShift > upperBound) { hitShift = 0.5f; }
            else if (hitShift < lowerBound) { hitShift = -0.5f; }
            // ...and then spread/shift it to [-0.5, 0.5]
            else { hitShift = (hitShift - lowerBound) / (upperBound - lowerBound) - 0.5f; }

            return (myRotation, hitShift);
        }


        /*
        [HarmonyPatch(typeof(PlayerBase), "OnTriggerEnter", new Type[] { typeof(Collider) })]
        public class bhaptics_TriggerEnter
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerBase __instance, Collider other)
            {
                //tactsuitVr.LOG("Collider: " + __instance.transform.position.ToString() + " " + other.transform.position.ToString());
                if (other.name != "PlayerController") return;
                float hitAngle;
                float hitShift;
                //tactsuitVr.LOG("Other: " + other.isTrigger.ToString() + " " + other.name + " " + __instance.name + " " + (__instance == __instance.Player).ToString());
                (hitAngle, hitShift) = getAngleAndShift(__instance.Player.transform, other.transform.position);
                if ((hitShift >= 0.5f) || (hitShift <= -0.5f)) return;
                tactsuitVr.PlayBackHit("Bump", hitAngle, hitShift);

            }
        }
        */

        [HarmonyPatch(typeof(BackpackAutoSorting), "SnapLoot", new Type[] { })]
        public class bhaptics_SnapLoot
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerBase __instance)
            {
                string pattern = "StoreShoulder";
                if (isRightHanded) pattern += "_L";
                else pattern += "_R";
                tactsuitVr.PlaybackHaptics(pattern);
            }
        }

        [HarmonyPatch(typeof(SnapZone), "GrabEquipped", new Type[] { typeof(Grabber) })]
        public class bhaptics_GrabEquipped
        {
            [HarmonyPostfix]
            public static void Postfix(SnapZone __instance, Grabber grabber)
            {
                if (__instance == null) return;
                string pattern = "";
                string leftSuffix = "_L";
                string rightSuffix = "_R";
                if (!isRightHanded) { leftSuffix = "_R"; rightSuffix = "_L"; }
                switch (__instance.name)
                {
                    case "KnifeHolster":
                        pattern = "HolsterChest" + leftSuffix;
                        break;
                    case "PistolHolster":
                        pattern = "HolsterHip" + rightSuffix;
                        break;
                    case "LighterHolster":
                        pattern = "HolsterChest" + rightSuffix;
                        break;
                    case "BackpackHolster":
                        pattern = "ReceiveShoulder" + leftSuffix;
                        break;
                    case "AkHolster":
                        pattern = "ReceiveShoulder" + rightSuffix;
                        break;
                    case "GasMaskHolster":
                        pattern = "HolsterHip" + leftSuffix;
                        break;
                    default:
                        pattern = "";
                        break;
                }
                if (pattern == "") return;
                tactsuitVr.PlaybackHaptics(pattern);
            }
        }

        [HarmonyPatch(typeof(SnapZone), "GrabGrabbable", new Type[] { typeof(Grabbable) })]
        public class bhaptics_GrabGrabbable
        {
            [HarmonyPostfix]
            public static void Postfix(SnapZone __instance)
            {
                if (__instance == null) return;
                string pattern = "";
                string leftSuffix = "_L";
                string rightSuffix = "_R";
                if (!isRightHanded) { leftSuffix = "_R"; rightSuffix = "_L"; }
                switch (__instance.name)
                {
                    case "KnifeHolster":
                        pattern = "HolsterChest" + leftSuffix;
                        break;
                    case "PistolHolster":
                        pattern = "HolsterHip" + rightSuffix;
                        break;
                    case "LighterHolster":
                        pattern = "HolsterChest" + rightSuffix;
                        break;
                    case "BackpackHolster":
                        pattern = "StoreShoulder" + leftSuffix;
                        break;
                    case "AkHolster":
                        pattern = "StoreShoulder" + rightSuffix;
                        break;
                    case "GasMaskHolster":
                        pattern = "HolsterHip" + leftSuffix;
                        break;
                    default:
                        pattern = "";
                        break;
                }
                if (pattern == "") return;
                tactsuitVr.PlaybackHaptics(pattern);
            }
        }

        [HarmonyPatch(typeof(SnapZone), "ReleaseAll", new Type[] {  })]
        public class bhaptics_GrabReleaseAll
        {
            [HarmonyPostfix]
            public static void Postfix(SnapZone __instance)
            {
                if (__instance == null) return;
                //tactsuitVr.LOG("ReleaseAll: " + __instance.name);
                //tactsuitVr.PlaybackHaptics(pattern);
            }
        }



        [HarmonyPatch(typeof(PlayerBase), "UpdateHPBar", new Type[] {  })]
        public class bhaptics_UpdateHealth
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerBase __instance)
            {
                //if (__instance.HP <= 0.0f) { tactsuitVr.StopHeartBeat(); return; }
                if (__instance.HP <= 25.0f) tactsuitVr.StartHeartBeat();
                else tactsuitVr.StopHeartBeat();
            }
        }

        [HarmonyPatch(typeof(InGameMenu), "LeftHanded", new Type[] { })]
        public class bhaptics_LeftHandedInGame
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                isRightHanded = false;
            }
        }

        [HarmonyPatch(typeof(InGameMenu), "RightHanded", new Type[] { })]
        public class bhaptics_RightHandedInGame
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                isRightHanded = true;
            }
        }

        [HarmonyPatch(typeof(MainMenu), "LeftHanded", new Type[] { })]
        public class bhaptics_LeftHanded
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                isRightHanded = false;
            }
        }

        [HarmonyPatch(typeof(MainMenu), "RightHanded", new Type[] { })]
        public class bhaptics_RightHanded
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                isRightHanded = true;
            }
        }


    }
}
