/***********************************************
 * Project: Pressure Sensor 
 * Date: July 16, 2019
 * Rev 2
 * 
 * This is the Arduino code for the rev 1 pressure sensor mat.
 * It is made for a small prototype of 9 sensors connected
 * through a 8:1 mux reading at A1 and the remaining sensor at A0.
 * It sends a converted pressure string to the frontend (VS).
 * 
 * Author: Andrea Martinez Chung
 *         Yue chen (Cindy) Yu
 ***********************************************/

#define NUM_INPUTS 8         //number of inputs going through the multiplexer

#define SENSOR_9 A0          //9th sensor pin
#define MUX A1               //multiplexer pin

#define PIN_A 7              //pin for control signal A
#define PIN_B 6              //pin for control signal B
#define PIN_C 5              //pin for control signal C

bool ctrl_A = 0;                  //value at control A
bool ctrl_B = 0;                  //value at control B
bool ctrl_C = 0;                  //value at control C

float pressure[9];

void setup() {
  Serial.begin(9600);             //Serial initialization
  pinMode(PIN_A, OUTPUT);         //Initializing control pins as ouputs
  pinMode(PIN_B, OUTPUT);
  pinMode(PIN_C, OUTPUT);

  pinMode(MUX, INPUT);            //Initializing sensor pins as inputs
  pinMode(SENSOR_9, INPUT);
}

void loop(){
  cycle_mux();
  send_string();
}

void cycle_mux(){
  for(int i = 0; i < NUM_INPUTS; i++){  //cycling through all possible 3-bit values to read from every possible sensor
    ctrl_A = bitRead(i,0);              //Take first bit from binary value of i channel.
    ctrl_B = bitRead(i,1);              //Take second bit from binary value of i channel.
    ctrl_C = bitRead(i,2);              //Take third bit from value of i channel.

    digitalWrite(PIN_A, ctrl_A);        //Write to digital pins to send control signals
    digitalWrite(PIN_B, ctrl_B);
    digitalWrite(PIN_C, ctrl_C);

    pressure[i] = analogRead(MUX);      //Read value coming from mux
    pressure[8] = analogRead(SENSOR_9); //Read value coming from sensor_9

    bits_to_pressure(i);                //Convert 1023bits (max 10bit value from ADC) to voltage
  }
}

void bits_to_pressure(int chosen_sensor){         //converts bits to voltage to resistance to force to pressure
  float temp = pressure[chosen_sensor]*(5/1023);  //based on the equation y = (5/1023)x //converts to voltage
  temp = (50000-(10000*temp))/temp;               //converts to resistance based on KCL analysis of voltage divider
  //convert resistance to force
  //convert force to pressure (Force/Area)
}

void send_string(){
  for(int i = 0; i < (NUM_INPUTS+1); i++){  //sending variables to Visual Studio separated by '@'
    Serial.print(pressure[i]);
    Serial.print('@');
  } 
  delay(1000);    
}
