#include <WiFi.h>
#include <HTTPClient.h>

int LED_BUILTIN = 2; //this is the blue led on the esp32 that lights up when connected to WIFI
int PIN_MOTOR = 23; //this is the signal pin connected to the esp32

const char* ssid = "YourWifiSSID"; //change this to your WIFI SSID
const char* password = "YourWifiPassword"; //change this to your WIFI password
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
    char incoming = RemoteClient.read(ReceiveBuffer, sizeof(ReceiveBuffer)); 
    RemoteClient.write(ReceiveBuffer, incoming);
    Serial.print("Received: ");
    Serial.println(incoming);

    digitalWrite(PIN_MOTOR, HIGH); //This will turn ON the ebb motor
    delay(75); //Change this to match your timing for the ebb. e.g. 75ms will run the ebb for 1 firing cycle.
    digitalWrite(PIN_MOTOR, LOW); //This will turn OFF the ebb motor      
  }
}

void setup() {
  pinMode(LED_BUILTIN, OUTPUT);
  pinMode (PIN_MOTOR, OUTPUT);//Specify that PIN MOTOR is output
  
  Serial.begin(115200);
  
  WiFi.begin(ssid, password);
  Serial.println("Connecting");
  while(WiFi.status() != WL_CONNECTED) { 
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
  if(WiFi.status()== WL_CONNECTED){ 
    CheckForConnections();
    EchoReceivedData();
  } else {
    Serial.println("WiFi Disconnected");
    digitalWrite(LED_BUILTIN, LOW);
  }
}
