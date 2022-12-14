c

This repo contains the hardware list, arduino firmware, software and STL files I used to build a VR Haptic Gun.

Full tutorial videos: 

https://www.youtube.com/watch?v=5z2f2qFMsvU

https://youtu.be/VvRvuhjjCRs

Half-Life Alyx gameplay: https://www.youtube.com/watch?v=Ae00qbjxJs0

Pistol Whip gameplay: https://www.youtube.com/watch?v=IaUro4AgwZc

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
12. MOSFET (20 pcs) $10: https://www.amazon.com/gp/product/B07MW1N4Q5/ref=as_li_tl?ie=UTF8&camp=1789&creative=9325&creativeASIN=B07MW1N4Q5&linkCode=as2&tag=diyi0t-20&linkId=61d738477c1332b796febffe72b8747d
13. Diode (100 pcs) $5: https://www.amazon.com/gp/product/B071YWNBVM/ref=as_li_tl?ie=UTF8&camp=1789&creative=9325&creativeASIN=B071YWNBVM&linkCode=as2&tag=diyi0t-20&linkId=fc65d651ea3e32ed36e7bae2abe47e61
14. Resistors (1 kit) $12: https://www.amazon.com/gp/product/B072BL2VX1/ref=as_li_tl?ie=UTF8&camp=1789&creative=9325&creativeASIN=B072BL2VX1&linkCode=as2&tag=diyi0t-20&linkId=a8e83b518a10f9e815cd58dc759716cf

Total cost for 1 gun is roughly $70. The most expensive part is the gun and the batteries. A bunch of the components come in bundles so buying them cost more. Obviously if you already own some of the parts, it can get cheaper.

# Building:
The esp32 cannot drive the motor directly, hence you need to add some components to do this.

Here is the link to assemble the circuitry to control the motor using the esp32 (including the parts used): https://diyi0t.com/control-dc-motor-without-ic-motor-driver/

And this is how I connected it:
![ebbConnection buck](https://user-images.githubusercontent.com/10041871/185839941-cfa3aa06-466c-4cf3-883a-3b95f5192b74.png)

Here is a good video on how to disassemble a Recoil RK-45 spitfire pistol: https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&cad=rja&uact=8&ved=2ahUKEwiE4Ouiidz5AhUKLzQIHRUZBM8QwqsBegQIBhAB&url=https%3A%2F%2Fwww.youtube.com%2Fwatch%3Fv%3DS81l8VBC7dw&usg=AOvVaw36cihDIYPepnFAOpwIpYmh

1. You can connect a buck converter if you want to lower the voltage going to the esp32, i ended up not installing this since the 6V battery didn't affect the esp32 (only got a little warm). I think the 5V pin can handle upto 12V but some boards may not like this and will heat up.
2. You can use any pin for the signal to the MOSFET's gate, in my final build i used pin23 for the signal.
3. The ebb i used was from a Recoil SR-12 rifle which uses 9V (6-AA batteries), so i used a step up boost converter to raise the voltage to 9V.
4. I used the hitec cable to connect/disconnect the battery from the gun. So when i need to charge the battery, i can just disconnect it from the gun and connect it to the charger.
5. I used the lower half of the pistol grip and attached a magnet. Doing this makes it easy to remove the pistol grip from the gun to mount the quest 2 controller, and then reattach it to the gun. (I could not find a 3D file for a quest 2 gun grip, otherwise i would have 3d printed instead of buying)
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

The blue light in the esp32 indicates it's connected to wifi.

# Firmware:
1. In order to communicate with your esp32 via COM port, you need to install the Silabs CP210x USB to UART chip driver:

    a. Go to Silicon Labs' driver download page: https://www.silabs.com/developers/usb-to-uart-bridge-vcp-drivers
    
    b. Go to the downloads section and download the "CP210x Windows Drivers" (third in the list).
    
    c. Extract the .zip and run CP210xVCPInstaller_x64.exe.
    
    d. In your window computer, go to Settings -> Devices Manager, under Ports section, you will see Silicon Labs CP210x USB to UART Bridge (COM<number>). This is the port you should select in your IDE. Make sure your micro USB cable is capable of transmitting data and not just for charging.
    
2. Next you need to add the ESP32 package to your Arduino IDE:

    a. Go to File > Preferences in your Arduino IDE
    
    b. In the "Additional Board Manager URLs" field, enter this link: https://dl.espressif.com/dl/package_esp32_index.json and click the OK button
    
    c. Go to Tools > Board > Boards Manager
    
    d. Search "esp32" and install the "ESP32 by Espressif Systems" package.
    
    e. Select your ESP32 board from Tools -> Board -> Boards Manager -> ESP32 Dev Module
    
    f. Go to Tools > Manage Libraries and search for "WifiManager" byTzapu and install it.
    
    g. Go to Tools > Manage Libraries and search for "ESP_DoubleResetDetector" by Khoi Huang and install it.

3. Open `ESP32-HLA-WIFI.ino` file located in \Games\Half Life Alyx\Esp32\ESP32-HLA-WIFI and change the following if you want to:

        const uint ServerPort = 23; //if you change this port, make sure to match this port number you specify in the companion app.
           
4. Go ahead and upload the files to your esp32 board. If you are getting an error, you might need to press the boot button in the esp32 while uploading.

5. Turn On the gun.
        
    a. Open up your smartphone (iOS and Android) and go to Wifi Settings
    
    b. Connect to `HapticGun_Connect` access point.
    
    c. Select or enter your Wifi SSID. Then enter your wifi password.
    
    d. Check your esp32, the blue led should light up if the connection was successful.
    
    e. If you double click the Reset Button, this will reset the wifi settings, and you will be able to connect to the `HapticGun_Connect` access point again.
    
    ![Screen Shot 2022-09-05 at 8 50 41 PM](https://user-images.githubusercontent.com/10041871/188543318-d1873d3b-3fd7-4a06-92f2-d02b00914bff.png)
    
# VR Haptic Gun Supported Games

Check out the current supported games and instructions on how to implement the mods:

https://github.com/robegamesios/VRHapticGun/tree/main/Games

