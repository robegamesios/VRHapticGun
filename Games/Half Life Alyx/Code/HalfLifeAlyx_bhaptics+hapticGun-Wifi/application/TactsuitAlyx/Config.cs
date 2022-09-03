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
                case TactsuitVR.FeedbackType.DefaultHead: return Properties.Settings.Default.intensityMultiplierDefaultHead;
                case TactsuitVR.FeedbackType.UnarmedHead: return Properties.Settings.Default.intensityMultiplierUnarmedHead;
                case TactsuitVR.FeedbackType.GunHead: return Properties.Settings.Default.intensityMultiplierGunHead;
                case TactsuitVR.FeedbackType.UnarmedBloater: return Properties.Settings.Default.intensityMultiplierUnarmedBloater;
                case TactsuitVR.FeedbackType.UnarmedHeadcrab: return Properties.Settings.Default.intensityMultiplierUnarmedHeadcrab;
                case TactsuitVR.FeedbackType.UnarmedHeadcrabArmored: return Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabArmored;
                case TactsuitVR.FeedbackType.UnarmedHeadcrabBlack: return Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabBlack;
                case TactsuitVR.FeedbackType.UnarmedHeadcrabFast: return Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabFast;
                case TactsuitVR.FeedbackType.UnarmedHeadcrabRunner: return Properties.Settings.Default.intensityMultiplierUnarmedHeadcrabRunner;
                case TactsuitVR.FeedbackType.UnarmedFastZombie: return Properties.Settings.Default.intensityMultiplierUnarmedFastZombie;
                case TactsuitVR.FeedbackType.UnarmedPoisonZombie: return Properties.Settings.Default.intensityMultiplierUnarmedPoisonZombie;
                case TactsuitVR.FeedbackType.UnarmedZombie: return Properties.Settings.Default.intensityMultiplierUnarmedZombie;
                case TactsuitVR.FeedbackType.UnarmedZombieBlind: return Properties.Settings.Default.intensityMultiplierUnarmedZombieBlind;
                case TactsuitVR.FeedbackType.UnarmedZombine: return Properties.Settings.Default.intensityMultiplierUnarmedZombine;
                case TactsuitVR.FeedbackType.UnarmedAntlion: return Properties.Settings.Default.intensityMultiplierUnarmedAntlion;
                case TactsuitVR.FeedbackType.UnarmedAntlionGuard: return Properties.Settings.Default.intensityMultiplierUnarmedAntlionGuard;
                case TactsuitVR.FeedbackType.UnarmedManhack: return Properties.Settings.Default.intensityMultiplierUnarmedManhack;
                case TactsuitVR.FeedbackType.GrabbedByBarnacle: return Properties.Settings.Default.intensityMultiplierGrabbedByBarnacle;
                case TactsuitVR.FeedbackType.ConcussionGrenade: return Properties.Settings.Default.intensityMultiplierConcussionGrenade;
                case TactsuitVR.FeedbackType.BugBaitGrenade: return Properties.Settings.Default.intensityMultiplierBugBaitGrenade;
                case TactsuitVR.FeedbackType.FragGrenade: return Properties.Settings.Default.intensityMultiplierFragGrenade;
                case TactsuitVR.FeedbackType.SpyGrenade: return Properties.Settings.Default.intensityMultiplierSpyGrenade;
                case TactsuitVR.FeedbackType.HandGrenade: return Properties.Settings.Default.intensityMultiplierHandGrenade;
                case TactsuitVR.FeedbackType.RollerGrenade: return Properties.Settings.Default.intensityMultiplierRollerGrenade;
                case TactsuitVR.FeedbackType.RollerMine: return Properties.Settings.Default.intensityMultiplierRollerMine;
                case TactsuitVR.FeedbackType.Combine: return Properties.Settings.Default.intensityMultiplierCombine;
                case TactsuitVR.FeedbackType.CombineS: return Properties.Settings.Default.intensityMultiplierCombineS;
                case TactsuitVR.FeedbackType.CombineGantry: return Properties.Settings.Default.intensityMultiplierCombineGantry;
                case TactsuitVR.FeedbackType.MetroPolice: return Properties.Settings.Default.intensityMultiplierMetroPolice;
                case TactsuitVR.FeedbackType.Sniper: return Properties.Settings.Default.intensityMultiplierSniper;
                case TactsuitVR.FeedbackType.Strider: return Properties.Settings.Default.intensityMultiplierStrider;
                case TactsuitVR.FeedbackType.Turret: return Properties.Settings.Default.intensityMultiplierTurret;
                case TactsuitVR.FeedbackType.FoliageTurret: return Properties.Settings.Default.intensityMultiplierFoliageTurret;
                case TactsuitVR.FeedbackType.EnvironmentExplosion: return Properties.Settings.Default.intensityMultiplierEnvironmentExplosion;
                case TactsuitVR.FeedbackType.EnvironmentLaser: return Properties.Settings.Default.intensityMultiplierEnvironmentLaser;
                case TactsuitVR.FeedbackType.EnvironmentFire: return Properties.Settings.Default.intensityMultiplierEnvironmentFire;
                case TactsuitVR.FeedbackType.EnvironmentSpark: return Properties.Settings.Default.intensityMultiplierEnvironmentSpark;
                case TactsuitVR.FeedbackType.EnvironmentPoison: return Properties.Settings.Default.intensityMultiplierEnvironmentPoison;
                case TactsuitVR.FeedbackType.EnvironmentRadiation: return Properties.Settings.Default.intensityMultiplierEnvironmentRadiation;
                case TactsuitVR.FeedbackType.DamageExplosion: return Properties.Settings.Default.intensityMultiplierDamageExplosion;
                case TactsuitVR.FeedbackType.DamageLaser: return Properties.Settings.Default.intensityMultiplierDamageLaser;
                case TactsuitVR.FeedbackType.DamageFire: return Properties.Settings.Default.intensityMultiplierDamageFire;
                case TactsuitVR.FeedbackType.DamageSpark: return Properties.Settings.Default.intensityMultiplierDamageSpark;
                case TactsuitVR.FeedbackType.PlayerShootPistol: return Properties.Settings.Default.intensityMultiplierPlayerShootPistol;
                case TactsuitVR.FeedbackType.PlayerShootShotgun: return Properties.Settings.Default.intensityMultiplierPlayerShootShotgun;
                case TactsuitVR.FeedbackType.PlayerShootSMG: return Properties.Settings.Default.intensityMultiplierPlayerShootSMG;
                case TactsuitVR.FeedbackType.PlayerShootDefault: return Properties.Settings.Default.intensityMultiplierPlayerShootDefault;
                case TactsuitVR.FeedbackType.PlayerGrenadeLaunch: return Properties.Settings.Default.intensityMultiplierPlayerGrenadeLaunch;
                case TactsuitVR.FeedbackType.PlayerShootPistolLeft: return Properties.Settings.Default.intensityMultiplierPlayerShootPistol;
                case TactsuitVR.FeedbackType.PlayerShootShotgunLeft: return Properties.Settings.Default.intensityMultiplierPlayerShootShotgun;
                case TactsuitVR.FeedbackType.PlayerShootSMGLeft: return Properties.Settings.Default.intensityMultiplierPlayerShootSMG;
                case TactsuitVR.FeedbackType.PlayerShootDefaultLeft: return Properties.Settings.Default.intensityMultiplierPlayerShootDefault;
                case TactsuitVR.FeedbackType.PlayerGrenadeLaunchLeft: return Properties.Settings.Default.intensityMultiplierPlayerGrenadeLaunch;
                case TactsuitVR.FeedbackType.FallbackPistol: return Properties.Settings.Default.intensityMultiplierFallbackPistol;
                case TactsuitVR.FeedbackType.FallbackShotgun: return Properties.Settings.Default.intensityMultiplierFallbackShotgun;
                case TactsuitVR.FeedbackType.FallbackSMG: return Properties.Settings.Default.intensityMultiplierFallbackSMG;
                case TactsuitVR.FeedbackType.FallbackPistolLeft: return Properties.Settings.Default.intensityMultiplierFallbackPistol;
                case TactsuitVR.FeedbackType.FallbackShotgunLeft: return Properties.Settings.Default.intensityMultiplierFallbackShotgun;
                case TactsuitVR.FeedbackType.FallbackSMGLeft: return Properties.Settings.Default.intensityMultiplierFallbackSMG;
                case TactsuitVR.FeedbackType.KickbackPistol: return Properties.Settings.Default.intensityMultiplierKickbackPistol;
                case TactsuitVR.FeedbackType.KickbackShotgun: return Properties.Settings.Default.intensityMultiplierKickbackShotgun;
                case TactsuitVR.FeedbackType.KickbackSMG: return Properties.Settings.Default.intensityMultiplierKickbackSMG;
                case TactsuitVR.FeedbackType.KickbackPistolLeft: return Properties.Settings.Default.intensityMultiplierKickbackPistol;
                case TactsuitVR.FeedbackType.KickbackShotgunLeft: return Properties.Settings.Default.intensityMultiplierKickbackShotgun;
                case TactsuitVR.FeedbackType.KickbackSMGLeft: return Properties.Settings.Default.intensityMultiplierKickbackSMG;
                case TactsuitVR.FeedbackType.HeartBeat: return Properties.Settings.Default.intensityMultiplierHeartBeat;
                case TactsuitVR.FeedbackType.HeartBeatFast: return Properties.Settings.Default.intensityMultiplierHeartBeatFast;
                case TactsuitVR.FeedbackType.HealthPenUse: return Properties.Settings.Default.intensityMultiplierHealthPenUse;
                case TactsuitVR.FeedbackType.HealthStationUse: return Properties.Settings.Default.intensityMultiplierHealthStationUse;
                case TactsuitVR.FeedbackType.BackpackStoreClip: return Properties.Settings.Default.intensityMultiplierBackpackStoreClip;
                case TactsuitVR.FeedbackType.BackpackStoreResin: return Properties.Settings.Default.intensityMultiplierBackpackStoreResin;
                case TactsuitVR.FeedbackType.BackpackRetrieveClip: return Properties.Settings.Default.intensityMultiplierBackpackRetrieveClip;
                case TactsuitVR.FeedbackType.BackpackRetrieveResin: return Properties.Settings.Default.intensityMultiplierBackpackRetrieveResin;
                case TactsuitVR.FeedbackType.ItemHolderStore: return Properties.Settings.Default.intensityMultiplierItemHolderStore;
                case TactsuitVR.FeedbackType.ItemHolderRemove: return Properties.Settings.Default.intensityMultiplierItemHolderRemove;
                case TactsuitVR.FeedbackType.BackpackStoreClipLeft: return Properties.Settings.Default.intensityMultiplierBackpackStoreClip;
                case TactsuitVR.FeedbackType.BackpackStoreResinLeft: return Properties.Settings.Default.intensityMultiplierBackpackStoreResin;
                case TactsuitVR.FeedbackType.BackpackRetrieveClipLeft: return Properties.Settings.Default.intensityMultiplierBackpackRetrieveClip;
                case TactsuitVR.FeedbackType.BackpackRetrieveResinLeft: return Properties.Settings.Default.intensityMultiplierBackpackRetrieveResin;
                case TactsuitVR.FeedbackType.ItemHolderStoreLeft: return Properties.Settings.Default.intensityMultiplierItemHolderStore;
                case TactsuitVR.FeedbackType.ItemHolderRemoveLeft: return Properties.Settings.Default.intensityMultiplierItemHolderRemove;
                case TactsuitVR.FeedbackType.GravityGloveLockOn: return Properties.Settings.Default.intensityMultiplierGravityGloveLockOn;
                case TactsuitVR.FeedbackType.GravityGlovePull: return Properties.Settings.Default.intensityMultiplierGravityGlovePull;
                case TactsuitVR.FeedbackType.GravityGloveCatch: return Properties.Settings.Default.intensityMultiplierGravityGloveCatch;
                case TactsuitVR.FeedbackType.GravityGloveLockOnLeft: return Properties.Settings.Default.intensityMultiplierGravityGloveLockOn;
                case TactsuitVR.FeedbackType.GravityGlovePullLeft: return Properties.Settings.Default.intensityMultiplierGravityGlovePull;
                case TactsuitVR.FeedbackType.GravityGloveCatchLeft: return Properties.Settings.Default.intensityMultiplierGravityGloveCatch;

                case TactsuitVR.FeedbackType.ClipInserted: return Properties.Settings.Default.intensityMultiplierClipInserted;
                case TactsuitVR.FeedbackType.ClipInsertedLeft: return Properties.Settings.Default.intensityMultiplierClipInserted;
                case TactsuitVR.FeedbackType.ChamberedRound: return Properties.Settings.Default.intensityMultiplierChamberedRound;
                case TactsuitVR.FeedbackType.ChamberedRoundLeft: return Properties.Settings.Default.intensityMultiplierChamberedRound;

                case TactsuitVR.FeedbackType.Cough: return Properties.Settings.Default.intensityMultiplierCough;
                case TactsuitVR.FeedbackType.CoughHead: return Properties.Settings.Default.intensityMultiplierCoughHead;

                case TactsuitVR.FeedbackType.ShockOnHandLeft: return Properties.Settings.Default.intensityMultiplierShockOnHand;
                case TactsuitVR.FeedbackType.ShockOnHandRight: return Properties.Settings.Default.intensityMultiplierShockOnHand;


                case TactsuitVR.FeedbackType.DefaultDamage: return Properties.Settings.Default.intensityMultiplierDefault;
            }

            return Properties.Settings.Default.intensityMultiplierDefault;
        }
    }
}
