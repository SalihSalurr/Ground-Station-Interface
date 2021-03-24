using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using MapWinGIS;
using AxMapWinGIS;

namespace ARAYÜZTASARIMI
{
    public partial class Form1 : Form
    {
        int providerid;
        TileProviders providers;
        string[] Gyro;
        private string data;
        long max = 30, min = 0;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            comboBox1.DataSource = SerialPort.GetPortNames();
            comboBox2.Items.Add(4800);
            comboBox2.Items.Add(9600);
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialPort1_DataReceived);
            providers = axMap1.Tiles.Providers;
            providerid = (int)tkTileProvider.ProviderCustom + 1001;

            providers.Add(providerid, "map", "http://127.0.0.1/sat/z{zoom}/{y}/{x}.jpg",tkTileProjection.SphericalMercator,0,10);

            axMap1.Tiles.ProviderId = providerid;
        }
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            git:
            data = serialPort1.ReadLine();  
            Gyro= data.Split(',');
            if (Gyro.Length != 7)
                goto git;
            this.Invoke(new EventHandler(displayData_event));
        }
        private void displayData_event(object sender, EventArgs e)
        {
            textBox4.Text = Gyro[0];
            textBox8.Text = Gyro[1];
            textBox9.Text = Gyro[2];
            textBox10.Text = Gyro[3];
            textBox7.Text = Gyro[4];
            textBox6.Text = Gyro[5];
            textBox5.Text = Gyro[6];

            chart1.ChartAreas[0].AxisX.Minimum = min;
            chart1.ChartAreas[0].AxisX.Maximum = max;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 50;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(min, max);

            chart2.ChartAreas[0].AxisX.Minimum = min;
            chart2.ChartAreas[0].AxisX.Maximum = max;
            chart2.ChartAreas[0].AxisY.Minimum = -3;
            chart2.ChartAreas[0].AxisY.Maximum = 3;
            chart2.ChartAreas[0].AxisX.ScaleView.Zoom(min, max);

            this.chart1.Series[0].Points.AddXY((min + max) / 2, Gyro[0]);
            this.chart2.Series[0].Points.AddXY((min + max) / 2, Gyro[1]);
            max++;
            min++;
           

            serialPort1.DiscardInBuffer();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            serialPort1.BaudRate = int.Parse(comboBox2.Text);
            serialPort1.PortName = comboBox1.Text;
        }

      
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;  
                serialPort1.BaudRate = 9600;            
                serialPort1.Open();
                button3.Enabled = true;
                button1.Enabled = false; 
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");    
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen == true)
              serialPort1.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            button1.Enabled = true;
            button3.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string filelocation = @"C:\Users\Salih\Desktop\Roket\ARAYÜZ\ARAYÜZTASARIMI";                                   
                string filename = "data.txt";                                                               
                System.IO.File.WriteAllText(filelocation + filename, "Sıcaklık: " + textBox4.Text + "\tGyroX: " + textBox7.Text + "\tGyroY: " + textBox6.Text + "\tGyroZ: " + textBox5.Text + "\tIvmeX: " + textBox8.Text + "\tIvmeY: " + textBox9.Text + "\tIvmeZ: " + textBox10.Text);
                MessageBox.Show("Dosya başarıyla kaydedildi", "Mesaj");                                    
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message, "Hata");       //Hata mesajı
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label14.Text = DateTime.Now.ToLongDateString();
            label15.Text = DateTime.Now.ToLongTimeString();
        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        
    }
}
