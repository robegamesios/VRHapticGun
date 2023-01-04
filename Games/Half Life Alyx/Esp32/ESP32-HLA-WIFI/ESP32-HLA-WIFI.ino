#define ESP_DRD_USE_EEPROM      true
#define ESP_DRD_USE_SPIFFS      false    //false
#define DOUBLERESETDETECTOR_DEBUG       true  //false

#include <ESP_DoubleResetDetector.h>  
#include <WiFi.h>
#include <HTTPClient.h>
#include <WiFiManager.h>

// Number of seconds after reset during which a
// subseqent reset will be considered a double reset.
#define DRD_TIMEOUT 10

// RTC Memory Address for the DoubleResetDetector to use
#define DRD_ADDRESS 0

DoubleResetDetector* drd;

int LED_BUILTIN = 2; //this is the blue led on the esp32 that lights up when connected to WIFI
int PIN_MOTOR = 23; //this is the signal pin connected to the esp32

const uint ServerPort = 23; //23 is your port number. Change this to match the port number you specified in Programs.cs file
WiFiServer Server(ServerPort);
WiFiClient RemoteClient;

void CheckForConnections()
{
  if (Server.hasClient())
  {
    // If we are already connected to another computer, 
    // then reject the new connection. Otherwise accept
    // the connection. 
    if (RemoteClient.connected())
    {
      Serial.println("Connection rejected");
      Server.available().stop();
    }
    else
    {
      Serial.println("Connection accepted");
      RemoteClient = Server.available();
    }
  }
}

void EchoReceivedData()
{
  uint8_t ReceiveBuffer[30];
  while (RemoteClient.connected() && RemoteClient.available())
  {
    char incoming = RemoteClient.read();
    RemoteClient.write(ReceiveBuffer, incoming);

      digitalWrite(PIN_MOTOR, HIGH); //This will turn ON the ebb motor
      delay(75); //Change this to match your timing for the ebb. e.g. 75ms will run the ebb for 1 firing cycle.
      digitalWrite(PIN_MOTOR, LOW); //This will turn OFF the ebb motor

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
  }
}

void setup() {
  pinMode(LED_BUILTIN, OUTPUT);
  pinMode (PIN_MOTOR, OUTPUT);//Specify that PIN MOTOR is output

  //setup wifi
  WiFi.mode(WIFI_STA); // explicitly set mode, esp defaults to STA+AP    
  Serial.begin(115200);

  WiFiManager wm;
  
  //wm.resetSettings(); //incase you need to manually reset the esp32 board for testing
  
  drd = new DoubleResetDetector(DRD_TIMEOUT, DRD_ADDRESS);

  if (drd->detectDoubleReset())
  {
    Serial.println("Double Reset Detected");
    digitalWrite(LED_BUILTIN, LOW);
    wm.resetSettings();
  }
  else {
    Serial.println("No Double Reset Detected");
    if(WiFi.status()== WL_CONNECTED) {
       digitalWrite(LED_BUILTIN, HIGH);
    } else {
      digitalWrite(LED_BUILTIN, LOW);
    }
  }
  

  wm.autoConnect("HapticGun_Connect");
  
  Serial.println("Connecting");
  while(WiFi.status() != WL_CONNECTED) {
    digitalWrite(LED_BUILTIN, LOW); 
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.print("Connected to WiFi network with IP Address: ");
  Serial.println(WiFi.localIP());
  digitalWrite(LED_BUILTIN, HIGH); 
  
  Server.begin();
}

void loop() {
  // Call the double reset detector loop method every so often,
  // so that it can recognise when the timeout expires.
  // You can also call drd.stop() when you wish to no longer
  // consider the next reset as a double reset.
  drd->loop();
    
  if(WiFi.status()== WL_CONNECTED){ 
    CheckForConnections();
    EchoReceivedData();
  } else {
    Serial.println("WiFi Disconnected");
    digitalWrite(LED_BUILTIN, LOW);
  }
}
