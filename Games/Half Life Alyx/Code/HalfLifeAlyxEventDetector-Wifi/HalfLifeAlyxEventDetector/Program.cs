using System;
using System.Threading;
using System.IO.Ports;
using static HalfLifeAlyxEventDetector.HalfLifeAlyxGameMonitor;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace HalfLifeAlyxEventDetector
{
    class Program
    {
        public static SerialPort port;
        public static TcpClient tcpclnt;
        static void Main(string[] args)
        {
            try
            {
                tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                //Change this to use the ip address of your esp32
                tcpclnt.Connect("esp32IPAddress", 23); //23 is your port number. Change this to match the port number you specified in the esp32 code
                // use the ipaddress as in the server program

             }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }

            if (!Environment.Is64BitProcess)
                throw new Exception("Please build this application with 64 bit");
            HalfLifeAlyx_Manager HLA_Manager = new HalfLifeAlyx_Manager();
            HalfLifeAlyx_Autoexec HLA_Autoexec =
                new HalfLifeAlyx_Autoexec().
                UnlimitedMagazineInBag(true).
                GiveAllUnlockedWeapons(true);
            if (!HLA_Manager.RestartHalfLifeAlyx(HLA_Autoexec))
                throw new Exception("Cannot open Half-Life: Alyx!");

            HalfLifeAlyxGameMonitor GameMonitor = new HalfLifeAlyxGameMonitor();
            GameMonitor.RegisterEventHandler(EventCallbackHandler);
            //GameMonitor
            //    .SetBuddha(true)
            //    .Map("a1_intro_world");
            Console.ReadLine();
        }
        static void EventCallbackHandler(NowWeaponType WeaponType, HapticEventType Haptic)
        {
            if (Haptic == HapticEventType.WeaponShoot)
                ShootHapticFeedback(WeaponType);
            else if (Haptic == HapticEventType.WeaponChange)
                SwitchWeaponHapticFeedback(WeaponType);
            else if (Haptic == HapticEventType.InsertBullet)
                InsertBulletHapticFeedback(WeaponType);
            else if (Haptic == HapticEventType.LeftHandGrabStart || Haptic == HapticEventType.RightHandGrabStart)
                GloveStartGrab(Haptic == HapticEventType.RightHandGrabStart);
            else if (Haptic == HapticEventType.RightHandGrabStop || Haptic == HapticEventType.RightHandPickupItem)
                GloveStopGrab(true);
            else if (Haptic == HapticEventType.LeftHandGrabStop || Haptic == HapticEventType.LeftHandPickupItem)
                GloveStopGrab(false);
            else if (Haptic == HapticEventType.PlayerTeleportStart)
                StartTeleportation();
            else if (WeaponType == NowWeaponType.Energygun && Haptic == HapticEventType.EnergygunEjectClip)
                EnergygunEjectClip();
            else if (WeaponType == NowWeaponType.Energygun && Haptic == HapticEventType.ChamberedRound)
                EnergygunChamberedRound();
            else if (WeaponType == NowWeaponType.Shotgun && Haptic == HapticEventType.Shotgun_Open)
                Shotgun_Open();
            else if (WeaponType == NowWeaponType.Shotgun && Haptic == HapticEventType.Shotgun_Close)
                Shotgun_Close();
            else if (WeaponType == NowWeaponType.Shotgun && Haptic == HapticEventType.Shotgun_Chambered)
                Shotgun_Chambered();
            Console.WriteLine($"Event {Enum.GetName(typeof(HapticEventType), Haptic)} with Weapon {Enum.GetName(typeof(NowWeaponType), WeaponType)}");
        }
        static void ShootHapticFeedback(NowWeaponType WeaponType)
        {
            Stream stm = tcpclnt.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();

            switch (WeaponType)
            {
                case NowWeaponType.Energygun:
                    byte[] ba = asen.GetBytes("1"); 
                    stm.Write(ba, 0, ba.Length);
                    Console.WriteLine("sending 1");
                    break;
                case NowWeaponType.Rapidfire:
                    byte[] bb = asen.GetBytes("0");
                    stm.Write(bb, 0, bb.Length);
                    Console.WriteLine("sending 0");
                    break;
                case NowWeaponType.Shotgun:
                    byte[] bc = asen.GetBytes("2");
                    stm.Write(bc, 0, bc.Length);
                    Console.WriteLine("sending 2");
                    break;
            }
        }
        static void SwitchWeaponHapticFeedback(NowWeaponType WeaponType)
        {
            switch (WeaponType)
            {
                case NowWeaponType.Shotgun:
                    break;
                case NowWeaponType.Rapidfire:
                    break;
                case NowWeaponType.Energygun:
                    break;
            }
            return;
        }
        static void InsertBulletHapticFeedback(NowWeaponType WeaponType)
        {
            switch (WeaponType)
            {
                case NowWeaponType.Energygun:
                    break;
                case NowWeaponType.Shotgun:
                    break;
                case NowWeaponType.Rapidfire:
                    break;
            }
        }
        static void GloveStartGrab(bool RightHand)
        {
        }
        static void GloveStopGrab(bool RightHand)
        {
        }
        static void StartTeleportation()
        {
        }

        static void EnergygunEjectClip()
        {
        }

        static void EnergygunChamberedRound()
        {
        }

        static void Shotgun_Open()
        {
        }

        static void Shotgun_Close()
        {
        }

        static void Shotgun_Chambered()
        {
        }

    }
}
