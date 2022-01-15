/**********************************************************************************************************
* Project: Pressure Sensitive Walkway
* Description: Code to read pressure values from a 23 x 18 pressure sensor matrix
* Original Source Code: https://www.sensitronics.com/tutorials/fsr-matrix-array/page3.php
* Edited by: Mavelyn Breiva
* LastRev: 11/25/2019
* Version 11 - Connects each MUX to its own COM pin on Arduino
/**********************************************************************************************************
* MACROS / PIN DEFS
**********************************************************************************************************/
#define BAUD_RATE                 115200
#define ROW_COUNT                 18
#define COLUMN_COUNT              23

#define PIN_ADC_INPUT_0           A0          //MUX input
#define PIN_ADC_INPUT_1           A1
#define PIN_ADC_INPUT_2           A2
#define PIN_SHIFT_REGISTER_DATA   2
#define PIN_SHIFT_REGISTER_CLOCK  3
#define PIN_SHIFT_REGISTER_LATCH  10 //latch = storage register clock pin
#define PIN_MUX_CHANNEL_0         4  //A Channel pins 0, 1, 2, etc must be wired to consecutive Arduino pins
#define PIN_MUX_CHANNEL_1         5  //B
#define PIN_MUX_CHANNEL_2         6  //C
#define PIN_MUX_INHIBIT_0         7  //inhibit = active low enable. All mux IC enables must be wired to consecutive Arduino pins ***INHIBITS MUST BE CONSECUTIVE FOR CODE TO WORK***
#define PIN_MUX_INHIBIT_1         8
#define PIN_MUX_INHIBIT_2         9

#define ROWS_PER_MUX              8
#define MUX_COUNT                 3
#define CHANNEL_PINS_PER_MUX      3

int sensorMatrix[ROW_COUNT][COLUMN_COUNT];
double pressureMatrix[ROW_COUNT][COLUMN_COUNT];

int numOfRegisters = 3;
byte* registerState;

int voltage;

double sensorArea = 6.35*6.35;
double pressure;
double current;
double resistance;
double force;

/**********************************************************************************************************
* GLOBALS
**********************************************************************************************************/

int current_enabled_mux = 0;  //Start with first MUX

/**********************************************************************************************************
* setup()
**********************************************************************************************************/
void setup()
{
  Serial.begin(BAUD_RATE);
  pinMode(PIN_ADC_INPUT_0, INPUT);
  pinMode(PIN_ADC_INPUT_1, INPUT);
  pinMode(PIN_ADC_INPUT_2, INPUT);
  pinMode(PIN_SHIFT_REGISTER_DATA, OUTPUT);
  pinMode(PIN_SHIFT_REGISTER_CLOCK, OUTPUT);
  pinMode(PIN_SHIFT_REGISTER_LATCH, OUTPUT);
  pinMode(PIN_MUX_CHANNEL_0, OUTPUT);
  pinMode(PIN_MUX_CHANNEL_1, OUTPUT);
  pinMode(PIN_MUX_CHANNEL_2, OUTPUT);
  pinMode(PIN_MUX_INHIBIT_0, OUTPUT);
  pinMode(PIN_MUX_INHIBIT_1, OUTPUT);
  pinMode(PIN_MUX_INHIBIT_2, OUTPUT);

  //Initialize array
  registerState = new byte[numOfRegisters];
  for (size_t i = 0; i < numOfRegisters; i++) {
    registerState[i] = 0;
  }

  digitalWrite(PIN_MUX_INHIBIT_0, HIGH);
  digitalWrite(PIN_MUX_INHIBIT_1, HIGH);
  digitalWrite(PIN_MUX_INHIBIT_2, HIGH);
}


/**********************************************************************************************************
* loop()
**********************************************************************************************************/
void loop(){
  readSensor();
  printMatrix(sensorMatrix);
  //printPressureMatrix(pressureMatrix);
}

void setRow(int row_number)                             // Enable single MUX IC and Channel to read specified matrix row
{
  for(int i = 0; i < CHANNEL_PINS_PER_MUX; i ++)        //Setting the channel pins
  {
    if(bitRead((row_number % 8), i))                          //0 = 000, 1 = 100,  2 = 010,..., 7 = 111  ---- If this returns true (1), write 1 (HIGH) to select pin
    {
      digitalWrite(PIN_MUX_CHANNEL_0 + i, HIGH);
    }
    else                                                      //If this returns false (0), write 0 (LOW) to select pin
    {
      digitalWrite(PIN_MUX_CHANNEL_0 + i, LOW);
    }
  }
}

void readSensor(){
  for(int i = 0; i < COLUMN_COUNT; i++){  //Cycle through columns
    regWrite(i,HIGH);
    
    for(int j = 0; j < ROW_COUNT; j++){
      setRow(j);
      
      if(j < 8){
        sensorMatrix[j][i] = analogRead(PIN_ADC_INPUT_0);
        pressureMatrix[j][i] = bits_to_pressure(sensorMatrix[j][i]);
      }
      else if(j < 16){
        sensorMatrix[j][i] = analogRead(PIN_ADC_INPUT_1);
        pressureMatrix[j][i] = bits_to_pressure(sensorMatrix[j][i]);
      }
      else{
        sensorMatrix[j][i] = analogRead(PIN_ADC_INPUT_2);
        pressureMatrix[j][i] = bits_to_pressure(sensorMatrix[j][i]);
      }
    }
    
    regWrite(i,LOW);

  }
  delay(300);
}

void printMatrix(int matrix[][COLUMN_COUNT]){         // Printing values obtained from the sensor matrix with 23 columns
    for(int i = 0; i < ROW_COUNT; i++){
      for(int j = 0; j < COLUMN_COUNT; j++){
        if(matrix[i][j] < 10){                        // Single digit formatting
          Serial.print("   ");
        }
        else if(matrix[i][j] < 100){                  // Double digit formatting
          Serial.print("  ");
        }
        else if(matrix[i][j] < 1000){                 // Triple digit formatting
          Serial.print(" ");
        }
        Serial.print(" ");                            // Quadruple digit formatting; Max value is 1023 
        Serial.print(matrix[i][j]);
      }
      Serial.print("\n");                             // Prints new line
    }
    Serial.print("\n");
}

void printPressureMatrix(double matrix[][COLUMN_COUNT]){         // Printing values obtained from the sensor matrix with 23 columns
    for(int i = 0; i < ROW_COUNT; i++){
      for(int j = 0; j < COLUMN_COUNT; j++){
        if(matrix[i][j] < 10.0){                        // Single digit formatting
          Serial.print("   ");
        }
        else if(matrix[i][j] < 100.0){                  // Double digit formatting
          Serial.print("  ");
        }
        else if(matrix[i][j] < 1000.0){                 // Triple digit formatting
          Serial.print(" ");
        }
        Serial.print(" ");                            // Quadruple digit formatting; Max value is 1023 
        Serial.print(matrix[i][j]);
      }
      Serial.print("\n");                             // Prints new line
    }
    Serial.print("\n");
}

void regWrite(int pin, bool state){
  //Determines register
  int reg = pin / 8;
  //Determines pin for actual register
  int actualPin = pin - (8 * reg);

  //Begin session
  digitalWrite(PIN_SHIFT_REGISTER_LATCH, LOW);

  for (int i = 0; i < numOfRegisters; i++){
    //Get actual states for register
    byte* states = &registerState[i];

    //Update state
    if (i == reg){
      bitWrite(*states, actualPin, state);
    }

    //Write
    shiftOut(PIN_SHIFT_REGISTER_DATA, PIN_SHIFT_REGISTER_CLOCK, MSBFIRST, *states);
  }

  //End session
  digitalWrite(PIN_SHIFT_REGISTER_LATCH, HIGH);
}

double bits_to_pressure(int bitValue){
  double voltage = bitValue*(5.0/1023.0); //in volts
  if(voltage == 0.00){
    return 0;
  }
  current = voltage/10000.0;    // initialize current; voltage / pull-down resistance in amps
  resistance = (5.0-voltage)/current;     // Ohm's Law V=IR in ohms
  force = 7.557*pow(10, 4)*pow(1/resistance, 1.666);     //relationship between force and resistance was determine by testing in a study
  pressure = force / sensorArea;

  return pressure;
}
