
#include "UnoJoy.h"

#define UP_PIN 2
#define RIGHT_PIN 3
#define LEFT_PIN 4
#define DOWN_PIN 5
#define SQUARE_PIN 9
#define TRIANGLE_PIN 10
#define CIRCLE_PIN 11
#define CROSS_PIN 12

#define BASELINE 2

void setup(){
  //setupPins();
  setupUnoJoy();
}

void loop(){
  // Always be getting fresh data
  dataForController_t controllerData = getControllerData();
  setControllerData(controllerData);
}

void setupPins(void){
  // Set all the digital pins as inputs
  // with the pull-up enabled, except for the 
  // two serial line pins
}

// readCapacitivePin
//  Input: Arduino pin number
//  Output: A number, from 0 to 17 expressing
//          how much capacitance is on the pin
//  When you touch the pin, or whatever you have
//  attached to it, the number will get higher
uint8_t readCapacitivePin(int pinToMeasure){
  // This is how you declare a variable which
  //  will hold the PORT, PIN, and DDR registers
  //  on an AVR
  volatile uint8_t* port;
  volatile uint8_t* ddr;
  volatile uint8_t* pin;
  // Here we translate the input pin number from
  //  Arduino pin number to the AVR PORT, PIN, DDR,
  //  and which bit of those registers we care about.
  byte bitmask;
  if ((pinToMeasure >= 0) && (pinToMeasure <= 7)){
    port = &PORTD;
    ddr = &DDRD;
    bitmask = 1 << pinToMeasure;
    pin = &PIND;
  }
  if ((pinToMeasure > 7) && (pinToMeasure <= 13)){
    port = &PORTB;
    ddr = &DDRB;
    bitmask = 1 << (pinToMeasure - 8);
    pin = &PINB;
  }
  if ((pinToMeasure > 13) && (pinToMeasure <= 19)){
    port = &PORTC;
    ddr = &DDRC;
    bitmask = 1 << (pinToMeasure - 13);
    pin = &PINC;
  }
  // Discharge the pin first by setting it low and output
  *port &= ~(bitmask);
  *ddr  |= bitmask;
  delay(1);
  // Make the pin an input with the internal pull-up on
  *ddr &= ~(bitmask);
  *port |= bitmask;
  // Now see how long the pin to get pulled up
  int cycles = 17;
  for(int i = 0; i < 16; i++){
    if (*pin & bitmask){
      cycles = i;
      break;
    }
  }
  // Discharge the pin again by setting it low and output
  //  It's important to leave the pins low if you want to 
  //  be able to touch more than 1 sensor at a time - if
  //  the sensor is left pulled high, when you touch
  //  two sensors, your body will transfer the charge between
  //  sensors.
  *port &= ~(bitmask);
  *ddr  |= bitmask;
  
  return cycles;
}

dataForController_t getControllerData(void){
  // Set up a place for our controller data
  dataForController_t controllerData = getBlankDataForController();
 
  if (readCapacitivePin(DOWN_PIN) > BASELINE)
    controllerData.dpadDownOn = 1;

  if (readCapacitivePin(UP_PIN) > BASELINE)
    controllerData.dpadUpOn = 1;
    
  if (readCapacitivePin(LEFT_PIN) > BASELINE)
    controllerData.dpadLeftOn = 1;
    
  if (readCapacitivePin(RIGHT_PIN) > BASELINE)
    controllerData.dpadRightOn = 1;
    
  if (readCapacitivePin(SQUARE_PIN) > BASELINE)
    controllerData.squareOn = 1;
    
  if (readCapacitivePin(TRIANGLE_PIN) > BASELINE)
    controllerData.triangleOn = 1;
    
  if (readCapacitivePin(CIRCLE_PIN) > BASELINE)
    controllerData.circleOn = 1;
    
  if (readCapacitivePin(CROSS_PIN) > BASELINE)
    controllerData.crossOn = 1;
    
  // And return the data!
  return controllerData;
}

void printPins(){
  Serial.print("2: ");
  Serial.println(readCapacitivePin(2));
  Serial.print("3: ");
  Serial.println(readCapacitivePin(3));
  Serial.print("4: ");
  Serial.println(readCapacitivePin(4));
  Serial.print("5: ");
  Serial.println(readCapacitivePin(5));
  Serial.print("6: ");
  Serial.println(readCapacitivePin(6));
  Serial.print("7: ");
  Serial.println(readCapacitivePin(7));
  Serial.print("8: ");
  Serial.println(readCapacitivePin(8));
  Serial.print("9: ");
  Serial.println(readCapacitivePin(9));
  Serial.println();
}
