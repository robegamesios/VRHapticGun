using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TactsuitAlyx
{
    public partial class SettingsForm : Form
    {
        private Form1 mainForm;
        public SettingsForm(Form1 fm)
        {
            mainForm = fm;
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            numDefaultHead.Value = (decimal)Properties.Settings.Default.intensityMultiplierDefaultHead;
            numUnarmedHead.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedHead;
            numGunHead.Value = (decimal)Properties.Settings.Default.intensityMultiplierGunHead;
            numBloater.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedBloater;
            numHeadcrab.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedHeadcrab;
            numArmoredHeadcrab.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabArmored;
            numToxicHeadcrab.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabBlack;
            numLightningDog.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabFast;
            numRunnerHeadcrab.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabRunner;
            numFastZombie.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedFastZombie;
            numPoisonZombie.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedPoisonZombie;
            numZombie.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedZombie;
            numBlindZombie.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedZombieBlind;
            numZombine.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedZombine;
            numAntlion.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedAntlion;
            numAntlionGuard.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedAntlionGuard;
            numManhack.Value = (decimal)Properties.Settings.Default.intensityMultiplierUnarmedManhack;
            numBarnacle.Value = (decimal)Properties.Settings.Default.intensityMultiplierGrabbedByBarnacle;
            numConcussionGrenade.Value = (decimal)Properties.Settings.Default.intensityMultiplierConcussionGrenade;
            numBugBaitGrenade.Value = (decimal)Properties.Settings.Default.intensityMultiplierBugBaitGrenade;
            numFragGrenade.Value = (decimal)Properties.Settings.Default.intensityMultiplierFragGrenade;
            numSpyGrenade.Value = (decimal)Properties.Settings.Default.intensityMultiplierSpyGrenade;
            numHandGrenade.Value = (decimal)Properties.Settings.Default.intensityMultiplierHandGrenade;
            numRollerGrenade.Value = (decimal)Properties.Settings.Default.intensityMultiplierRollerGrenade;
            numRollerMine.Value = (decimal)Properties.Settings.Default.intensityMultiplierRollerMine;
            numCombine.Value = (decimal)Properties.Settings.Default.intensityMultiplierCombine;
            numCombineGantry.Value = (decimal)Properties.Settings.Default.intensityMultiplierCombineGantry;
            numCombineS.Value = (decimal)Properties.Settings.Default.intensityMultiplierCombineS;
            numMetroPolice.Value = (decimal)Properties.Settings.Default.intensityMultiplierMetroPolice;
            numSniper.Value = (decimal)Properties.Settings.Default.intensityMultiplierSniper;
            numStrider.Value = (decimal)Properties.Settings.Default.intensityMultiplierStrider;
            numTurret.Value = (decimal)Properties.Settings.Default.intensityMultiplierTurret;
            numFoliageTurret.Value = (decimal)Properties.Settings.Default.intensityMultiplierFoliageTurret;
            numEnvironmentalExplosion.Value = (decimal)Properties.Settings.Default.intensityMultiplierEnvironmentExplosion;
            numEnvironmentalLaser.Value = (decimal)Properties.Settings.Default.intensityMultiplierEnvironmentLaser;
            numEnvironmentalFire.Value = (decimal)Properties.Settings.Default.intensityMultiplierEnvironmentFire;
            numEnvironmentalSpark.Value = (decimal)Properties.Settings.Default.intensityMultiplierEnvironmentSpark;
            numEnvironmentalPoison.Value = (decimal)Properties.Settings.Default.intensityMultiplierEnvironmentPoison;
            numEnvironmentalRadiation.Value = (decimal)Properties.Settings.Default.intensityMultiplierEnvironmentRadiation;
            numDamageExplosion.Value = (decimal)Properties.Settings.Default.intensityMultiplierDamageExplosion;
            numDamageLaser.Value = (decimal)Properties.Settings.Default.intensityMultiplierDamageLaser;
            numDamageFire.Value = (decimal)Properties.Settings.Default.intensityMultiplierDamageFire;
            numDamageSpark.Value = (decimal)Properties.Settings.Default.intensityMultiplierDamageSpark;
            numPlayerShootPistol.Value = (decimal)Properties.Settings.Default.intensityMultiplierPlayerShootPistol;
            numPlayerShootShotgun.Value = (decimal)Properties.Settings.Default.intensityMultiplierPlayerShootShotgun;
            numPlayerShootSMG.Value = (decimal)Properties.Settings.Default.intensityMultiplierPlayerShootSMG;
            numPlayerShootDefault.Value = (decimal)Properties.Settings.Default.intensityMultiplierPlayerShootDefault;
            numFallbackSMG.Value = (decimal)Properties.Settings.Default.intensityMultiplierFallbackSMG;
            numPlayerGrenadeLaunch.Value = (decimal)Properties.Settings.Default.intensityMultiplierPlayerGrenadeLaunch;
            numFallbackPistol.Value = (decimal)Properties.Settings.Default.intensityMultiplierFallbackPistol;
            numFallbackShotgun.Value = (decimal)Properties.Settings.Default.intensityMultiplierFallbackShotgun;
            numKickbackPistol.Value = (decimal)Properties.Settings.Default.intensityMultiplierKickbackPistol;
            numKickbackShotgun.Value = (decimal)Properties.Settings.Default.intensityMultiplierKickbackShotgun;
            numKickbackSMG.Value = (decimal)Properties.Settings.Default.intensityMultiplierKickbackSMG;
            numHeartbeat.Value = (decimal)Properties.Settings.Default.intensityMultiplierHeartBeat;
            numHeartbeatFast.Value = (decimal)Properties.Settings.Default.intensityMultiplierHeartBeatFast;
            numHealthPenUse.Value = (decimal)Properties.Settings.Default.intensityMultiplierHealthPenUse;
            numHealthStationUse.Value = (decimal)Properties.Settings.Default.intensityMultiplierHealthStationUse;
            numHealthStationUseArm.Value = (decimal)Properties.Settings.Default.intensityMultiplierHealthStationUseArm;
            numStoreBackpackClip.Value = (decimal)Properties.Settings.Default.intensityMultiplierBackpackStoreClip;
            numStoreBackpackResin.Value = (decimal)Properties.Settings.Default.intensityMultiplierBackpackStoreResin;
            numRetrieveBackpackClip.Value = (decimal)Properties.Settings.Default.intensityMultiplierBackpackRetrieveClip;
            numRetrieveBackpackResin.Value = (decimal)Properties.Settings.Default.intensityMultiplierBackpackRetrieveResin;
            numItemHolderStore.Value = (decimal)Properties.Settings.Default.intensityMultiplierItemHolderStore;
            numItemHolderRemove.Value = (decimal)Properties.Settings.Default.intensityMultiplierItemHolderRemove;
            numGravityGloveLockOn.Value = (decimal)Properties.Settings.Default.intensityMultiplierGravityGloveLockOn;
            numGravityGlovePull.Value = (decimal)Properties.Settings.Default.intensityMultiplierGravityGlovePull;
            numGravityGloveCatch.Value = (decimal)Properties.Settings.Default.intensityMultiplierGravityGloveCatch;
            numWeaponClipInsert.Value = (decimal)Properties.Settings.Default.intensityMultiplierClipInserted;
            numWeaponChamberRound.Value = (decimal)Properties.Settings.Default.intensityMultiplierChamberedRound;
            numCough.Value = (decimal)Properties.Settings.Default.intensityMultiplierCough;
            numCoughHead.Value = (decimal)Properties.Settings.Default.intensityMultiplierCoughHead;
            numEnvironmentalDefault.Value = (decimal)Properties.Settings.Default.intensityMultiplierDefault;

            numDurationHeartbeat.Value = Properties.Settings.Default.sleepDurationHeartBeat;
            numDurationHeartbeatFast.Value = Properties.Settings.Default.sleepDurationHeartBeatFast;
            numDurationHeartbeatTooFast.Value = Properties.Settings.Default.sleepDurationHeartBeatTooFast;
            numDurationGravityLock.Value = Properties.Settings.Default.sleepDurationGrabbityLock;
            numDurationBarnacleGrab.Value = Properties.Settings.Default.sleepDurationBarnacleGrab;
            numDurationCoughing.Value = Properties.Settings.Default.sleepDurationCoughing;

        }

        private void SaveSettings()
        {
            Properties.Settings.Default.intensityMultiplierDefaultHead = (float)numDefaultHead.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedHead = (float)numUnarmedHead.Value;
            Properties.Settings.Default.intensityMultiplierGunHead = (float)numGunHead.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedBloater = (float)numBloater.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedHeadcrab = (float)numHeadcrab.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabArmored = (float)numArmoredHeadcrab.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabBlack = (float)numToxicHeadcrab.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabFast = (float)numLightningDog.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabRunner = (float)numRunnerHeadcrab.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedFastZombie = (float)numFastZombie.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedPoisonZombie = (float)numPoisonZombie.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedZombie = (float)numZombie.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedZombieBlind = (float)numBlindZombie.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedZombine = (float)numZombine.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedAntlion = (float)numAntlion.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedAntlionGuard = (float)numAntlionGuard.Value;
            Properties.Settings.Default.intensityMultiplierUnarmedManhack = (float)numManhack.Value;
            Properties.Settings.Default.intensityMultiplierGrabbedByBarnacle = (float)numBarnacle.Value;
            Properties.Settings.Default.intensityMultiplierConcussionGrenade = (float)numConcussionGrenade.Value;
            Properties.Settings.Default.intensityMultiplierBugBaitGrenade = (float)numBugBaitGrenade.Value;
            Properties.Settings.Default.intensityMultiplierFragGrenade = (float)numFragGrenade.Value;
            Properties.Settings.Default.intensityMultiplierSpyGrenade = (float)numSpyGrenade.Value;
            Properties.Settings.Default.intensityMultiplierHandGrenade = (float)numHandGrenade.Value;
            Properties.Settings.Default.intensityMultiplierRollerGrenade = (float)numRollerGrenade.Value;
            Properties.Settings.Default.intensityMultiplierRollerMine = (float)numRollerMine.Value;
            Properties.Settings.Default.intensityMultiplierCombine = (float)numCombine.Value;
            Properties.Settings.Default.intensityMultiplierCombineGantry = (float)numCombineGantry.Value;
            Properties.Settings.Default.intensityMultiplierCombineS = (float)numCombineS.Value;
            Properties.Settings.Default.intensityMultiplierMetroPolice = (float)numMetroPolice.Value;
            Properties.Settings.Default.intensityMultiplierSniper = (float)numSniper.Value;
            Properties.Settings.Default.intensityMultiplierStrider = (float)numStrider.Value;
            Properties.Settings.Default.intensityMultiplierTurret = (float)numTurret.Value;
            Properties.Settings.Default.intensityMultiplierFoliageTurret = (float)numFoliageTurret.Value;
            Properties.Settings.Default.intensityMultiplierEnvironmentExplosion = (float)numEnvironmentalExplosion.Value;
            Properties.Settings.Default.intensityMultiplierEnvironmentLaser = (float)numEnvironmentalLaser.Value;
            Properties.Settings.Default.intensityMultiplierEnvironmentFire = (float)numEnvironmentalFire.Value;
            Properties.Settings.Default.intensityMultiplierEnvironmentSpark = (float)numEnvironmentalSpark.Value;
            Properties.Settings.Default.intensityMultiplierEnvironmentPoison = (float)numEnvironmentalPoison.Value;
            Properties.Settings.Default.intensityMultiplierEnvironmentRadiation = (float)numEnvironmentalRadiation.Value;
            Properties.Settings.Default.intensityMultiplierDamageExplosion = (float)numDamageExplosion.Value;
            Properties.Settings.Default.intensityMultiplierDamageLaser = (float)numDamageLaser.Value;
            Properties.Settings.Default.intensityMultiplierDamageFire = (float)numDamageFire.Value;
            Properties.Settings.Default.intensityMultiplierDamageSpark = (float)numDamageSpark.Value;
            Properties.Settings.Default.intensityMultiplierPlayerShootPistol = (float)numPlayerShootPistol.Value;
            Properties.Settings.Default.intensityMultiplierPlayerShootShotgun = (float)numPlayerShootShotgun.Value;
            Properties.Settings.Default.intensityMultiplierPlayerShootSMG = (float)numPlayerShootSMG.Value;
            Properties.Settings.Default.intensityMultiplierPlayerShootDefault = (float)numPlayerShootDefault.Value;
            Properties.Settings.Default.intensityMultiplierFallbackSMG = (float)numFallbackSMG.Value;
            Properties.Settings.Default.intensityMultiplierPlayerGrenadeLaunch = (float)numPlayerGrenadeLaunch.Value;
            Properties.Settings.Default.intensityMultiplierFallbackPistol = (float)numFallbackPistol.Value;
            Properties.Settings.Default.intensityMultiplierFallbackShotgun = (float)numFallbackShotgun.Value;
            Properties.Settings.Default.intensityMultiplierKickbackPistol = (float)numKickbackPistol.Value;
            Properties.Settings.Default.intensityMultiplierKickbackShotgun = (float)numKickbackShotgun.Value;
            Properties.Settings.Default.intensityMultiplierKickbackSMG = (float)numKickbackSMG.Value;
            Properties.Settings.Default.intensityMultiplierHeartBeat = (float)numHeartbeat.Value;
            Properties.Settings.Default.intensityMultiplierHeartBeatFast = (float)numHeartbeatFast.Value;
            Properties.Settings.Default.intensityMultiplierHealthPenUse = (float)numHealthPenUse.Value;
            Properties.Settings.Default.intensityMultiplierHealthStationUse = (float)numHealthStationUse.Value;
            Properties.Settings.Default.intensityMultiplierHealthStationUseArm = (float)numHealthStationUseArm.Value;
            Properties.Settings.Default.intensityMultiplierBackpackStoreClip = (float)numStoreBackpackClip.Value;
            Properties.Settings.Default.intensityMultiplierBackpackStoreResin = (float)numStoreBackpackResin.Value;
            Properties.Settings.Default.intensityMultiplierBackpackRetrieveClip = (float)numRetrieveBackpackClip.Value;
            Properties.Settings.Default.intensityMultiplierBackpackRetrieveResin = (float)numRetrieveBackpackResin.Value;
            Properties.Settings.Default.intensityMultiplierItemHolderStore = (float)numItemHolderStore.Value;
            Properties.Settings.Default.intensityMultiplierItemHolderRemove = (float)numItemHolderRemove.Value;
            Properties.Settings.Default.intensityMultiplierGravityGloveLockOn = (float)numGravityGloveLockOn.Value;
            Properties.Settings.Default.intensityMultiplierGravityGlovePull = (float)numGravityGlovePull.Value;
            Properties.Settings.Default.intensityMultiplierGravityGloveCatch = (float)numGravityGloveCatch.Value;
            Properties.Settings.Default.intensityMultiplierClipInserted = (float)numWeaponClipInsert.Value;
            Properties.Settings.Default.intensityMultiplierChamberedRound = (float)numWeaponChamberRound.Value;

            Properties.Settings.Default.intensityMultiplierCough = (float)numCough.Value;
            Properties.Settings.Default.intensityMultiplierCoughHead = (float)numCoughHead.Value;

            Properties.Settings.Default.intensityMultiplierShockOnHand = (float)numShockOnHand.Value;


            Properties.Settings.Default.intensityMultiplierDefault = (float)numEnvironmentalDefault.Value;

            Properties.Settings.Default.sleepDurationHeartBeat = (int)numDurationHeartbeat.Value;
            Properties.Settings.Default.sleepDurationHeartBeatFast = (int)numDurationHeartbeatFast.Value;
            Properties.Settings.Default.sleepDurationHeartBeatTooFast = (int)numDurationHeartbeatTooFast.Value;
            Properties.Settings.Default.sleepDurationGrabbityLock = (int)numDurationGravityLock.Value;
            Properties.Settings.Default.sleepDurationBarnacleGrab = (int)numDurationBarnacleGrab.Value;
            Properties.Settings.Default.sleepDurationCoughing = (int)numDurationCoughing.Value;

            Properties.Settings.Default.Save();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            MessageBox.Show("Saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.btnSettings.Enabled = true;
        }

        private void label_Click(object sender, EventArgs e)
        {
            SaveSettings();
            if (mainForm.tactsuitVr != null)
            {
                float locationHeight = ((float)(RandomNumber.Between(0, 100)) / 100.0f) - 0.5f;
                float locationAngle = (float)(RandomNumber.Between(0, 360));

                if (sender == lblIntensityDefaultHead) mainForm.tactsuitVr.ProvideHapticFeedback(0, locationHeight, TactsuitVR.FeedbackType.DefaultHead, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityUnarmedHead) mainForm.tactsuitVr.ProvideHapticFeedback(0, locationHeight, TactsuitVR.FeedbackType.UnarmedHead, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityGunHead) mainForm.tactsuitVr.ProvideHapticFeedback(0, locationHeight, TactsuitVR.FeedbackType.GunHead, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityBloaterZombie) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.UnarmedBloater, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityHeadcrab) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, 0.35f, TactsuitVR.FeedbackType.UnarmedHeadcrab, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityArmoredHeadcrab) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, 0.35f, TactsuitVR.FeedbackType.UnarmedHeadcrabArmored, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityToxicHeadcrab) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, 0.35f, TactsuitVR.FeedbackType.UnarmedHeadcrabBlack, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityLightningDog) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, 0.35f, TactsuitVR.FeedbackType.UnarmedHeadcrabFast, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityRunnerHeadcrab) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, 0.35f, TactsuitVR.FeedbackType.UnarmedHeadcrabRunner, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityFastZombie) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.UnarmedFastZombie, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityPoisonZombie) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.UnarmedPoisonZombie, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityZombie) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.UnarmedZombie, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityBlindZombie) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.UnarmedZombieBlind, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityZombine) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.UnarmedZombine, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityAntlion) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.UnarmedAntlion, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityAntlionGuard) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.UnarmedAntlionGuard, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityManhack) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.UnarmedManhack, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityBarnacle) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.GrabbedByBarnacle, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityConcussionGrenade) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.ConcussionGrenade, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityBugbaitGrenade) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.BugBaitGrenade, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityFragGrenade) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.FragGrenade, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensitySpyGrenade) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.SpyGrenade, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityHandGrenade) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HandGrenade, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityRollerGrenade) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.RollerGrenade, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityRollerMine) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.RollerMine, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityCombine) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.Combine, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityCombineHeavy) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.CombineS, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityCombineGantry) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.CombineGantry, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityMetroPolice) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.MetroPolice, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensitySniper) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.Sniper, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityStrider) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.Strider, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityTurret) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.Turret, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityFoliageTurret) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.FoliageTurret, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityEnvironmentalExplosion) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentExplosion, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityEnvironmentalLaser) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentLaser, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityEnvironmentalFire) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentFire, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityEnvironmentalSpark) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentSpark, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityEnvironmentalPoison) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentPoison, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityEnvironmentalRadiation) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentRadiation, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityDamageExplosion) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.DamageExplosion, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityDamageLaser) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.DamageLaser, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityDamageFire) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.DamageFire, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityDamageSpark) mainForm.tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, TactsuitVR.FeedbackType.DamageSpark, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityPlayerPistol) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.PlayerShootPistol, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityPlayerShotgun) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.PlayerShootShotgun, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityPlayerSMG) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.PlayerShootSMG, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityPlayerShootDefault) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.PlayerShootDefault, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityGrenadeLaunch) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.PlayerGrenadeLaunch, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityFallbackPistol) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.FallbackPistol, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityFallbackShotgun) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.FallbackShotgun, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityFallbackSMG) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.FallbackSMG, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityKickbackPistol) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.KickbackPistol, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityKickbackShotgun) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.KickbackShotgun, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityKickbackSMG) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.KickbackSMG, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityHeartbeat) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HeartBeat, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityHeartbeatFast) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HeartBeatFast, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityHealthPenUse) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HealthPenUse, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityHealthstationUse) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HealthStationUse, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityHealthstationArm) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HealthStationUseLeftArm, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityBackpackStoreClip) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.BackpackStoreClip, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityBackpackStoreResin) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.BackpackStoreResin, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityBackpackRetrieveClip) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.BackpackRetrieveClip, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityBackpackRetrieveResin) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.BackpackRetrieveResin, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityItemHolderStore) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.ItemHolderStore, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityItemHolderRemove) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.ItemHolderRemove, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityGravityGloveLock) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.GravityGloveLockOn, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityGravityGlovePull) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.GravityGlovePull, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityGravityGloveCatch) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.GravityGloveCatch, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityWeaponClipInsert) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.ClipInserted, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityWeaponChamberRound) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.ChamberedRound, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityCough) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.Cough, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityCoughHead) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.CoughHead, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityShockOnHand) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.ShockOnHandLeft, false, TactsuitVR.FeedbackType.NoFeedback);
                if (sender == lblIntensityEnvironmentalDefault) mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.DefaultDamage, false, TactsuitVR.FeedbackType.NoFeedback);

            }
        }

        private void lblDuration_Click(object sender, EventArgs e)
        {
            SaveSettings();

            if (mainForm.tactsuitVr != null)
            {
                if (sender == lblDurationHeartbeat)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HeartBeat, false,
                            TactsuitVR.FeedbackType.NoFeedback);

                        Thread.Sleep(Properties.Settings.Default.sleepDurationHeartBeat);
                    }
                }
                else if (sender == lblDurationHeartbeatFast)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HeartBeatFast, false,
                            TactsuitVR.FeedbackType.NoFeedback);

                        Thread.Sleep(Properties.Settings.Default.sleepDurationHeartBeatFast);
                    }
                }
                else if (sender == lblDurationHeartbeatTooFast)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HeartBeatFast, false,
                            TactsuitVR.FeedbackType.NoFeedback);

                        Thread.Sleep(Properties.Settings.Default.sleepDurationHeartBeatTooFast);
                    }
                }
                else if (sender == lblDurationGravityLock)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.GravityGloveLockOn, false,
                            TactsuitVR.FeedbackType.NoFeedback);

                        Thread.Sleep(Properties.Settings.Default.sleepDurationGrabbityLock);
                    }
                }
                else if (sender == lblDurationBarnacleGrab)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.GrabbedByBarnacle, false,
                            TactsuitVR.FeedbackType.NoFeedback);

                        Thread.Sleep(Properties.Settings.Default.sleepDurationBarnacleGrab);
                    }
                }
                else if (sender == lblDurationCoughing)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        mainForm.tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.Cough, false,
                            TactsuitVR.FeedbackType.NoFeedback);

                        Thread.Sleep(Properties.Settings.Default.sleepDurationCoughing);
                    }
                }
            }
        }
    }
}
