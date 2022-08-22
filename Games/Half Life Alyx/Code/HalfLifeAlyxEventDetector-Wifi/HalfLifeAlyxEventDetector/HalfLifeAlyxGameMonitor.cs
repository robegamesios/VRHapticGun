using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HalfLifeAlyxEventDetector
{
    class HalfLifeAlyxGameMonitor
    {
        public enum NowWeaponType
        {
            None,
            Energygun,
            Rapidfire,
            Shotgun
        }
        public enum HapticEventType
        {
            WeaponChange,
            WeaponShoot,
            PlayerTeleportStart,
            PlayerTeleportStop,
            PlayerQuickTurned,
            LeftHandPickupItem,
            RightHandPickupItem,
            LeftHandGrabStart,
            LeftHandGrabStop,
            RightHandGrabStart,
            RightHandGrabStop,
            InsertBullet,
            EnergygunEjectClip,
            ChamberedRound,
            Shotgun_Open,
            Shotgun_Close,
            Shotgun_Chambered,
            GameReloaded
        }
        public enum ShotgunStateType
        {
            Open,
            Close,
            Chambered
        }
        private Action<NowWeaponType, HapticEventType> EventCallbackHandler;
        Socket socket;
        Thread ReceiveEventThread;
        private int PlayerReloadedCount = 0;
        const uint DataWindow = 6;
        private uint EventCounter = DataWindow;
        private uint SwitchWeaponAppearsAt = 0;
        private uint ItemPickUpAt = 0;
        private uint GlovePullAt = 0;
        private uint GloveCatchAt = 0;
        private uint ShotGunStateChangeAt = 0;
        private uint GunFiresAt = 0;
        private NowWeaponType NowWeapon = NowWeaponType.None;
        private ShotgunStateType ShotgunState = ShotgunStateType.Chambered;

        public HalfLifeAlyxGameMonitor()
        {
            var IpAddress = IPAddress.Parse("127.0.0.1");
            var IPEndPoint = new IPEndPoint(IpAddress, 29000);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(IPEndPoint);
            ReceiveEventThread = new Thread(ReceiveEventHandler);
            ReceiveEventThread.Start();
        }
        public HalfLifeAlyxGameMonitor SendCommandToHalfLifeAlyx(string Command)
        {
            var LowerCommand = Command.ToLower().Trim();
            byte[] Headers = {0x43, 0x4D, 0x4E, 0x44, 0x00, 0xD3, 0x00, 0x00, 0x00};
            List<byte> TargetArray = new List<byte>();
            TargetArray.AddRange(Headers);
            byte[] LowerCommandBytes = Encoding.GetEncoding("UTF-8").GetBytes(LowerCommand);
            byte ArrayLength = (byte)(13 + LowerCommand.Length);
            TargetArray.Add((byte)ArrayLength);
            TargetArray.Add(0);
            TargetArray.Add(0);
            TargetArray.AddRange(LowerCommandBytes);
            TargetArray.Add(0);
            byte[] SendDataArray = TargetArray.ToArray();
            socket.Send(SendDataArray);
            return this;
        }
        /// <summary>
        /// A blocking function that return value after the player rejoins the game.
        /// The function is used when player reloads/relocates in the game.
        /// </summary>
        public HalfLifeAlyxGameMonitor WaitPlayerReloaded()
        {
            int CurrentPlayerReloadedCount = PlayerReloadedCount;
            while (CurrentPlayerReloadedCount == PlayerReloadedCount)
                Thread.Sleep(10);
            return this;
        }
        /// <summary>
        /// Reload game saved files. 
        /// It occurs a reload event. Use WaitPlayerReloaded() to wait for the game ready.
        /// </summary>
        /// <param name="FilePath">Game saved file name. LoadSavedFile("test"); equals to load gamesaved file: 'save\test.sav'</param>
        public HalfLifeAlyxGameMonitor LoadSavedFile(string FilePath = "autosave")
        {
            SendCommandToHalfLifeAlyx($"load {FilePath}");
            return this;
        }
        /// <summary>
        /// You still take damage but can’t die (Jeff can still kill you).
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="IsOn">Turn On/Off this feature</param>
        public HalfLifeAlyxGameMonitor SetBuddha(bool IsOn)
        {
            SendCommandToHalfLifeAlyx($"buddha {(IsOn ? 1 : 0)}");
            return this;
        }
        /// <summary>
        /// Toggles invincibility.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="IsOn">Turn On/Off this feature</param>
        public HalfLifeAlyxGameMonitor SetGod(bool IsOn)
        {
            SendCommandToHalfLifeAlyx($"god {(IsOn ? 1 : 0)}");
            return this;
        }
        /// <summary>
        /// Changes the gravity for the player, props, and entities. 0 = no gravity, ~500 = normal gravity.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="val">Changes the gravity.</param>
        public HalfLifeAlyxGameMonitor SetSV_Gravity(ushort val = 500)
        {
            SendCommandToHalfLifeAlyx($"sv_gravity {val}");
            return this;
        }
        /// <summary>
        /// Shows an overlay with the name, position, and more of the currently looked at entity.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="val">Changes the flying speed (default 60).</param>
        public HalfLifeAlyxGameMonitor SetVR_Fly_Speed(ushort val = 60)
        {
            SendCommandToHalfLifeAlyx($"vrfly_speed {val}");
            return this;
        }
        /// <summary>
        /// Spawns an item of the given name at your feet.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="item_name">Type Item Name.</param>
        public HalfLifeAlyxGameMonitor Give(string item_name = "item_hlvr_health_station_vial")
        {
            SendCommandToHalfLifeAlyx($"give {item_name}");
            return this;
        }
        /// <summary>
        /// Used to load a map. Can also load maps from the SteamVR Workshop Tools.
        /// It occurs a reload event. Use WaitPlayerReloaded() to wait for the game ready.
        /// Src: https://indiefaq.com/guides/1471-half-life-alyx.html
        /// </summary>
        /// <param name="map_name">Type Map Name.</param>
        public HalfLifeAlyxGameMonitor Map(string map_name = "startup")
        {
            SendCommandToHalfLifeAlyx($"map {map_name}");
            return this;
        }
        public void RegisterEventHandler(Action<NowWeaponType, HapticEventType> EventCallbackHandler)
        {
            this.EventCallbackHandler = EventCallbackHandler;
        }
        private void ReceiveEventHandler()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Thread.CurrentThread.IsBackground = true;
            byte[] Packet = new byte[4 * 1024 * 1024];
            //Drop first 5 second data
            //Thread.Sleep(5000);
            socket.Receive(Packet);
            while (true)
            {
                try
                {
                    int ReceivedSize = socket.Receive(Packet);
                    string EventTextPool = Encoding.ASCII.GetString(Packet, 0, ReceivedSize);
                    int ParsedIndex = 0;
                    int EventIndexStart;
                    while ((EventIndexStart = EventTextPool.IndexOf("PRNT", ParsedIndex)) >= 0)
                    {
                        ParsedIndex = EventTextPool.IndexOf("\n", EventIndexStart);
                        if (ParsedIndex < 0)
                            ParsedIndex = EventTextPool.Length;
                        int StartParsing = EventTextPool.LastIndexOf("\0", ParsedIndex);
                        string PureEventString = EventTextPool.Substring(StartParsing + 1, ParsedIndex - StartParsing - 1);
                        ParseEvent(PureEventString);
                    }
                }catch (SocketException e)
                {
                    if (Process.GetProcessesByName("hlvr").Length == 0)
                        Process.GetCurrentProcess().Kill();
                    Console.WriteLine($"Socket Exception {e.ToString()}. Is Half-Life: Alyx still running?");
                    Process.GetCurrentProcess().Kill();
                }
            }
        }
        private void NotifyGameReloaded()
        {
            ++PlayerReloadedCount;
            EventCallbackHandler(NowWeapon, HapticEventType.GameReloaded);
        }
        private void ParseEvent(string Event)
        {
            if (Event.Contains("player_connect_full"))
                NotifyGameReloaded();
            else if (Event.Contains("weapon_switch"))
                SwitchWeaponAppearsAt = EventCounter;
            else if (Event.Contains("player_teleport_start"))
                EventCallbackHandler(NowWeapon, HapticEventType.PlayerTeleportStart);
            else if (Event.Contains("player_teleport_finish"))
                EventCallbackHandler(NowWeapon, HapticEventType.PlayerTeleportStop);
            else if (Event.Contains("player_quick_turned"))
                EventCallbackHandler(NowWeapon, HapticEventType.PlayerQuickTurned);
            else if (Event.Contains("item_pickup"))
                ItemPickUpAt = EventCounter;
            else if (Event.Contains("grabbity_glove_pull"))
                GlovePullAt = EventCounter;
            else if (Event.Contains("grabbity_glove_catch"))
                GloveCatchAt = EventCounter;
            else if (Event.Contains("player_shoot_weapon"))
            {
                GunFiresAt = EventCounter;
                EventCallbackHandler(NowWeapon, HapticEventType.WeaponShoot);
            }
            else if (Event.Contains("\"item\""))
            {
                if (SwitchWeaponAppearsAt + 2 == EventCounter)
                {
                    if (Event.Contains("hlvr_weapon_energygun"))
                        NowWeapon = NowWeaponType.Energygun;
                    else if (Event.Contains("hlvr_weapon_rapidfire"))
                        NowWeapon = NowWeaponType.Rapidfire;
                    else if (Event.Contains("hlvr_weapon_shotgun"))
                        NowWeapon = NowWeaponType.Shotgun;
                    else
                        NowWeapon = NowWeaponType.None;
                    EventCallbackHandler(NowWeapon, HapticEventType.WeaponChange);
                }
            }
            else if (Event.Contains("vr_tip_attachment"))
            {
                bool IsLeft = Event.Contains("\"2\"");
                bool IsRight = Event.Contains("\"1\"");
                if (IsLeft || IsRight)
                {
                    if (ItemPickUpAt + 5 == EventCounter)
                        if (IsLeft)
                            EventCallbackHandler(NowWeapon, HapticEventType.LeftHandPickupItem);
                        else
                            EventCallbackHandler(NowWeapon, HapticEventType.RightHandPickupItem);
                    if (GlovePullAt + 6 == EventCounter)
                        if (IsLeft)
                            EventCallbackHandler(NowWeapon, HapticEventType.LeftHandGrabStart);
                        else
                            EventCallbackHandler(NowWeapon, HapticEventType.RightHandGrabStart);
                    if (GloveCatchAt + 5 == EventCounter)
                        if (IsLeft)
                            EventCallbackHandler(NowWeapon, HapticEventType.LeftHandGrabStop);
                        else
                            EventCallbackHandler(NowWeapon, HapticEventType.RightHandGrabStop);
                }
            }
            else
            {
                switch (NowWeapon)
                {
                    case NowWeaponType.Energygun:
                        if (Event.Contains("player_pistol_clip_inserted"))
                            EventCallbackHandler(NowWeapon, HapticEventType.InsertBullet);
                        else if (Event.Contains("player_pistol_chambered_round"))
                            EventCallbackHandler(NowWeapon, HapticEventType.ChamberedRound);
                        else if (Event.Contains("player_eject_clip"))
                            EventCallbackHandler(NowWeapon, HapticEventType.EnergygunEjectClip);
                        break;

                    case NowWeaponType.Rapidfire:
                        if (Event.Contains("player_rapidfire_inserted_capsule_in_chamber"))
                            EventCallbackHandler(NowWeapon, HapticEventType.InsertBullet);
                        break;

                    case NowWeaponType.Shotgun:
                        if (Event.Contains("player_shotgun_loaded_shells"))
                            EventCallbackHandler(NowWeapon, HapticEventType.InsertBullet);
                        else if (Event.Contains("player_shotgun_state_changed"))
                            ShotGunStateChangeAt = EventCounter;
                        else if (Event.Contains("\"shotgun_state\" =") && ShotGunStateChangeAt + 2 == EventCounter)
                        {
                            if (Event.Contains("\"3\"") && ShotgunState != ShotgunStateType.Open)
                            {
                                EventCallbackHandler(NowWeapon, HapticEventType.Shotgun_Open);
                                ShotgunState = ShotgunStateType.Open;
                            }
                            else if ((Event.Contains("\"1\"") || Event.Contains("\"0\"")) && ShotgunState != ShotgunStateType.Close)
                            {
                                EventCallbackHandler(NowWeapon, HapticEventType.Shotgun_Close);
                                ShotgunState = ShotgunStateType.Close;
                            }
                        }
                        else if (Event.Contains("player_shotgun_is_ready") && GunFiresAt + 6 < EventCounter && ShotgunState != ShotgunStateType.Chambered)
                        {
                            EventCallbackHandler(NowWeapon, HapticEventType.Shotgun_Chambered);
                            ShotgunState = ShotgunStateType.Chambered;
                        }
                        break;
                }
            }
            EventCounter++;
        }
    }
}
