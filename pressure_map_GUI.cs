/***********************************************
 * Project: Pressure Sensor 
 * Date: July 29, 2019
 * Rev 1
 * 
 * GUI code for visualizing the 9 sensor mat.
 * 
 * Author: Mavelyn Breiva
 *         Pavitra Kurseja
 ***********************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Globalization;
using System.Windows.Forms;

namespace PressureMap
{
    public partial class Form1 : Form
    {
        String str;                                                //To hold the string sent by the Arduino 
        String[] splitter = new String[9];                         //String array to split str 
        char[] separator = new char[] { '@' };                     //Separator sent through the Arduino string to assign values to their corresponding variable 
        float[] pressure = new float[9];                           //Array to hold the pressure at the sensors

        public Form1()
        {
            InitializeComponent();                                 //Initialization
            serialPort1.Open();                                    //Open Serial Port   
            Thread Loop_Thread = new Thread(Loop);                 //Begin Thread
            Loop_Thread.Start();
        }

        public void Loop()                                         //Infinite loop
        {
            while (true)
            {
                Display();                                         //Display the pressure map
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(Read_String);    //Read string sent by Arduino
            }
        }

        public void Display()                                      //Display values at each sensor obtained from backend Arduino code
        {
            for(int i = 0; i < pressure.Length; i++)
            {
                if(pressure[i] >= 200)
                {
                    switch (i)                                     //Set pressure map colour. Options are red, orange, yellow where red means there's a lot of pressure at that sensor
                    {
                        case 0:
                            Sensor_1.BackColor = Color.Red;
                            break;
                        case 1:
                            Sensor_2.BackColor = Color.Red;
                            break;
                        case 2:
                            Sensor_3.BackColor = Color.Red;
                            break;
                        case 3:
                            Sensor_4.BackColor = Color.Red;
                            break;
                        case 4:
                            Sensor_5.BackColor = Color.Red;
                            break;
                        case 5:
                            Sensor_6.BackColor = Color.Red;
                            break;
                        case 6:
                            Sensor_7.BackColor = Color.Red;
                            break;
                        case 7:
                            Sensor_8.BackColor = Color.Red;
                            break;
                    }

                }
                else if(pressure[i] < 200 && pressure[i] >= 10)
                {
                    switch (i)
                    {
                        case 0:
                            Sensor_1.BackColor = Color.Orange;
                            break;
                        case 1:
                            Sensor_2.BackColor = Color.Orange;
                            break;
                        case 2:
                            Sensor_3.BackColor = Color.Orange;
                            break;
                        case 3:
                            Sensor_4.BackColor = Color.Orange;
                            break;
                        case 4:
                            Sensor_5.BackColor = Color.Orange;
                            break;
                        case 5:
                            Sensor_6.BackColor = Color.Orange;
                            break;
                        case 6:
                            Sensor_7.BackColor = Color.Orange;
                            break;
                        case 7:
                            Sensor_8.BackColor = Color.Orange;
                            break;
                    }
                }
            }
        }

        public void Read_String(object sender, EventArgs e)         //Read string sent by Arduino, values are sent as one string each value seperated by an "@"
        {                                                           //pressure0@pressure1@pressure2...
            str = serialPort1.ReadExisting();                       //Read from serial port
            splitter = str.Split(separator, StringSplitOptions.None); //Split the string, seperator = '@' and assign to splitter[]

            float.TryParse(splitter[0], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[0]);   //Assign pressure[0] = splitter[0]
            float.TryParse(splitter[1], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[1]);   //Assign pressure[1] = splitter[1]...
            float.TryParse(splitter[2], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[2]);
            float.TryParse(splitter[3], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[3]);
            float.TryParse(splitter[4], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[4]);
            float.TryParse(splitter[5], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[5]);
            float.TryParse(splitter[6], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[6]);
            float.TryParse(splitter[7], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[7]);
            float.TryParse(splitter[8], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[8]);

            Thread.Sleep(500);                                                                              //Delay so to not spam the serial port. Delay matches Arduino delay.
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnSensor1_Click(object sender, EventArgs e)
        {
            Sensor_1.BackColor = Color.Red;
        }
    }
}

