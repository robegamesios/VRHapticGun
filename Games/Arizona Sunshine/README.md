# VR Haptic Gun - Arizona Sunshine Support

Arizona Sunshine gameplays:

https://youtu.be/fzrwk0Q53G0

https://youtu.be/qlAa17PazDs
    
# Software: 
I forked a BHaptics Arizona Sunshine repo and updated it to work with the gun's ebb. Credits goes to Florian Fahrenberger https://github.com/floh-bhaptics for creating the original implementation.

Repository of Arizona Sunshine BHaptics: https://github.com/floh-bhaptics/ArizonaSunshine_bhaptics
    
1. Download and install MelonLoader (and the requirements for it if needed) into the game's directory: https://melonwiki.xyz/#/

2. Copy the contents of Publish folder https://github.com/robegamesios/VRHapticGun/tree/main/Games/Arizona%20Sunshine/Publish to your Arizona Sunshine MelonLoader Mods folder (e.g. C:\\Program Files (x86)\Steam\steamapps\common\Arizona Sunshine\Mods)

    ![Screenshot (13)](https://user-images.githubusercontent.com/10041871/190916459-03a49faa-2432-457d-bfb1-91e2b7a7dfb8.png)

3. Open `hapticGunConfig.txt` 

    a. Change the first line to your first Haptic Gun's IP Address.

    b. If you changed the Port number in the firmware, then update the second line to the correct port number you are using. 

    c. If you have another Haptic gun you want to use, change the third line to your non-dominant hand Gun's IP Address, otherwise leave the third line blank. 

    <img width="794" alt="Screen Shot 2022-09-17 at 9 14 12 PM" src="https://user-images.githubusercontent.com/10041871/190885569-a4474cf4-7de6-4ffe-929d-a5e8aaf51fb2.png">

    d. Save and close the file. You only need to do this once until you need to update your guns' IP address and port number.


4. Launch SteamVR and start the game.

NOTE: This app will work with the Haptic Gun even if you do not have BHaptics gear (tactsuit, tactosy, etc). Likewise, it should work with BHaptics gear without having a Haptic Gun. 

# Note for Visual Studio:

1. You might need to re-include the reference dll files (Assemby-CSharp.dll, UnityEngine.dll, etc).

2. If it's a Mono game, look for the reference files in the folder "PistolWhip_data/Managed/".

3. If it's il2cpp, then it will only be available once you run the game with MelonLoader installed, and in "MelonLoader/Managed".

# Repository of Arizona Sunshine BHaptics:
https://github.com/floh-bhaptics/ArizonaSunshine_bhaptics
