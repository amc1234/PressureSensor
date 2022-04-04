#include <Arduino.h>
// #include file used for bluetooth portion

import controlP5.*;             //import controlP5 library
import processing.serial.*;     //serial port library
​
Serial port;             //serial port definition
​
ControlP5 cp5;          //create an object of class controlP5 so we can display rectangles
​
Button sensor1;         //9 rectangles that are displayed
Button sensor2;
Button sensor3;
Button sensor4;
Button sensor5;
Button sensor6;
Button sensor7;
Button sensor8;
Button sensor9;
​
float[] sensor_data = {0,0,0,0,0,0,0,0,0}; //array to hold sensor data
​
char separator = '@';  //separator definition
​
void setup(){           //setup function
   port = new Serial(this, "COM5", 9600);  //i have connected arduino to COM5, it would be different in linux and mac os
   
}

void loop ( ){
  read_string();
  
  bluetoothWrite( sensor_data ); // Function taken from the bluetooth portion and might have to be renamed and included as header
  delay(1000); // takes input once per second
}
​
void read_string(){
  String input = port.readString();
  if(input != null){
    float[] data_read = float(split(input, separator));
  
    for (int i = 0; i < 9; i++){
      sensor_data[i]=data_read[i];
    }
  }
}
