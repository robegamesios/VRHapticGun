using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactsuitAlyx
{
    static class Config
    {
        public static int LowHealthAmount = 33;
        public static int VeryLowHealthAmount = 20;
        public static int TooLowHealthAmount = 10;
        
        public static float GetIntensityMultiplier(TactsuitVR.FeedbackType feedbackType)
        {
            switch (feedbackType)
            {
                case TactsuitVR.FeedbackType.DefaultHead: return Properties.Settings.Default.intensityMultiplierDefaultHead; break;
                case TactsuitVR.FeedbackType.UnarmedHead: return Properties.Settings.Default.intensityMultiplierUnarmedHead; break;
                case TactsuitVR.FeedbackType.GunHead: return Properties.Settings.Default.intensityMultiplierGunHead; break;
                case TactsuitVR.FeedbackType.UnarmedBloater: return Properties.Settings.Default.intensityMultiplierUnarmedBloater; break;
                case TactsuitVR.FeedbackType.UnarmedHeadcrab: return Properties.Settings.Default.intensityMultiplierUnarmedHeadcrab; break;
                case TactsuitVR.FeedbackType.UnarmedHeadcrabArmored: return Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabArmored; break;
                case TactsuitVR.FeedbackType.UnarmedHeadcrabBlack: return Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabBlack; break;
                case TactsuitVR.FeedbackType.UnarmedHeadcrabFast: return Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabFast; break;
                case TactsuitVR.FeedbackType.UnarmedHeadcrabRunner: return Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabRunner; break;
                case TactsuitVR.FeedbackType.UnarmedFastZombie: return Properties.Settings.Default.intensityMultiplierUnarmedFastZombie; break;
                case TactsuitVR.FeedbackType.UnarmedPoisonZombie: return Properties.Settings.Default.intensityMultiplierUnarmedPoisonZombie; break;
                case TactsuitVR.FeedbackType.UnarmedZombie: return Properties.Settings.Default.intensityMultiplierUnarmedZombie; break;
                case TactsuitVR.FeedbackType.UnarmedZombieBlind: return Properties.Settings.Default.intensityMultiplierUnarmedZombieBlind; break;
                case TactsuitVR.FeedbackType.UnarmedZombine: return Properties.Settings.Default.intensityMultiplierUnarmedZombine; break;
                case TactsuitVR.FeedbackType.UnarmedAntlion: return Properties.Settings.Default.intensityMultiplierUnarmedAntlion; break;
                case TactsuitVR.FeedbackType.UnarmedAntlionGuard: return Properties.Settings.Default.intensityMultiplierUnarmedAntlionGuard; break;
                case TactsuitVR.FeedbackType.UnarmedManhack: return Properties.Settings.Default.intensityMultiplierUnarmedManhack; break;
                case TactsuitVR.FeedbackType.GrabbedByBarnacle: return Properties.Settings.Default.intensityMultiplierGrabbedByBarnacle; break;
                case TactsuitVR.FeedbackType.ConcussionGrenade: return Properties.Settings.Default.intensityMultiplierConcussionGrenade; break;
                case TactsuitVR.FeedbackType.BugBaitGrenade: return Properties.Settings.Default.intensityMultiplierBugBaitGrenade; break;
                case TactsuitVR.FeedbackType.FragGrenade: return Properties.Settings.Default.intensityMultiplierFragGrenade; break;
                case TactsuitVR.FeedbackType.SpyGrenade: return Properties.Settings.Default.intensityMultiplierSpyGrenade; break;
                case TactsuitVR.FeedbackType.HandGrenade: return Properties.Settings.Default.intensityMultiplierHandGrenade; break;
                case TactsuitVR.FeedbackType.RollerGrenade: return Properties.Settings.Default.intensityMultiplierRollerGrenade; break;
                case TactsuitVR.FeedbackType.RollerMine: return Properties.Settings.Default.intensityMultiplierRollerMine; break;
                case TactsuitVR.FeedbackType.Combine: return Properties.Settings.Default.intensityMultiplierCombine; break;
                case TactsuitVR.FeedbackType.CombineS: return Properties.Settings.Default.intensityMultiplierCombineS; break;
                case TactsuitVR.FeedbackType.CombineGantry: return Properties.Settings.Default.intensityMultiplierCombineGantry; break;
                case TactsuitVR.FeedbackType.MetroPolice: return Properties.Settings.Default.intensityMultiplierMetroPolice; break;
                case TactsuitVR.FeedbackType.Sniper: return Properties.Settings.Default.intensityMultiplierSniper; break;
                case TactsuitVR.FeedbackType.Strider: return Properties.Settings.Default.intensityMultiplierStrider; break;
                case TactsuitVR.FeedbackType.Turret: return Properties.Settings.Default.intensityMultiplierTurret; break;
                case TactsuitVR.FeedbackType.FoliageTurret: return Properties.Settings.Default.intensityMultiplierFoliageTurret; break;
                case TactsuitVR.FeedbackType.EnvironmentExplosion: return Properties.Settings.Default.intensityMultiplierEnvironmentExplosion; break;
                case TactsuitVR.FeedbackType.EnvironmentLaser: return Properties.Settings.Default.intensityMultiplierEnvironmentLaser; break;
                case TactsuitVR.FeedbackType.EnvironmentFire: return Properties.Settings.Default.intensityMultiplierEnvironmentFire; break;
                case TactsuitVR.FeedbackType.EnvironmentSpark: return Properties.Settings.Default.intensityMultiplierEnvironmentSpark; break;
                case TactsuitVR.FeedbackType.EnvironmentPoison: return Properties.Settings.Default.intensityMultiplierEnvironmentPoison; break;
                case TactsuitVR.FeedbackType.EnvironmentRadiation: return Properties.Settings.Default.intensityMultiplierEnvironmentRadiation; break;
                case TactsuitVR.FeedbackType.DamageExplosion: return Properties.Settings.Default.intensityMultiplierDamageExplosion; break;
                case TactsuitVR.FeedbackType.DamageLaser: return Properties.Settings.Default.intensityMultiplierDamageLaser; break;
                case TactsuitVR.FeedbackType.DamageFire: return Properties.Settings.Default.intensityMultiplierDamageFire; break;
                case TactsuitVR.FeedbackType.DamageSpark: return Properties.Settings.Default.intensityMultiplierDamageSpark; break;
                case TactsuitVR.FeedbackType.PlayerShootPistol: return Properties.Settings.Default.intensityMultiplierPlayerShootPistol; break;
                case TactsuitVR.FeedbackType.PlayerShootShotgun: return Properties.Settings.Default.intensityMultiplierPlayerShootShotgun; break;
                case TactsuitVR.FeedbackType.PlayerShootSMG: return Properties.Settings.Default.intensityMultiplierPlayerShootSMG; break;
                case TactsuitVR.FeedbackType.PlayerShootDefault: return Properties.Settings.Default.intensityMultiplierPlayerShootDefault; break;
                case TactsuitVR.FeedbackType.PlayerGrenadeLaunch: return Properties.Settings.Default.intensityMultiplierPlayerGrenadeLaunch; break;
                case TactsuitVR.FeedbackType.PlayerShootPistolLeft: return Properties.Settings.Default.intensityMultiplierPlayerShootPistol; break;
                case TactsuitVR.FeedbackType.PlayerShootShotgunLeft: return Properties.Settings.Default.intensityMultiplierPlayerShootShotgun; break;
                case TactsuitVR.FeedbackType.PlayerShootSMGLeft: return Properties.Settings.Default.intensityMultiplierPlayerShootSMG; break;
                case TactsuitVR.FeedbackType.PlayerShootDefaultLeft: return Properties.Settings.Default.intensityMultiplierPlayerShootDefault; break;
                case TactsuitVR.FeedbackType.PlayerGrenadeLaunchLeft: return Properties.Settings.Default.intensityMultiplierPlayerGrenadeLaunch; break;
                case TactsuitVR.FeedbackType.FallbackPistol: return Properties.Settings.Default.intensityMultiplierFallbackPistol; break;
                case TactsuitVR.FeedbackType.FallbackShotgun: return Properties.Settings.Default.intensityMultiplierFallbackShotgun; break;
                case TactsuitVR.FeedbackType.FallbackSMG: return Properties.Settings.Default.intensityMultiplierFallbackSMG; break;
                case TactsuitVR.FeedbackType.FallbackPistolLeft: return Properties.Settings.Default.intensityMultiplierFallbackPistol; break;
                case TactsuitVR.FeedbackType.FallbackShotgunLeft: return Properties.Settings.Default.intensityMultiplierFallbackShotgun; break;
                case TactsuitVR.FeedbackType.FallbackSMGLeft: return Properties.Settings.Default.intensityMultiplierFallbackSMG; break;
                case TactsuitVR.FeedbackType.KickbackPistol: return Properties.Settings.Default.intensityMultiplierKickbackPistol; break;
                case TactsuitVR.FeedbackType.KickbackShotgun: return Properties.Settings.Default.intensityMultiplierKickbackShotgun; break;
                case TactsuitVR.FeedbackType.KickbackSMG: return Properties.Settings.Default.intensityMultiplierKickbackSMG; break;
                case TactsuitVR.FeedbackType.KickbackPistolLeft: return Properties.Settings.Default.intensityMultiplierKickbackPistol; break;
                case TactsuitVR.FeedbackType.KickbackShotgunLeft: return Properties.Settings.Default.intensityMultiplierKickbackShotgun; break;
                case TactsuitVR.FeedbackType.KickbackSMGLeft: return Properties.Settings.Default.intensityMultiplierKickbackSMG; break;
                case TactsuitVR.FeedbackType.HeartBeat: return Properties.Settings.Default.intensityMultiplierHeartBeat; break;
                case TactsuitVR.FeedbackType.HeartBeatFast: return Properties.Settings.Default.intensityMultiplierHeartBeatFast; break;
                case TactsuitVR.FeedbackType.HealthPenUse: return Properties.Settings.Default.intensityMultiplierHealthPenUse; break;
                case TactsuitVR.FeedbackType.HealthStationUse: return Properties.Settings.Default.intensityMultiplierHealthStationUse; break;
                case TactsuitVR.FeedbackType.BackpackStoreClip: return Properties.Settings.Default.intensityMultiplierBackpackStoreClip; break;
                case TactsuitVR.FeedbackType.BackpackStoreResin: return Properties.Settings.Default.intensityMultiplierBackpackStoreResin; break;
                case TactsuitVR.FeedbackType.BackpackRetrieveClip: return Properties.Settings.Default.intensityMultiplierBackpackRetrieveClip; break;
                case TactsuitVR.FeedbackType.BackpackRetrieveResin: return Properties.Settings.Default.intensityMultiplierBackpackRetrieveResin; break;
                case TactsuitVR.FeedbackType.ItemHolderStore: return Properties.Settings.Default.intensityMultiplierItemHolderStore; break;
                case TactsuitVR.FeedbackType.ItemHolderRemove: return Properties.Settings.Default.intensityMultiplierItemHolderRemove; break;
                case TactsuitVR.FeedbackType.BackpackStoreClipLeft: return Properties.Settings.Default.intensityMultiplierBackpackStoreClip; break;
                case TactsuitVR.FeedbackType.BackpackStoreResinLeft: return Properties.Settings.Default.intensityMultiplierBackpackStoreResin; break;
                case TactsuitVR.FeedbackType.BackpackRetrieveClipLeft: return Properties.Settings.Default.intensityMultiplierBackpackRetrieveClip; break;
                case TactsuitVR.FeedbackType.BackpackRetrieveResinLeft: return Properties.Settings.Default.intensityMultiplierBackpackRetrieveResin; break;
                case TactsuitVR.FeedbackType.ItemHolderStoreLeft: return Properties.Settings.Default.intensityMultiplierItemHolderStore; break;
                case TactsuitVR.FeedbackType.ItemHolderRemoveLeft: return Properties.Settings.Default.intensityMultiplierItemHolderRemove; break;
                case TactsuitVR.FeedbackType.GravityGloveLockOn: return Properties.Settings.Default.intensityMultiplierGravityGloveLockOn; break;
                case TactsuitVR.FeedbackType.GravityGlovePull: return Properties.Settings.Default.intensityMultiplierGravityGlovePull; break;
                case TactsuitVR.FeedbackType.GravityGloveCatch: return Properties.Settings.Default.intensityMultiplierGravityGloveCatch; break;
                case TactsuitVR.FeedbackType.GravityGloveLockOnLeft: return Properties.Settings.Default.intensityMultiplierGravityGloveLockOn; break;
                case TactsuitVR.FeedbackType.GravityGlovePullLeft: return Properties.Settings.Default.intensityMultiplierGravityGlovePull; break;
                case TactsuitVR.FeedbackType.GravityGloveCatchLeft: return Properties.Settings.Default.intensityMultiplierGravityGloveCatch; break;

                case TactsuitVR.FeedbackType.ClipInserted: return Properties.Settings.Default.intensityMultiplierClipInserted; break;
                case TactsuitVR.FeedbackType.ClipInsertedLeft: return Properties.Settings.Default.intensityMultiplierClipInserted; break;
                case TactsuitVR.FeedbackType.ChamberedRound: return Properties.Settings.Default.intensityMultiplierChamberedRound; break;
                case TactsuitVR.FeedbackType.ChamberedRoundLeft: return Properties.Settings.Default.intensityMultiplierChamberedRound; break;

                case TactsuitVR.FeedbackType.Cough: return Properties.Settings.Default.intensityMultiplierCough; break;
                case TactsuitVR.FeedbackType.CoughHead: return Properties.Settings.Default.intensityMultiplierCoughHead; break;

                case TactsuitVR.FeedbackType.ShockOnHandLeft: return Properties.Settings.Default.intensityMultiplierShockOnHand; break;
                case TactsuitVR.FeedbackType.ShockOnHandRight: return Properties.Settings.Default.intensityMultiplierShockOnHand; break;


                case TactsuitVR.FeedbackType.DefaultDamage: return Properties.Settings.Default.intensityMultiplierDefault; break;
            }

            return Properties.Settings.Default.intensityMultiplierDefault;
        }
    }
}
