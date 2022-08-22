# VRHapticGun

This repo contains the arduino firmware, software and STL files I used to build a VR Haptic Gun.

# Hardware:
For items 1 and 2, I think any toy gun with electric blowback (ebb) will work including those airsoft guns with ebb.
1. Recoil RK-45 Spitfire laser tag pistol https://www.amazon.com/Recoil-Laser-Combat-Spitfire-Blaster/dp/B071KV9YP1
2. Optional: Recoil SR-12 laser tag gun (I used the electrib blowback of this gun and fitted it inside the Recoil RK-45 body)
3. ESP32 WROOM dev board: https://www.amazon.com/dp/B07QCP2451?psc=1&ref=ppx_yo2ov_dt_b_product_details
4. 6V RC battery: https://www.amazon.com/dp/B08VWHWZ35?psc=1&ref=ppx_yo2ov_dt_b_product_details
5. Battery charger: https://www.amazon.com/dp/B07V3L7F91?psc=1&ref=ppx_yo2ov_dt_b_product_details
6. Step up boost converter: https://www.amazon.com/dp/B089JYBF25?psc=1&ref=ppx_yo2ov_dt_b_product_details
7. Battery indicator: https://www.amazon.com/dp/B07YKGHVSV?psc=1&ref=ppx_yo2ov_dt_b_product_details
8. Toggle switch: https://www.amazon.com/dp/B07XC5KB8D?psc=1&ref=ppx_yo2ov_dt_b_product_details
9. Quest 2 pistol grip: https://www.amazon.com/dp/B09MCT4LC1?psc=1&ref=ppx_yo2ov_dt_b_product_details
10. Hitec cable: https://www.amazon.com/dp/B07QJSWNFM?psc=1&ref=ppx_yo2ov_dt_b_product_details
11. Neodymium Magnet: https://www.amazon.com/dp/B08YYNFQ41?psc=1&ref=ppx_yo2ov_dt_b_product_details

# Software:
I forked a Half Life Alyx Event Detector repo and updated it to work with the VR gun.

Repository of Half-Life: Alyx Event Detector: https://github.com/Solla/HalfLifeAlyxEventDetector

# Firmware:
Open ESP32-HLA-WIFI.ino file located in \Games\Half Life Alyx\Esp32\ESP32-HLA-WIFI and change the following:

    const char* ssid = "YourWifiSSID"; //change this to your wifi SSID

    const char* password = "YourWifiPassword"; //change this to your wifi password

    const uint ServerPort = 23; //if you change this port, make sure to update the Programs.cs file to match this port

Go ahead and download the files to your esp32 board.
