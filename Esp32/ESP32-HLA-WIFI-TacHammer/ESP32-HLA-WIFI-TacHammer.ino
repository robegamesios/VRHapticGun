#define ESP_DRD_USE_EEPROM true
#define ESP_DRD_USE_SPIFFS false        //false
#define DOUBLERESETDETECTOR_DEBUG true  //false

#include <ESP_DoubleResetDetector.h>
#include <WiFi.h>
#include <HTTPClient.h>
#include <WiFiManager.h>

#include <Wire.h>
#include "Adafruit_DRV2605.h"

// Number of seconds after reset during which a
// subseqent reset will be considered a double reset.
#define DRD_TIMEOUT 10

// RTC Memory Address for the DoubleResetDetector to use
#define DRD_ADDRESS 0

DoubleResetDetector* drd;

int LED_BUILTIN = 2;  //this is the blue led on the esp32 that lights up when connected to WIFI

const uint ServerPort = 23;  //23 is your port number. Change this to match the port number you specified in Programs.cs file
WiFiServer Server(ServerPort);
WiFiClient RemoteClient;

Adafruit_DRV2605 drv0;
Adafruit_DRV2605 drv1;
Adafruit_DRV2605 drv2;
Adafruit_DRV2605 drv3;

void CheckForConnections() {
  if (Server.hasClient()) {
    // If we are already connected to another computer,
    // then reject the new connection. Otherwise accept
    // the connection.
    if (RemoteClient.connected()) {
      Serial.println("Connection rejected");
      Server.available().stop();
    } else {
      Serial.println("Connection accepted");
      RemoteClient = Server.available();
    }
  }
}

void EchoReceivedData() {
  uint8_t ReceiveBuffer[30];
  while (RemoteClient.connected() && RemoteClient.available()) {
    int Received = RemoteClient.read(ReceiveBuffer, sizeof(ReceiveBuffer));
    RemoteClient.write(ReceiveBuffer, Received);

    Serial.print("received = ");
    Serial.println(Received);
    
    if (Received == 1) {
      TCA9548A(0);
      // set the effect to play
      Serial.println(F("-------------------TCA9548A(0)------------------"));
      drv0.setWaveform(0, 1);  // play effect strong click 100%
      drv0.setWaveform(1, 0);       // end waveform
      // play the effect!
      drv0.go();

    } else if (Received == 2) {
      TCA9548A(1);
      // set the effect to play
      Serial.println(F("-------------------TCA9548A(1)------------------"));
      drv1.setWaveform(0, 1);  // play effect strong click 100% 
      drv1.setWaveform(1, 0);       // end waveform
      // play the effect!
      drv1.go();
      
    } else if (Received == 3) {
      TCA9548A(2);
      // set the effect to play
      Serial.println(F("-------------------TCA9548A(2)------------------"));
      drv2.setWaveform(0, 1);  // play effect strong click 100%
      drv2.setWaveform(1, 0);       // end waveform
      // play the effect!
      drv2.go();

    } else if (Received == 4) {
      TCA9548A(3);
      // set the effect to play
      Serial.println(F("-------------------TCA9548A(3)------------------"));
      drv3.setWaveform(0, 1);  // play effect strong click 100% 
      drv3.setWaveform(1, 0);       // end waveform
      // play the effect!
      drv3.go();
    }
  }
}

void setup() {
  pinMode(LED_BUILTIN, OUTPUT);
  
  //wifi setup begin
  WiFi.mode(WIFI_STA);  // explicitly set mode, esp defaults to STA+AP
  Serial.begin(115200);

  WiFiManager wm;

  //wm.resetSettings(); //incase you need to manually reset the esp32 board for testing

  //DRD setup begin
  drd = new DoubleResetDetector(DRD_TIMEOUT, DRD_ADDRESS);

  if (drd->detectDoubleReset()) {
    Serial.println("Double Reset Detected");
    digitalWrite(LED_BUILTIN, LOW);
    wm.resetSettings();
  } else {
    Serial.println("No Double Reset Detected");
    if (WiFi.status() == WL_CONNECTED) {
      digitalWrite(LED_BUILTIN, HIGH);
    } else {
      digitalWrite(LED_BUILTIN, LOW);
    }
  }
  //DRD setup end

  wm.autoConnect("HapticGun_Connect");

  Serial.println("Connecting");
  while (WiFi.status() != WL_CONNECTED) {
    digitalWrite(LED_BUILTIN, LOW);
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.print("Connected to WiFi network with IP Address: ");
  Serial.println(WiFi.localIP());
  digitalWrite(LED_BUILTIN, HIGH);
  // wifi setup end

 // Start I2C communication with the Multiplexer
  Wire.begin();
  
  TCA9548A(0);
  //DRV2605l setup begin
  // Serial.begin(9600);
  Serial.println("DRV0 test");
  drv0.begin();
  
  // I2C trigger by sending 'go' command
  drv0.setMode(DRV2605_MODE_INTTRIG);  // default, internal trigger when sending GO command
 
  drv0.selectLibrary(1);
  //DRV2605l setup end

  TCA9548A(1);
  //DRV2605l setup begin
  // Serial.begin(9600);
  Serial.println("DRV1 test");
  drv1.begin();
  
  // I2C trigger by sending 'go' command
  drv1.setMode(DRV2605_MODE_INTTRIG);  // default, internal trigger when sending GO command
 
  drv1.selectLibrary(1);
  //DRV2605l setup end

   TCA9548A(2);
  //DRV2605l setup begin
  // Serial.begin(9600);
  Serial.println("DRV2 test");
  drv2.begin();
  
  // I2C trigger by sending 'go' command
  drv2.setMode(DRV2605_MODE_INTTRIG);  // default, internal trigger when sending GO command
 
  drv2.selectLibrary(1);
  //DRV2605l setup end

  TCA9548A(3);
  //DRV2605l setup begin
  // Serial.begin(9600);
  Serial.println("DRV3 test");
  drv3.begin();
  
  // I2C trigger by sending 'go' command
  drv3.setMode(DRV2605_MODE_INTTRIG);  // default, internal trigger when sending GO command
 
  drv3.selectLibrary(1);
  //DRV2605l setup end
 
  Server.begin();
}

void loop() {
  // Call the double reset detector loop method every so often,
  // so that it can recognise when the timeout expires.
  // You can also call drd.stop() when you wish to no longer
  // consider the next reset as a double reset.
  drd->loop();
 
  if (WiFi.status() == WL_CONNECTED) {
    CheckForConnections();
    EchoReceivedData();
//    playAllEffects();
  } else {
    Serial.println("WiFi Disconnected");
    digitalWrite(LED_BUILTIN, LOW);
  }
}

void TCA9548A(uint8_t bus){
  Wire.beginTransmission(0x70);  // TCA9548A address is 0x70
  Wire.write(1 << bus);          // send byte to select bus
  Wire.endTransmission();
  Serial.print(bus);
}

uint8_t effect = 1;
void playAllEffects() {
//  Serial.print("Effect #"); Serial.println(effect);

   if (effect == 1) {
    Serial.println("11.2 Waveform Library Effects List");
  }

  if (effect == 1) {
    Serial.println(F("1 − Strong Click - 100%"));
  }
  if (effect == 2) {
    Serial.println(F("2 − Strong Click - 60%"));
  }
  if (effect == 3) {
    Serial.println(F("3 − Strong Click - 30%"));
  }
  if (effect == 4) {
    Serial.println(F("4 − Sharp Click - 100%"));
  }
  if (effect == 5) {
    Serial.println(F("5 − Sharp Click - 60%"));
  }
  if (effect == 6) {
    Serial.println(F("6 − Sharp Click - 30%"));
  }
  if (effect == 7) {
    Serial.println(F("7 − Soft Bump - 100%"));
  }
  if (effect == 8) {
    Serial.println(F("8 − Soft Bump - 60%"));
  }
  if (effect == 9) {
    Serial.println(F("9 − Soft Bump - 30%"));
  }
  if (effect == 10) {
    Serial.println(F("10 − Double Click - 100%"));
  }
  if (effect == 11) {
    Serial.println(F("11 − Double Click - 60%"));
  }
  if (effect == 12) {
    Serial.println(F("12 − Triple Click - 100%"));
  }
  if (effect == 13) {
    Serial.println(F("13 − Soft Fuzz - 60%"));
  }
  if (effect == 14) {
    Serial.println(F("14 − Strong Buzz - 100%"));
  }
  if (effect == 15) {
    Serial.println(F("15 − 750 ms Alert 100%"));
  }
  if (effect == 16) {
    Serial.println(F("16 − 1000 ms Alert 100%"));
  }
  if (effect == 17) {
    Serial.println(F("17 − Strong Click 1 - 100%"));
  }
  if (effect == 18) {
    Serial.println(F("18 − Strong Click 2 - 80%"));
  }
  if (effect == 19) {
    Serial.println(F("19 − Strong Click 3 - 60%"));
  }
  if (effect == 20) {
    Serial.println(F("20 − Strong Click 4 - 30%"));
  }
  if (effect == 21) {
    Serial.println(F("21 − Medium Click 1 - 100%"));
  }
  if (effect == 22) {
    Serial.println(F("22 − Medium Click 2 - 80%"));
  }
  if (effect == 23) {
    Serial.println(F("23 − Medium Click 3 - 60%"));
  }
  if (effect == 24) {
    Serial.println(F("24 − Sharp Tick 1 - 100%"));
  }
  if (effect == 25) {
    Serial.println(F("25 − Sharp Tick 2 - 80%"));
  }
  if (effect == 26) {
    Serial.println(F("26 − Sharp Tick 3 – 60%"));
  }
  if (effect == 27) {
    Serial.println(F("27 − Short Double Click Strong 1 – 100%"));
  }
  if (effect == 28) {
    Serial.println(F("28 − Short Double Click Strong 2 – 80%"));
  }
  if (effect == 29) {
    Serial.println(F("29 − Short Double Click Strong 3 – 60%"));
  }
  if (effect == 30) {
    Serial.println(F("30 − Short Double Click Strong 4 – 30%"));
  }
  if (effect == 31) {
    Serial.println(F("31 − Short Double Click Medium 1 – 100%"));
  }
  if (effect == 32) {
    Serial.println(F("32 − Short Double Click Medium 2 – 80%"));
  }
  if (effect == 33) {
    Serial.println(F("33 − Short Double Click Medium 3 – 60%"));
  }
  if (effect == 34) {
    Serial.println(F("34 − Short Double Sharp Tick 1 – 100%"));
  }
  if (effect == 35) {
    Serial.println(F("35 − Short Double Sharp Tick 2 – 80%"));
  }
  if (effect == 36) {
    Serial.println(F("36 − Short Double Sharp Tick 3 – 60%"));
  }
  if (effect == 37) {
    Serial.println(F("37 − Long Double Sharp Click Strong 1 – 100%"));
  }
  if (effect == 38) {
    Serial.println(F("38 − Long Double Sharp Click Strong 2 – 80%"));
  }
  if (effect == 39) {
    Serial.println(F("39 − Long Double Sharp Click Strong 3 – 60%"));
  }
  if (effect == 40) {
    Serial.println(F("40 − Long Double Sharp Click Strong 4 – 30%"));
  }
  if (effect == 41) {
    Serial.println(F("41 − Long Double Sharp Click Medium 1 – 100%"));
  }
  if (effect == 42) {
    Serial.println(F("42 − Long Double Sharp Click Medium 2 – 80%"));
  }
  if (effect == 43) {
    Serial.println(F("43 − Long Double Sharp Click Medium 3 – 60%"));
  }
  if (effect == 44) {
    Serial.println(F("44 − Long Double Sharp Tick 1 – 100%"));
  }
  if (effect == 45) {
    Serial.println(F("45 − Long Double Sharp Tick 2 – 80%"));
  }
  if (effect == 46) {
    Serial.println(F("46 − Long Double Sharp Tick 3 – 60%"));
  }
  if (effect == 47) {
    Serial.println(F("47 − Buzz 1 – 100%"));
  }
  if (effect == 48) {
    Serial.println(F("48 − Buzz 2 – 80%"));
  }
  if (effect == 49) {
    Serial.println(F("49 − Buzz 3 – 60%"));
  }
  if (effect == 50) {
    Serial.println(F("50 − Buzz 4 – 40%"));
  }
  if (effect == 51) {
    Serial.println(F("51 − Buzz 5 – 20%"));
  }
  if (effect == 52) {
    Serial.println(F("52 − Pulsing Strong 1 – 100%"));
  }
  if (effect == 53) {
    Serial.println(F("53 − Pulsing Strong 2 – 60%"));
  }
  if (effect == 54) {
    Serial.println(F("54 − Pulsing Medium 1 – 100%"));
  }
  if (effect == 55) {
    Serial.println(F("55 − Pulsing Medium 2 – 60%"));
  }
  if (effect == 56) {
    Serial.println(F("56 − Pulsing Sharp 1 – 100%"));
  }
  if (effect == 57) {
    Serial.println(F("57 − Pulsing Sharp 2 – 60%"));
  }
  if (effect == 58) {
    Serial.println(F("58 − Transition Click 1 – 100%"));
  }
  if (effect == 59) {
    Serial.println(F("59 − Transition Click 2 – 80%"));
  }
  if (effect == 60) {
    Serial.println(F("60 − Transition Click 3 – 60%"));
  }
  if (effect == 61) {
    Serial.println(F("61 − Transition Click 4 – 40%"));
  }
  if (effect == 62) {
    Serial.println(F("62 − Transition Click 5 – 20%"));
  }
  if (effect == 63) {
    Serial.println(F("63 − Transition Click 6 – 10%"));
  }
  if (effect == 64) {
    Serial.println(F("64 − Transition Hum 1 – 100%"));
  }
  if (effect == 65) {
    Serial.println(F("65 − Transition Hum 2 – 80%"));
  }
  if (effect == 66) {
    Serial.println(F("66 − Transition Hum 3 – 60%"));
  }
  if (effect == 67) {
    Serial.println(F("67 − Transition Hum 4 – 40%"));
  }
  if (effect == 68) {
    Serial.println(F("68 − Transition Hum 5 – 20%"));
  }
  if (effect == 69) {
    Serial.println(F("69 − Transition Hum 6 – 10%"));
  }
  if (effect == 70) {
    Serial.println(F("70 − Transition Ramp Down Long Smooth 1 – 100 to 0%"));
  }
  if (effect == 71) {
    Serial.println(F("71 − Transition Ramp Down Long Smooth 2 – 100 to 0%"));
  }
  if (effect == 72) {
    Serial.println(F("72 − Transition Ramp Down Medium Smooth 1 – 100 to 0%"));
  }
  if (effect == 73) {
    Serial.println(F("73 − Transition Ramp Down Medium Smooth 2 – 100 to 0%"));
  }
  if (effect == 74) {
    Serial.println(F("74 − Transition Ramp Down Short Smooth 1 – 100 to 0%"));
  }
  if (effect == 75) {
    Serial.println(F("75 − Transition Ramp Down Short Smooth 2 – 100 to 0%"));
  }
  if (effect == 76) {
    Serial.println(F("76 − Transition Ramp Down Long Sharp 1 – 100 to 0%"));
  }
  if (effect == 77) {
    Serial.println(F("77 − Transition Ramp Down Long Sharp 2 – 100 to 0%"));
  }
  if (effect == 78) {
    Serial.println(F("78 − Transition Ramp Down Medium Sharp 1 – 100 to 0%"));
  }
  if (effect == 79) {
    Serial.println(F("79 − Transition Ramp Down Medium Sharp 2 – 100 to 0%"));
  }
  if (effect == 80) {
    Serial.println(F("80 − Transition Ramp Down Short Sharp 1 – 100 to 0%"));
  }
  if (effect == 81) {
    Serial.println(F("81 − Transition Ramp Down Short Sharp 2 – 100 to 0%"));
  }
  if (effect == 82) {
    Serial.println(F("82 − Transition Ramp Up Long Smooth 1 – 0 to 100%"));
  }
  if (effect == 83) {
    Serial.println(F("83 − Transition Ramp Up Long Smooth 2 – 0 to 100%"));
  }
  if (effect == 84) {
    Serial.println(F("84 − Transition Ramp Up Medium Smooth 1 – 0 to 100%"));
  }
  if (effect == 85) {
    Serial.println(F("85 − Transition Ramp Up Medium Smooth 2 – 0 to 100%"));
  }
  if (effect == 86) {
    Serial.println(F("86 − Transition Ramp Up Short Smooth 1 – 0 to 100%"));
  }
  if (effect == 87) {
    Serial.println(F("87 − Transition Ramp Up Short Smooth 2 – 0 to 100%"));
  }
  if (effect == 88) {
    Serial.println(F("88 − Transition Ramp Up Long Sharp 1 – 0 to 100%"));
  }
  if (effect == 89) {
    Serial.println(F("89 − Transition Ramp Up Long Sharp 2 – 0 to 100%"));
  }
  if (effect == 90) {
    Serial.println(F("90 − Transition Ramp Up Medium Sharp 1 – 0 to 100%"));
  }
  if (effect == 91) {
    Serial.println(F("91 − Transition Ramp Up Medium Sharp 2 – 0 to 100%"));
  }
  if (effect == 92) {
    Serial.println(F("92 − Transition Ramp Up Short Sharp 1 – 0 to 100%"));
  }
  if (effect == 93) {
    Serial.println(F("93 − Transition Ramp Up Short Sharp 2 – 0 to 100%"));
  }
  if (effect == 94) {
    Serial.println(F("94 − Transition Ramp Down Long Smooth 1 – 50 to 0%"));
  }
  if (effect == 95) {
    Serial.println(F("95 − Transition Ramp Down Long Smooth 2 – 50 to 0%"));
  }
  if (effect == 96) {
    Serial.println(F("96 − Transition Ramp Down Medium Smooth 1 – 50 to 0%"));
  }
  if (effect == 97) {
    Serial.println(F("97 − Transition Ramp Down Medium Smooth 2 – 50 to 0%"));
  }
  if (effect == 98) {
    Serial.println(F("98 − Transition Ramp Down Short Smooth 1 – 50 to 0%"));
  }
  if (effect == 99) {
    Serial.println(F("99 − Transition Ramp Down Short Smooth 2 – 50 to 0%"));
  }
  if (effect == 100) {
    Serial.println(F("100 − Transition Ramp Down Long Sharp 1 – 50 to 0%"));
  }
  if (effect == 101) {
    Serial.println(F("101 − Transition Ramp Down Long Sharp 2 – 50 to 0%"));
  }
  if (effect == 102) {
    Serial.println(F("102 − Transition Ramp Down Medium Sharp 1 – 50 to 0%"));
  }
  if (effect == 103) {
    Serial.println(F("103 − Transition Ramp Down Medium Sharp 2 – 50 to 0%"));
  }
  if (effect == 104) {
    Serial.println(F("104 − Transition Ramp Down Short Sharp 1 – 50 to 0%"));
  }
  if (effect == 105) {
    Serial.println(F("105 − Transition Ramp Down Short Sharp 2 – 50 to 0%"));
  }
  if (effect == 106) {
    Serial.println(F("106 − Transition Ramp Up Long Smooth 1 – 0 to 50%"));
  }
  if (effect == 107) {
    Serial.println(F("107 − Transition Ramp Up Long Smooth 2 – 0 to 50%"));
  }
  if (effect == 108) {
    Serial.println(F("108 − Transition Ramp Up Medium Smooth 1 – 0 to 50%"));
  }
  if (effect == 109) {
    Serial.println(F("109 − Transition Ramp Up Medium Smooth 2 – 0 to 50%"));
  }
  if (effect == 110) {
    Serial.println(F("110 − Transition Ramp Up Short Smooth 1 – 0 to 50%"));
  }
  if (effect == 111) {
    Serial.println(F("111 − Transition Ramp Up Short Smooth 2 – 0 to 50%"));
  }
  if (effect == 112) {
    Serial.println(F("112 − Transition Ramp Up Long Sharp 1 – 0 to 50%"));
  }
  if (effect == 113) {
    Serial.println(F("113 − Transition Ramp Up Long Sharp 2 – 0 to 50%"));
  }
  if (effect == 114) {
    Serial.println(F("114 − Transition Ramp Up Medium Sharp 1 – 0 to 50%"));
  }
  if (effect == 115) {
    Serial.println(F("115 − Transition Ramp Up Medium Sharp 2 – 0 to 50%"));
  }
  if (effect == 116) {
    Serial.println(F("116 − Transition Ramp Up Short Sharp 1 – 0 to 50%"));
  }
  if (effect == 117) {
    Serial.println(F("117 − Transition Ramp Up Short Sharp 2 – 0 to 50%"));
  }
  if (effect == 118) {
    Serial.println(F("118 − Long buzz for programmatic stopping – 100%"));
  }
  if (effect == 119) {
    Serial.println(F("119 − Smooth Hum 1 (No kick or brake pulse) – 50%"));
  }
  if (effect == 120) {
    Serial.println(F("120 − Smooth Hum 2 (No kick or brake pulse) – 40%"));
  }
  if (effect == 121) {
    Serial.println(F("121 − Smooth Hum 3 (No kick or brake pulse) – 30%"));
  }
  if (effect == 122) {
    Serial.println(F("122 − Smooth Hum 4 (No kick or brake pulse) – 20%"));
  }
  if (effect == 123) {
    Serial.println(F("123 − Smooth Hum 5 (No kick or brake pulse) – 10%"));
  }

  if (effect %2 == 0) {
    TCA9548A(0);
    // set the effect to play
    Serial.println(F("-------------------TCA9548A(0)------------------"));
    drv0.setWaveform(0, effect);  // play effect 
    drv0.setWaveform(1, 0);       // end waveform
    // play the effect!
    drv0.go();

  } else {
    TCA9548A(1);
    // set the effect to play
    Serial.println(F("-------------------TCA9548A(1)------------------"));
    drv1.setWaveform(0, effect);  // play effect 
    drv1.setWaveform(1, 0);       // end waveform
    // play the effect!
    drv1.go();
  }

  // wait a bit
  delay(500);

  effect++;

  if (effect > 117) effect = 1;
}
