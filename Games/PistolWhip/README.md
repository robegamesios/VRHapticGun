# VR Haptic Gun - Pistol Whip Support

Pistol Whip gameplay: https://www.youtube.com/watch?v=IaUro4AgwZc

    
# Software: 
I forked a BHaptics Pistol Whip repo and updated it to work with the gun's ebb. Credits goes to Florian Fahrenberger https://github.com/floh-bhaptics for creating the original implementation.

Repository of Pistol Whip BHaptics: https://github.com/floh-bhaptics/PistolWhip_bhaptics
    
1. Install MelonLoader: https://melonwiki.xyz/#/

2. Until I am able to create a better GUI to set the gun's IP address, do this for now:
  
   a. Download this repo and Open up `PistolWhip_bhaptics.sln` and go to line 38 and change it to your gun's IP address.
   
   <img width="1552" alt="Screen Shot 2022-09-15 at 9 49 10 AM" src="https://user-images.githubusercontent.com/10041871/190464686-703ef215-af6d-49a3-ace2-57436bf08002.png">


   b. Click the Build button to build the solution.

   c. Go back to the root folder where you downloading this repo, then go to `/bin/Debug` folder. 

   d. Copy `PistolWhip_bhaptics.dll` to your Pistol Whip MelonLoader Mods folder.

4. Launch SteamVR and start Pistol Whip.

NOTE: This app will work with the Haptic Gun even if you do not have BHaptics gear (tactsuit, tactosy, etc). Likewise, it should work with BHaptics gear without having a Haptic Gun. 
    
# TODOs

1. Create GUI to configure IP Address instead of opening Visual Studio solution.

# Repository of Pistol Whip BHaptics:
https://github.com/floh-bhaptics/PistolWhip_bhaptics
