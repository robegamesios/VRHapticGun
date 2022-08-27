#include <WiFi.h>
#include <HTTPClient.h>

int LED_BUILTIN = 2;
int PIN_MOTOR = 23;

const char* ssid = "YourWifiSSID";
const char* password = "YourWifiPassword";
const uint ServerPort = 23;
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

    digitalWrite(PIN_MOTOR, HIGH);
    delay(75);
    digitalWrite(PIN_MOTOR, LOW);      
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
