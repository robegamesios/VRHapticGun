using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using Bhaptics.Tact;

namespace TactsuitAlyx
{
    public class Engine
    {
        private TactsuitVR tactsuitVr;

        private int playerRemainingHealth;

        private bool lowHealthFeedbackPlaying = false;
        private bool veryLowHealthFeedbackPlaying = false;

        private int grenadeLauncherState = 0;

        public bool twoHandedMode = false;

        public bool menuOpen = false;

        public bool leftHandedMode = false;

        public bool gravityPrimaryLock = false;
        public bool gravitySecondaryLock = false;

        public bool barnacleGrab = false;

        public bool coughing = false;


        public Engine(TactsuitVR _tactsuitVr)
        {
            tactsuitVr = _tactsuitVr;
        }

        private void LowHealthFeedback()
        {
            while (playerRemainingHealth > Config.VeryLowHealthAmount && playerRemainingHealth <= Config.LowHealthAmount)
            {
                if (!menuOpen)
                {
                    tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HeartBeat, false, TactsuitVR.FeedbackType.NoFeedback);
                }

                Thread.Sleep(Properties.Settings.Default.sleepDurationHeartBeat);
            }

            lowHealthFeedbackPlaying = false;
        }

        private void VeryLowHealthFeedback(bool tooLow)
        {
            while (playerRemainingHealth < Config.VeryLowHealthAmount && playerRemainingHealth > 0)
            {
                if (!menuOpen)
                {
                    tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HeartBeatFast, false, TactsuitVR.FeedbackType.NoFeedback);
                }

                Thread.Sleep(tooLow ? Properties.Settings.Default.sleepDurationHeartBeatTooFast : Properties.Settings.Default.sleepDurationHeartBeatFast);
            }

            veryLowHealthFeedbackPlaying = false;
        }

        public void HealthRemaining(int healthRemaining)
        {
            playerRemainingHealth = healthRemaining;

            if (playerRemainingHealth <= Config.TooLowHealthAmount)
            {
                if (!veryLowHealthFeedbackPlaying)
                {
                    veryLowHealthFeedbackPlaying = true;
                    Thread thread = new Thread(() => VeryLowHealthFeedback(true));
                    thread.Start();
                }
            }
            else if (playerRemainingHealth <= Config.VeryLowHealthAmount)
            {
                if (!veryLowHealthFeedbackPlaying)
                {
                    veryLowHealthFeedbackPlaying = true;
                    Thread thread = new Thread(() => VeryLowHealthFeedback(false));
                    thread.Start();
                }
            }
            else if (playerRemainingHealth <= Config.LowHealthAmount)
            {
                if (!lowHealthFeedbackPlaying)
                {
                    lowHealthFeedbackPlaying = true;
                    Thread thread = new Thread(LowHealthFeedback);
                    thread.Start();
                }
            }
        }

        public void PlayerHurt(int healthRemaining, string enemy, float locationAngle, string enemyName, string enemyDebugName)
        {
            //Heartbeat stuff
            HealthRemaining(healthRemaining);

            //Damage stuff

            float locationHeight = 0.5f;

            TactsuitVR.FeedbackType feedback = tactsuitVr.GetFeedbackTypeOfEnemyAttack(enemy, enemyName);

            bool headcrab = (tactsuitVr.HeadCrabFeedback(feedback));

            if (headcrab)
            {
                locationHeight = 0.35f;
            }

            if (!headcrab && !tactsuitVr.EnvironmentFeedback(feedback))
            {
                locationHeight = ((float)(RandomNumber.Between(0, 100)) / 100.0f) - 0.5f;
            }

            if (locationHeight > 0.485f || headcrab)
            {
                tactsuitVr.ProvideHapticFeedback(locationAngle, 0, tactsuitVr.GetHeadFeedbackVersion(feedback), false, TactsuitVR.FeedbackType.NoFeedback);
            }
            tactsuitVr.ProvideHapticFeedback(locationAngle, locationHeight, feedback, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void PlayerShoot(string weapon)
        {
            TactsuitVR.FeedbackType feedback = tactsuitVr.GetFeedbackTypeOfWeaponFromPlayer(weapon, leftHandedMode);

            if ((leftHandedMode && !(tactsuitVr.hapticPlayer.IsActive(PositionType.ForearmL) || tactsuitVr.hapticPlayer.IsActive(PositionType.Left))) ||
                (!leftHandedMode && !(tactsuitVr.hapticPlayer.IsActive(PositionType.ForearmR) || tactsuitVr.hapticPlayer.IsActive(PositionType.Right))))
            {
                //Use Fallback instead.
                feedback = tactsuitVr.GetFallbackTypeOfWeaponFromPlayer(feedback, leftHandedMode);
            }
           
            tactsuitVr.ProvideHapticFeedback(0, 0, feedback, false, twoHandedMode ? tactsuitVr.GetOtherHandFeedback(feedback) : TactsuitVR.FeedbackType.NoFeedback);
            tactsuitVr.ProvideHapticFeedback(0, 0, tactsuitVr.GetKickbackOfWeaponFromPlayer(feedback, leftHandedMode), false, TactsuitVR.FeedbackType.NoFeedback);
        }

        private void GrabbityLock(bool primaryHand)
        {
            while ((primaryHand && gravityPrimaryLock) || (!primaryHand && gravitySecondaryLock))
            {
                if (!menuOpen)
                {
                    float locationHeight = ((float)(RandomNumber.Between(0, 100)) / 100.0f) - 0.5f;
                    tactsuitVr.ProvideHapticFeedback(0, locationHeight, (leftHandedMode ? primaryHand : !primaryHand) ? TactsuitVR.FeedbackType.GravityGloveLockOnLeft : TactsuitVR.FeedbackType.GravityGloveLockOn, false, TactsuitVR.FeedbackType.NoFeedback);
                }

                Thread.Sleep(Properties.Settings.Default.sleepDurationGrabbityLock);
            }
        }

        public void GrabbityLockStart(bool primaryHand)
        {
            if (primaryHand && !gravityPrimaryLock)
            {
                gravityPrimaryLock = true;
                Thread thread = new Thread(() => GrabbityLock(true));
                thread.Start();
            }
            else if (!primaryHand && !gravitySecondaryLock)
            {
                gravitySecondaryLock = true;
                Thread thread = new Thread(() => GrabbityLock(false));
                thread.Start();
            }
        }

        public void GrabbityLockStop(bool primaryHand)
        {
            if (primaryHand)
            {
                gravityPrimaryLock = false;
            }
            else
            {
                gravitySecondaryLock = false;
            }
            tactsuitVr.StopHapticFeedback((leftHandedMode ? primaryHand : !primaryHand) ? TactsuitVR.FeedbackType.GravityGloveLockOnLeft : TactsuitVR.FeedbackType.GravityGloveLockOn);
        }

        public void GrabbityGlovePull(bool primaryHand)
        {
            tactsuitVr.ProvideHapticFeedback(0, 0, (leftHandedMode ? primaryHand : !primaryHand) ? TactsuitVR.FeedbackType.GravityGlovePullLeft : TactsuitVR.FeedbackType.GravityGlovePull, true, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void GrenadeLauncherStateChange(int newState)
        {
            if (grenadeLauncherState == 2 && newState == 0)
            {
                TactsuitVR.FeedbackType feedback = leftHandedMode
                    ? TactsuitVR.FeedbackType.PlayerGrenadeLaunchLeft
                    : TactsuitVR.FeedbackType.PlayerGrenadeLaunch;
                tactsuitVr.ProvideHapticFeedback(0, 0, feedback, false, tactsuitVr.GetOtherHandFeedback(feedback));
                tactsuitVr.ProvideHapticFeedback(0, 0, tactsuitVr.GetKickbackOfWeaponFromPlayer(TactsuitVR.FeedbackType.PlayerGrenadeLaunch, leftHandedMode), false, TactsuitVR.FeedbackType.NoFeedback);
            }

            grenadeLauncherState = newState;
        }

        private void BarnacleGrab()
        {
            while (barnacleGrab)
            {
                if (!menuOpen)
                {
                    tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.GrabbedByBarnacle, false, TactsuitVR.FeedbackType.NoFeedback);
                }

                Thread.Sleep(Properties.Settings.Default.sleepDurationBarnacleGrab);
            }
        }

        public void BarnacleGrabStart()
        {
            if (!barnacleGrab)
            {
                barnacleGrab = true;
                Thread thread = new Thread(BarnacleGrab);
                thread.Start();
            }
        }

        public void Reset()
        {
            playerRemainingHealth = 100;
            grenadeLauncherState = 0;
            barnacleGrab = false;
            coughing = false;
            gravityPrimaryLock = false;
            gravitySecondaryLock = false;
            lowHealthFeedbackPlaying = false;
            veryLowHealthFeedbackPlaying = false;
            tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HeartBeat, true, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void PlayerDeath(int damagebits)
        {
            playerRemainingHealth = 0;
            barnacleGrab = false;
            coughing = false;
            gravityPrimaryLock = false;
            gravitySecondaryLock = false;
            lowHealthFeedbackPlaying = false;
            veryLowHealthFeedbackPlaying = false;
            int remaining = damagebits;

            if (remaining >= 268435456) //DMG_DIRECT
            {
                remaining -= 268435456;
            }
            if (remaining >= 134217728) //DMG_BLAST_SURFACE
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentExplosion, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 134217728;
            }
            if (remaining >= 67108864) //DMG_DISSOLVE	
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentRadiation, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 67108864;
            }
            if (remaining >= 33554432) //DMG_AIRBOAT
            {
                //Do stuff here
                remaining -= 33554432;
            }
            if (remaining >= 16777216) //DMG_PLASMA
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentLaser, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 16777216;
            }
            if (remaining >= 8388608) //DMG_PHYSGUN
            {
                //Do stuff here
                remaining -= 8388608;
            }
            if (remaining >= 4194304) //DMG_REMOVENORAGDOLL
            {
                //Do stuff here
                remaining -= 4194304;
            }
            if (remaining >= 2097152) //DMG_SLOWBURN
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentPoison, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 2097152;
            }
            if (remaining >= 1048576) //DMG_ACID
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentPoison, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 1048576;
            }
            if (remaining >= 524288) //DMG_DROWNRECOVER
            {
                //Do stuff here
                remaining -= 524288;
            }
            if (remaining >= 262144) //DMG_RADIATION
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentRadiation, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 262144;
            }
            if (remaining >= 131072) //DMG_POISON
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentPoison, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 131072;
            }
            if (remaining >= 65536) //DMG_NERVEGAS
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentPoison, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 65536;
            }
            if (remaining >= 32768) //DMG_PARALYZE
            {
                //Do stuff here
                remaining -= 32768;
            }
            if (remaining >= 16384) //DMG_DROWN
            {
                //Do stuff here
                remaining -= 16384;
            }
            if (remaining >= 8192) //DMG_ALWAYSGIB
            {
                //Do stuff here
                remaining -= 8192;
            }
            if (remaining >= 4096) //DMG_NEVERGIB
            {
                //Do stuff here
                remaining -= 4096;
            }
            if (remaining >= 2048) //DMG_PREVENT_PHYSICS_FORCE
            {
                //Do stuff here
                remaining -= 2048;
            }
            if (remaining >= 1024) //DMG_ENERGYBEAM
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentLaser, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 1024;
            }
            if (remaining >= 512) //DMG_SONIC	
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentExplosion, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 512;
            }
            if (remaining >= 256) //DMG_SHOCK	
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentSpark, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 256;
            }
            if (remaining >= 128) //DMG_CLUB	
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.DefaultDamage, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 128;
            }
            if (remaining >= 64) //DMG_BLAST
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentExplosion, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 64;
            }
            if (remaining >= 32) //DMG_FALL	
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.DefaultDamage, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 32;
            }
            if (remaining >= 16) //DMG_VEHICLE	
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.DefaultDamage, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 16;
            }
            if (remaining >= 8) //DMG_BURN	
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.EnvironmentFire, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 8;
            }
            if (remaining >= 4) //DMG_SLASH	
            {
                //Do stuff here
                remaining -= 4;
            }
            if (remaining >= 2) //DMG_BULLET	
            {
                //Do stuff here
                remaining -= 2;
            }
            if (remaining >= 1) //DMG_CRUSH		
            {
                //Do stuff here
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.DefaultDamage, false, TactsuitVR.FeedbackType.NoFeedback);
                remaining -= 1;
            }
            //TODO ADD EFFECT CALLS HERE
        }

        public void GrabbityGloveCatch(bool primaryHand)
        {
            TactsuitVR.FeedbackType feedback = (leftHandedMode ? primaryHand : !primaryHand)
                ? TactsuitVR.FeedbackType.GravityGloveCatchLeft
                : TactsuitVR.FeedbackType.GravityGloveCatch;
            tactsuitVr.ProvideHapticFeedback(0, 0, feedback, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void DropAmmoInBackpack(bool leftShoulder)
        {
            tactsuitVr.ProvideHapticFeedback(0, 0, leftShoulder ? TactsuitVR.FeedbackType.BackpackStoreClipLeft : TactsuitVR.FeedbackType.BackpackStoreClip, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void DropResinInBackpack(bool leftShoulder)
        {
            tactsuitVr.ProvideHapticFeedback(0, 0, leftShoulder ? TactsuitVR.FeedbackType.BackpackStoreResinLeft : TactsuitVR.FeedbackType.BackpackStoreResin, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void RetrievedBackpackClip(bool leftShoulder)
        {
            tactsuitVr.ProvideHapticFeedback(0, 0, leftShoulder ? TactsuitVR.FeedbackType.BackpackRetrieveClipLeft : TactsuitVR.FeedbackType.BackpackRetrieveClip, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void RetrievedBackpackResin(bool leftShoulder)
        {
            tactsuitVr.ProvideHapticFeedback(0, 0, leftShoulder ? TactsuitVR.FeedbackType.BackpackRetrieveResinLeft : TactsuitVR.FeedbackType.BackpackRetrieveResin, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void StoredItemInItemholder(bool leftHolder)
        {
            tactsuitVr.ProvideHapticFeedback(0, 0, leftHolder ? TactsuitVR.FeedbackType.ItemHolderStoreLeft : TactsuitVR.FeedbackType.ItemHolderStore, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void RemovedItemFromItemholder(bool leftHolder)
        {
            tactsuitVr.ProvideHapticFeedback(0, 0, leftHolder ? TactsuitVR.FeedbackType.ItemHolderRemoveLeft : TactsuitVR.FeedbackType.ItemHolderRemove, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void HealthPenUse(float angle)
        {
            tactsuitVr.ProvideHapticFeedback(angle, 0, TactsuitVR.FeedbackType.HealthPenUse, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        private void HealthStationUseFunc(bool leftArm)
        {
            Thread.Sleep(2000);
            for (int i = 0; i < (100 - playerRemainingHealth)/12; i++)
            {
                tactsuitVr.ProvideHapticFeedback(0, 0,
                    leftArm
                        ? TactsuitVR.FeedbackType.HealthStationUseLeftArm
                        : TactsuitVR.FeedbackType.HealthStationUseRightArm, false, TactsuitVR.FeedbackType.NoFeedback);
                tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.HealthStationUse, false,
                    TactsuitVR.FeedbackType.NoFeedback);
                Thread.Sleep(2000);
            }
        }

        public void HealthStationUse(bool leftArm)
        {
            Thread thread = new Thread( () => HealthStationUseFunc(leftArm));
            thread.Start();
        }

        public void ClipInserted()
        {
            tactsuitVr.ProvideHapticFeedback(0, 0, leftHandedMode ? TactsuitVR.FeedbackType.ClipInsertedLeft : TactsuitVR.FeedbackType.ClipInserted, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        public void ChamberedRound()
        {
            tactsuitVr.ProvideHapticFeedback(0, 0, leftHandedMode ? TactsuitVR.FeedbackType.ChamberedRoundLeft : TactsuitVR.FeedbackType.ChamberedRound, false, TactsuitVR.FeedbackType.NoFeedback);
        }

        private void CoughFunc()
        {
            while(coughing)
            {
                if (!menuOpen)
                {
                    tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.CoughHead, false, TactsuitVR.FeedbackType.NoFeedback);

                    tactsuitVr.ProvideHapticFeedback(0, 0, TactsuitVR.FeedbackType.Cough, false, TactsuitVR.FeedbackType.NoFeedback);
                }

                Thread.Sleep(Properties.Settings.Default.sleepDurationCoughing);
            }
        }

        public void Cough()
        {
            coughing = true;
            Thread thread = new Thread(CoughFunc);
            thread.Start();
        }

        public void ShockOnArm(bool leftArm)
        {
            tactsuitVr.ProvideHapticFeedback(0, 0, leftArm ? TactsuitVR.FeedbackType.ShockOnHandLeft : TactsuitVR.FeedbackType.ShockOnHandRight, false, TactsuitVR.FeedbackType.NoFeedback);
        }
    }
}
