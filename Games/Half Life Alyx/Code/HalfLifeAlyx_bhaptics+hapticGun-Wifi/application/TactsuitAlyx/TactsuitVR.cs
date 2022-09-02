using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Bhaptics.Tact;

namespace TactsuitAlyx
{
    public class TactsuitVR
    {
        public TactsuitVR()
        {
            FillFeedbackList();
        }

        public bool systemInitialized = false;
        public HapticPlayer hapticPlayer;
        
        Dictionary<FeedbackType, Feedback> feedbackMap = new Dictionary<FeedbackType, Feedback>();

        public enum FeedbackType
        {
            //Attacks on Player's head
            DefaultHead,
            UnarmedHead,
            GunHead,

            //Unarmed Enemies
            UnarmedBloater,
            UnarmedHeadcrab,
            UnarmedHeadcrabArmored,
            UnarmedHeadcrabBlack, //Toxic
            UnarmedHeadcrabFast, //lightning dog
            UnarmedHeadcrabRunner, 
            UnarmedFastZombie,
            UnarmedPoisonZombie,
            UnarmedZombie,
            UnarmedZombieBlind,
            UnarmedZombine,
            UnarmedAntlion,
            UnarmedAntlionGuard,
            UnarmedManhack,

            GrabbedByBarnacle,

            //Grenade/Mine
            ConcussionGrenade,
            BugBaitGrenade,
            FragGrenade,
            SpyGrenade,
            HandGrenade,
            RollerGrenade,
            RollerMine,

            //Enemies with guns
            Combine,
            CombineS,

            CombineGantry,


            MetroPolice,
            Sniper,
            Strider,
            Turret,
            FoliageTurret,
            
            //On whole body
            EnvironmentExplosion,
            EnvironmentLaser,
            EnvironmentFire,
            EnvironmentSpark,
            EnvironmentPoison,
            EnvironmentRadiation,

            //Uses location
            DamageExplosion,
            DamageLaser,
            DamageFire,
            DamageSpark,

            //Player weapon shoot
            PlayerShootPistol,
            PlayerShootShotgun,
            PlayerShootSMG,

            PlayerShootDefault,

            PlayerGrenadeLaunch,

            PlayerShootPistolLeft,
            PlayerShootShotgunLeft,
            PlayerShootSMGLeft,

            PlayerShootDefaultLeft,

            PlayerGrenadeLaunchLeft,

            FallbackPistol,
            FallbackShotgun,
            FallbackSMG,

            FallbackPistolLeft,
            FallbackShotgunLeft,
            FallbackSMGLeft,

            KickbackPistol,
            KickbackShotgun,
            KickbackSMG,

            KickbackPistolLeft,
            KickbackShotgunLeft,
            KickbackSMGLeft,

            //Special stuff
            HeartBeat,
            HeartBeatFast,

            HealthPenUse,
            HealthStationUse,
            HealthStationUseLeftArm,
            HealthStationUseRightArm,

            BackpackStoreClip,
            BackpackStoreResin,
            BackpackRetrieveClip,
            BackpackRetrieveResin,
            ItemHolderStore,
            ItemHolderRemove,

            BackpackStoreClipLeft,
            BackpackStoreResinLeft,
            BackpackRetrieveClipLeft,
            BackpackRetrieveResinLeft,
            ItemHolderStoreLeft,
            ItemHolderRemoveLeft,

            GravityGloveLockOn,
            GravityGlovePull,
            GravityGloveCatch,

            GravityGloveLockOnLeft,
            GravityGlovePullLeft,
            GravityGloveCatchLeft,

            ClipInserted,
            ChamberedRound,

            ClipInsertedLeft,
            ChamberedRoundLeft,

            Cough,
            CoughHead,

            ShockOnHandLeft,
            ShockOnHandRight,

            DefaultDamage,

            NoFeedback
        }

        struct Feedback
        {
            public Feedback(FeedbackType _feedbackType, string _prefix, int _feedbackFileCount)
            {
                feedbackType = _feedbackType;
                prefix = _prefix;
                feedbackFileCount = _feedbackFileCount;
            }
            public FeedbackType feedbackType;
            public string prefix;
            public int feedbackFileCount;
        };

        public bool HeadCrabFeedback(FeedbackType feedback)
        {
            return feedback == FeedbackType.UnarmedHeadcrab
                   || feedback == FeedbackType.UnarmedHeadcrabArmored
                   || feedback == FeedbackType.UnarmedHeadcrabBlack
                   || feedback == FeedbackType.UnarmedHeadcrabFast
                   || feedback == FeedbackType.UnarmedHeadcrabRunner;
        }

        public bool EnvironmentFeedback(FeedbackType feedback)
        {
            return feedback == FeedbackType.DefaultDamage
                   || feedback == FeedbackType.EnvironmentExplosion
                   || feedback == FeedbackType.EnvironmentFire
                   || feedback == FeedbackType.EnvironmentLaser
                   || feedback == FeedbackType.EnvironmentSpark
                   || feedback == FeedbackType.EnvironmentPoison
                   || feedback == FeedbackType.EnvironmentRadiation;
        }

        public FeedbackType GetHeadFeedbackVersion(FeedbackType feedback)
        {
            if (feedback == FeedbackType.UnarmedHeadcrab
                || feedback == FeedbackType.UnarmedHeadcrabArmored
                || feedback == FeedbackType.UnarmedHeadcrabBlack
                || feedback == FeedbackType.UnarmedHeadcrabFast
                || feedback == FeedbackType.UnarmedHeadcrabRunner
                || feedback == FeedbackType.UnarmedAntlion
                || feedback == FeedbackType.UnarmedAntlionGuard
                || feedback == FeedbackType.UnarmedFastZombie
                || feedback == FeedbackType.UnarmedPoisonZombie
                || feedback == FeedbackType.UnarmedManhack
                || feedback == FeedbackType.UnarmedZombie
                || feedback == FeedbackType.UnarmedZombieBlind
                || feedback == FeedbackType.UnarmedZombine
            )
            {
                return FeedbackType.UnarmedHead;
            }
            else if(feedback == FeedbackType.Combine
                    || feedback == FeedbackType.CombineS
                    || feedback == FeedbackType.CombineGantry
                    || feedback == FeedbackType.MetroPolice
                    || feedback == FeedbackType.FoliageTurret
                    || feedback == FeedbackType.Turret
                    || feedback == FeedbackType.Sniper
                    || feedback == FeedbackType.Strider)
            {
                return FeedbackType.GunHead;
            }
            else
            {
                return FeedbackType.NoFeedback;
            }
        }
        
        public FeedbackType GetOtherHandFeedback(FeedbackType feedback)
        {
            switch (feedback)
            {
                case FeedbackType.PlayerShootPistol:
                    return FeedbackType.PlayerShootPistolLeft;
                    break;
                case FeedbackType.PlayerShootShotgun:
                    return FeedbackType.PlayerShootShotgunLeft;
                    break;
                case FeedbackType.PlayerShootSMG:
                    return FeedbackType.PlayerShootSMGLeft;
                    break;
                case FeedbackType.PlayerShootPistolLeft:
                    return FeedbackType.PlayerShootPistol;
                    break;
                case FeedbackType.PlayerShootShotgunLeft:
                    return FeedbackType.PlayerShootShotgun;
                    break;
                case FeedbackType.PlayerShootSMGLeft:
                    return FeedbackType.PlayerShootSMG;
                    break;
                case FeedbackType.PlayerShootDefault:
                    return FeedbackType.PlayerShootDefaultLeft;
                    break;
                case FeedbackType.PlayerShootDefaultLeft:
                    return FeedbackType.PlayerShootDefault;
                    break;
                default:
                    return FeedbackType.NoFeedback;
                    break;
            }

            return FeedbackType.NoFeedback;
        }

        public FeedbackType GetFallbackTypeOfWeaponFromPlayer(FeedbackType feedback, bool leftHanded)
        {
            switch (feedback)
            {
                case FeedbackType.PlayerShootPistol:
                    return leftHanded ? FeedbackType.FallbackPistolLeft : FeedbackType.FallbackPistol;
                    break;
                case FeedbackType.PlayerShootShotgun:
                    return leftHanded ? FeedbackType.FallbackShotgunLeft : FeedbackType.FallbackShotgun;
                    break;
                case FeedbackType.PlayerShootSMG:
                    return leftHanded ? FeedbackType.FallbackSMGLeft : FeedbackType.FallbackSMG;
                    break;
                case FeedbackType.PlayerShootPistolLeft:
                    return leftHanded ? FeedbackType.FallbackPistolLeft : FeedbackType.FallbackPistol;
                    break;
                case FeedbackType.PlayerShootShotgunLeft:
                    return leftHanded ? FeedbackType.FallbackShotgunLeft : FeedbackType.FallbackShotgun;
                    break;
                case FeedbackType.PlayerShootSMGLeft:
                    return leftHanded ? FeedbackType.FallbackSMGLeft : FeedbackType.FallbackSMG;
                    break;
                case FeedbackType.PlayerShootDefault:
                    return leftHanded ? FeedbackType.FallbackPistolLeft : FeedbackType.FallbackPistol;
                    break;
                case FeedbackType.PlayerShootDefaultLeft:
                    return leftHanded ? FeedbackType.FallbackPistolLeft : FeedbackType.FallbackPistol;
                    break;
                default:
                    return FeedbackType.NoFeedback;
                    break;
            }

            return FeedbackType.NoFeedback;
        }

        public FeedbackType GetKickbackOfWeaponFromPlayer(FeedbackType feedback, bool leftHanded)
        {
            switch (feedback)
            {
                case FeedbackType.PlayerShootPistol:
                    return leftHanded ? FeedbackType.KickbackPistolLeft : FeedbackType.KickbackPistol;
                    break;
                case FeedbackType.PlayerShootShotgun:
                    return leftHanded ? FeedbackType.KickbackShotgunLeft : FeedbackType.KickbackShotgun;
                    break;
                case FeedbackType.PlayerShootSMG:
                    return leftHanded ? FeedbackType.KickbackSMGLeft : FeedbackType.KickbackSMG;
                    break;
                case FeedbackType.PlayerGrenadeLaunch:
                    return leftHanded ? FeedbackType.KickbackPistolLeft : FeedbackType.KickbackPistol;
                    break;
                case FeedbackType.PlayerShootDefault:
                    return leftHanded ? FeedbackType.KickbackPistolLeft : FeedbackType.FallbackPistol;
                    break;
                case FeedbackType.PlayerShootPistolLeft:
                    return leftHanded ? FeedbackType.KickbackPistolLeft : FeedbackType.KickbackPistol;
                    break;
                case FeedbackType.PlayerShootShotgunLeft:
                    return leftHanded ? FeedbackType.KickbackShotgunLeft : FeedbackType.KickbackShotgun;
                    break;
                case FeedbackType.PlayerShootSMGLeft:
                    return leftHanded ? FeedbackType.KickbackSMGLeft : FeedbackType.KickbackSMG;
                    break;
                case FeedbackType.PlayerGrenadeLaunchLeft:
                    return leftHanded ? FeedbackType.KickbackPistolLeft : FeedbackType.KickbackPistol;
                    break;
                case FeedbackType.PlayerShootDefaultLeft:
                    return leftHanded ? FeedbackType.KickbackPistolLeft : FeedbackType.FallbackPistol;
                    break;
                default:
                    return leftHanded ? FeedbackType.KickbackPistolLeft : FeedbackType.FallbackPistol;
                    break;
            }

            return leftHanded ? FeedbackType.KickbackPistolLeft : FeedbackType.FallbackPistol;
        }

        public FeedbackType GetFeedbackTypeOfWeaponFromPlayer(string weapon, bool leftHanded)
        {
            switch (weapon)
            {
                case "hlvr_weapon_crowbar":
                case "hlvr_weapon_crowbar_physics":
                case "hlvr_weapon_energygun":
                    return leftHanded ? FeedbackType.PlayerShootPistolLeft : FeedbackType.PlayerShootPistol;
                    break;
                case "hlvr_weapon_shotgun":
                    return leftHanded ? FeedbackType.PlayerShootShotgunLeft : FeedbackType.PlayerShootShotgun;
                    break;
                case "hlvr_weapon_rapidfire":
                case "hlvr_weapon_rapidfire_ammo_capsule":
                case "hlvr_weapon_rapidfire_bullets_manager":
                case "hlvr_weapon_rapidfire_energy_ball":
                case "hlvr_weapon_rapidfire_extended_magazine":
                case "hlvr_weapon_rapidfire_tag_dart":
                case "hlvr_weapon_rapidfire_tag_marker":
                case "hlvr_weapon_rapidfire_upgrade_model":
                    return leftHanded ? FeedbackType.PlayerShootSMGLeft : FeedbackType.PlayerShootSMG;
                    break;
                default:
                    return leftHanded ? FeedbackType.PlayerShootDefaultLeft : FeedbackType.PlayerShootDefault;
                    break;
            }

            return leftHanded ? FeedbackType.PlayerShootDefaultLeft : FeedbackType.PlayerShootDefault;
        }

        public FeedbackType GetFeedbackTypeOfEnemyAttack(string enemy, string enemyName)
        {
            if(enemy == "npc_combine_s" && enemyName.Contains("gantry"))
            { 
                return FeedbackType.CombineGantry;
            }

            if (enemy.Contains("grenade") || enemy.Contains("mine"))
            {
                if(enemy.Contains("concussion"))
                { 
                    return FeedbackType.ConcussionGrenade;
                }
                else if (enemy.Contains("hand") || enemy.Contains("xen"))
                {
                    return FeedbackType.HandGrenade;
                }
                else if (enemy.Contains("bugbait"))
                {
                    return FeedbackType.BugBaitGrenade;
                }
                else if (enemy.Contains("frag"))
                {
                    return FeedbackType.FragGrenade;
                }
                else if (enemy.Contains("spy"))
                {
                    return FeedbackType.SpyGrenade;
                }
                else if (enemy.Contains("rollergrenade"))
                {
                    return FeedbackType.RollerGrenade;
                }
                else if (enemy.Contains("rollermine"))
                {
                    return FeedbackType.RollerMine;
                }
                else if (enemy.Contains("hand"))
                {
                    return FeedbackType.HandGrenade;
                }
                else
                {
                    return FeedbackType.FragGrenade;
                }
            }
            
            switch (enemy)
            {
                case "npc_headcrab":
                    return FeedbackType.UnarmedHeadcrab;
                    break;
                case "npc_headcrab_armored":
                    return FeedbackType.UnarmedHeadcrabArmored;
                    break;
                case "npc_headcrab_black":
                    return FeedbackType.UnarmedHeadcrabBlack;
                    break;
                case "npc_headcrab_fast":
                    return FeedbackType.UnarmedHeadcrabFast;
                    break;
                case "npc_headcrab_runner":
                    return FeedbackType.UnarmedHeadcrabRunner;
                    break;
                case "npc_fastzombie":
                    return FeedbackType.UnarmedFastZombie;
                    break;
                case "npc_poisonzombie":
                    return FeedbackType.UnarmedPoisonZombie;
                    break;
                case "npc_zombie":
                    return FeedbackType.UnarmedZombie;
                    break;
                case "npc_zombie_blind":
                    return FeedbackType.UnarmedZombieBlind;
                    break;
                case "npc_zombine":
                    return FeedbackType.UnarmedZombine;
                    break;
                case "npc_manhack":
                    return FeedbackType.UnarmedManhack;
                    break;
                case "npc_antlion":
                    return FeedbackType.UnarmedAntlion;
                    break;
                case "npc_antlionguard":
                case "npc_barnacle":
                case "npc_barnacle_tongue_tip":
                    return FeedbackType.UnarmedAntlionGuard;
                    break;
                case "xen_foliage_bloater":
                    return FeedbackType.UnarmedBloater;
                    break;
                case "env_explosion":
                    return FeedbackType.DamageExplosion;
                    break;
                case "env_fire":
                    return FeedbackType.DamageFire;
                    break;
                case "env_laser":
                    return FeedbackType.DamageLaser;
                    break;
                case "env_physexplosion":
                    return FeedbackType.DamageExplosion;
                    break;
                case "env_physimpact":
                    return FeedbackType.DamageExplosion;
                    break;
                case "env_spark":
                    return FeedbackType.DamageSpark;
                    break;
                case "npc_combine": return FeedbackType.Combine; break;
                case "npc_combine_s": return FeedbackType.CombineS; break;
                case "npc_metropolice": return FeedbackType.MetroPolice; break;
                case "npc_sniper": return FeedbackType.Sniper; break;
                case "npc_strider": return FeedbackType.Strider; break;
                case "npc_hunter": return FeedbackType.DamageExplosion; break;
                case "npc_hunter_invincible": return FeedbackType.DamageExplosion; break;
                case "npc_turret_ceiling": return FeedbackType.Turret; break;
                case "npc_turret_ceiling_pulse": return FeedbackType.Turret; break;
                case "npc_turret_citizen": return FeedbackType.Turret; break;
                case "npc_turret_floor": return FeedbackType.Turret; break;
                case "xen_foliage_turret": return FeedbackType.FoliageTurret; break;
                case "xen_foliage_turret_projectile": return FeedbackType.FoliageTurret; break;
            }

            if (enemy == "prop_physics" && enemyName.Contains("grenade"))
            {
                if (enemyName.Contains("concussion"))
                    return FeedbackType.ConcussionGrenade;
                else if (enemyName.Contains("hand") || enemyName.Contains("xen"))
                    return FeedbackType.HandGrenade;
                else if (enemyName.Contains("bugbait"))
                    return FeedbackType.BugBaitGrenade;
                else if (enemyName.Contains("frag"))
                    return FeedbackType.FragGrenade;
                else if (enemyName.Contains("spy"))
                    return FeedbackType.SpyGrenade;
                else if (enemyName.Contains("roller"))
                    return FeedbackType.RollerGrenade;
            }
            else if (enemy == "prop_physics" && enemyName.Contains("mine"))
            {
                return FeedbackType.RollerMine;
            }

            return FeedbackType.DefaultDamage;
        }

        void FillFeedbackList()
        {
            feedbackMap.Clear();
            feedbackMap.Add(FeedbackType.DefaultHead, new Feedback(FeedbackType.DefaultHead, "DefaultHead_", 0));
            feedbackMap.Add(FeedbackType.UnarmedHead, new Feedback(FeedbackType.UnarmedHead, "UnarmedHead_", 0));
            feedbackMap.Add(FeedbackType.GunHead, new Feedback(FeedbackType.GunHead, "GunHead_", 0));

            feedbackMap.Add(FeedbackType.UnarmedBloater, new Feedback(FeedbackType.UnarmedBloater, "UnarmedBloater_", 0));
            feedbackMap.Add(FeedbackType.UnarmedHeadcrab, new Feedback(FeedbackType.UnarmedHeadcrab, "UnarmedHeadcrab_", 0));
            feedbackMap.Add(FeedbackType.UnarmedHeadcrabArmored, new Feedback(FeedbackType.UnarmedHeadcrabArmored, "UnarmedHeadcrabArmored_", 0));
            feedbackMap.Add(FeedbackType.UnarmedHeadcrabBlack, new Feedback(FeedbackType.UnarmedHeadcrabBlack, "UnarmedHeadcrabBlack_", 0));
            feedbackMap.Add(FeedbackType.UnarmedHeadcrabFast, new Feedback(FeedbackType.UnarmedHeadcrabFast, "UnarmedHeadcrabFast_", 0));
            feedbackMap.Add(FeedbackType.UnarmedHeadcrabRunner, new Feedback(FeedbackType.UnarmedHeadcrabRunner, "UnarmedHeadcrabRunner_", 0));
            feedbackMap.Add(FeedbackType.UnarmedFastZombie, new Feedback(FeedbackType.UnarmedFastZombie, "UnarmedFastZombie_", 0));
            feedbackMap.Add(FeedbackType.UnarmedPoisonZombie, new Feedback(FeedbackType.UnarmedPoisonZombie, "UnarmedPoisonZombie_", 0));
            feedbackMap.Add(FeedbackType.UnarmedZombie, new Feedback(FeedbackType.UnarmedZombie, "UnarmedZombie_", 0));
            feedbackMap.Add(FeedbackType.UnarmedZombieBlind, new Feedback(FeedbackType.UnarmedZombieBlind, "UnarmedZombieBlind_", 0));
            feedbackMap.Add(FeedbackType.UnarmedZombine, new Feedback(FeedbackType.UnarmedZombine, "UnarmedZombine_", 0));
            feedbackMap.Add(FeedbackType.UnarmedAntlion, new Feedback(FeedbackType.UnarmedAntlion, "UnarmedAntlion_", 0));
            feedbackMap.Add(FeedbackType.UnarmedAntlionGuard, new Feedback(FeedbackType.UnarmedAntlionGuard, "UnarmedAntlionGuard_", 0));
            feedbackMap.Add(FeedbackType.UnarmedManhack, new Feedback(FeedbackType.UnarmedManhack, "UnarmedManhack_", 0));

            feedbackMap.Add(FeedbackType.GrabbedByBarnacle, new Feedback(FeedbackType.GrabbedByBarnacle, "GrabbedByBarnacle_", 0));

            feedbackMap.Add(FeedbackType.ConcussionGrenade, new Feedback(FeedbackType.ConcussionGrenade, "ConcussionGrenade_", 0));
            feedbackMap.Add(FeedbackType.BugBaitGrenade, new Feedback(FeedbackType.BugBaitGrenade, "BugBaitGrenade_", 0));
            feedbackMap.Add(FeedbackType.FragGrenade, new Feedback(FeedbackType.FragGrenade, "FragGrenade_", 0));
            feedbackMap.Add(FeedbackType.SpyGrenade, new Feedback(FeedbackType.SpyGrenade, "SpyGrenade_", 0));
            feedbackMap.Add(FeedbackType.HandGrenade, new Feedback(FeedbackType.HandGrenade, "HandGrenade_", 0));
            feedbackMap.Add(FeedbackType.RollerGrenade, new Feedback(FeedbackType.RollerGrenade, "RollerGrenade_", 0));
            feedbackMap.Add(FeedbackType.RollerMine, new Feedback(FeedbackType.RollerMine, "RollerMine_", 0));

            feedbackMap.Add(FeedbackType.Combine, new Feedback(FeedbackType.Combine, "Combine_", 0));
            feedbackMap.Add(FeedbackType.CombineS, new Feedback(FeedbackType.CombineS, "CombineS_", 0));
            feedbackMap.Add(FeedbackType.CombineGantry, new Feedback(FeedbackType.CombineGantry, "CombineGantry_", 0));
            feedbackMap.Add(FeedbackType.MetroPolice, new Feedback(FeedbackType.MetroPolice, "MetroPolice_", 0));
            feedbackMap.Add(FeedbackType.Sniper, new Feedback(FeedbackType.Sniper, "Sniper_", 0));
            feedbackMap.Add(FeedbackType.Strider, new Feedback(FeedbackType.Strider, "Strider_", 0));
            feedbackMap.Add(FeedbackType.Turret, new Feedback(FeedbackType.Turret, "Turret_", 0));
            feedbackMap.Add(FeedbackType.FoliageTurret, new Feedback(FeedbackType.FoliageTurret, "FoliageTurret_", 0));

            feedbackMap.Add(FeedbackType.EnvironmentExplosion, new Feedback(FeedbackType.EnvironmentExplosion, "EnvironmentExplosion_", 0));
            feedbackMap.Add(FeedbackType.EnvironmentLaser, new Feedback(FeedbackType.EnvironmentLaser, "EnvironmentLaser_", 0));
            feedbackMap.Add(FeedbackType.EnvironmentFire, new Feedback(FeedbackType.EnvironmentFire, "EnvironmentFire_", 0));
            feedbackMap.Add(FeedbackType.EnvironmentSpark, new Feedback(FeedbackType.EnvironmentSpark, "EnvironmentSpark_", 0));
            feedbackMap.Add(FeedbackType.EnvironmentPoison, new Feedback(FeedbackType.EnvironmentPoison, "EnvironmentPoison_", 0));
            feedbackMap.Add(FeedbackType.EnvironmentRadiation, new Feedback(FeedbackType.EnvironmentRadiation, "EnvironmentRadiation_", 0));

            feedbackMap.Add(FeedbackType.DamageExplosion, new Feedback(FeedbackType.DamageExplosion, "DamageExplosion_", 0));
            feedbackMap.Add(FeedbackType.DamageLaser, new Feedback(FeedbackType.DamageLaser, "DamageLaser_", 0));
            feedbackMap.Add(FeedbackType.DamageFire, new Feedback(FeedbackType.DamageFire, "DamageFire_", 0));
            feedbackMap.Add(FeedbackType.DamageSpark, new Feedback(FeedbackType.DamageSpark, "DamageSpark_", 0));

            feedbackMap.Add(FeedbackType.PlayerShootPistol, new Feedback(FeedbackType.PlayerShootPistol, "PlayerShootPistol_", 0));
            feedbackMap.Add(FeedbackType.PlayerShootShotgun, new Feedback(FeedbackType.PlayerShootShotgun, "PlayerShootShotgun_", 0));
            feedbackMap.Add(FeedbackType.PlayerShootSMG, new Feedback(FeedbackType.PlayerShootSMG, "PlayerShootSMG_", 0));
            feedbackMap.Add(FeedbackType.PlayerShootDefault, new Feedback(FeedbackType.PlayerShootDefault, "PlayerShootDefault_", 0));
            feedbackMap.Add(FeedbackType.PlayerGrenadeLaunch, new Feedback(FeedbackType.PlayerGrenadeLaunch, "PlayerGrenadeLaunch_", 0));

            feedbackMap.Add(FeedbackType.PlayerShootPistolLeft, new Feedback(FeedbackType.PlayerShootPistolLeft, "PlayerShootPistolLeft_", 0));
            feedbackMap.Add(FeedbackType.PlayerShootShotgunLeft, new Feedback(FeedbackType.PlayerShootShotgunLeft, "PlayerShootShotgunLeft_", 0));
            feedbackMap.Add(FeedbackType.PlayerShootSMGLeft, new Feedback(FeedbackType.PlayerShootSMGLeft, "PlayerShootSMGLeft_", 0));
            feedbackMap.Add(FeedbackType.PlayerShootDefaultLeft, new Feedback(FeedbackType.PlayerShootDefaultLeft, "PlayerShootDefaultLeft_", 0));
            feedbackMap.Add(FeedbackType.PlayerGrenadeLaunchLeft, new Feedback(FeedbackType.PlayerGrenadeLaunchLeft, "PlayerGrenadeLaunchLeft_", 0));

            feedbackMap.Add(FeedbackType.FallbackPistol, new Feedback(FeedbackType.FallbackPistol, "FallbackPistol_", 0));
            feedbackMap.Add(FeedbackType.FallbackShotgun, new Feedback(FeedbackType.FallbackShotgun, "FallbackShotgun_", 0));
            feedbackMap.Add(FeedbackType.FallbackSMG, new Feedback(FeedbackType.FallbackSMG, "FallbackSMG_", 0));

            feedbackMap.Add(FeedbackType.FallbackPistolLeft, new Feedback(FeedbackType.FallbackPistolLeft, "FallbackPistolLeft_", 0));
            feedbackMap.Add(FeedbackType.FallbackShotgunLeft, new Feedback(FeedbackType.FallbackShotgunLeft, "FallbackShotgunLeft_", 0));
            feedbackMap.Add(FeedbackType.FallbackSMGLeft, new Feedback(FeedbackType.FallbackSMGLeft, "FallbackSMGLeft_", 0));

            feedbackMap.Add(FeedbackType.KickbackPistol, new Feedback(FeedbackType.KickbackPistol, "KickbackPistol_", 0));
            feedbackMap.Add(FeedbackType.KickbackShotgun, new Feedback(FeedbackType.KickbackShotgun, "KickbackShotgun_", 0));
            feedbackMap.Add(FeedbackType.KickbackSMG, new Feedback(FeedbackType.KickbackSMG, "KickbackSMG_", 0));

            feedbackMap.Add(FeedbackType.KickbackPistolLeft, new Feedback(FeedbackType.KickbackPistolLeft, "KickbackPistolLeft_", 0));
            feedbackMap.Add(FeedbackType.KickbackShotgunLeft, new Feedback(FeedbackType.KickbackShotgunLeft, "KickbackShotgunLeft_", 0));
            feedbackMap.Add(FeedbackType.KickbackSMGLeft, new Feedback(FeedbackType.KickbackSMGLeft, "KickbackSMGLeft_", 0));

            feedbackMap.Add(FeedbackType.HeartBeat, new Feedback(FeedbackType.HeartBeat, "HeartBeat_", 0));
            feedbackMap.Add(FeedbackType.HeartBeatFast, new Feedback(FeedbackType.HeartBeatFast, "HeartBeatFast_", 0));

            feedbackMap.Add(FeedbackType.HealthPenUse, new Feedback(FeedbackType.HealthPenUse, "HealthPenUse_", 0));
            feedbackMap.Add(FeedbackType.HealthStationUse, new Feedback(FeedbackType.HealthStationUse, "HealthStationUse_", 0));
            feedbackMap.Add(FeedbackType.HealthStationUseLeftArm, new Feedback(FeedbackType.HealthStationUseLeftArm, "HealthStationUseLeftArm_", 0));
            feedbackMap.Add(FeedbackType.HealthStationUseRightArm, new Feedback(FeedbackType.HealthStationUseRightArm, "HealthStationUseRightArm_", 0));

            feedbackMap.Add(FeedbackType.BackpackStoreClip, new Feedback(FeedbackType.BackpackStoreClip, "BackpackStoreClipRight_", 0));
            feedbackMap.Add(FeedbackType.BackpackStoreResin, new Feedback(FeedbackType.BackpackStoreResin, "BackpackStoreResinRight_", 0));
            feedbackMap.Add(FeedbackType.BackpackRetrieveClip, new Feedback(FeedbackType.BackpackRetrieveClip, "BackpackRetrieveClipRight_", 0));
            feedbackMap.Add(FeedbackType.BackpackRetrieveResin, new Feedback(FeedbackType.BackpackRetrieveResin, "BackpackRetrieveResinRight_", 0));
            feedbackMap.Add(FeedbackType.ItemHolderStore, new Feedback(FeedbackType.ItemHolderStore, "ItemHolderStore_", 0));
            feedbackMap.Add(FeedbackType.ItemHolderRemove, new Feedback(FeedbackType.ItemHolderRemove, "ItemHolderRemove_", 0));

            feedbackMap.Add(FeedbackType.BackpackStoreClipLeft, new Feedback(FeedbackType.BackpackStoreClipLeft, "BackpackStoreClipLeft_", 0));
            feedbackMap.Add(FeedbackType.BackpackStoreResinLeft, new Feedback(FeedbackType.BackpackStoreResinLeft, "BackpackStoreResinLeft_", 0));
            feedbackMap.Add(FeedbackType.BackpackRetrieveClipLeft, new Feedback(FeedbackType.BackpackRetrieveClipLeft, "BackpackRetrieveClipLeft_", 0));
            feedbackMap.Add(FeedbackType.BackpackRetrieveResinLeft, new Feedback(FeedbackType.BackpackRetrieveResinLeft, "BackpackRetrieveResinLeft_", 0));
            feedbackMap.Add(FeedbackType.ItemHolderStoreLeft, new Feedback(FeedbackType.ItemHolderStoreLeft, "ItemHolderStoreLeft_", 0));
            feedbackMap.Add(FeedbackType.ItemHolderRemoveLeft, new Feedback(FeedbackType.ItemHolderRemoveLeft, "ItemHolderRemoveLeft_", 0));

            feedbackMap.Add(FeedbackType.GravityGloveLockOn, new Feedback(FeedbackType.GravityGloveLockOn, "GravityGloveLockOn_", 0));
            feedbackMap.Add(FeedbackType.GravityGlovePull, new Feedback(FeedbackType.GravityGlovePull, "GravityGlovePull_", 0));
            feedbackMap.Add(FeedbackType.GravityGloveCatch, new Feedback(FeedbackType.GravityGloveCatch, "GravityGloveCatch_", 0));

            feedbackMap.Add(FeedbackType.GravityGloveLockOnLeft, new Feedback(FeedbackType.GravityGloveLockOnLeft, "GravityGloveLockOnLeft_", 0));
            feedbackMap.Add(FeedbackType.GravityGlovePullLeft, new Feedback(FeedbackType.GravityGlovePullLeft, "GravityGlovePullLeft_", 0));
            feedbackMap.Add(FeedbackType.GravityGloveCatchLeft, new Feedback(FeedbackType.GravityGloveCatchLeft, "GravityGloveCatchLeft_", 0));

            feedbackMap.Add(FeedbackType.ClipInserted, new Feedback(FeedbackType.ClipInserted, "ClipInserted_", 0));
            feedbackMap.Add(FeedbackType.ChamberedRound, new Feedback(FeedbackType.ChamberedRound, "ChamberedRound_", 0));
            feedbackMap.Add(FeedbackType.ClipInsertedLeft, new Feedback(FeedbackType.ClipInsertedLeft, "ClipInsertedLeft_", 0));
            feedbackMap.Add(FeedbackType.ChamberedRoundLeft, new Feedback(FeedbackType.ChamberedRoundLeft, "ChamberedRoundLeft_", 0));

            feedbackMap.Add(FeedbackType.Cough, new Feedback(FeedbackType.Cough, "Cough_", 0));
            feedbackMap.Add(FeedbackType.CoughHead, new Feedback(FeedbackType.CoughHead, "CoughHead_", 0));

            feedbackMap.Add(FeedbackType.ShockOnHandLeft, new Feedback(FeedbackType.ShockOnHandLeft, "ShockOnHandLeft_", 0));
            feedbackMap.Add(FeedbackType.ShockOnHandRight, new Feedback(FeedbackType.ShockOnHandRight, "ShockOnHandRight_", 0));


            feedbackMap.Add(FeedbackType.DefaultDamage, new Feedback(FeedbackType.DefaultDamage, "DefaultDamage_", 0));
        }

        void TactFileRegister(string configPath, string filename, Feedback feedback)
        {
            if (feedbackMap.ContainsKey(feedback.feedbackType))
            {
                Feedback f = feedbackMap[feedback.feedbackType];
                f.feedbackFileCount += 1;
                feedbackMap[feedback.feedbackType] = f;
                
                hapticPlayer.RegisterTactFileStr(feedback.prefix + (feedbackMap[feedback.feedbackType].feedbackFileCount).ToString(), File.ReadAllText(configPath + "\\" + filename));
            }
        }

        void RegisterFeedbackFiles()
        {
            string configPath = Directory.GetCurrentDirectory() + "\\bHaptics";

            DirectoryInfo d = new DirectoryInfo(configPath);
            FileInfo[] Files = d.GetFiles("*.tact");
            
            for (int i = 0; i < Files.Length; i++)
            {
                string filename = Files[i].Name;

                if (filename == "." || filename == "..")
                    continue;

                bool found = false;
                foreach (var element in feedbackMap)
                {
                    if (filename.StartsWith(element.Value.prefix))
                    {
                        TactFileRegister(configPath, filename, element.Value);
                        found = true;
                        break;
                    }
                }
            }
        }

        public void CreateSystem()
        {
            if (!systemInitialized)
            {
                hapticPlayer = new HapticPlayer();

                if (hapticPlayer != null)
                {
                    RegisterFeedbackFiles();
                    systemInitialized = true;
                }
            }
        }

        bool IsPlayingKeyAll(string prefix, int feedbackFileCount)
        {
            for (int i = 1; i <= feedbackFileCount; i++)
            {
                string key = prefix + i.ToString();
                if (hapticPlayer.IsPlaying(key))
                {
                    return true;
                }
            }
            return false;
        }

        void ProvideHapticFeedbackThread(float locationAngle, float locationHeight, FeedbackType effect, float intensityMultiplier, bool waitToPlay)
        {
            if (intensityMultiplier < 0.001)
                return;
            
            if (!systemInitialized || hapticPlayer == null)
                CreateSystem();

            if (hapticPlayer != null)
            {
                if (feedbackMap.ContainsKey(effect))
                {
                    if (feedbackMap[effect].feedbackFileCount > 0)
                    {
                        if (waitToPlay)
                        {
                            if (IsPlayingKeyAll(feedbackMap[effect].prefix, feedbackMap[effect].feedbackFileCount))
                            {
                                return;
                            }
                        }

                        string key = feedbackMap[effect].prefix + (RandomNumber.Between(1, feedbackMap[effect].feedbackFileCount)).ToString();

                        if (locationHeight < -0.5f)
                            locationHeight = -0.5f;
                        else if (locationHeight > 0.5f)
                            locationHeight = 0.5f;

                        Bhaptics.Tact.RotationOption RotOption = new RotationOption(locationAngle, locationHeight);

                        Bhaptics.Tact.ScaleOption scaleOption = new ScaleOption(intensityMultiplier, 1.0f);

                        //hapticPlayer.SubmitRegistered(key, scaleOption);
                        hapticPlayer.SubmitRegisteredVestRotation(key, RotOption, scaleOption);
                    }
                }
            }
        }

        public void ProvideHapticFeedback(float locationAngle, float locationHeight, FeedbackType effect, bool waitToPlay, FeedbackType secondEffect)
        {
            if (effect != FeedbackType.NoFeedback)
            {
                float intensityMultiplier = Config.GetIntensityMultiplier(effect);
                if (intensityMultiplier > 0.01f)
                {
                    Thread thread = new Thread(() =>
                        ProvideHapticFeedbackThread(locationAngle, locationHeight, effect, intensityMultiplier, waitToPlay));
                    thread.Start();
                    if (secondEffect != FeedbackType.NoFeedback)
                    {
                        Thread thread2 = new Thread(() =>
                            ProvideHapticFeedbackThread(locationAngle, locationHeight, secondEffect, intensityMultiplier, waitToPlay));
                        thread2.Start();
                    }
                }
            }
        }

        public void StopHapticFeedback(FeedbackType effect)
        {
            if (hapticPlayer != null)
            {
                if (feedbackMap.ContainsKey(effect))
                {
                    if (feedbackMap[effect].feedbackFileCount > 0)
                    {
                        for (int i = 1; i <= feedbackMap[effect].feedbackFileCount; i++)
                        {
                            string key = feedbackMap[effect].prefix + i.ToString();
                            hapticPlayer.TurnOff(key);
                        }
                    }
                }
            }
        }

        public void PlayRandom()
        {
            ProvideHapticFeedback(0, 0, (FeedbackType)(RandomNumber.Between(0, 97)), false, FeedbackType.NoFeedback);
        }
    }
}
