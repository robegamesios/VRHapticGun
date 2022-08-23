# VRHapticGun

This repo contains the hardware list, arduino firmware, software and STL files I used to build a VR Haptic Gun.

# Hardware:
For items 1 and 2, I think any toy gun with electric blowback (ebb) will work including those airsoft guns with ebb. I bought a lot of four RK45 and one SR12 for $50 including shipping from facebook marketplace.
1. Recoil RK-45 Spitfire laser tag pistol https://www.amazon.com/Recoil-Laser-Combat-Spitfire-Blaster/dp/B071KV9YP1
2. (Optional)Recoil SR-12 laser tag gun (I used the electric blowback of this gun and fitted it inside the Recoil RK-45 body)
3. ESP32 WROOM dev board (2 pcs) $16: https://www.amazon.com/dp/B07QCP2451?psc=1&ref=ppx_yo2ov_dt_b_product_details
4. 6V RC battery (2 pcs) $20: https://www.amazon.com/dp/B08VWHWZ35?psc=1&ref=ppx_yo2ov_dt_b_product_details
5. Battery charger $13: https://www.amazon.com/dp/B07V3L7F91?psc=1&ref=ppx_yo2ov_dt_b_product_details
6. Step up boost converter (10 pcs) $11: https://www.amazon.com/dp/B089JYBF25?psc=1&ref=ppx_yo2ov_dt_b_product_details
7. (Optional)Battery indicator (2 pcs) $7: https://www.amazon.com/dp/B07YKGHVSV?psc=1&ref=ppx_yo2ov_dt_b_product_details
8. Toggle switch (10 pcs) $10: https://www.amazon.com/dp/B07XC5KB8D?psc=1&ref=ppx_yo2ov_dt_b_product_details
9. Quest 2 pistol grip (2 pcs) $15: https://www.amazon.com/dp/B09MCT4LC1?psc=1&ref=ppx_yo2ov_dt_b_product_details
10. Hitec cable (10 pcs) $12: https://www.amazon.com/dp/B07QJSWNFM?psc=1&ref=ppx_yo2ov_dt_b_product_details
11. Neodymium Magnet (6 pcs) $16: https://www.amazon.com/dp/B08YYNFQ41?psc=1&ref=ppx_yo2ov_dt_b_product_details

Total cost for 1 gun is roughly $70. A bunch of the components come in bundles so buying them cost more. Obviously if you got some parts lying around, it can get cheaper.

# Building:
The esp32 cannot drive the motor directly, hence you need to add some components to do this.

Here is the link to assemble the circuitry to control the motor using the esp32 (including the parts used): https://diyi0t.com/control-dc-motor-without-ic-motor-driver/

And this is how I connected it:
![ebbConnection buck](https://user-images.githubusercontent.com/10041871/185839941-cfa3aa06-466c-4cf3-883a-3b95f5192b74.png)

1. You can connect a buck converter if you want to lower the voltage going to the esp32, i ended up not installing this since the 6V battery didn't affect the esp32 (only got a little warm). I think the 5V pin can handle upto 12V but some boards may not like this and will heat up.
2. You can use any pin for the signal to the MOSFET's gate, in my final build i used pin23 for the signal.
3. The ebb i used was from a Recoil SR-12 rifle which uses 9V (6-AA batteries), so i used a step up boost converted to raise the voltage to 9V.
4. I used the hitec cable to connect/disconnect the battery from the gun. So when i need to charge the battery, i can just disconnect it from the gun and connect it to the charger.
5. I used the lower half of the pistol grip and attached a magnet. Doing this makes it easy to remove the pistol grip from the gun to mount the quest 2 controller, and then reattach it to the gun.
    ![6](https://user-images.githubusercontent.com/10041871/186064966-eae7154e-f186-4b3b-bc4d-9c9363f4dff0.png)

6. I 3d printed a battery holder, a cover for the esp32 and clip to attach velcro to the gun grip. All these are optional though.
    ![5](https://user-images.githubusercontent.com/10041871/186065066-80069735-4d23-472b-a432-5ae17775a55d.png)
    ![4](https://user-images.githubusercontent.com/10041871/186065080-e8c14e76-a184-4530-a942-ed7fc75ef555.png)

7. I used some velcro and a cushion strap from an old bag to make a strap.
    ![3](https://user-images.githubusercontent.com/10041871/186065163-1c6b8482-f6e6-4671-b518-20f89cd77017.png)

8. I added a toggle switch to turn the gun On/Off.

9. Finally, i fitted all the parts to the Recoil RK-45 pistol (had to cut and grind to fit the parts) and added some paint.

![1](https://user-images.githubusercontent.com/10041871/186065155-fa454b3d-5eb0-4384-975d-64ec3bbcf4c7.png)
![2](https://user-images.githubusercontent.com/10041871/186065159-8d996391-8d25-4ed5-839f-b0f8264caeb4.png)


# Firmware:
Open `ESP32-HLA-WIFI.ino` file located in \Games\Half Life Alyx\Esp32\ESP32-HLA-WIFI and change the following:

    const char* ssid = "YourWifiSSID"; //change this to your wifi SSID

    const char* password = "YourWifiPassword"; //change this to your wifi password

    const uint ServerPort = 23; //if you change this port, make sure to update the Programs.cs file to match this port

Go ahead and download the files to your esp32 board.

# Software:
I forked a Half Life Alyx Event Detector repo and updated it to work with the VR gun.

Repository of Half-Life: Alyx Event Detector: https://github.com/Solla/HalfLifeAlyxEventDetector

Open `Programs.cs` and change the following:

in Line 24:

    //Change this to use the ip address of your esp32
    tcpclnt.Connect("esp32IPAddress", 23); //23 is your port number. Change this to match the port number you specified in the esp32 code

in Line 36, change the values to `false` if you don't want unlimited ammo and all the weapons:

    HalfLifeAlyx_Autoexec HLA_Autoexec =
        new HalfLifeAlyx_Autoexec().
        UnlimitedMagazineInBag(true).
        GiveAllUnlockedWeapons(true);
                
Once you're done with updating the firmware, turn on the gun and go ahead and run the program, it will start HLA. You can also publish an executable file (I included one in the PUBLISH folder) and just run it directly without having Visual Studio opened.

# TODOs

1. Add a GUI so you can enter IP address before running the game.
2. Detect when you disconnect/reconnect the gun grip from the gun. Then in the game, the weapon will automatically get removed (show only the hand) or automatically switch to the last weapon selected when reconnected.

# Repository of Half-Life: Alyx Event Detector

https://github.com/Solla/HalfLifeAlyxEventDetector
