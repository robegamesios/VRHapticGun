# VR Haptic Gun - Half-Life Alyx Support

Half-Life Alyx gameplays: 

https://youtu.be/NB-SUvXFdc8

https://youtu.be/Ae00qbjxJs0    
    
# Software: 
I forked a BHaptics Half-Life Alyx repo and updated it to work with the gun's ebb. Credits goes to them for creating the original implementation.

Repository of BHaptics Half-Life Alyx: https://github.com/bhaptics/bhaptics-half-life-alyx
    
1. In the Publish Folder https://github.com/robegamesios/VRHapticGun/tree/main/Games/Half%20Life%20Alyx/Code/Publish, download and unzip `Tactsuit+Gun_Alyx.zip`.
                
2. Turn on your haptic devices and go ahead and run `TactsuitAlyx.exe`.
    
3. Once `bHaptics Tactsuit + Haptic Gun Alyx Interface` is open:
    
    a. Click the `Browse...` button and select your HLA folder (e.g. `C:\Program Files (x86)\Steam\steamapps\common\Half-Life Alyx`).
    
    b. Enter `Right Hand IP Add` for the primary hand (shooting hand).
    
    c. Enter `Left Hand IP Add` for the secondary hand (grabbity glove). If you do not have a secondary haptic device, leave this blank.
    
    d. Enter `Port number` (defaults to 23).
    
    e. If you are using your left hand for shooting, just change it in the game settings, the haptic devices will adjust automatically.
    
    f. Click Start. The haptic devices will trigger if connection was successful.
    
    g. under the hood, the program will copy `vscripts\tactsuit.lua` to your game directory's vscripts folder (e.g. `C:\Program Files (x86)\Steam\steamapps\common\Half-Life Alyx\game\hlvr\scripts\vscripts\tactsuit.lua`)
    
    h. click Test button - if your gun is properly connected, it should activate (trigger fire).
    
![bHaptics Tactsuit + Haptic Gun Alyx Interface 10_15_2022 8_48_41 PM](https://user-images.githubusercontent.com/10041871/196017083-e8c6a6b1-975b-46d0-b132-9f189f57d668.png)

4. Launch SteamVR and start Half-Life Alyx game.

NOTE: This app will work with the Haptic Gun even if you do not have BHaptics gear (tactsuit, tactosy, etc). Likewise, it should work with BHaptics gear without having a Haptic Gun. 
    
# Repository of BHaptics Half-Life Alyx:
https://github.com/bhaptics/bhaptics-half-life-alyx
