# VRHapticGun

This repo contains the hardware list, arduino firmware, software and STL files I used to build a VR Haptic Gun.

# Hardware:
For items 1 and 2, I think any toy gun with electric blowback (ebb) will work including those airsoft guns with ebb. I bought a lot of four RK45 and one SR12 for $50 including shipping from facebook marketplace.
1. Recoil RK-45 Spitfire laser tag pistol https://www.amazon.com/Recoil-Laser-Combat-Spitfire-Blaster/dp/B071KV9YP1
2. Optional: Recoil SR-12 laser tag gun (I used the electric blowback of this gun and fitted it inside the Recoil RK-45 body)
3. ESP32 WROOM dev board (2 pcs) $16: https://www.amazon.com/dp/B07QCP2451?psc=1&ref=ppx_yo2ov_dt_b_product_details
4. 6V RC battery (2 pcs) $20: https://www.amazon.com/dp/B08VWHWZ35?psc=1&ref=ppx_yo2ov_dt_b_product_details
5. Battery charger $13: https://www.amazon.com/dp/B07V3L7F91?psc=1&ref=ppx_yo2ov_dt_b_product_details
6. Step up boost converter (10 pcs) $11: https://www.amazon.com/dp/B089JYBF25?psc=1&ref=ppx_yo2ov_dt_b_product_details
7. Battery indicator (2 pcs) $7: https://www.amazon.com/dp/B07YKGHVSV?psc=1&ref=ppx_yo2ov_dt_b_product_details
8. Toggle switch (10 pcs) $10: https://www.amazon.com/dp/B07XC5KB8D?psc=1&ref=ppx_yo2ov_dt_b_product_details
9. Quest 2 pistol grip (2 pcs) $15: https://www.amazon.com/dp/B09MCT4LC1?psc=1&ref=ppx_yo2ov_dt_b_product_details
10. Hitec cable (10 pcs) $12: https://www.amazon.com/dp/B07QJSWNFM?psc=1&ref=ppx_yo2ov_dt_b_product_details
11. Neodymium Magnet (6 pcs) $16: https://www.amazon.com/dp/B08YYNFQ41?psc=1&ref=ppx_yo2ov_dt_b_product_details

Total cost for 1 gun is roughly $70. A bunch of the components come in bundles so buying them cost more. Obviously if you got some parts lying around, it can get cheaper.

# Building:
The esp32 cannot drive the motor directly, hence you need to add some components to do this.

Here is the link to assemble the circuitry to control the motor using the esp32 (including the parts used): https://diyi0t.com/control-dc-motor-without-ic-motor-driver/

# Firmware:
Open ESP32-HLA-WIFI.ino file located in \Games\Half Life Alyx\Esp32\ESP32-HLA-WIFI and change the following:

    const char* ssid = "YourWifiSSID"; //change this to your wifi SSID

    const char* password = "YourWifiPassword"; //change this to your wifi password

    const uint ServerPort = 23; //if you change this port, make sure to update the Programs.cs file to match this port

Go ahead and download the files to your esp32 board.

# Software:
I forked a Half Life Alyx Event Detector repo and updated it to work with the VR gun.

Repository of Half-Life: Alyx Event Detector: https://github.com/Solla/HalfLifeAlyxEventDetector

Open Programs.cs and change the following:

in Line 24:

    //Change this to use the ip address of your esp32
    tcpclnt.Connect("esp32IPAddress", 23); //23 is your port number. Change this to match the port number you specified in the esp32 code

in Line 36, change the values to `false` if you don't want unlimited ammo and all the weapons:

    HalfLifeAlyx_Autoexec HLA_Autoexec =
        new HalfLifeAlyx_Autoexec().
        UnlimitedMagazineInBag(true).
        GiveAllUnlockedWeapons(true);
                
Once you're done with updating the firmware, turn on the gun and go ahead and run the program, it will start HLA. 
