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
        float[] pressure = new float[9];

        public Form1()
        {
            InitializeComponent();
            serialPort1.Open();
            Thread Loop_Thread = new Thread(Loop);
            Loop_Thread.Start();
        }

        public void Loop()
        {
            while (true)
            {
                Display();
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(Read_String);
            }
        }

        public void Display() // obtain array of pressure from backend (?)
        {
            for(int i = 0; i < pressure.Length; i++)
            {
                if(pressure[i] >= 200)
                {
                    switch (i)
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

        public void Read_String(object sender, EventArgs e)
        {
            str = serialPort1.ReadExisting();
            splitter = str.Split(separator, StringSplitOptions.None);

            float.TryParse(splitter[0], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[0]);   /*Vin_48V*/
            float.TryParse(splitter[1], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[1]);   /*Iin_48V*/
            float.TryParse(splitter[2], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[2]);   /*Vin_12V*/
            float.TryParse(splitter[3], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[3]);   /*Iin_12V*/
            float.TryParse(splitter[4], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[4]);   /*Vin_48V*/
            float.TryParse(splitter[5], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[5]);   /*Iin_48V*/
            float.TryParse(splitter[6], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[6]);   /*Vin_12V*/
            float.TryParse(splitter[7], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[7]);   /*Iin_12V*/
            float.TryParse(splitter[8], NumberStyles.Any, CultureInfo.InvariantCulture, out pressure[8]);

            Thread.Sleep(500);
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

