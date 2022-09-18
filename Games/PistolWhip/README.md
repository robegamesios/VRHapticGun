# VR Haptic Gun - Pistol Whip Support

Pistol Whip gameplay: https://www.youtube.com/watch?v=IaUro4AgwZc

    
# Software: 
I forked a BHaptics Pistol Whip repo and updated it to work with the gun's ebb. Credits goes to Florian Fahrenberger https://github.com/floh-bhaptics for creating the original implementation.

Repository of Pistol Whip BHaptics: https://github.com/floh-bhaptics/PistolWhip_bhaptics
    
1. Download and install MelonLoader (and the requirements for it if needed) into the game's directory: https://melonwiki.xyz/#/

2. Copy the contents of Publish folder https://github.com/robegamesios/VRHapticGun/tree/main/Games/PistolWhip/Publish to your Pistol Whip MelonLoader Mods folder (e.g. C:\\Program Files (x86)\Steam\steamapps\common\Pistol Whip\Mods)

    ![pistolwhipdir](https://user-images.githubusercontent.com/10041871/190724502-3a9425dd-cd33-4dbf-b886-76562df5ec00.png)

3. Open `hapticGunConfig.txt` 

    a.  Change the first line to your first Haptic Gun's IP Address.

    b. If you changed the Port number in the firmware, then update the second line to the correct port number you are using. 

    c. If you have another Haptic gun you want to use, change the third line to your non-dominant hand Gun's IP Address, otherwise leave the third line blank.

    d. Save and close the file. You only need to do this once until you need to update your guns' IP address and port number.

    <img width="794" alt="Screen Shot 2022-09-17 at 9 14 12 PM" src="https://user-images.githubusercontent.com/10041871/190885509-b65afabf-f80d-4dcb-a586-f0d11f521637.png">

4. Launch SteamVR and start Pistol Whip.

NOTE: This app will work with the Haptic Gun even if you do not have BHaptics gear (tactsuit, tactosy, etc). Likewise, it should work with BHaptics gear without having a Haptic Gun. 

# Note for Visual Studio:

1. You might need to re-include the reference dll files (Assemby-CSharp.dll, UnityEngine.dll, etc).

2. If it's a Mono game, look for the reference files in the folder "PistolWhip_data/Managed/". 

3. If it's il2cpp, then it will only be available once you run the game with MelonLoader installed, and in "MelonLoader/Managed".

# Repository of Pistol Whip BHaptics:
https://github.com/floh-bhaptics/PistolWhip_bhaptics
