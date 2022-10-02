using System;
using System.Collections.Generic;

using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;
using UnityEngine;

//Haptic gun imports
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Linq;

namespace ArizonaSunshine_bhaptics
{
    public class ArizonaSunshine_bhaptics : MelonMod
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
                byte[] ba = asen.GetBytes("0");
                stm.Write(ba, 0, ba.Length);
            }
        }

        public static void createGunHapticFeedbackLeft()
        {
            if (tcpclntLeft.Connected) {
                Stream stm = (tcpclntLeft.GetStream());
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes("0");
                stm.Write(ba, 0, ba.Length);
            }
        }

        private static KeyValuePair<float, float> getAngleAndShift(Transform player, Vector3 hit, Quaternion playerRotation)
        {
            // bhaptics pattern starts in the front, then rotates to the left. 0° is front, 90° is left, 270° is right.
            // y is "up", z is "forward" in local coordinates
            Vector3 patternOrigin = new Vector3(0f, 0f, 1f);
            Vector3 hitPosition = hit - player.position;
            Quaternion myPlayerRotation = playerRotation;
            Vector3 playerDir = myPlayerRotation.eulerAngles;
            // get rid of the up/down component to analyze xz-rotation
            Vector3 flattenedHit = new Vector3(hitPosition.x, 0f, hitPosition.z);

            // get angle. .Net < 4.0 does not have a "SignedAngle" function...
            float hitAngle = Vector3.Angle(flattenedHit, patternOrigin);
            // check if cross product points up or down, to make signed angle myself
            Vector3 crossProduct = Vector3.Cross(flattenedHit, patternOrigin);
            if (crossProduct.y > 0f) { hitAngle *= -1f; }
            // relative to player direction
            float myRotation = hitAngle - playerDir.y;
            // switch directions (bhaptics angles are in mathematically negative direction)
            myRotation *= -1f;
            // convert signed angle into [0, 360] rotation
            if (myRotation < 0f) { myRotation = 360f + myRotation; }


            // up/down shift is in y-direction
            // in Shadow Legend, the torso Transform has y=0 at the neck,
            // and the torso ends at roughly -0.5 (that's in meters)
            // so cap the shift to [-0.5, 0]...
            float hitShift = hitPosition.y;
            //tactsuitVr.LOG("HitShift: " + hitShift);
            float upperBound = 0.5f;
            float lowerBound = -0.5f;
            if (hitShift > upperBound) { hitShift = 0.5f; }
            else if (hitShift < lowerBound) { hitShift = -0.5f; }
            // ...and then spread/shift it to [-0.5, 0.5]
            else { hitShift = (hitShift - lowerBound) / (upperBound - lowerBound) - 0.5f; }

            //tactsuitVr.LOG("Relative x-z-position: " + relativeHitDir.x.ToString() + " "  + relativeHitDir.z.ToString());
            //tactsuitVr.LOG("HitAngle: " + hitAngle.ToString());
            //tactsuitVr.LOG("HitShift: " + hitShift.ToString());

            // No tuple returns available in .NET < 4.0, so this is the easiest quickfix
            return new KeyValuePair<float, float>(myRotation, hitShift);
        }


        [HarmonyPatch(typeof(Player), "ZombieHit", new Type[] { typeof(float), typeof(Zombie) })]
        public class bhaptics_ZombieHit
        {
            [HarmonyPostfix]
            public static void Postfix(Player __instance, Zombie zombie)
            {
                if (!__instance.IsLocalPlayer) return;
                //tactsuitVr.LOG("ZombieHit: " + zombie.Position.x.ToString() + " " + zombie.Position.y.ToString() + " " + zombie.Position.z.ToString());
                //tactsuitVr.LOG("PlayerPosition: " + __instance.BasePosition.x.ToString() + " " + __instance.BasePosition.y.ToString() + " " + __instance.BasePosition.z.ToString());
                Vector3 hitPosition = zombie.Position;
                Transform playerPosition = __instance.Transform;
                //Quaternion playerRotation = __instance.BaseRotation;
                Quaternion playerRotation = __instance.HeadRotation;
                var angleShift = getAngleAndShift(playerPosition, hitPosition, playerRotation);
                if (zombie.Locomotion.IsCrawling)
                {
                    tactsuitVr.PlaybackHaptics("ExplosionFeet");
                    tactsuitVr.PlayBackHit("Slash", angleShift.Key, -0.5f);
                    return;
                }
                tactsuitVr.PlayBackHit("Slash", angleShift.Key, angleShift.Value);

            }
        }

        [HarmonyPatch(typeof(Gun), "ShootBullet", new Type[] {  })]
        public class bhaptics_ShootHaptics
        {
            [HarmonyPostfix]
            public static void Postfix(Gun __instance, bool __result)
            {
                if (!__result) return;
                bool twoHanded = (__instance.IsTwoHanded && __instance.IsTwoHandedOffHandAttached);
                bool isRight = (__instance.EquipmentSlot.SlotID == E_EQUIPMENT_SLOT_ID.RIGHT_HAND);

                //hapticGun feedback
                if (twoHanded)
                {
                    ArizonaSunshine_bhaptics.createGunHapticFeedbackRight();
                    ArizonaSunshine_bhaptics.createGunHapticFeedbackLeft();
                }
                else if (isRight)
                {
                    ArizonaSunshine_bhaptics.createGunHapticFeedbackRight();
                }
                else
                {
                    ArizonaSunshine_bhaptics.createGunHapticFeedbackLeft();
                }

                //bHaptics feedback
                tactsuitVr.Recoil("Pistol", isRight, twoHanded);
            }
        }

        [HarmonyPatch(typeof(Player), "FullyHeal", new Type[] { })]
        public class bhaptics_PlayerHeal
        {
            [HarmonyPostfix]
            public static void Postfix(Player __instance)
            {
                if (!__instance.IsLocalPlayer) return;
                tactsuitVr.PlaybackHaptics("Healing");
            }
        }

        [HarmonyPatch(typeof(SlidingLocomotionController), "PlayFootstepAudio", new Type[] { typeof(Vector3), typeof(bool) })]
        public class bhaptics_PlayerFootstep
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (footStepRight) tactsuitVr.PlaybackHaptics("FootStep_R");
                else tactsuitVr.PlaybackHaptics("FootStep_L");
                footStepRight = !footStepRight;
            }
        }

        [HarmonyPatch(typeof(ExplosiveItem), "Explode", new Type[] { })]
        public class bhaptics_ExplodeItem
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("ExplosionBelly");
                tactsuitVr.PlaybackHaptics("ExplosionFeet");
            }
        }

        [HarmonyPatch(typeof(ExplosiveBehaviour), "Explode", new Type[] { typeof(Vector3), typeof(bool) })]
        public class bhaptics_ExplosionBehaviour
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("ExplosionBelly");
                tactsuitVr.PlaybackHaptics("ExplosionFeet");
            }
        }

        [HarmonyPatch(typeof(ExplosiveHittableBehaviour), "Explode", new Type[] { typeof(bool) })]
        public class bhaptics_ExplosiveHittableBehaviour
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("ExplosionBelly");
                tactsuitVr.PlaybackHaptics("ExplosionFeet");
            }
        }

        [HarmonyPatch(typeof(Player), "Kill", new Type[] {  })]
        public class bhaptics_PlayerKilled
        {
            [HarmonyPostfix]
            public static void Postfix(Player __instance)
            {
                if (!__instance.IsLocalPlayer) return;
                tactsuitVr.StopThreads();
            }
        }

        [HarmonyPatch(typeof(Player), "Update", new Type[] { })]
        public class bhaptics_PlayerUpdate
        {
            [HarmonyPostfix]
            public static void Postfix(Player __instance)
            {
                if (!__instance.IsLocalPlayer) return;
                if (__instance.Health <= 30.0f) tactsuitVr.StartHeartBeat();
                else tactsuitVr.StopHeartBeat();
            }
        }
    }
}
