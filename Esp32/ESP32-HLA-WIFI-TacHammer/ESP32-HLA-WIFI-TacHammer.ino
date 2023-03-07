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

Adafruit_DRV2605 drv;

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
    char incoming = RemoteClient.read();
    RemoteClient.write(ReceiveBuffer, incoming);

    //     if (strcmp(&incoming, "a") == 0) {
    //      digitalWrite(PIN_MOTOR, HIGH); //This will turn ON the ebb motor
    //      delay(75); //Change this to match your timing for the ebb. e.g. 75ms will run the ebb for 1 firing cycle.
    //      digitalWrite(PIN_MOTOR, LOW); //This will turn OFF the ebb motor
    //
    //    } else if (strcmp(&incoming, "1") == 0) {
    //      digitalWrite(PIN_MOTOR, HIGH); //This will turn ON the ebb motor
    //
    //    } else if (strcmp(&incoming, "0") == 0) {
    //      digitalWrite(PIN_MOTOR, LOW); //This will turn OFF the ebb motor
    //    }

    // set the effect to play
    drv.setWaveform(0, 1);  // play effect
    drv.setWaveform(1, 0);       // end waveform

    // play the effect!
    drv.go();
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

  //DRV2605l setup begin
  // Serial.begin(9600);
  Serial.println("DRV test");
  drv.begin();

  // I2C trigger by sending 'go' command
  drv.setMode(DRV2605_MODE_INTTRIG);  // default, internal trigger when sending GO command

  drv.selectLibrary(1);
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
  } else {
    Serial.println("WiFi Disconnected");
    digitalWrite(LED_BUILTIN, LOW);
  }
}
