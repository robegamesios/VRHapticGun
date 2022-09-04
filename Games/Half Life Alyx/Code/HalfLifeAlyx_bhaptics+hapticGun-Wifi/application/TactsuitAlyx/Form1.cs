using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bhaptics.Tact;
using CustomWebSocketSharp;
using Microsoft.WindowsAPICodePack.Dialogs;

//Haptic gun imports
using System.Net.Sockets;

namespace TactsuitAlyx
{
    public partial class Form1 : Form
    {
        bool parsingMode = false;

        public TactsuitVR tactsuitVr;
        public Engine engine;

        public static TcpClient tcpclnt;

        private delegate void SafeCallDelegate(string text);

        public Form1()
        {
            InitializeComponent();
        }
        public static IEnumerable<string> ReadLines(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        private void WriteTextSafe(string text)
        {
            if (lblInfo.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                lblInfo.Invoke(d, new object[] { text });
            }
            else
            {
                lblInfo.Text = text;
            }
        }

        void ParseLine(string line)
        {
            string newLine = line.Replace('{', ' ');
            newLine = newLine.Replace('}', ' ');
            newLine = newLine.Trim();

            string[] splitted = newLine.Split(new char[] {'|'});

            if (splitted.Length > 0)
            {
                string command = splitted[0].Trim();
                if (command == "PlayerHealth")
                {
                    if (splitted.Length > 1)
                    {
                        int health = int.Parse(splitted[1].Trim());
                        if (health >= 0)
                        {
                            engine.HealthRemaining(health);
                        }
                    }
                }
                else if (command == "PlayerHurt")
                {
                    if (splitted.Length > 1)
                    {
                        int healthRemaining = int.Parse(splitted[1].Trim());
                        string enemy = "";
                        float playerAngle = 0;
                        string enemyName = "";
                        string enemyDebugName = "";

                        if (splitted.Length > 2)
                        {
                            enemy = splitted[2].Trim();
                        }

                        if (splitted.Length > 3)
                        {
                            playerAngle = float.Parse(splitted[3].Trim());
                        }
                        if (splitted.Length > 4)
                        {
                            enemyName = (splitted[4].Trim());
                        }
                        if (splitted.Length > 5)
                        {
                            enemyDebugName = (splitted[5].Trim());
                        }

                        engine.PlayerHurt(healthRemaining, enemy, playerAngle, enemyName, enemyDebugName);
                    }
                }
                else if (command == "PlayerShootWeapon")
                {
                    if (splitted.Length > 1)
                    {
                        //Haptic gun send signal to esp32
                        Stream stm = tcpclnt.GetStream();
                        ASCIIEncoding asen = new ASCIIEncoding();
                        byte[] ba = asen.GetBytes("0");
                        stm.Write(ba, 0, ba.Length);
                        
                        //BHaptics
                        engine.PlayerShoot(splitted[1].Trim());
                    }
                }
                else if (command == "TwoHandStart")
                {
                    engine.twoHandedMode = true;
                }
                else if (command == "TwoHandEnd")
                {
                    engine.twoHandedMode = false;
                }
                else if (command == "PlayerOpenedGameMenu")
                {
                    engine.menuOpen = true;
                }
                else if (command == "PlayerClosedGameMenu")
                {
                    engine.menuOpen = false;
                }
                else if (command == "PlayerShotgunUpgradeGrenadeLauncherState")
                {
                    if (splitted.Length > 1)
                    {
                        int state = int.Parse(splitted[1].Trim());

                        engine.GrenadeLauncherStateChange(state);
                    }
                }
                else if (command == "PlayerGrabbityLockStart")
                {
                    if (splitted.Length > 1)
                    {
                        int primary = int.Parse(splitted[1].Trim());
                        
                        engine.GrabbityLockStart(primary == 1);
                    }
                }
                else if (command == "PlayerGrabbityLockStop")
                {
                    if (splitted.Length > 1)
                    {
                        int primary = int.Parse(splitted[1].Trim());

                        engine.GrabbityLockStop(primary == 1);
                    }
                }
                else if (command == "PlayerGrabbityPull")
                {
                    if (splitted.Length > 1)
                    {
                        int primary = int.Parse(splitted[1].Trim());

                        engine.GrabbityGlovePull(primary == 1);
                    }
                }
                else if (command == "PlayerGrabbedByBarnacle")
                {
                    engine.BarnacleGrabStart();
                }
                else if (command == "PlayerReleasedByBarnacle")
                {
                    engine.barnacleGrab = false;
                }
                else if (command == "PlayerDeath")
                {
                    if (splitted.Length > 1)
                    {
                        int damagebits = int.Parse(splitted[1].Trim());

                        engine.PlayerDeath(damagebits);
                    }
                }
                else if (command == "Reset")
                {
                    engine.Reset();
                }
                else if (command == "PlayerCoughStart")
                {
                    engine.Cough();
                }
                else if (command == "PlayerCoughEnd")
                {
                    engine.coughing = false;
                }
                else if (command == "PlayerCoveredMouth")
                {
                    engine.coughing = false;
                }
                else if (command == "GrabbityGloveCatch")
                {
                    if (splitted.Length > 1)
                    {
                        int primary = int.Parse(splitted[1].Trim());

                        engine.GrabbityGloveCatch(primary == 1);
                    }
                }
                else if (command == "PlayerDropAmmoInBackpack")
                {
                    if (splitted.Length > 1)
                    {
                        int leftShoulder = int.Parse(splitted[1].Trim());
                        engine.DropAmmoInBackpack(leftShoulder == 1);
                    }
                }
                else if (command == "PlayerDropResinInBackpack")
                {
                    if (splitted.Length > 1)
                    {
                        int leftShoulder = int.Parse(splitted[1].Trim());
                        engine.DropResinInBackpack(leftShoulder == 1);
                    }
                }
                else if (command == "PlayerRetrievedBackpackClip")
                {
                    if (splitted.Length > 1)
                    {
                        int leftShoulder = int.Parse(splitted[1].Trim());
                        engine.RetrievedBackpackClip(leftShoulder == 1);
                    }
                }
                else if (command == "PlayerStoredItemInItemholder"
                         || command == "HealthPenTeachStorage"
                         //|| command == "HealthVialTeachStorage"
                         )
                {
                    if (splitted.Length > 1)
                    {
                        int leftHolder = int.Parse(splitted[1].Trim());
                        engine.StoredItemInItemholder(leftHolder == 1);
                    }
                }
                else if (command == "PlayerRemovedItemFromItemholder")
                {
                    if (splitted.Length > 1)
                    {
                        int leftHolder = int.Parse(splitted[1].Trim());
                        engine.RemovedItemFromItemholder(leftHolder == 1);
                    }
                }
                else if (command == "PrimaryHandChanged" || command == "SingleControllerModeChanged")
                {
                    if (splitted.Length > 1)
                    {
                        int leftHanded = int.Parse(splitted[1].Trim());

                        engine.leftHandedMode = leftHanded == 1;
                    }
                }
                else if (command == "PlayerHeal")
                {
                    float angle = 0;
                    if (splitted.Length > 1)
                    {
                        angle = float.Parse(splitted[1].Trim());
                    }
                    engine.HealthPenUse(angle);
                }
                else if (command == "PlayerUsingHealthstation")
                {
                    if (splitted.Length > 1)
                    {
                        int leftArm = int.Parse(splitted[1].Trim());
                        engine.HealthStationUse(leftArm == 1);
                    }
                }
                else if (command == "ItemPickup")
                {
                    if (splitted.Length > 1)
                    {
                        string item = splitted[1].Trim();

                        if (item == "item_hlvr_crafting_currency_large" || item == "item_hlvr_crafting_currency_small")
                        {
                            if (splitted.Length > 2)
                            {
                                int leftShoulder = int.Parse(splitted[2].Trim());
                                engine.RetrievedBackpackResin(leftShoulder == 1);
                            }
                        }
                    }
                }
                else if (command == "ItemReleased")
                {
                    if (splitted.Length > 1)
                    {
                        string item = splitted[1].Trim();

                        if (item == "item_hlvr_prop_battery")
                        {
                            if (splitted.Length > 2)
                            {
                                int leftArm = int.Parse(splitted[2].Trim());
                                engine.ShockOnArm(leftArm == 1);
                            }
                        }
                    }
                }
                else if (command == "PlayerPistolClipInserted" || command == "PlayerShotgunShellLoaded" || command == "PlayerRapidfireInsertedCapsuleInChamber" || command == "PlayerRapidfireInsertedCapsuleInMagazine")
                {
                    engine.ClipInserted();
                }
                else if (command == "PlayerPistolChamberedRound" || command == "PlayerShotgunLoadedShells" || command == "PlayerRapidfireClosedCasing" || command == "PlayerRapidfireOpenedCasing")
                {
                    engine.ChamberedRound();
                }
            }
            WriteTextSafe(line);

            GC.Collect();
        }

        void ParseConsole()
        {
            string filePath = txtAlyxDirectory.Text + "\\game\\hlvr\\console.log";

            bool first = true;
            int counter = 0;

            while (parsingMode)
            {
                if (File.Exists(filePath))
                {
                    if (first)
                    {
                        first = false;
                        WriteTextSafe("Interface active");
                        counter = ReadLines(filePath).Count();
                    }
                    int lineCount = ReadLines(filePath).Count();//read text file line count to establish length for array
                    
                    if (counter < lineCount && lineCount > 0)//if counter is less than lineCount keep reading lines
                    {
                        var lines = Enumerable.ToList(ReadLines(filePath).Skip(counter).Take(lineCount - counter));
                        for (int i = 0; i < lines.Count; i++)
                        {
                            if (lines[i].Contains("[Tactsuit]"))
                            {
                                //Do haptic feedback
                                string line = lines[i].Substring(lines[i].LastIndexOf(']') + 1);
                                Thread thread = new Thread(() => ParseLine(line));
                                thread.Start();
                            }
                            else if (lines[i].Contains("unpaused the game"))
                            {
                                engine.menuOpen = false;
                            }
                            else if (lines[i].Contains("paused the game"))
                            {
                                engine.menuOpen = true;
                            }
                            else if (lines[i].Contains("Quitting"))
                            {
                                engine.Reset();
                            }
                        }

                        counter += lines.Count;
                    }
                    else if (counter == lineCount && lineCount > 0)
                    {
                        Thread.Sleep(50);
                    }
                    else
                    {
                        counter = 0;
                    }
                }
                else
                {
                    WriteTextSafe("Cannot file console.log. Waiting.");

                    Thread.Sleep(2000);
                }
            }
            WriteTextSafe("Waiting...");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string exePath = txtAlyxDirectory.Text + "\\game\\bin\\win64\\hlvr.exe";
            if (!File.Exists(exePath))
            {
                MessageBox.Show("Please select your Half-Life Alyx installation folder correctly first.", "Error Starting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string scriptPath = txtAlyxDirectory.Text + "\\game\\hlvr\\scripts\\vscripts\\tactsuit.lua";
            if (!File.Exists(scriptPath))
            {
                MessageBox.Show("Script file installation is not correct. Please read the instructions on the mod page and reinstall.", "Script Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string scriptLoaderPath = txtAlyxDirectory.Text + "\\game\\hlvr\\cfg\\skill_manifest.cfg";
            string configText = File.ReadAllText(scriptLoaderPath);
            if (!configText.Contains("script_reload_code tactsuit.lua"))
            {
                MessageBox.Show("skill_manifest.cfg file installation is not correct. Please read the instructions on the mod page and reinstall.", "Script Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            label4.Text = "";
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnBrowse.Enabled = false;
            btnTest.Enabled = true;
            tactsuitVr = new TactsuitVR();
            tactsuitVr.CreateSystem();
            engine = new Engine(tactsuitVr);

            WriteTextSafe("Starting...");
            
            parsingMode = true;

            //Haptic Gun connect to Wifi
            try
            {
                tcpclnt = new TcpClient();
            
                if (txtHapticGunIpAddress.Text != "" && txtHapticGunPortNumber.Text != "")
                {
                    Console.WriteLine("Connecting.....");

                    //Change this to use the ip address of your esp32
                    tcpclnt.Connect(txtHapticGunIpAddress.Text, Int32.Parse(txtHapticGunPortNumber.Text)); //23 is your port number. Change this to match the port number you specified in the esp32 code
                    saveHapticGunIpAddress();
                }
            }
            catch (Exception err)
            {
                clearHapticGunIpAddress();
                label4.Text = "Haptic Gun Connection error: check IP Address, Port Number, and the gun is turned ON";
                Console.WriteLine("Error..... " + err.StackTrace);
            }

            Thread thread = new Thread(ParseConsole);
            thread.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnBrowse.Enabled = true;
            btnTest.Enabled = false;
            label4.Text = "";

            parsingMode = false;

            if (tactsuitVr.hapticPlayer != null)
            {
                tactsuitVr.hapticPlayer.Disable();
                tactsuitVr.hapticPlayer.Dispose();
            }

            //Haptic gun close tcp connection
            tcpclnt.Close();

            WriteTextSafe("Stopping...");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.RestoreDirectory = true;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                txtAlyxDirectory.Text = dialog.FileName;
                Properties.Settings.Default.AlyxDirectory = txtAlyxDirectory.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void saveHapticGunIpAddress()
        {
            if (txtHapticGunIpAddress.Text.Length < 6)
                return;
            if (txtHapticGunPortNumber.Text.Length == 0)
                return;

            Properties.Settings.Default.HapticGunPortNumber = Int32.Parse(txtHapticGunPortNumber.Text); 
            Properties.Settings.Default.HapticGunIpAddress = txtHapticGunIpAddress.Text;
            Properties.Settings.Default.Save();
        }

        private void clearHapticGunIpAddress()
        {
            txtHapticGunIpAddress.Text = "";
            Properties.Settings.Default.HapticGunIpAddress = "";
            Properties.Settings.Default.Save();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SettingsForm sForm = new SettingsForm(this);
            sForm.Show();
            btnSettings.Enabled = false;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (tactsuitVr != null)
            {
                string active = ((tactsuitVr.hapticPlayer._activePosition.Contains(PositionType.All)) ? " {All} " : "") + ((tactsuitVr.hapticPlayer._activePosition.Contains(PositionType.Vest)) ? " {Vest} " : "") + ((tactsuitVr.hapticPlayer._activePosition.Contains(PositionType.ForearmR) || tactsuitVr.hapticPlayer._activePosition.Contains(PositionType.Right)) ? " {Right Arm} " : "") + ((tactsuitVr.hapticPlayer._activePosition.Contains(PositionType.ForearmL) || tactsuitVr.hapticPlayer._activePosition.Contains(PositionType.Left)) ? " {Left Arm} " : "") + ((tactsuitVr.hapticPlayer._activePosition.Contains(PositionType.Head)) ? " {Head} " : "") + ((tcpclnt.Connected) ? " {Haptic Gun}" : "");

                //string activeDevices = "";
                //for (int i = 0; i < tactsuitVr.hapticPlayer._activePosition.Count; i++)
                //{
                //    if (i > 0)
                //    {
                //        activeDevices += ",";
                //    }
                //    activeDevices += (int)tactsuitVr.hapticPlayer._activePosition[i];
                //}

                if (active.Trim().IsNullOrEmpty())
                    active = "{None}";

                //Send signal to Haptic gun
                if (tcpclnt.Connected)
                {
                    Stream stm = tcpclnt.GetStream();
                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] ba = asen.GetBytes("0");
                    stm.Write(ba, 0, ba.Length);
                }

                WriteTextSafe("Active devices: " + active);

                tactsuitVr.PlayRandom();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnBrowse.Enabled = true;
            btnTest.Enabled = false;

            parsingMode = false;

            if (tactsuitVr != null && tactsuitVr.hapticPlayer != null)
            {
                WriteTextSafe("Stopping...");
                tactsuitVr.hapticPlayer.Disable();
                tactsuitVr.hapticPlayer.Dispose();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtAlyxDirectory.Text = Properties.Settings.Default.AlyxDirectory;
        }
    }
}
