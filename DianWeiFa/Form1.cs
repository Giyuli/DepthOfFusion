using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using ZedGraph;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using DianWeiFa;
using System.Drawing.Drawing2D;


namespace DepthOfFusion

{
    public partial class Form1 : Form
    {
        #region///初始化
        public Form1()
        {

            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }
        #endregion
        #region (定义一些全局变量)
        zuixiaoercheng zuixiaoerchengfa = new zuixiaoercheng();
        int i = 0;
        int[] jiaodu1 = new int[10000];
        int[] jiaodu2 = new int[10000];
        double[] jiaodu3 = new double[1000000];
        double[] jiaodu4 = new double[1000000];
        double[,] data = new double[20, 10000];
        double[,] data_handle = new double[20, 10000];
        int[,] data2 =new int [18,100000];//最多保留一百万个数据数据
        double[] data3 = new double[10000];//最多保留一百万个数据数据
        double[] data4= new double[100000];//最多保留一百万个数据数据
        #region
        double[] data5 = new double[10000];//最多保留一百万个数据数据
        double[] data6 = new double[10000];//最多保留一百万个数据数据
        double[] data7 = new double[10000];//最多保留一百万个数据数据
        double[] data8 = new double[10000];//最多保留一百万个数据数据
        double[] data9 = new double[10000];//最多保留一百万个数据数据
        double[] data10 = new double[1000];//最多保留一百万个数据数据
        double[] data11 = new double[10000];//最多保留一百万个数据数据
        double[] data12 = new double[10000];//最多保留一百万个数据数据
        double[] data13 = new double[10000];//最多保留一百万个数据数据
        double[] data14 = new double[10000];//最多保留一百万个数据数据
        double[] data15 = new double[10000];//最多保留一百万个数据数据
        double[] data16 = new double[10000];//最多保留一百万个数据数据
        double[] data17 = new double[10000];//最多保留一百万个数据数据
        double[] data18 = new double[10000];//最多保留一百万个数据数据
        double[] data19 = new double[10000];//最多保留一百万个数据数据
        double[] data20 = new double[10000];//最多保留一百万个数据数据
        #endregion
        private int angle = 0;/////钝角、锐角焊缝的角度
        private int mean_count = 0;
        private double longth = 0;
        private double width = 0;
        private double lvbocanshu1 = 0;
        private double lvbocanshu2 = 0;
        private int edge_longth = 0;/////定义焊缝斜边长度
        long original_num = 0;

        long original_num1 = 0;
        int[] ADC_Value = new int[10000000];
        double ADC_Value_0;
        double ADC_Value_1;
        #region
        double ADC_Value_2;
        double ADC_Value_3;
        double ADC_Value_4;
        double ADC_Value_5;
        double ADC_Value_6;
        double ADC_Value_7;
        double ADC_Value_8;
        double ADC_Value_9;
        double ADC_Value_10;
        double ADC_Value_11;
        double ADC_Value_12;
        double ADC_Value_13;
        double ADC_Value_14;
        double ADC_Value_15;
        double ADC_Value_16;
        double ADC_Value_17;
        #endregion
        int Vcc = 5;
        int ADC_Full_Value = 16777215;
        double S1 = 0.0225;//0.75;//0.0225
        long l = 0;
        long n;
        private string FilePath;  //数据文件路径  

        private long displayrow = 0;   //以时间显示计数
        private long displayrow1 = 0;   //以时间显示计数
        private bool linkflag = false;
        private bool startflag = false;
        private bool chselectokflag = false;
        private bool browseflag = false;
        private bool[] chflag = new bool[18]; //通道选取标志 
        #endregion
        #region（开始按钮，设置串口初始化参数，设置串口接收方式，端口查询）
        private void StartButton_Click_1(object sender, EventArgs e)
        {
            //mean_count = int.Parse(mean_count1.Text);//读入多少个值做一次平均（设置默认结果为1）
            mean_count = 1;//更改值得平均计算，改为1，删除了对应的按钮
           
            if (StartButton.Text == "停止检测")
            {
                serialPort1.Close();
                StartButton.Text = "开始检测";
                StartButton.BackColor = Color.Pink;
                StartButton.Enabled = true;//打开串口按钮不可用
                SaveButton.Enabled = true;
                browseButton.Enabled = true;
                runButton.Enabled = true;
                exitButton.Enabled = true;
                //timer1.Enabled = false;
            }
            else
            {                
                serialPort1.Open();//关闭串口
                
                StartButton.Text = "停止检测";
                StartButton.BackColor = Color.GreenYellow;
                StartButton.Enabled = true;//打开串口按钮不可用
                SaveButton.Enabled = true;
                browseButton.Enabled = true;
                runButton.Enabled = true;
                exitButton.Enabled = true;              
                serialPort1Writeint();//向串口发送aa请求串口回复数据                  
            }
        }       
       
        private void Form1_Load(object sender, EventArgs e)//主窗体入口
        {
            timer.Enabled = true;
            //zedGraphControl1.Height = 390;
            //panel2.Visible = false;
            //panel2.SendToBack();//置于底层
            //ZedGraph1Init();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;//下拉框只能选择已有选项
            //搜索端口
            string[] portName = SerialPort.GetPortNames();
            for (int i = 0; i < portName.Length; i++)//读取
            {
                comboBox1.Items.Add(portName[i]);
            }
            comboBox1.SelectedIndex = 0;
            //comboBox2.Text = "115200";//波特率默认值

            serialPort1.BaudRate = Convert.ToInt32("115200", 10);//十进制数据转换

            /*****************非常重要************************/
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);//必须手动添加事件处理程序
            StartButton.Enabled = true;

        }
        

        private void button1_Click(object sender, EventArgs e)//端口查询
        {

            comboBox1.Items.Clear();
            string[] portName = SerialPort.GetPortNames();
            for (int i = 0; i < portName.Length; i++)//读取
            {
                comboBox1.Items.Add(portName[i]);
            }
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show(" 无端口可用");
            }
        }
        #endregion
        #region（向端写入数据以请求以请求串口发送）
        public void serialPort1Writeint()
        {

            byte[] a = new byte[1];
            a[0] = 0xaa;
            try
            {
                Thread.Sleep(15);//此处做线程延时有利于缓存充分读取
                serialPort1.Write(a, 0, 1);
                //Thread.Sleep(1);此处做线程延时会导致程序异常中断
            }
            catch
            {
            }

        }
        #endregion     
        #region（串口数据接收及处理）（加数据处理部分）
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)//串口数据接收事件
        {
            byte [] readBuffer = new byte[serialPort1.ReadBufferSize];//serialPort1.ReadBufferSize
            try { serialPort1.Read(readBuffer, 0, readBuffer.Length); }//readBuffer.Length
            catch {  }
            {
                if (readBuffer[0] == 165 && readBuffer[1] == 165 && readBuffer[2] == 165 && readBuffer[61] == 255)
                {
                    jiaodu1[i] = readBuffer[57] + readBuffer[58] * 256;
                    jiaodu2[i] = readBuffer[59] + readBuffer[60] * 256;
                    data2[0, i] = readBuffer[3]* 65536 + readBuffer[4] * 256 + readBuffer[5];
                    data2[1, i] = readBuffer[6]* 65536 + readBuffer[7] * 256 + readBuffer[8];
                    #region
                    data2[2, i] = readBuffer[9]* 65536 + readBuffer[10] * 256 + readBuffer[11];
                    data2[3, i] = readBuffer[12]* 65536 + readBuffer[13] * 256 + readBuffer[14];
                    data2[4, i] = readBuffer[15]* 65536 + readBuffer[16] * 256 + readBuffer[17];
                    data2[5, i] = readBuffer[18]* 65536+ readBuffer[19] * 256 + readBuffer[20];
                    data2[6, i] = readBuffer[21]* 65536 + readBuffer[22] * 256 + readBuffer[23];
                    data2[7, i] = readBuffer[24]* 65536 + readBuffer[25] * 256 + readBuffer[26];
                    data2[8, i] = readBuffer[27]* 65536 + readBuffer[28] * 256 + readBuffer[29];
                    data2[9, i] = readBuffer[30]* 65536 + readBuffer[31] * 256 + readBuffer[32];
                    data2[10, i] = readBuffer[33]* 65536 + readBuffer[34] * 256 + readBuffer[35];
                    data2[11, i] = readBuffer[36]* 65536 + readBuffer[37] * 256 + readBuffer[38];
                    data2[12, i] = readBuffer[39]* 65536 + readBuffer[40] * 256 + readBuffer[41];
                    data2[13, i] = readBuffer[42]* 65536 + readBuffer[43] * 256 + readBuffer[44];
                    data2[14, i] = readBuffer[45]* 65536 + readBuffer[46] * 256 + readBuffer[47];
                    data2[15, i] = readBuffer[48]* 65536 + readBuffer[49] * 256 + readBuffer[50];
                    data2[16, i] = readBuffer[51]* 65536 + readBuffer[52] * 256 + readBuffer[53];
                    data2[17, i] = readBuffer[54]* 65536 + readBuffer[55] * 256 + readBuffer[56];
                    #endregion
                    i++;
                    n++;
                       
                    if (n == 12)
                    {
                        int sum = 0;
                        int sum1 = 0;
                        #region
                        int sum2 = 0;
                        int sum3 = 0;
                        int sum4 = 0;
                        int sum5 = 0;
                        int sum6 = 0;
                        int sum7 = 0;
                        int sum8 = 0;
                        int sum9 = 0;
                        int sum10 = 0;
                        int sum11 = 0;
                        int sum12 = 0;
                        int sum13 = 0;
                        int sum14 = 0;
                        int sum15 = 0;
                        int sum16 = 0;
                        int sum17 = 0;
                        #endregion
                        for (i = 10; i < 12; i++)
                        {
                            sum += data2[0, i];
                            sum1 += data2[1, i];
                            #region
                            sum2 += data2[2, i];
                            sum3 += data2[3, i];
                            sum4 += data2[4, i];
                            sum5 += data2[5, i];
                            sum6 += data2[6, i];
                            sum7 += data2[7, i];
                            sum8 += data2[8, i];
                            sum9 += data2[9, i];
                            sum10 += data2[10, i];
                            sum11 += data2[11, i];
                            sum12 += data2[12, i];
                            sum13 += data2[13, i];
                            sum14 += data2[14, i];
                            sum15 += data2[15, i];
                            sum16 += data2[16, i];
                            sum17 += data2[17, i];
                            #endregion
                        }
                        ADC_Value_0 = sum / 2;
                        ADC_Value_1 = sum1 / 2;
                        #region
                        ADC_Value_2 = sum2 / 2;
                        ADC_Value_3 = sum3 / 2;
                        ADC_Value_4 = sum4 / 2;
                        ADC_Value_5 = sum5 / 2;
                        ADC_Value_6 = sum6 / 2;
                        ADC_Value_7 = sum7 / 2;
                        ADC_Value_8 = sum8 / 2;
                        ADC_Value_9 = sum9 / 2;
                        ADC_Value_10 = sum10 / 2;
                        ADC_Value_11 = sum11 / 2;
                        ADC_Value_12 = sum12 / 2;
                        ADC_Value_13 = sum13 / 2;
                        ADC_Value_14 = sum14 / 2;
                        ADC_Value_15 = sum15 / 2;
                        ADC_Value_16 = sum16 / 2;
                        ADC_Value_17 = sum17 / 2;
                        #endregion

                    }
                    if (n > 12 + mean_count * (l + 1))//根据平均数个数计算，多个平均的值
                    {
                        double sum = 0;
                        double sum1 = 0;
                        #region
                        double sum2 = 0;
                        double sum3 = 0;
                        double sum4 = 0;
                        double sum5 = 0;
                        double sum6 = 0;
                        double sum7 = 0;
                        double sum8 = 0;
                        double sum9 = 0;
                        double sum10 = 0;
                        double sum11 = 0;
                        double sum12 = 0;
                        double sum13 = 0;
                        double sum14 = 0;
                        double sum15 = 0;
                        double sum16 = 0;
                        double sum17 = 0;
                        #endregion
                        for (long j = n - mean_count - 1; j < 12 + mean_count * (l + 1); j++)
                        {   sum += data2[0, j];
                            sum1 += data2[1, j];
                            #region
                            sum2 += data2[2, j];
                            sum3 += data2[3, j];
                            sum4 += data2[4, j];
                            sum5 += data2[5, j];
                            sum6 += data2[6, j];
                            sum7 += data2[7, j];
                            sum8 += data2[8, j];
                            sum9 += data2[9, j];
                            sum10 += data2[10, j];
                            sum11 += data2[11, j];
                            sum12 += data2[12, j];
                            sum13 += data2[13, j];
                            sum14 += data2[14, j];
                            sum15 += data2[15, j];
                            sum16 += data2[16, j];
                            sum17 += data2[17, j];
                            #endregion
                        }
                        data3[l] = sum / mean_count;
                        data4[l] = sum1 / mean_count;
                        #region
                        data5[l] = sum2 / mean_count;
                        data6[l] = sum3 / mean_count;
                        data7[l] = sum4 / mean_count;
                        data8[l] = sum5 / mean_count;
                        data9[l] = sum6 / mean_count;
                        data10[l] = sum7 / mean_count;
                        data11[l] = sum8 / mean_count;
                        data12[l] = sum9 / mean_count;
                        data13[l] = sum10 / mean_count;
                        data14[l] = sum11 / mean_count;
                        data15[l] = sum12 / mean_count;
                        data16[l] = sum13 / mean_count;
                        data17[l] = sum14 / mean_count;
                        data18[l] = sum15 / mean_count;
                        data19[l] = sum16 / mean_count;
                        data20[l] = sum17 / mean_count;
                        #endregion
                        double ls1 = (double)(data3[l] - ADC_Value_0) / ADC_Full_Value;//临时变量
                        double ls2 = (double)(data4[l] - ADC_Value_1) / ADC_Full_Value;//临时变量
                        #region
                        double ls3 = (double)(data5[l] - ADC_Value_2) / ADC_Full_Value;//临时变量
                        double ls4 = (double)(data6[l] - ADC_Value_3) / ADC_Full_Value;//临时变量
                        double ls5 = (double)(data7[l] - ADC_Value_4) / ADC_Full_Value;//临时变量
                        double ls6 = (double)(data8[l] - ADC_Value_5) / ADC_Full_Value;//临时变量
                        double ls7 = (double)(data9[l] - ADC_Value_6) / ADC_Full_Value;//临时变量
                        double ls8 = (double)(data10[l] - ADC_Value_7) / ADC_Full_Value;//临时变量
                        double ls9 = (double)(data11[l] - ADC_Value_8) / ADC_Full_Value;//临时变量
                        double ls10 = (double)(data12[l] - ADC_Value_9) / ADC_Full_Value;//临时变量
                        double ls11 = (double)(data13[l] - ADC_Value_10) / ADC_Full_Value;//临时变量
                        double ls12 = (double)(data14[l] - ADC_Value_11) / ADC_Full_Value;//临时变量
                        double ls13 = (double)(data15[l] - ADC_Value_12) / ADC_Full_Value;//临时变量
                        double ls14 = (double)(data16[l] - ADC_Value_13) / ADC_Full_Value;//临时变量
                        double ls15 = (double)(data17[l] - ADC_Value_14) / ADC_Full_Value;//临时变量
                        double ls16 = (double)(data18[l] - ADC_Value_15) / ADC_Full_Value;//临时变量
                        double ls17 = (double)(data19[l] - ADC_Value_16) / ADC_Full_Value;//临时变量
                        double ls18 = (double)(data20[l] - ADC_Value_17) / ADC_Full_Value;//临时变量
                        #endregion
                        jiaodu3[l] = (jiaodu1[l] - 186) * 0.241611;//x方向角度
                        jiaodu4[l] = (jiaodu2[l] - 186) * 0.241611;//y方向角度
                        string str2 = Convert.ToString(Math.Round(90-jiaodu4[l], 0, MidpointRounding.AwayFromZero)).ToUpper();//将数据显示在窗口控件上
                       
                        data[0, l] =  ls1 * Vcc * 100000 / S1;
                        data[1, l] =  ls2  * Vcc * 100000 / S1;
                        #region
                        data[2, l] = ls3 * Vcc * 100000 / S1;
                        data[3, l] = ls4 * Vcc * 100000 / S1;
                        data[4, l] = ls5 * Vcc * 100000 / S1;
                        data[5, l] = ls6 * Vcc * 100000 / S1;
                        data[6, l] = ls7 * Vcc * 100000 / S1;
                        data[7, l] = ls8 * Vcc * 100000 / S1;
                        data[8, l] = ls9 * Vcc * 100000 / S1;
                        data[9, l] = ls10 * Vcc * 100000 / S1;
                        data[10, l] = ls11 * Vcc * 100000 / S1;
                        data[11, l] = ls12 * Vcc * 100000 / S1;
                        data[12, l] = ls13 * Vcc * 100000 / S1;
                        data[13, l] = ls14 * Vcc * 100000 / S1;
                        data[14, l] = ls15 * Vcc * 100000 / S1;
                        data[15, l] = ls16 * Vcc * 100000 / S1;
                        data[16, l] = ls17 * Vcc * 100000 / S1;
                        data[17, l] = ls18 * Vcc * 100000 / S1;
                        #endregion
                        data[18, l] = jiaodu3[l];
                        data[19, l] = jiaodu4[l];
                        l++;
                        original_num1 = l;
                        ZedGraph1Adddata();
                            
                    }
                    /******************************************************************************************************/

                }
            }
                serialPort1Writeint();                                                                                                     
        }
        #endregion    
        #region//3σ拉依达准则|Xi—X|《=2σ
        private void lvbochuli()
        {
            double[,] p = new double[18, 100000];
            double[,] p1 = new double[18, 100000];
            double E0 = 0;
            #region
            double E1 = 0;
            double E2 = 0;
            double E3 = 0;
            double E4 = 0;
            double E5 = 0;
            double E6 = 0;
            double E7 = 0;
            double E8 = 0;
            double E9 = 0;
            double E10 = 0;
            double E11 = 0;
            double E12 = 0;
            double E13 = 0;
            double E14 = 0;
            double E15 = 0;
            double E16 = 0;
            double E17 = 0;
            #endregion
            double D0 = 0;
            #region
            double D1 = 0;
            double D2 = 0;
            double D3 = 0;
            double D4 = 0;
            double D5 = 0;
            double D6 = 0;
            double D7 = 0;
            double D8 = 0;
            double D9 = 0;
            double D10 = 0;
            double D11 = 0;
            double D12 = 0;
            double D13 = 0;
            double D14 = 0;
            double D15 = 0;
            double D16 = 0;
            double D17 = 0;

            #endregion
            double lz0 = 0;
            #region
            double lz1 = 0;
            double lz2 = 0;
            double lz3 = 0;
            double lz4 = 0;
            double lz5 = 0;
            double lz6 = 0;
            double lz7 = 0;
            double lz8 = 0;
            double lz9 = 0;
            double lz10 = 0;
            double lz11 = 0;
            double lz12 = 0;
            double lz13 = 0;
            double lz14 = 0;
            double lz15 = 0;
            double lz16 = 0;
            double lz17 = 0;
            #endregion
            double hd0 = 0;
            #region
            double hd1 = 0;
            double hd2 = 0;
            double hd3 = 0;
            double hd4 = 0;
            double hd5 = 0;
            double hd6 = 0;
            double hd7 = 0;
            double hd8 = 0;
            double hd9 = 0;
            double hd10 = 0;
            double hd11 = 0;
            double hd12 = 0;
            double hd13 = 0;
            double hd14 = 0;
            double hd15 = 0;
            double hd16 = 0;
            double hd17 = 0;
            #endregion

            double[] d = new double[18];
            double[] t = new double[18];
            double[] lz = new double[18];
            int my = 0;

            lvbocanshu1 = 0.4;//（设置默认结果为1）
            lvbocanshu2 = 1;//（设置默认结果为1）
            /////把原始数据转换
            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < original_num; j++)
                {
                    data_handle[i, j] = data[i, j];
                }
            }
            do
            {
                my += 1;
                E0 = 0;
                #region
                E1 = 0;
                E2 = 0;
                E3 = 0;
                E4 = 0;
                E5 = 0;
                E6 = 0;
                E7 = 0;
                E8 = 0;
                E9 = 0;
                E10 = 0;
                E11 = 0;
                E12 = 0;
                E13 = 0;
                E14 = 0;
                E15 = 0;
                E16 = 0;
                E17 = 0;
                #endregion
                D0 = 0;
                #region
                D1 = 0;
                D2 = 0;
                D3 = 0;
                D4 = 0;
                D5 = 0;
                D6 = 0;
                D7 = 0;
                D8 = 0;
                D9 = 0;
                D10 = 0;
                D11 = 0;
                D12 = 0;
                D13 = 0;
                D14 = 0;
                D15 = 0;
                D16 = 0;
                D17 = 0;
                #endregion
                lz0 = 0;
                #region
                lz1 = 0;
                lz2 = 0;
                lz3 = 0;
                lz4 = 0;
                lz5 = 0;
                lz6 = 0;
                lz7 = 0;
                lz8 = 0;
                lz9 = 0;
                lz10 = 0;
                lz11 = 0;
                lz12 = 0;
                lz13 = 0;
                lz14 = 0;
                lz15 = 0;
                lz16 = 0;
                lz17 = 0;
                #endregion
                hd0 = 0;
                #region
                hd1 = 0;
                hd2 = 0;
                hd3 = 0;
                hd4 = 0;
                hd5 = 0;
                hd6 = 0;
                hd7 = 0;
                hd8 = 0;
                hd9 = 0;
                hd10 = 0;
                hd11 = 0;
                hd12 = 0;
                hd13 = 0;
                hd14 = 0;
                hd15 = 0;
                hd16 = 0;
                hd17 = 0;
                #endregion

                for (int i = 0; i < original_num; i++)
                {
                    for (int j = 0; j < 18; j++)
                    {
                        p[j, i] = data_handle[j, i] / original_num;
                    }
                    E0 += p[0, i];//计算期望
                    #region
                    E1 += p[1, i];
                    E2 += p[2, i];
                    E3 += p[3, i];
                    E4 += p[4, i];
                    E5 += p[5, i];
                    E6 += p[6, i];
                    E7 += p[7, i];
                    E8 += p[8, i];
                    E9 += p[9, i];
                    E10 += p[10, i];
                    E11 += p[11, i];
                    E12 += p[12, i];
                    E13 += p[13, i];
                    E14 += p[14, i];
                    E15 += p[15, i];
                    E16 += p[16, i];
                    E17 += p[17, i];
                    #endregion
                }
                double[] E = new double[18] { E0, E1, E2, E3, E4, E5, E6, E7, E8, E9, E10, E11, E12, E13, E14, E15, E16, E17 };

                for (int i1 = 0; i1 < original_num; i1++)
                {
                    for (int j1 = 0; j1 < 18; j1++)
                    {
                        p1[j1, i1] = Math.Pow(data_handle[j1, i1] - E[j1], 2) / original_num;
                    }
                    D0 += p1[0, i1];//计算方差
                    #region
                    D1 += p1[1, i1];
                    D2 += p1[2, i1];
                    D3 += p1[3, i1];
                    D4 += p1[4, i1];
                    D5 += p1[5, i1];
                    D6 += p1[6, i1];
                    D7 += p1[7, i1];
                    D8 += p1[8, i1];
                    D9 += p1[9, i1];
                    D10 += p1[10, i1];
                    D11 += p1[11, i1];
                    D12 += p1[12, i1];
                    D13 += p1[13, i1];
                    D14 += p1[14, i1];
                    D15 += p1[15, i1];
                    D16 += p1[16, i1];
                    D17 += p1[17, i1];
                    #endregion
                }
                double[] DD = new double[18] { D0, D1, D2, D3, D4, D5, D6, D7, D8, D9, D10, D11, D12, D13, D14, D15, D16, D17 };

                for (int j2 = 0; j2 < 18; j2++)
                {
                    d[j2] = Math.Pow(DD[j2], 0.5);//计算标准差
                }
                for (int s0 = 0; s0 < original_num; s0++)//1、判断d<E 看是否跳出此通道循环 2、判断data【0,0】是否需要代替3、判断换值
                {
                    if (Math.Abs(d[0]) < Math.Abs(lvbocanshu1 * E0))
                    {
                        hd0 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[0, 0] - E0) > lvbocanshu2 * d[0])
                    {
                        data_handle[0, 0] = E0;
                    }
                    if (Math.Abs(data_handle[0, s0] - E0) >= lvbocanshu2 * d[0])
                    {
                        data_handle[0, s0] = data_handle[0, s0 - 1];
                        lz0 = 1;
                    }
                }

                for (int s1 = 0; s1 < original_num; s1++)
                {
                    if (Math.Abs(d[1]) < Math.Abs(lvbocanshu1 * E1))
                    {
                        hd1 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[1, 0] - E1) > lvbocanshu2 * d[1])
                    {
                        data_handle[1, 0] = E1;
                    }
                    if (Math.Abs(data_handle[1, s1] - E1) >= lvbocanshu2 * d[1])
                    {
                        data_handle[1, s1] = data_handle[1, s1 - 1];
                        lz1 = 1;
                    }
                }
                for (int s2 = 0; s2 < original_num; s2++)
                {

                    if (Math.Abs(d[2]) < Math.Abs(lvbocanshu1 * E2))
                    {
                        hd2 = 1;
                        break;
                    }

                    if (Math.Abs(data_handle[2, 0] - E2) > lvbocanshu2 * d[2])
                    {
                        data_handle[2, 0] = E2;
                    }
                    if (Math.Abs(data_handle[2, s2] - E2) >= lvbocanshu2 * d[2])
                    {
                        data_handle[2, s2] = data_handle[2, s2 - 1];
                        lz2 = 1;
                    }

                }

                for (int s3 = 0; s3 < original_num; s3++)
                {

                    if (Math.Abs(d[3]) < Math.Abs(lvbocanshu1 * E3))
                    {
                        hd3 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[3, 0] - E3) > lvbocanshu2 * d[3])
                    {
                        data_handle[3, 0] = E3;
                    }
                    if (Math.Abs(data_handle[3, s3] - E3) >= lvbocanshu2 * d[3])
                    {
                        data_handle[3, s3] = data_handle[3, s3 - 1];
                        lz3 = 1;
                    }
                }

                for (int s4 = 0; s4 < original_num; s4++)
                {
                    if (Math.Abs(d[4]) < Math.Abs(lvbocanshu1 * E4))
                    {
                        hd4 = 1;
                        break;
                    }

                    if (Math.Abs(data_handle[4, 0] - E4) > lvbocanshu2 * d[4])
                    {
                        data_handle[4, 0] = E4;
                    }
                    if (Math.Abs(data_handle[4, s4] - E4) >= lvbocanshu2 * d[4])
                    {
                        data_handle[4, s4] = data_handle[4, s4 - 1];
                        lz4 = 1;
                    }
                }
                for (int s5 = 0; s5 < original_num; s5++)
                {

                    if (Math.Abs(d[5]) < Math.Abs(lvbocanshu1 * E5))
                    {
                        hd5 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[5, 0] - E5) > lvbocanshu2 * d[5])
                    {
                        data_handle[5, 0] = E5;
                    }
                    if (Math.Abs(data_handle[5, s5] - E5) >= lvbocanshu2 * d[5])
                    {
                        data_handle[5, s5] = data_handle[5, s5 - 1];
                        lz5 = 1;
                    }
                }
                for (int s6 = 0; s6 < original_num; s6++)
                {

                    if (Math.Abs(d[6]) < Math.Abs(lvbocanshu1 * E6))
                    {
                        hd6 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[6, 0] - E6) > lvbocanshu2 * d[6])
                    {
                        data_handle[6, 0] = E6;
                    }
                    if (Math.Abs(data_handle[6, s6] - E6) >= lvbocanshu2 * d[6])
                    {
                        data_handle[6, s6] = data_handle[6, s6 - 1];
                        lz6 = 1;
                    }
                }
                for (int s7 = 0; s7 < original_num; s7++)
                {

                    if (Math.Abs(d[7]) < Math.Abs(lvbocanshu1 * E7))
                    {
                        hd7 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[7, 0] - E7) > lvbocanshu2 * d[7])
                    {
                        data_handle[7, 0] = E7;
                    }
                    if (Math.Abs(data_handle[7, s7] - E7) >= lvbocanshu2 * d[7])
                    {
                        data_handle[7, s7] = data_handle[7, s7 - 1];
                        lz7 = 1;
                    }
                }
                for (int s8 = 0; s8 < original_num; s8++)
                {

                    if (Math.Abs(d[8]) < Math.Abs(lvbocanshu1 * E8))
                    {
                        hd8 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[8, 0] - E8) > lvbocanshu2 * d[8])
                    {
                        data_handle[8, 0] = E8;
                    }
                    if (Math.Abs(data_handle[8, s8] - E8) >= lvbocanshu2 * d[8])
                    {
                        data_handle[8, s8] = data_handle[8, s8 - 1];
                        lz8 = 1;
                    }
                }
                for (int s9 = 0; s9 < original_num; s9++)
                {

                    if (Math.Abs(d[9]) < Math.Abs(lvbocanshu1 * E9))
                    {
                        hd9 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[9, 0] - E9) > lvbocanshu2 * d[9])
                    {
                        data_handle[9, 0] = E9;
                    }
                    if (Math.Abs(data_handle[9, s9] - E9) >= lvbocanshu2 * d[9])
                    {
                        data_handle[9, s9] = data_handle[9, s9 - 1];
                        lz9 = 1;
                    }
                }
                for (int s10 = 0; s10 < original_num; s10++)
                {

                    if (Math.Abs(d[10]) < Math.Abs(lvbocanshu1 * E10))
                    {
                        hd10 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[10, 0] - E10) > lvbocanshu2 * d[10])
                    {
                        data_handle[10, 0] = E10;
                    }
                    if (Math.Abs(data_handle[10, s10] - E10) >= lvbocanshu2 * d[10])
                    {
                        data_handle[10, s10] = data_handle[10, s10 - 1];
                        lz10 = 1;
                    }
                }
                for (int s11 = 0; s11 < original_num; s11++)
                {

                    if (Math.Abs(d[11]) < Math.Abs(lvbocanshu1 * E11))
                    {
                        hd11 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[11, 0] - E11) > lvbocanshu2 * d[11])
                    {
                        data_handle[11, 0] = E11;
                    }
                    if (Math.Abs(data_handle[11, s11] - E11) >= lvbocanshu2 * d[11])
                    {
                        data_handle[11, s11] = data_handle[11, s11 - 1];
                        lz11 = 1;
                    }
                }
                for (int s12 = 0; s12 < original_num; s12++)
                {

                    if (Math.Abs(d[12]) < Math.Abs(lvbocanshu1 * E12))
                    {
                        hd12 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[12, 0] - E12) > lvbocanshu2 * d[12])
                    {
                        data_handle[12, 0] = E12;
                    }
                    if (Math.Abs(data_handle[12, s12] - E12) >= lvbocanshu2 * d[12])
                    {
                        data_handle[12, s12] = data_handle[12, s12 - 1];
                        lz12 = 1;
                    }
                }
                for (int s13 = 0; s13 < original_num; s13++)
                {

                    if (Math.Abs(d[13]) < Math.Abs(lvbocanshu1 * E13))
                    {
                        hd13 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[13, 0] - E13) > lvbocanshu2 * d[13])
                    {
                        data_handle[13, 0] = E13;
                    }

                    if (Math.Abs(data_handle[13, s13] - E13) >= lvbocanshu2 * d[13])
                    {
                        data_handle[13, s13] = data_handle[13, s13 - 1];
                        lz13 = 1;
                    }
                }
                for (int s14 = 0; s14 < original_num; s14++)
                {

                    if (Math.Abs(d[14]) < Math.Abs(lvbocanshu1 * E14))
                    {
                        hd14 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[14, 0] - E14) > lvbocanshu2 * d[14])
                    {
                        data_handle[14, 0] = E14;
                    }

                    if (Math.Abs(data_handle[14, s14] - E14) >= lvbocanshu2 * d[14])
                    {
                        data_handle[14, s14] = data_handle[14, s14 - 1];
                        lz14 = 1;
                    }
                }
                for (int s15 = 0; s15 < original_num; s15++)
                {

                    if (Math.Abs(d[15]) < Math.Abs(lvbocanshu1 * E15))
                    {
                        hd15 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[15, 0] - E15) > lvbocanshu2 * d[15])
                    {
                        data_handle[15, 0] = E15;
                    }
                    if (Math.Abs(data_handle[15, s15] - E15) >= lvbocanshu2 * d[15])
                    {
                        data_handle[15, s15] = data_handle[15, s15 - 1];
                        lz15 = 1;
                    }
                }
                for (int s16 = 0; s16 < original_num; s16++)
                {

                    if (Math.Abs(d[16]) < Math.Abs(lvbocanshu1 * E16))
                    {
                        hd16 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[16, 0] - E16) > lvbocanshu2 * d[16])
                    {
                        data_handle[16, 0] = E16;
                    }
                    if (Math.Abs(data_handle[16, s16] - E16) >= lvbocanshu2 * d[16])
                    {
                        data_handle[16, s16] = data_handle[16, s16 - 1];
                        lz16 = 1;
                    }
                }
                for (int s17 = 0; s17 < original_num; s17++)
                {

                    if (Math.Abs(d[17]) < Math.Abs(lvbocanshu1 * E17))
                    {
                        hd17 = 1;
                        break;
                    }
                    if (Math.Abs(data_handle[17, 0] - E17) > lvbocanshu2 * d[17])
                    {
                        data_handle[17, 0] = E17;
                    }
                    if (Math.Abs(data_handle[17, s17] - E17) >= lvbocanshu2 * d[17])
                    {
                        data_handle[17, s17] = data_handle[17, s17 - 1];
                        lz17 = 1;
                    }
                }

                if (hd0 + hd1 + hd2 + hd3 + hd4 + hd5 + hd6 + hd7 + hd8 + hd9 + hd10 + hd11 + hd12 + hd13 + hd14 + hd15 + hd16 + hd17 == 18)
                {
                    break;
                }
            } while (lz0 + lz1 + lz2 + lz3 + lz4 + lz5 + lz6 + lz7 + lz8 + lz9 + lz10 + lz11 + lz12 + lz13 + lz14 + lz15 + lz16 + lz17 != 0);
        }
        #endregion
        #region(数据处理)
        private void runButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            browseButton.Enabled = false;
            SaveButton.Enabled = false;
            if (browseflag == false)
            {
                if (comboBox2.Text == "钝角焊缝" || comboBox2.Text == "锐角焊缝")
                {
                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("请输入焊缝角度，单位：度", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        angle = Convert.ToInt32(textBox1.Text);
                    }
                }

                if (DistanceText.Text == "")
                {
                    MessageBox.Show("请输入扫描长度，单位：mm", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    { StartButton.Enabled = true; }  //开始按钮可用
                    browseButton.Enabled = true;  //预览数据按钮可用
                    if (!browseflag) { SaveButton.Enabled = true; }   //不是浏览得到的数据可以保存
                    exitButton.Enabled = true;
                    return;
                }
                else
                {
                    longth = float.Parse(DistanceText.Text);//扫描长度      
                }
            }
            draw_yuanshishuju();
            lvbochuli();
            dingshen();
            WS_solve();
            draw_nihequxian();
            draw_gedianshendu();
            draw_hankuanhangao();
            draw_hanfengrongshen();

            draw_yuanshiquxian0();
            { StartButton.Enabled = true; }   //开始按钮可用
            browseButton.Enabled = true;  //预览数据按钮可用
            if (!browseflag) { SaveButton.Enabled = true; }   //不是浏览得到的数据可以保存
            exitButton.Enabled = true;

        }
        #endregion
        #region(深度求解)

        double[] hanfengdepth1 = new double[1000000];
        double[] hanfengdepth2 = new double[1000000];
        double[] hanfengdepth3 = new double[1000000];
        double[] hanfengdepth4 = new double[1000000];
        double[] hanfengdepth5 = new double[1000000];
        double[] hanfengdepth6 = new double[1000000];
        double[] hanfengdepth7 = new double[1000000];
        double[] hanfengdepth8 = new double[1000000];
        double[] hanfengdepth9 = new double[1000000];
        double[] hanfengdepth10 = new double[1000000];
        double[] hanfengdepth11 = new double[1000000];
        double[] hanfengdepth12 = new double[1000000];
        double[] hanfengdepth13 = new double[1000000];
        double[] hanfengdepth14 = new double[1000000];
        double[] hanfengdepth15 = new double[1000000];
        double[] hanfengdepth16 = new double[1000000];
        double[] hanfengdepth17 = new double[1000000];
        double[] hanfengdepth18 = new double[1000000];
        double[,] C = new double[10, 1000000];
        double[,] D = new double[10, 1000000];
        double[,] Bxiao = new double[10, 1000];
        //double[] xzhou = new double[7] ;//{ 传感器大小距离值 };
        //double[] y1zhou = new double[9];//{ 测得的深度值 };
        //double[] y2zhou = new double[9];//{ 测得的深度值 };
        double[] nihezhi1 = new double[9];
        double[] x1zhi = new double[10000];
        double[] x2zhi = new double[10000];
        double[,] nihe1 = new double[1000, 9];
        double[] hangao1 = new double[1000000];
        double[] hankuan1 = new double[1000000];
        double[] hankuan2 = new double[1000000];
        double[] rongshenzhi1 = new double[1000000];
        double[] rongshenzhi2 = new double[1000000];
        //double[] rongshenzhi12 = new double[1000000];

        private void dingshen()//（定深，焊宽，焊高，熔深值求解）
        {
            for (int i3 = 0; i3 < original_num; i3++)//(求hanfengdepth的值)
            {
                //Bxiao[1, i] = data[1, i] / data[0, i];
                #region
                Bxiao[2, i3] = Math.Abs(data_handle[3, i3] / data_handle[2, i3]);
                Bxiao[3, i3] = Math.Abs(data_handle[5, i3] / data_handle[4, i3]);
                Bxiao[4, i3] = Math.Abs(data_handle[7, i3] / data_handle[6, i3]);
                Bxiao[5, i3] = Math.Abs(data_handle[9, i3] / data_handle[8, i3]);
                Bxiao[6, i3] = Math.Abs(data_handle[11, i3] / data_handle[10, i3]);
                Bxiao[7, i3] = Math.Abs(data_handle[13, i3] / data_handle[12, i3]);
                Bxiao[8, i3] = Math.Abs(data_handle[15, i3] / data_handle[14, i3]);
                //Bxiao[9, i] = data[17, i] / data[16, i];
                #endregion

                #region(探头间距为5mm时，深度值求解)
                ////当探头间距为5mm时
                //C[1,i] = Math.Pow((2 * h * Bxiao[1, i]), 2) - 4 * (Bxiao[1, i] - 1) * h * h * Bxiao[1, i];
                //D[1,i] = Math.Abs(C[1,i]);
                //hanfengdepth2[i] = (-2 * h * Bxiao[1, i] + Math.Sqrt(D[1,i])) / 2 * (Bxiao[1,i] - 1);
                //hanfengdepth1[i] = (-2 * h * Bxiao[1, i] - Math.Sqrt(D[1,i])) / 2 * (Bxiao[1,i] - 1);
                int h = 5;
                C[2, i3] = Math.Pow((2 * h * Bxiao[2, i3]), 2) - 4 * (Bxiao[2, i3] - 1) * h * h * Bxiao[2, i3];
                D[2, i3] = Math.Abs(C[2, i3]);
                hanfengdepth3[i3] = (-2 * h * Bxiao[2, i3] - Math.Sqrt(D[2, i3])) / (2 * (Bxiao[2, i3] - 1));
                hanfengdepth4[i3] = (-2 * h * Bxiao[2, i3] + Math.Sqrt(D[2, i3])) / (2 * (Bxiao[2, i3] - 1));

                C[3, i3] = Math.Pow((2 * h * Bxiao[3, i3]), 2) - 4 * (Bxiao[3, i3] - 1) * h * h * Bxiao[3, i3];
                D[3, i3] = Math.Abs(C[3, i3]);
                hanfengdepth5[i3] = (-2 * h * Bxiao[3, i3] - Math.Sqrt(D[3, i3])) / (2 * (Bxiao[3, i3] - 1));
                hanfengdepth6[i3] = (-2 * h * Bxiao[3, i3] + Math.Sqrt(D[3, i3])) / (2 * (Bxiao[3, i3] - 1));

                C[4, i3] = Math.Pow((2 * h * Bxiao[4, i3]), 2) - 4 * (Bxiao[4, i3] - 1) * h * h * Bxiao[4, i3];
                D[4, i3] = Math.Abs(C[4, i3]);
                hanfengdepth7[i3] = (-2 * h * Bxiao[4, i3] - Math.Sqrt(D[4, i3])) / (2 * (Bxiao[4, i3] - 1));
                hanfengdepth8[i3] = (-2 * h * Bxiao[4, i3] + Math.Sqrt(D[4, i3])) / (2 * (Bxiao[4, i3] - 1));

                C[5, i3] = Math.Pow((2 * h * Bxiao[5, i3]), 2) - 4 * (Bxiao[5, i3] - 1) * h * h * Bxiao[5, i3];
                D[5, i3] = Math.Abs(C[5, i3]);
                hanfengdepth9[i3] = (-2 * h * Bxiao[5, i3] - Math.Sqrt(D[5, i3])) / (2 * (Bxiao[5, i3] - 1));
                hanfengdepth10[i3] = (-2 * h * Bxiao[5, i3] + Math.Sqrt(D[5, i3])) / (2 * (Bxiao[5, i3] - 1));

                C[6, i3] = Math.Pow((2 * h * Bxiao[6, i3]), 2) - 4 * (Bxiao[6, i3] - 1) * h * h * Bxiao[6, i3];
                D[6, i3] = Math.Abs(C[6, i3]);
                hanfengdepth11[i3] = (-2 * h * Bxiao[6, i3] - Math.Sqrt(D[6, i3])) / (2 * (Bxiao[6, i3] - 1));
                hanfengdepth12[i3] = (-2 * h * Bxiao[6, i3] + Math.Sqrt(D[6, i3])) / (2 * (Bxiao[6, i3] - 1));

                C[7, i3] = Math.Pow((2 * h * Bxiao[7, i3]), 2) - 4 * (Bxiao[7, i3] - 1) * h * h * Bxiao[7, i3];
                D[7, i3] = Math.Abs(C[7, i3]);
                hanfengdepth13[i3] = (-2 * h * Bxiao[7, i3] - Math.Sqrt(D[7, i3])) / (2 * (Bxiao[7, i3] - 1));
                hanfengdepth14[i3] = (-2 * h * Bxiao[7, i3] + Math.Sqrt(D[7, i3])) / (2 * (Bxiao[7, i3] - 1));

                C[8, i3] = Math.Pow((2 * h * Bxiao[8, i3]), 2) - 4 * (Bxiao[8, i3] - 1) * h * h * Bxiao[8, i3];
                D[8, i3] = Math.Abs(C[8, i3]);
                hanfengdepth15[i3] = (-2 * h * Bxiao[8, i3] - Math.Sqrt(D[8, i3])) / (2 * (Bxiao[8, i3] - 1));
                hanfengdepth16[i3] = (-2 * h * Bxiao[8, i3] + Math.Sqrt(D[8, i3])) / (2 * (Bxiao[8, i3] - 1));

                //C[9,i] = Math.Pow((2 * h * Bxiao[9,i]), 2) - 4 * (Bxiao[9,i] - 1) * h * h * Bxiao[9,i];
                //D[9,i] = Math.Abs(C[9,i]);
                //hanfengdepth18[i] = (-2 * h * Bxiao[9,i] + Math.Sqrt(D[9,i])) / 2 * (Bxiao[9,i] - 1);
                //hanfengdepth17[i] = (-2 * h * Bxiao[9,i] - Math.Sqrt(D[9,i])) / 2 * (Bxiao[9,i] - 1);
                #endregion
            }

            ///判断逻辑还是有点问题
            for (int i4 = 1; i4 < original_num; i4++)//此处判断hanfengdepth是否为NaN
            {
                if (double.IsNaN(C[2, i4]))
                {
                    hanfengdepth4[i4] = (hanfengdepth4[i4 - 1] + hanfengdepth4[i4 + 1]) / 2;
                    hanfengdepth3[i4] = (hanfengdepth3[i4 - 1] + hanfengdepth3[i4 + 1]) / 2;//此处判断a为NaN
                }

                if (double.IsNaN(hanfengdepth4[i4]))
                {
                    hanfengdepth4[i4] = hanfengdepth4[i4 - 1]; //此处判断a为NaN
                }
                if (double.IsNaN(hanfengdepth3[i4]))
                {
                    hanfengdepth3[i4] = hanfengdepth3[i4 - 1]; //此处判断a为NaN
                }

                if (double.IsNaN(C[3, i4]))
                {
                    hanfengdepth6[i4] = (hanfengdepth6[i4 - 1] + hanfengdepth6[i4 + 1]) / 2;
                    hanfengdepth5[i4] = (hanfengdepth5[i4 - 1] + hanfengdepth5[i4 + 1]) / 2;//此处判断a为NaN
                }

                if (double.IsNaN(hanfengdepth6[i4]))
                {
                    hanfengdepth6[i4] = hanfengdepth6[i4 - 1]; //此处判断a为NaN
                }
                if (double.IsNaN(hanfengdepth5[i4]))
                {
                    hanfengdepth5[i4] = hanfengdepth5[i4 - 1]; //此处判断a为NaN
                }

                if (double.IsNaN(C[4, i4]))
                {
                    hanfengdepth8[i4] = (hanfengdepth8[i4 - 1] + hanfengdepth8[i4 + 1]) / 2;
                    hanfengdepth7[i4] = (hanfengdepth7[i4 - 1] + hanfengdepth7[i4 + 1]) / 2;//此处判断a为NaN
                }

                if (double.IsNaN(hanfengdepth8[i4]))
                {
                    hanfengdepth8[i4] = hanfengdepth8[i4 - 1]; //此处判断a为NaN
                }
                if (double.IsNaN(hanfengdepth7[i4]))
                {
                    hanfengdepth7[i4] = hanfengdepth7[i4 - 1]; //此处判断a为NaN
                }

                if (double.IsNaN(C[5, i4]))
                {
                    hanfengdepth10[i4] = (hanfengdepth10[i4 - 1] + hanfengdepth10[i4 + 1]) / 2;
                    hanfengdepth9[i4] = (hanfengdepth9[i4 - 1] + hanfengdepth9[i4 + 1]) / 2;//此处判断a为NaN
                }

                if (double.IsNaN(hanfengdepth10[i4]))
                {
                    hanfengdepth10[i4] = hanfengdepth10[i4 - 1]; //此处判断a为NaN
                }
                if (double.IsNaN(hanfengdepth9[i4]))
                {
                    hanfengdepth9[i4] = hanfengdepth9[i4 - 1]; //此处判断a为NaN
                }


                if (double.IsNaN(C[6, i4]))
                {
                    hanfengdepth12[i4] = (hanfengdepth12[i4 - 1] + hanfengdepth12[i4 + 1]) / 2;
                    hanfengdepth11[i4] = (hanfengdepth11[i4 - 1] + hanfengdepth11[i4 + 1]) / 2;//此处判断a为NaN
                }

                if (double.IsNaN(hanfengdepth12[i4]))
                {
                    hanfengdepth12[i4] = hanfengdepth12[i4 - 1]; //此处判断a为NaN
                }
                if (double.IsNaN(hanfengdepth11[i4]))
                {
                    hanfengdepth11[i4] = hanfengdepth11[i4 - 1]; //此处判断a为NaN
                }


                if (double.IsNaN(C[7, i4]))
                {
                    hanfengdepth14[i4] = (hanfengdepth14[i4 - 1] + hanfengdepth14[i4 + 1]) / 2;
                    hanfengdepth13[i4] = (hanfengdepth13[i4 - 1] + hanfengdepth13[i4 + 1]) / 2;//此处判断a为NaN
                }

                if (double.IsNaN(hanfengdepth14[i4]))
                {
                    hanfengdepth14[i4] = hanfengdepth14[i4 - 1]; //此处判断a为NaN
                }
                if (double.IsNaN(hanfengdepth13[i4]))
                {
                    hanfengdepth13[i4] = hanfengdepth13[i4 - 1]; //此处判断a为NaN
                }


                if (double.IsNaN(C[8, i4]))
                {
                    hanfengdepth16[i4] = (hanfengdepth16[i4 - 1] + hanfengdepth16[i4 + 1]) / 2;
                    hanfengdepth15[i4] = (hanfengdepth15[i4 - 1] + hanfengdepth15[i4 + 1]) / 2;//此处判断a为NaN
                }

                if (double.IsNaN(hanfengdepth16[i4]))
                {
                    hanfengdepth16[i4] = hanfengdepth16[i4 - 1]; //此处判断a为NaN
                }
                if (double.IsNaN(hanfengdepth15[i4]))
                {
                    hanfengdepth15[i4] = hanfengdepth15[i4 - 1]; //此处判断a为NaN
                }
            }

            for (int ig = 0; ig < original_num; ig++)
            {
                if (hanfengdepth2[ig] < 0)
                {
                    hanfengdepth2[ig] = -hanfengdepth2[ig];
                }
                if (hanfengdepth3[ig] < 0)
                {
                    hanfengdepth3[ig] = -hanfengdepth3[ig];
                }
                if (hanfengdepth4[ig] < 0)
                {
                    hanfengdepth4[ig] = -hanfengdepth4[ig];
                }
                if (hanfengdepth5[ig] < 0)
                {
                    hanfengdepth5[ig] = -hanfengdepth5[ig];
                }
                if (hanfengdepth6[ig] < 0)
                {
                    hanfengdepth6[ig] = -hanfengdepth6[ig];
                }
                if (hanfengdepth7[ig] < 0)
                {
                    hanfengdepth7[ig] = -hanfengdepth7[ig];
                }
                if (hanfengdepth8[ig] < 0)
                {
                    hanfengdepth8[ig] = -hanfengdepth8[ig];
                }
                if (hanfengdepth9[ig] < 0)
                {
                    hanfengdepth9[ig] = -hanfengdepth9[ig];
                }
                if (hanfengdepth10[ig] < 0)
                {
                    hanfengdepth10[ig] = -hanfengdepth10[ig];
                }
                if (hanfengdepth11[ig] < 0)
                {
                    hanfengdepth11[ig] = -hanfengdepth11[ig];
                }
                if (hanfengdepth12[ig] < 0)
                {
                    hanfengdepth12[ig] = -hanfengdepth12[ig];
                }
                if (hanfengdepth13[ig] < 0)
                {
                    hanfengdepth13[ig] = -hanfengdepth13[ig];
                }
                if (hanfengdepth14[ig] < 0)
                {
                    hanfengdepth14[ig] = -hanfengdepth14[ig];
                }
                if (hanfengdepth15[ig] < 0)
                {
                    hanfengdepth15[ig] = -hanfengdepth15[ig];
                }
                if (hanfengdepth16[ig] < 0)
                {
                    hanfengdepth16[ig] = -hanfengdepth16[ig];
                }
            }
        }

        #endregion
        #region（几种类型焊缝熔深计算）
        private void WS_solve()
        {
            int cishu = 2;
            double wp_ang_rad = 0;
            double[] xishu1 = new double[3];
            double[] xzhou ;
            double[] y1zhou;

            #region////直角焊缝求解
            if (comboBox2.SelectedIndex == 0)
            {
               

                for (int i = 0; i < original_num; i++)//(拟合，熔深，焊宽，焊高求解)
                {

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///拟合参数确定
                    if (comboBox3.SelectedIndex == 0)
                    {
                        edge_longth = 7;
                        xzhou = new double[7] { -3, -2, -1, 0, 1, 2, 3 };
                        y1zhou = new double[7] { hanfengdepth4[i], hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 1.9, hanfengdepth10[i] + 3.2, hanfengdepth12[i] + 2.2, hanfengdepth14[i] + 1.3, hanfengdepth16[i] };
                        /////y1zhou里面的参数需要改，根据不同传感器更改一下hanfengdepth的传递数组格式
                    }
                    else if (comboBox3.SelectedIndex == 1)
                    {
                        edge_longth = 5;
                        xzhou = new double[5] { -2, -1, 0, 1, 2 };
                        y1zhou = new double[5] { hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3 };
                    }
                    else if (comboBox3.SelectedIndex == 2)
                    {
                        edge_longth = 3;
                        xzhou = new double[3] { -1, 0, 1 };
                        y1zhou = new double[3] { hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1 };
                    }
                    else
                    {
                        edge_longth = 9;
                        xzhou = new double[9] { -4, -3, -2, -1, 0, 1, 2, 3, 4 };
                        y1zhou = new double[9] { hanfengdepth2[i] + 0.2, hanfengdepth4[i] + 0.9, hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3, hanfengdepth16[i] + 0.9, hanfengdepth18[i] + 0.2 };
                        /////y1zhou里面的参数需要改，根据不同传感器更改一下hanfengdepth的传递数组格式
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////
                    
                    jiaodu3[i] = data[18, i];
                    jiaodu4[i] = data[19, i];

                    xishu1 = zuixiaoerchengfa.MultiLine(xzhou, y1zhou, edge_longth , cishu);

                    ///假如一元二次方程的开口不是向下，更改方向
                    if (xishu1[2] > 0)
                    {
                        xishu1[2] = -xishu1[2];
                        xishu1[1] = -xishu1[1];
                        xishu1[0] = xishu1[0];
                    }
                    /////

                    for (int j = (edge_longth - 1) / 2, r = 0; j > -(edge_longth + 1) / 2; j--, r++)
                    {
                        nihezhi1[r] = xishu1[0] + xishu1[1] * j + xishu1[2] * Math.Pow(j, 2);
                    }
                    for (int h1 = 0; h1 < edge_longth; h1++)
                    {
                        nihe1[i, h1] = nihezhi1[h1];
                    }

                    ////////////////////////////////////////////////////////////////实际计算用不到这段，画图好像能用到
                    double t1 = Math.Pow(xishu1[1], 2) - 4 * xishu1[2] * xishu1[0];
                    double t = Math.Abs(t1);

                    x1zhi[i] = (-xishu1[1] + Math.Sqrt(t)) / (2 * xishu1[2]);
                    x2zhi[i] = (-xishu1[1] - Math.Sqrt(t)) / (2 * xishu1[2]);
                    //使x1zhi为较小值，即x1zhi为负，在y轴左侧
                    if (x1zhi[i] > x2zhi[i])
                    {
                        double temp;
                        temp = x1zhi[i];
                        x1zhi[i] = x2zhi[i];
                        x2zhi[i] = temp;
                    }
                    //////////////////////////////////////////////////////////////

                    double n = Math.PI * jiaodu4[i] / 180;//角度转变成弧度
                    double n1 = Math.PI * (90 - jiaodu4[i]) / 180;

                    hankuan1[i] = Math.Sin(n) * edge_longth ;
                    hankuan2[i] = Math.Cos(n) * edge_longth ;
                    hangao1[i] = Math.Sin(n1) * hankuan1[i];
                    //double xl1 = Math.Tan(n);///线的斜率
                    //double xl2 = Math.Tan(n1);

                    double c1 = xishu1[0] - Math.Pow(xishu1[1] - Math.Tan(n), 2) / 4 / xishu1[2];
                    double c2 = xishu1[0] - Math.Pow(xishu1[1] + Math.Tan(n1), 2) / 4 / xishu1[2];

                    double l1 = Math.Pow(Math.Tan(n), 2) + 1;
                    double ll1 = Math.Sqrt(l1);//求根号下A^2+B^2的值
                    //double lll1 = Math.Tan(n) * x1zhi[i] + c1;
                    double lll1 = Math.Tan(n) * -edge_longth /2 + c1;
                    rongshenzhi1[i] = Math.Abs(lll1 / ll1);

                    double l2 = Math.Pow(Math.Tan(n1), 2) + 1;
                    double ll2 = Math.Sqrt(l2);
                    //double lll2 = -Math.Tan(n1) * x2zhi[i] + c2;
                    double lll2 = -Math.Tan(n1) * edge_longth /2 + c2;
                    rongshenzhi2[i] = Math.Abs(lll2 / ll2);
                    if (rongshenzhi1[i] < 0)
                    {
                        rongshenzhi1[i] = -rongshenzhi1[i];
                    }
                    if (rongshenzhi2[i] < 0)
                    {
                        rongshenzhi2[i] = -rongshenzhi2[i];
                    }
                }
            }
            #endregion


            #region////钝角焊缝求解
            if (comboBox2.SelectedIndex == 1)
            {
                for (int i = 0; i < original_num; i++)//(拟合，熔深，焊宽，焊高求解)
                {

                    if (comboBox3.SelectedIndex == 0)
                    {
                        edge_longth = 7;
                        xzhou = new double[7] { -3, -2, -1, 0, 1, 2, 3 };
                        y1zhou = new double[7] { hanfengdepth4[i], hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3, hanfengdepth16[i] };
                        /////y1zhou里面的参数需要改，根据不同传感器更改一下hanfengdepth的传递数组格式
                    }
                    else if (comboBox3.SelectedIndex == 1)
                    {
                        edge_longth = 5;
                        xzhou = new double[5] { -2, -1, 0, 1, 2 };
                        y1zhou = new double[5] { hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3 };
                    }
                    else if (comboBox3.SelectedIndex == 2)
                    {
                        edge_longth = 3;
                        xzhou = new double[3] { -1, 0, 1 };
                        y1zhou = new double[3] { hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1 };
                    }
                    else
                    {
                        edge_longth = 9;
                        xzhou = new double[9] { -4, -3, -2, -1, 0, 1, 2, 3, 4 };
                        y1zhou = new double[9] { hanfengdepth2[i] + 0.2, hanfengdepth4[i] + 0.9, hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3, hanfengdepth16[i] + 0.9, hanfengdepth18[i] + 0.2 };
                        /////y1zhou里面的参数需要改，根据不同传感器更改一下hanfengdepth的传递数组格式
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////

                    jiaodu3[i] = data[18, i];
                    jiaodu4[i] = data[19, i];

                    xishu1 = zuixiaoerchengfa.MultiLine(xzhou, y1zhou, edge_longth, cishu);

                    ///假如一元二次方程的开口不是向下，更改方向
                    if (xishu1[2] > 0)
                    {
                        xishu1[2] = -xishu1[2];
                        xishu1[1] = -xishu1[1];
                        xishu1[0] = xishu1[0];
                    }
                    /////

                    for (int j = (edge_longth - 1) / 2, r = 0; j > -(edge_longth + 1) / 2; j--, r++)
                    {
                        nihezhi1[r] = xishu1[0] + xishu1[1] * j + xishu1[2] * Math.Pow(j, 2);
                    }
                    for (int h1 = 0; h1 < edge_longth; h1++)
                    {
                        nihe1[i, h1] = nihezhi1[h1];
                    }


                    ////////////////////////////////////////////////////////////////实际计算用不到这段，画图好像能用到
                    double t1 = Math.Pow(xishu1[1], 2) - 4 * xishu1[2] * xishu1[0];
                    double t = Math.Abs(t1);

                    x1zhi[i] = (-xishu1[1] + Math.Sqrt(t)) / (2 * xishu1[2]);
                    x2zhi[i] = (-xishu1[1] - Math.Sqrt(t)) / (2 * xishu1[2]);
                    //使x1zhi为较小值，即x1zhi为负，在y轴左侧
                    if (x1zhi[i] > x2zhi[i])
                    {
                        double temp;
                        temp = x1zhi[i];
                        x1zhi[i] = x2zhi[i];
                        x2zhi[i] = temp;
                    }
                    //////////////////////////////////////////////////////////////

                    double n = Math.PI * jiaodu4[i] / 180;//检测到的角度转变成弧度
                    wp_ang_rad = Math.PI * float.Parse(textBox1.Text) / 180;  ///钝角焊缝的角度转化成弧度

                    hankuan1[i] = edge_longth * Math.Sin(n) / Math.Sin(wp_ang_rad); ////根据正弦定理求焊宽
                    hankuan2[i] = edge_longth * Math.Sin(Math.PI - n - wp_ang_rad) / Math.Sin(wp_ang_rad); ////根据正弦定理求另一个焊宽
                    hangao1[i] = hankuan2[i] * Math.Sin(n);

                    double xl1 = Math.Tan(n);
                    double xl2 = Math.Tan(Math.PI - n - wp_ang_rad);

                    double c1 = xishu1[0] - Math.Pow(xishu1[1] - xl1, 2) / 4 / xishu1[2];
                    double c2 = xishu1[0] - Math.Pow(xishu1[1] + xl2, 2) / 4 / xishu1[2];

                    double l1 = Math.Pow(xl1, 2) + 1;
                    double ll1 = Math.Sqrt(l1);//求根号下A^2+B^2的值
                    //double lll1 = Math.Tan(n) * x1zhi[i] + c1;
                    double lll1 = xl1 * -edge_longth / 2 + c1;
                    rongshenzhi1[i] = Math.Abs(lll1 / ll1);

                    double l2 = Math.Pow(xl2, 2) + 1;
                    double ll2 = Math.Sqrt(l2);
                    //double lll2 = -Math.Tan(n1) * x2zhi[i] + c2;
                    double lll2 = -xl2 * edge_longth / 2 + c2;
                    rongshenzhi2[i] = Math.Abs(lll2 / ll2);
                    if (rongshenzhi1[i] < 0)
                    {
                        rongshenzhi1[i] = -rongshenzhi1[i];
                    }
                    if (rongshenzhi2[i] < 0)
                    {
                        rongshenzhi2[i] = -rongshenzhi2[i];
                    }
                }
            }
            #endregion

            #region////锐角焊缝求解
            if (comboBox2.SelectedIndex == 2)
            {
                for (int i = 0; i < original_num; i++)//(拟合，熔深，焊宽，焊高求解)
                {

                    if (comboBox3.SelectedIndex == 0)
                    {
                        edge_longth = 7;
                        xzhou = new double[7] { -3, -2, -1, 0, 1, 2, 3 };
                        y1zhou = new double[7] { hanfengdepth4[i], hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3, hanfengdepth16[i] };
                        /////y1zhou里面的参数需要改，根据不同传感器更改一下hanfengdepth的传递数组格式
                    }
                    else if (comboBox3.SelectedIndex == 1)
                    {
                        edge_longth = 5;
                        xzhou = new double[5] { -2, -1, 0, 1, 2 };
                        y1zhou = new double[5] { hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3 };
                    }
                    else
                    {
                        edge_longth = 3;
                        xzhou = new double[3] { -1, 0, 1 };
                        y1zhou = new double[3] { hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1 };
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////

                    jiaodu3[i] = data[18, i];
                    jiaodu4[i] = data[19, i];

                    xishu1 = zuixiaoerchengfa.MultiLine(xzhou, y1zhou, edge_longth, cishu);

                    ///假如一元二次方程的开口不是向下，更改方向
                    if (xishu1[2] > 0)
                    {
                        xishu1[2] = -xishu1[2];
                        xishu1[1] = -xishu1[1];
                        xishu1[0] = xishu1[0];
                    }
                    /////

                    for (int j = (edge_longth - 1) / 2, r = 0; j > -(edge_longth + 1) / 2; j--, r++)
                    {
                        nihezhi1[r] = xishu1[0] + xishu1[1] * j + xishu1[2] * Math.Pow(j, 2);
                    }
                    for (int h1 = 0; h1 < edge_longth; h1++)
                    {
                        nihe1[i, h1] = nihezhi1[h1];
                    }


                    ////////////////////////////////////////////////////////////////实际计算用不到这段，画图好像能用到
                    double t1 = Math.Pow(xishu1[1], 2) - 4 * xishu1[2] * xishu1[0];
                    double t = Math.Abs(t1);

                    x1zhi[i] = (-xishu1[1] + Math.Sqrt(t)) / (2 * xishu1[2]);
                    x2zhi[i] = (-xishu1[1] - Math.Sqrt(t)) / (2 * xishu1[2]);
                    //使x1zhi为较小值，即x1zhi为负，在y轴左侧
                    if (x1zhi[i] > x2zhi[i])
                    {
                        double temp;
                        temp = x1zhi[i];
                        x1zhi[i] = x2zhi[i];
                        x2zhi[i] = temp;
                    }
                    //////////////////////////////////////////////////////////////

                    double n = Math.PI * jiaodu4[i] / 180;//检测到的角度转变成弧度
                    wp_ang_rad = Math.PI * float.Parse(textBox1.Text) / 180;  ///锐角焊缝的角度转化成弧度

                    hankuan1[i] = edge_longth * Math.Sin(n) / Math.Sin(wp_ang_rad); ////根据正弦定理求焊宽
                    hankuan2[i] = edge_longth * Math.Sin(Math.PI - n - wp_ang_rad) / Math.Sin(wp_ang_rad); ////根据正弦定理求另一个焊宽
                    hangao1[i] = hankuan2[i] * Math.Sin(n);

                    double xl1 = Math.Tan(n);
                    double xl2 = Math.Tan(Math.PI - n - wp_ang_rad);

                    double c1 = xishu1[0] - Math.Pow(xishu1[1] - xl1, 2) / 4 / xishu1[2];
                    double c2 = xishu1[0] - Math.Pow(xishu1[1] + xl2, 2) / 4 / xishu1[2];

                    double l1 = Math.Pow(xl1, 2) + 1;
                    double ll1 = Math.Sqrt(l1);//求根号下A^2+B^2的值
                    //double lll1 = Math.Tan(n) * x1zhi[i] + c1;
                    double lll1 = xl1 * -edge_longth / 2 + c1;
                    rongshenzhi1[i] = Math.Abs(lll1 / ll1);

                    double l2 = Math.Pow(xl2, 2) + 1;
                    double ll2 = Math.Sqrt(l2);
                    //double lll2 = -Math.Tan(n1) * x2zhi[i] + c2;
                    double lll2 = -xl2 * edge_longth / 2 + c2;
                    rongshenzhi2[i] = Math.Abs(lll2 / ll2);
                    if (rongshenzhi1[i] < 0)
                    {
                        rongshenzhi1[i] = -rongshenzhi1[i];
                    }
                    if (rongshenzhi2[i] < 0)
                    {
                        rongshenzhi2[i] = -rongshenzhi2[i];
                    }
                }
            }
            #endregion
            #region////搭接焊缝求解
            if (comboBox2.SelectedIndex == 3)
            {                
                for (int i = 0; i < original_num; i++)//(拟合，熔深，焊宽，焊高求解)
                {
                    jiaodu4[i] = data[19, i];

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///拟合参数确定
                    if (comboBox3.SelectedIndex == 0)
                    {
                        edge_longth = 7;
                        xzhou = new double[7] { -3, -2, -1, 0, 1, 2, 3 };
                        y1zhou = new double[7] { hanfengdepth4[i], hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3, hanfengdepth16[i] };
                        /////y1zhou里面的参数需要改，根据不同传感器更改一下hanfengdepth的传递数组格式
                    }
                    else if (comboBox3.SelectedIndex == 1)
                    {
                        edge_longth = 5;
                        xzhou = new double[5] { -2, -1, 0, 1, 2 };
                        y1zhou = new double[5] { hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3 };
                    }
                    else
                    {
                        edge_longth = 3;
                        xzhou = new double[3] { -1, 0, 1 };
                        y1zhou = new double[3] { hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1 };
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////

                    xishu1 = zuixiaoerchengfa.MultiLine(xzhou, y1zhou, 7, cishu);

                    ///假如一元二次方程的开口不是向下，更改方向
                    if (xishu1[2] > 0)
                    {
                        xishu1[2] = -xishu1[2];
                        xishu1[1] = -xishu1[1];
                        xishu1[0] = xishu1[0];
                    }

                    for (int j = (edge_longth - 1) / 2, r = 0; j > -(edge_longth + 1) / 2; j--, r++)
                    {
                        nihezhi1[r] = xishu1[0] + xishu1[1] * j + xishu1[2] * Math.Pow(j, 2);
                    }
                    for (int h1 = 0; h1 < edge_longth; h1++)
                    {
                        nihe1[i, h1] = nihezhi1[h1];
                    }

                    double t1 = Math.Pow(xishu1[1], 2) - 4 * xishu1[2] * xishu1[0];
                    double t = Math.Abs(t1);
                    double x1zhi = (-xishu1[1] + Math.Sqrt(t)) / (2 * xishu1[2]);
                    double x2zhi = (-xishu1[1] - Math.Sqrt(t)) / (2 * xishu1[2]);
                    //使x1zhi为较小值，即x1zhi为负，在y轴左侧
                    if (x1zhi > x2zhi)
                    {
                        double temp;
                        temp = x1zhi;
                        x1zhi = x2zhi;
                        x2zhi = temp;
                    }

                    double n = Math.PI * jiaodu4[i] / 180;//角度转变成弧度
                    double n1 = Math.PI * (90 - jiaodu4[i]) / 180;
                    hankuan1[i] = Math.Sin(n) * (x2zhi - x1zhi);
                    rongshenzhi1[i] = hankuan1[i] + 0.02;

                }
            }
            #endregion
            #region////对接焊缝求解
            if (comboBox2.SelectedIndex == 4)
            {
                
                for (int i = 0; i < original_num; i++)//(拟合，熔深，焊宽，焊高求解)
                {
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///拟合参数确定
                    if (comboBox3.SelectedIndex == 0)
                    {
                        edge_longth = 7;
                        xzhou = new double[7] { -3, -2, -1, 0, 1, 2, 3 };
                        y1zhou = new double[7] { hanfengdepth4[i], hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3, hanfengdepth16[i] };
                        /////y1zhou里面的参数需要改，根据不同传感器更改一下hanfengdepth的传递数组格式
                    }
                    else if (comboBox3.SelectedIndex == 1)
                    {
                        edge_longth = 5;
                        xzhou = new double[5] { -2, -1, 0, 1, 2 };
                        y1zhou = new double[5] { hanfengdepth6[i] + 1.3, hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1, hanfengdepth14[i] + 1.3 };
                    }
                    else
                    {
                        edge_longth = 3;
                        xzhou = new double[3] { -1, 0, 1 };
                        y1zhou = new double[3] { hanfengdepth8[i] + 2.1, hanfengdepth10[i] + 3, hanfengdepth12[i] + 2.1 };
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////

                    xishu1 = zuixiaoerchengfa.MultiLine(xzhou, y1zhou, edge_longth , cishu);
                    for (int j = (edge_longth - 1) / 2, r = 0; j > -(edge_longth + 1) / 2; j--, r++)
                    {
                        nihezhi1[r] = xishu1[0] + xishu1[1] * j + xishu1[2] * Math.Pow(j, 2);
                    }
                    for (int h1 = 0; h1 < edge_longth; h1++)
                    {
                        nihe1[i, h1] = nihezhi1[h1];
                    }
                    /////此变量就是抛物线顶点的横坐标
                    double qiedianzhi = -xishu1[1] / 2 / xishu1[2];
                    /////熔深计算。对接焊缝熔深就是指拟合曲线最低点的深度
                    rongshenzhi1[i] = xishu1[2] * Math.Pow(qiedianzhi, 2) + xishu1[1] * qiedianzhi + xishu1[0];
                    ////焊宽计算。曲线与坐标有两个交点，交点到最深处的距离（交点距离的一半）
                    double t1 = Math.Pow(xishu1[1], 2) - 4 * xishu1[2] * xishu1[0];
                    double t = Math.Abs(t1);

                    x1zhi[i] = (-xishu1[1] + Math.Sqrt(t)) / (2 * xishu1[2]);
                    x2zhi[i] = (-xishu1[1] - Math.Sqrt(t)) / (2 * xishu1[2]);
                    //使x1zhi为较小值，即x1zhi为负，在y轴左侧
                    if (x1zhi[i] > x2zhi[i])
                    {
                        double temp;
                        temp = x1zhi[i];
                        x1zhi[i] = x2zhi[i];
                        x2zhi[i] = temp;
                    }
                    //////////
                    hankuan1[i] = Math.Abs(x1zhi[i] - qiedianzhi);
                    hankuan2[i] = Math.Abs(x2zhi[i] - qiedianzhi);
                }
            }
            #endregion
        }
        #endregion
        #region(曲线取点，显示坐标)
        //曲线取点，显示坐标
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            return "位置:" + string.Format("{0:0.00}", pt.X) + " 熔深:" + string.Format("{0:0.00}", pt.Y);
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl2.IsShowPointValues = true;
            zedGraphControl3.IsShowPointValues = true;
            zedGraphControl4.IsShowPointValues = true;
            zedGraphControl5.IsShowPointValues = true;
            zedGraphControl6.IsShowPointValues = true;

            zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl2.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl3.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl4.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl5.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            zedGraphControl6.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
        }
        /////在图上面标数字，显示尺寸
        private void zedGraphControl5_MouseClick(object sender, MouseEventArgs e)//
        {
            PointF mousePt = new PointF(e.X, e.Y);
            string tooltip = string.Empty;
            ////Find the Chart rect that contains the current mouse location
            GraphPane pane4 = ((ZedGraphControl)sender).MasterPane.FindChartRect(mousePt);
            ////If pane is non-null, we have a valid location.  Otherwise, the mouse is not
            ////within any chart rect.
            double x11 = 0;

            double wp_depth1 = 0;
            double wp_depth2 = 0;//熔深

            double wp_width1 = 0;
            double wp_width2 = 0;//焊宽

            double wp_high = 0;//焊高
            double wp_thick = 0;//最大焊缝厚度

            double fit_x1 = 0;
            double fit_x2 = 0;

            //double x1_revise_x = 0;
            //double x1_revise_y = 0;
            //double x2_revise_x = 0;
            //double x2_revise_y = 0;

            label12.BringToFront();
            label16.BringToFront();
            label13.BringToFront();
            label15.BringToFront();
            label14.BringToFront();

            int r = 0;
            if (pane4 != null)
            {
                #region//////直角焊缝图像的绘图
                if (comboBox2.SelectedIndex == 0)
                {
                    double data_x1, data_y1;
                    pane4.ReverseTransform(mousePt, out data_x1, out data_y1);
                    for (int i = 0; i < original_num; i++)
                    {

                        x11 = i * longth / (original_num - 1);
                        if (data_x1 - x11 < longth / (original_num - 1))
                        {
                            if (data_x1 - x11 < (i + 1) * longth / (original_num - 1) - data_x1)
                            {
                                x11 = i * longth / (original_num - 1);
                                wp_depth1 = rongshenzhi1[i];
                                wp_depth2 = rongshenzhi2[i];
                                wp_width1 = hankuan1[i];
                                wp_width2 = hankuan2[i];
                                wp_high = hangao1[i];
                                wp_thick = nihe1[i, 3];
                                r = i;

                                fit_x1 = x1zhi[i];
                                fit_x2 = x2zhi[i];
                            }
                            else
                            {
                                x11 = (i + 1) * longth / (original_num - 1);
                                wp_depth1 = rongshenzhi1[i + 1];
                                wp_depth2 = rongshenzhi2[i + 1];
                                wp_width1 = hankuan1[i + 1];
                                wp_width2 = hankuan2[i + 1];
                                wp_high = hangao1[i + 1];
                                wp_thick = nihe1[i + 1, 3];
                                r = i + 1;

                                fit_x1 = x1zhi[i + 1];
                                fit_x2 = x2zhi[i + 1];
                            }
                            break;
                        }
                    }

                    float con_ratio = 0;
                    con_ratio = 220 / 50 * 2;
                    this.pictureBox1.Refresh();
                    Graphics g = this.pictureBox1.CreateGraphics();

                    Pen pen1 = new Pen(Color.Blue, 2);
                    Pen pen2 = new Pen(Color.Red, 3);
                    Pen pen3 = new Pen(Color.Green, 2);
                    Pen pen4 = new Pen(Color.LimeGreen, 1);
                    ////////////////////////////////////////
                    g.DrawLine(pen1, 30, 150, 250, 150);///最下面线，量得其尺寸为50mm，坐标长220。

                    g.DrawLine(pen1, 30, 90, 30, 150);
                    g.DrawLine(pen1, 250, 90, 250, 150);

                    g.DrawLine(pen1, 30, 90, 115, 90);
                    g.DrawLine(pen1, 165, 90, 250, 90);

                    g.DrawLine(pen1, 115, 30, 115, 90);
                    g.DrawLine(pen1, 165, 30, 165, 90);

                    g.DrawLine(pen1, 115, 30, 165, 30);
                    ///////////////////////////////////////画的是轮廓
                    float coe1 = 0;
                    coe1 = (float)(wp_width1 * con_ratio);
                    g.DrawLine(pen2, 165, 90 - coe1, 165, 90);
                    float coe2 = 0;
                    coe2 = (float)(wp_width2 * con_ratio);
                    g.DrawLine(pen2, 165, 90, 165 + coe2, 90);
                    g.DrawLine(pen2, 165, 90 - coe1, 165 + coe2, 90);

                    double b_xq = 0;
                    double b_yq = 0;   ///拟合出的第一个点在焊缝斜边上的投影的坐标
                    double[] b_x = new double[edge_longth ];
                    double[] b_y = new double[edge_longth ];////需要求的点在焊缝斜边上的投影的坐标
                    double ang = 0;
                    ang = data[19, r] * Math.PI / 180;/////此处的角度
                    b_xq = 165 + 0.5 * Math.Cos(ang) * con_ratio;
                    b_yq = 90 - coe1 + 0.5 * Math.Sin(ang) * con_ratio;

                    for (int j = 0; j < edge_longth ; j++)    ///拟合点在焊缝斜边上投影对应的坐标值
                    {
                        b_x[j] = b_xq + j * 1 * Math.Cos(ang) * con_ratio;
                        b_y[j] = b_yq + j * 1 * Math.Sin(ang) * con_ratio;
                    }

                    double[] fit_depth = new double[edge_longth ];
                    for (int j = 0; j < edge_longth ; j++)
                    {
                        fit_depth[j] = nihe1[r, j];
                    }

                    double[] nh_x = new double[edge_longth ];
                    double[] nh_y = new double[edge_longth ];///7个拟合点的坐标值
                    for (int j = 0; j < edge_longth ; j++)
                    {
                        nh_x[j] = b_x[j] - fit_depth[j] * Math.Sin(ang) * con_ratio;
                        nh_y[j] = b_y[j] + fit_depth[j] * Math.Cos(ang) * con_ratio;
                    }

                    double nh_x1_x = 0;
                    double nh_x1_y = 0;
                    double nh_x2_x = 0;
                    double nh_x2_y = 0;////一元二次方程与坐标轴的交点

                    //nh_x1_x = b_x[3] + Math.Abs(fit_x1) * Math.Cos(ang) * con_ratio;
                    //nh_x1_y = b_y[3] + Math.Abs(fit_x1) * Math.Sin(ang) * con_ratio;
                    //nh_x2_x = b_x[3] - Math.Abs(fit_x2) * Math.Cos(ang) * con_ratio;
                    //nh_x2_y = b_y[3] - Math.Abs(fit_x2) * Math.Sin(ang) * con_ratio;
                    ///////前面计算焊宽时，使焊缝斜边默认长度为7，拟合计算出来的曲线与x轴交点长度不一定为7，那么要对其压缩，使其变为7
                    //if (fit_x2 - fit_x1 != edge_longth )
                    //{
                    //    x2_revise_x = (fit_x2 - edge_longth / 2 ) * Math.Cos(ang) * con_ratio;
                    //    x2_revise_y = (fit_x2 - edge_longth / 2) * Math.Sin(ang) * con_ratio;
                    //    x1_revise_x = (fit_x1 + edge_longth / 2) * Math.Cos(ang) * con_ratio;
                    //    x1_revise_y = (fit_x1 + edge_longth / 2) * Math.Sin(ang) * con_ratio;
                    //    nh_x2_x = nh_x2_x + x2_revise_x;
                    //    nh_x2_y = nh_x2_y + x2_revise_y;
                    //    nh_x1_x = nh_x1_x + x1_revise_x;
                    //    nh_x1_y = nh_x1_y + x1_revise_y;
                    //}

                    
                    ////////////////////////画抛物线
                    if (edge_longth == 7)
                    {
                        nh_x1_x = b_x[3] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[3] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[3] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[3] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x[3]),(int)(nh_y [3])),
                             new Point((int)(nh_x[4]),(int)(nh_y [4])),
                             new Point((int)(nh_x[5]),(int)(nh_y [5])),
                             new Point((int)(nh_x[6]),(int)(nh_y [6])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    else if (edge_longth == 5)
                    {
                        nh_x1_x = b_x[2] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[2] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[2] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[2] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x[3]),(int)(nh_y [3])),
                             new Point((int)(nh_x[4]),(int)(nh_y [4])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    else if (edge_longth == 3)
                    {
                        nh_x1_x = b_x[1] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[1] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[1] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[1] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    else
                    {
                        nh_x1_x = b_x[4] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[4] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[4] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[4] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x[3]),(int)(nh_y [3])),
                             new Point((int)(nh_x[4]),(int)(nh_y [4])),
                             new Point((int)(nh_x[5]),(int)(nh_y [5])),
                             new Point((int)(nh_x[6]),(int)(nh_y [6])),
                             new Point((int)(nh_x[7]),(int)(nh_y [7])),
                             new Point((int)(nh_x[8]),(int)(nh_y [8])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    /////////////////////

                    g.DrawLine(pen4, 165, 90 - coe1, 180, 90 - coe1);
                    g.DrawLine(pen4, 165 + coe2, 90, 165 + coe2, 75);

                    g.DrawLine(pen4, 175, 90 - coe1, 175, 30);//////焊宽1的标注线
                    g.DrawLine(pen4, 175, 30, 220, 30);//////焊宽1的标注线
                    g.DrawLine(pen4, 165 + coe2, 80, 250, 80);////焊宽2的标注线
                    g.DrawLine(pen4, 165 - (float)(wp_depth2) * con_ratio, 45, 110, 45);////熔深2的标注线
                    g.DrawLine(pen4, 220, 90 + (float)(wp_depth1) * con_ratio, 220, 140);////熔深1的标注线
                    g.DrawLine(pen4, 220, 140, 260, 140);////熔深1的标注线
                    g.DrawLine(pen4, (float)(165 + wp_high * Math.Sin(ang) * con_ratio), (float)(90 - wp_high * Math.Cos(ang) * con_ratio), (float)(165 + (wp_high + 2) * Math.Sin(ang) * con_ratio), (float)(90 - (wp_high + 2) * Math.Cos(ang) * con_ratio));
                    g.DrawLine(pen4, (float)(165 + (wp_high + 2) * Math.Sin(ang) * con_ratio), (float)(90 - (wp_high + 2) * Math.Cos(ang) * con_ratio), (float)(165 + (wp_high + 2) * Math.Sin(ang) * con_ratio) + 35, (float)(90 - (wp_high + 2) * Math.Cos(ang) * con_ratio));
                    ////////抛物线的切线
                    g.DrawLine(pen4, 165 - (float)(wp_depth2) * con_ratio, 35, 165 - (float)(wp_depth2) * con_ratio, 100);
                    g.DrawLine(pen4, 150, 90 + (float)(wp_depth1) * con_ratio, 240, 90 + (float)(wp_depth1) * con_ratio);
                    ////////
                    pen4.StartCap = LineCap.ArrowAnchor;//定义线头的样式为箭头
                    pen4.EndCap = LineCap.ArrowAnchor;//定义线尾的样式为箭头

                    g.DrawLine(pen4, 175, 90, 175, 90 - coe1);////焊宽1的箭头线
                    g.DrawLine(pen4, 165, 80, 165 + coe2, 80);////焊宽2的箭头线
                    g.DrawLine(pen4, 165 - (float)(wp_depth2) * con_ratio, 45, 165, 45);//////熔深2的箭头线
                    g.DrawLine(pen4, 220, 90, 220, 90 + (float)(wp_depth1) * con_ratio);/////熔深1的箭头线
                    g.DrawLine(pen4, 165, 90, (float)(165 + wp_high * Math.Sin(ang) * con_ratio), (float)(90 - wp_high * Math.Cos(ang) * con_ratio));////焊高标注箭头

                    pictureBox1.Location = new Point(181, 246);
                    label12.Location = new Point(220 + 80, 240 + 37);       /////图像中显示尺寸的label的位置
                    label16.Location = new Point(220 + 183, 240 + 132);
                    label13.Location = new Point(220 + 145, 240 + 20);
                    label15.Location = new Point(220 + 176, 240 + 73);
                    label14.Location = new Point(186 + (int)(165 + (wp_high + 2) * Math.Sin(ang) * con_ratio), 232+(int)(90 - (wp_high + 2) * Math.Cos(ang) * con_ratio));                   
                    
                    ////焊宽
                    //double xxx = 0;
                    //double xxx1 = 0;
                    //if (r * longth / (original_num - 1) < 41 & r * longth / (original_num - 1) > 39)
                    //{
                    //    xxx = 1;
                    //    xxx1 = 1;
                    //}
                    //else
                    //{
                    //    xxx = 1.16;
                    //    xxx1 = 1.23;
                    //}
                    label20.Text = Convert.ToString(Math.Round(wp_width1 , 2, MidpointRounding.AwayFromZero)) + "  " + Convert.ToString(Math.Round(wp_width2 , 2, MidpointRounding.AwayFromZero));
                    label13.Text = Convert.ToString(Math.Round(wp_width1 , 2, MidpointRounding.AwayFromZero));
                    label15.Text = Convert.ToString(Math.Round(wp_width2 , 2, MidpointRounding.AwayFromZero));

                    ////熔深
                    label21.Text = Convert.ToString(Math.Round(wp_depth1, 2, MidpointRounding.AwayFromZero)) + "  " + Convert.ToString(Math.Round(wp_depth2, 2, MidpointRounding.AwayFromZero));
                    label12.Text = Convert.ToString(Math.Round(wp_depth2, 2, MidpointRounding.AwayFromZero));
                    label16.Text = Convert.ToString(Math.Round(wp_depth1, 2, MidpointRounding.AwayFromZero));

                  
                        label22.Text = Convert.ToString(Math.Round(wp_high*1.16, 2, MidpointRounding.AwayFromZero));
                        label14.Text = Convert.ToString(Math.Round(wp_high*1.16, 2, MidpointRounding.AwayFromZero));
                    
                    //label27.Text = Convert.ToString(Math.Round(wp_thick, 2, MidpointRounding.AwayFromZero));
                }
                #endregion

                #region////钝角焊缝的画图
                if (comboBox2.SelectedIndex == 1)
                {
                    double data_x1, data_y1;
                    pane4.ReverseTransform(mousePt, out data_x1, out data_y1);
                    for (int i = 0; i < original_num; i++)
                    {
                        x11 = i * longth / (original_num - 1);
                        if (data_x1 - x11 < longth / (original_num - 1))
                        {
                            if (data_x1 - x11 < (i + 1) * longth / (original_num - 1) - data_x1)
                            {
                                x11 = i * longth / (original_num - 1);
                                wp_depth1 = rongshenzhi1[i];
                                wp_depth2 = rongshenzhi2[i];
                                wp_width1 = hankuan1[i];
                                wp_width2 = hankuan2[i];
                                wp_high = hangao1[i];
                                wp_thick = nihe1[i, 3];
                                r = i;

                                fit_x1 = x1zhi[i];
                                fit_x2 = x2zhi[i];
                            }
                            else
                            {
                                x11 = (i + 1) * longth / (original_num - 1);
                                wp_depth1 = rongshenzhi1[i + 1];
                                wp_depth2 = rongshenzhi2[i + 1];
                                wp_width1 = hankuan1[i + 1];
                                wp_width2 = hankuan2[i + 1];
                                wp_high = hangao1[i + 1];
                                wp_thick = nihe1[i + 1, 3];
                                r = i + 1;

                                fit_x1 = x1zhi[i + 1];
                                fit_x2 = x2zhi[i + 1];
                            }
                            break;
                        }
                    }
                    float con_ratio = 0;
                    con_ratio = 220 / 50 * 2;
                    this.pictureBox1.Refresh();
                    Graphics g = this.pictureBox1.CreateGraphics();

                    Pen pen1 = new Pen(Color.Blue, 2);
                    Pen pen2 = new Pen(Color.Red, 3);
                    Pen pen3 = new Pen(Color.Green, 2);
                    Pen pen4 = new Pen(Color.LimeGreen, 1);
                    ////////////////////////////////////////
                    g.DrawLine(pen1, 30, 150, 250, 150);///最下面线，量得其尺寸为50mm，坐标长220。

                    g.DrawLine(pen1, 30, 90, 30, 150);
                    g.DrawLine(pen1, 250, 90, 250, 150);

                    g.DrawLine(pen1, 30, 90, 115, 90);
                    g.DrawLine(pen1, 165, 90, 250, 90);

                    g.DrawLine(pen1, 115, 90, 115 - (int)(60 * Math.Tan((angle-90) * Math.PI / 180)), 30);
                    g.DrawLine(pen1, 165, 90, 165 - (int)(60 * Math.Tan((angle-90) * Math.PI / 180)), 30);

                    g.DrawLine(pen1, 115 - (int)(60 * Math.Tan((angle-90) * Math.PI / 180)), 30, 165 - (int)(60 * Math.Tan((angle-90) * Math.PI / 180)), 30);
                    ////焊宽的画线
                    g.DrawLine(pen2, 165, 90, 165 - (int)(con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180)), 90 - (int)(con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180)));
                    g.DrawLine(pen2, 165, 90, 165 + (int)(con_ratio * wp_width2), 90);
                    g.DrawLine(pen2, 165 - (int)(con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180)), 90 - (int)(con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180)), 165 + (int)(con_ratio * wp_width2), 90);
                    /////焊缝形状的画出
                    double b_xq = 0;
                    double b_yq = 0;   ///拟合出的第一个点在焊缝斜边上的投影的坐标
                    double[] b_x = new double[edge_longth];
                    double[] b_y = new double[edge_longth];////需要求的点在焊缝斜边上的投影的坐标
                    double ang = 0;
                    ang = data[19, r] * Math.PI / 180;/////此处的角度

                    b_xq = 165 - con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180) + 0.5 * Math.Cos(ang) * con_ratio;
                    b_yq = 90 - con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180) + 0.5 * Math.Sin(ang) * con_ratio;

                    for (int j = 0; j < edge_longth; j++)    ///拟合点在焊缝斜边上投影对应的坐标值
                    {
                        b_x[j] = b_xq + j * 1 * Math.Cos(ang) * con_ratio;
                        b_y[j] = b_yq + j * 1 * Math.Sin(ang) * con_ratio;
                    }

                    ////////////////////////////////////////////////////////////////////////////////
                    g.FillEllipse(Brushes.Black, (float)(b_x[0]), (float)(b_y[0]), 4, 4);
                    g.FillEllipse(Brushes.Black, (float)(b_x[1]), (float)(b_y[1]), 4, 4);
                    g.FillEllipse(Brushes.Black, (float)(b_x[2]), (float)(b_y[2]), 4, 4);
                    g.FillEllipse(Brushes.Black, (float)(b_x[3]), (float)(b_y[3]), 4, 4);
                    g.FillEllipse(Brushes.Black, (float)(b_x[4]), (float)(b_y[4]), 4, 4);
                    g.FillEllipse(Brushes.Black, (float)(b_x[5]), (float)(b_y[5]), 4, 4);
                    g.FillEllipse(Brushes.Black, (float)(b_x[6]), (float)(b_y[6]), 4, 4); 
                    ///////////////////////////////////////////////////////////////////////////////


                    double[] fit_depth = new double[edge_longth];
                    for (int j = 0; j < edge_longth; j++)
                    {
                        fit_depth[j] = nihe1[r, j];
                    }
                    double[] nh_x = new double[edge_longth];
                    double[] nh_y = new double[edge_longth];///7个拟合点的坐标值
                    for (int j = 0; j < edge_longth; j++)
                    {
                        nh_x[j] = b_x[j] - fit_depth[j] * Math.Sin(ang) * con_ratio;
                        nh_y[j] = b_y[j] + fit_depth[j] * Math.Cos(ang) * con_ratio;
                    }
                   

                    double nh_x1_x = 0;
                    double nh_x1_y = 0;
                    double nh_x2_x = 0;
                    double nh_x2_y = 0;////一元二次方程与坐标轴的交点

                    //nh_x1_x = b_x[3] + Math.Abs(fit_x1) * Math.Cos(ang) * con_ratio;
                    //nh_x1_y = b_y[3] + Math.Abs(fit_x1) * Math.Sin(ang) * con_ratio;
                    //nh_x2_x = b_x[3] - Math.Abs(fit_x2) * Math.Cos(ang) * con_ratio;
                    //nh_x2_y = b_y[3] - Math.Abs(fit_x2) * Math.Sin(ang) * con_ratio;
                    ///////前面计算焊宽时，使焊缝斜边默认长度为7，拟合计算出来的曲线与x轴交点长度不一定为7，那么要对其压缩，使其变为7
                    //if (fit_x2 - fit_x1 != edge_longth )
                    //{
                    //    x2_revise_x = (fit_x2 - edge_longth / 2 ) * Math.Cos(ang) * con_ratio;
                    //    x2_revise_y = (fit_x2 - edge_longth / 2) * Math.Sin(ang) * con_ratio;
                    //    x1_revise_x = (fit_x1 + edge_longth / 2) * Math.Cos(ang) * con_ratio;
                    //    x1_revise_y = (fit_x1 + edge_longth / 2) * Math.Sin(ang) * con_ratio;
                    //    nh_x2_x = nh_x2_x + x2_revise_x;
                    //    nh_x2_y = nh_x2_y + x2_revise_y;
                    //    nh_x1_x = nh_x1_x + x1_revise_x;
                    //    nh_x1_y = nh_x1_y + x1_revise_y;
                    //}


                    ////////////////////////画抛物线
                    if (edge_longth == 7)
                    {
                        nh_x1_x = b_x[3] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[3] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[3] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[3] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x[3]),(int)(nh_y [3])),
                             new Point((int)(nh_x[4]),(int)(nh_y [4])),
                             new Point((int)(nh_x[5]),(int)(nh_y [5])),
                             new Point((int)(nh_x[6]),(int)(nh_y [6])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    else if (edge_longth == 5)
                    {
                        nh_x1_x = b_x[2] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[2] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[2] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[2] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x[3]),(int)(nh_y [3])),
                             new Point((int)(nh_x[4]),(int)(nh_y [4])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    else if (edge_longth == 3)
                    {
                        nh_x1_x = b_x[1] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[1] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[1] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[1] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    else
                    {
                        nh_x1_x = b_x[4] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[4] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[4] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[4] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x[3]),(int)(nh_y [3])),
                             new Point((int)(nh_x[4]),(int)(nh_y [4])),
                             new Point((int)(nh_x[5]),(int)(nh_y [5])),
                             new Point((int)(nh_x[6]),(int)(nh_y [6])),
                             new Point((int)(nh_x[7]),(int)(nh_y [7])),
                             new Point((int)(nh_x[8]),(int)(nh_y [8])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    /////////////////////焊宽1标注
                    g.DrawLine(pen4, 165, 90, 165 + (int)(2 * con_ratio * Math.Cos((angle - 90) * Math.PI / 180)), 90 - (int)(2 * con_ratio * Math.Sin((angle - 90) * Math.PI / 180)));
                    g.DrawLine(pen4, 165 - (int)(con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180)), 90 - (int)(con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180)),
                        165 - (int)(con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180)) + (int)(2 * con_ratio * Math.Cos((angle - 90) * Math.PI / 180)), 90 - (int)(con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(2 * con_ratio * Math.Sin((angle - 90) * Math.PI / 180)));
                    g.DrawLine(pen4, 165 - (int)(con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180)) + (int)(1.5 * con_ratio * Math.Cos((angle - 90) * Math.PI / 180)), 90 - (int)(con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(1.5 * con_ratio * Math.Sin((angle - 90) * Math.PI / 180)),
                        165 - (int)(con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180)) + (int)(1.5 * con_ratio * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(25 * Math.Sin((angle - 90) * Math.PI / 180)), 90 - (int)(con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(1.5 * con_ratio * Math.Sin((angle - 90) * Math.PI / 180)) - (int)(25 * Math.Cos((angle - 90) * Math.PI / 180)));
                    g.DrawLine(pen4, 165 - (int)(con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180)) + (int)(1.5 * con_ratio * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(25 * Math.Sin((angle - 90) * Math.PI / 180)), 90 - (int)(con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(1.5 * con_ratio * Math.Sin((angle - 90) * Math.PI / 180)) - (int)(25 * Math.Cos((angle - 90) * Math.PI / 180)),
                         165 - (int)(con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180)) + (int)(1.5 * con_ratio * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(25 * Math.Sin((angle - 90) * Math.PI / 180)) + 30, 90 - (int)(con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(1.5 * con_ratio * Math.Sin((angle - 90) * Math.PI / 180)) - (int)(25 * Math.Cos((angle - 90) * Math.PI / 180)));
                    /////////////////////焊宽2标注
                    g.DrawLine(pen4, 165, 90, 165, 110);
                    g.DrawLine(pen4, 165 + (int)(con_ratio * wp_width2), 90, 165 + (int)(con_ratio * wp_width2), 110);
                    g.DrawLine(pen4, 165 + (int)(con_ratio * wp_width2), 105, 165 + (int)(con_ratio * wp_width2)+30, 105);
                    ////////////////////熔深1标注
                    g.DrawLine(pen4, 145, (int)(90 + con_ratio * wp_depth1), 233, (int)(90 + con_ratio * wp_depth1));
                    g.DrawLine (pen4,230, (int)(90 + con_ratio * wp_depth1),230, (int)(90 + con_ratio * wp_depth1+30));
                    g.DrawLine(pen4, 230, (int)(90 + con_ratio * wp_depth1 + 30), 250, (int)(90 + con_ratio * wp_depth1 + 30));

                    ////////////////////熔深2标注
                    g.DrawLine(pen4, (float)(165 - wp_depth2 * Math.Cos((angle - 90) * Math.PI / 180) * con_ratio - 50 * Math.Sin((angle - 90) * Math.PI / 180)), (float)(90 + wp_depth2 * Math.Sin((angle - 90) * Math.PI / 180) * con_ratio - 50 * Math.Cos((angle - 90) * Math.PI / 180)),
                        (float)(165 - wp_depth2 * Math.Cos((angle - 90) * Math.PI / 180) * con_ratio + 15 * Math.Sin((angle - 90) * Math.PI / 180)), (float)(90 + wp_depth2 * Math.Sin((angle - 90) * Math.PI / 180) * con_ratio + 15 * Math.Cos((angle - 90) * Math.PI / 180)));
                    g.DrawLine (pen4,(float)(165 - wp_depth2 * Math.Cos((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Sin((angle - 90) * Math.PI / 180)), (float)(90 + wp_depth2 * Math.Sin((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Cos((angle - 90) * Math.PI / 180)),
                        (float)(165 - wp_depth2 * Math.Cos((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Sin((angle - 90) * Math.PI / 180)-10*Math.Cos((angle - 90) * Math.PI / 180)), (float)(90 + wp_depth2 * Math.Sin((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Cos((angle - 90) * Math.PI / 180)+10*Math.Cos((angle - 90) * Math.PI / 180)));
                    g.DrawLine(pen4, (float)(165 - wp_depth2 * Math.Cos((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Sin((angle - 90) * Math.PI / 180) - 10 * Math.Cos((angle - 90) * Math.PI / 180)), (float)(90 + wp_depth2 * Math.Sin((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Cos((angle - 90) * Math.PI / 180) + 10 * Math.Cos((angle - 90) * Math.PI / 180)),
                         (float)(165 - wp_depth2 * Math.Cos((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Sin((angle - 90) * Math.PI / 180) - 10 * Math.Cos((angle - 90) * Math.PI / 180)-40), (float)(90 + wp_depth2 * Math.Sin((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Cos((angle - 90) * Math.PI / 180) + 10 * Math.Cos((angle - 90) * Math.PI / 180)));
                    //////////////////焊高标注
                    g.DrawLine (pen4,(int)(165 + con_ratio*wp_high * Math.Sin(ang)), (int)(90 - con_ratio*wp_high * Math.Cos(ang )),(int)(165 + con_ratio*wp_high * Math.Sin(ang)+20*Math .Sin (ang )), (int)(90 - con_ratio*wp_high * Math.Cos(ang)-20*Math .Cos (ang)));
                    g.DrawLine(pen4, (int)(165 + con_ratio * wp_high * Math.Sin(ang) + 20 * Math.Sin(ang)), (int)(90 - con_ratio * wp_high * Math.Cos(ang) - 20 * Math.Cos(ang)), (int)(165 + con_ratio * wp_high * Math.Sin(ang) + 20 * Math.Sin(ang) + 30), (int)(90 - con_ratio * wp_high * Math.Cos(ang) - 20 * Math.Cos(ang)));
                    
                    pen4.StartCap = LineCap.ArrowAnchor;//定义线头的样式为箭头
                    pen4.EndCap = LineCap.ArrowAnchor;//定义线尾的样式为箭头

                    //////焊宽1箭头线
                    g.DrawLine (pen4,165 + (int)(1.5 * con_ratio * Math.Cos((angle - 90) * Math.PI / 180)), 90 - (int)(1.5 * con_ratio * Math.Sin((angle - 90) * Math.PI / 180)),
                        165 - (int)(con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180)) + (int)(1.5 * con_ratio * Math.Cos((angle - 90) * Math.PI / 180)), 90 - (int)(con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(1.5 * con_ratio * Math.Sin((angle - 90) * Math.PI / 180)));
                    /////焊宽2箭头线
                    g.DrawLine(pen4, 165, 105, 165 + (int)(con_ratio * wp_width2), 105);
                    /////熔深1箭头线
                    g.DrawLine(pen4, 230, (int)(90 + con_ratio * wp_depth1 + 30), 230, 90);
                    /////熔深2箭头线
                    g.DrawLine (pen4,(float)(165 - wp_depth2 * Math.Cos((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Sin((angle - 90) * Math.PI / 180)), (float)(90 + wp_depth2 * Math.Sin((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Cos((angle - 90) * Math.PI / 180)),
                        (float)(165 - wp_depth2 * Math.Cos((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Sin((angle - 90) * Math.PI / 180) + con_ratio * wp_depth2 * Math.Cos((angle - 90) * Math.PI / 180)), (float)(90 + wp_depth2 * Math.Sin((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Cos((angle - 90) * Math.PI / 180) - con_ratio * wp_depth2 * Math.Sin((angle - 90) * Math.PI / 180)));
                    //////焊高箭头线
                    g.DrawLine(pen4, 165, 90, (int)(165 + con_ratio*wp_high * Math.Sin(ang)), (int)(90 - con_ratio*wp_high * Math.Cos(ang )));

                    /////label的位置调整
                    label13.Location = new Point(183 + 165 - (int)(con_ratio * wp_width1 * Math.Sin((angle - 90) * Math.PI / 180)) + (int)(1.5 * con_ratio * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(25 * Math.Sin((angle - 90) * Math.PI / 180)) , 240 + 90 - (int)(con_ratio * wp_width1 * Math.Cos((angle - 90) * Math.PI / 180)) - (int)(1.5 * con_ratio * Math.Sin((angle - 90) * Math.PI / 180)) - (int)(25 * Math.Cos((angle - 90) * Math.PI / 180))-14);
                    label15.Location = new Point(183 + 165 + (int)(con_ratio * wp_width2), 239 + 105 - 14);
                    label12.Location = new Point(165+30 - (int)(wp_depth2 * Math.Cos((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Sin((angle - 90) * Math.PI / 180) - 10 * Math.Cos((angle - 90) * Math.PI / 180) - 40), 239+90 + (int)(wp_depth2 * Math.Sin((angle - 90) * Math.PI / 180) * con_ratio - 47 * Math.Cos((angle - 90) * Math.PI / 180) + 10 * Math.Cos((angle - 90) * Math.PI / 180)-14));
                    label16.Location = new Point(183+230, 239+(int)(90 + con_ratio * wp_depth1 + 30-14));
                    label14.Location = new Point(183 + (int)(165 + con_ratio * wp_high * Math.Sin(ang) + 20 * Math.Sin(ang)), 239+(int)(90 - con_ratio * wp_high * Math.Cos(ang) - 20 * Math.Cos(ang))-14);
                    ////焊宽
                    label20.Text = Convert.ToString(Math.Round(wp_width1, 2, MidpointRounding.AwayFromZero)) + "  " + Convert.ToString(Math.Round(wp_width2, 2, MidpointRounding.AwayFromZero));
                    label13.Text = Convert.ToString(Math.Round(wp_width1, 2, MidpointRounding.AwayFromZero));
                    label15.Text = Convert.ToString(Math.Round(wp_width2, 2, MidpointRounding.AwayFromZero));

                    ////熔深
                    label21.Text = Convert.ToString(Math.Round(wp_depth1, 2, MidpointRounding.AwayFromZero)) + "  " + Convert.ToString(Math.Round(wp_depth2, 2, MidpointRounding.AwayFromZero));
                    label12.Text = Convert.ToString(Math.Round(wp_depth2, 2, MidpointRounding.AwayFromZero));
                    label16.Text = Convert.ToString(Math.Round(wp_depth1, 2, MidpointRounding.AwayFromZero));

                    //////焊高
                    label22.Text = Convert.ToString(Math.Round(wp_high, 2, MidpointRounding.AwayFromZero));
                    label14.Text = Convert.ToString(Math.Round(wp_high, 2, MidpointRounding.AwayFromZero));

                }
                #endregion


                #region////锐角焊缝的画图
                if (comboBox2.SelectedIndex == 2)
                {
                    double data_x1, data_y1;
                    pane4.ReverseTransform(mousePt, out data_x1, out data_y1);
                    for (int i = 0; i < original_num; i++)
                    {
                        x11 = i * longth / (original_num - 1);
                        if (data_x1 - x11 < longth / (original_num - 1))
                        {
                            if (data_x1 - x11 < (i + 1) * longth / (original_num - 1) - data_x1)
                            {
                                x11 = i * longth / (original_num - 1);
                                wp_depth1 = rongshenzhi1[i];
                                wp_depth2 = rongshenzhi2[i];
                                wp_width1 = hankuan1[i];
                                wp_width2 = hankuan2[i];
                                wp_high = hangao1[i];
                                wp_thick = nihe1[i, 3];
                                r = i;

                                fit_x1 = x1zhi[i];
                                fit_x2 = x2zhi[i];
                            }
                            else
                            {
                                x11 = (i + 1) * longth / (original_num - 1);
                                wp_depth1 = rongshenzhi1[i + 1];
                                wp_depth2 = rongshenzhi2[i + 1];
                                wp_width1 = hankuan1[i + 1];
                                wp_width2 = hankuan2[i + 1];
                                wp_high = hangao1[i + 1];
                                wp_thick = nihe1[i + 1, 3];
                                r = i + 1;

                                fit_x1 = x1zhi[i + 1];
                                fit_x2 = x2zhi[i + 1];
                            }
                            break;
                        }
                    }
                    float con_ratio = 0;
                    con_ratio = 220 / 50 * 2;
                    this.pictureBox1.Refresh();
                    Graphics g = this.pictureBox1.CreateGraphics();

                    Pen pen1 = new Pen(Color.Blue, 2);
                    Pen pen2 = new Pen(Color.Red, 3);
                    Pen pen3 = new Pen(Color.Green, 2);
                    Pen pen4 = new Pen(Color.LimeGreen, 1);
                    ////////////////////////////////////////
                    g.DrawLine(pen1, 30, 150, 258, 150);///最下面线，量得其尺寸为50mm，坐标长220。

                    g.DrawLine(pen1, 30, 90, 30, 150);
                    g.DrawLine(pen1, 258, 90, 258, 150);

                    g.DrawLine(pen1, 30, 90, 115, 90);
                    g.DrawLine(pen1, 165, 90, 258, 90);

                    g.DrawLine(pen1, 115, 90, 115 + (int)(60 * Math.Tan((90-angle) * Math.PI / 180)), 20);
                    g.DrawLine(pen1, 165, 90, 165 + (int)(60 * Math.Tan((90-angle) * Math.PI / 180)), 20);

                    g.DrawLine(pen1, 115 + (int)(60 * Math.Tan((90-angle) * Math.PI / 180)), 20, 165 + (int)(60 * Math.Tan((90-angle) * Math.PI / 180)), 20);
                    ////焊宽的画线
                    g.DrawLine(pen2, 165, 90, 165 + (int)(wp_width1 * Math.Cos(angle * Math.PI / 180) * con_ratio), 90 - (int)(wp_width1 * Math.Sin(angle * Math.PI / 180)*con_ratio ));
                    g.DrawLine(pen2, 165, 90, 165 + (int)(wp_width2 * con_ratio), 90);
                    g.DrawLine(pen2, 165 + (int)(wp_width1 * Math.Cos(angle * Math.PI / 180) * con_ratio), 90 - (int)(wp_width1 * Math.Sin(angle * Math.PI / 180) * con_ratio), 165 + (int)(wp_width2 * con_ratio), 90);
                    //////焊缝形状的画出
                    double b_xq = 0;
                    double b_yq = 0;   ///拟合出的第一个点在焊缝斜边上的投影的坐标
                    double[] b_x = new double[edge_longth];
                    double[] b_y = new double[edge_longth];////需要求的点在焊缝斜边上的投影的坐标

                    double ang = 0;
                    ang = data[19, r] * Math.PI / 180;/////此处的角度

                    b_xq = 165 + wp_width1 * Math.Cos(angle * Math.PI / 180) * con_ratio + 0.5 * Math.Cos(ang) * con_ratio;
                    b_yq = 90 - wp_width1 * Math.Sin(angle * Math.PI / 180) * con_ratio + 0.5 * Math.Sin(ang) * con_ratio;

                    for (int j = 0; j < edge_longth; j++)    ///拟合点在焊缝斜边上投影对应的坐标值
                    {
                        b_x[j] = b_xq + j * 1 * Math.Cos(ang) * con_ratio;
                        b_y[j] = b_yq + j * 1 * Math.Sin(ang) * con_ratio;
                    }
                    //////////////////////////////////////////////////////////////////////////////////////
                    //g.FillEllipse(Brushes.Black, (float)(b_x[0]), (float)(b_y[0]), 4, 4);
                    //g.FillEllipse(Brushes.Black, (float)(b_x[1]), (float)(b_y[1]), 4, 4);
                    //g.FillEllipse(Brushes.Black, (float)(b_x[2]), (float)(b_y[2]), 4, 4);
                    //g.FillEllipse(Brushes.Black, (float)(b_x[3]), (float)(b_y[3]), 4, 4);
                    //g.FillEllipse(Brushes.Black, (float)(b_x[4]), (float)(b_y[4]), 4, 4);
                    //g.FillEllipse(Brushes.Black, (float)(b_x[5]), (float)(b_y[5]), 4, 4);
                    //g.FillEllipse(Brushes.Black, (float)(b_x[6]), (float)(b_y[6]), 4, 4); 
                    //////////////////////////////////////////////////////////////////////////////////////
                    double[] fit_depth = new double[edge_longth];
                    for (int j = 0; j < edge_longth; j++)
                    {
                        fit_depth[j] = nihe1[r, j];
                    }
                    double[] nh_x = new double[edge_longth];
                    double[] nh_y = new double[edge_longth];///7个拟合点的坐标值
                    for (int j = 0; j < edge_longth; j++)
                    {
                        nh_x[j] = b_x[j] - fit_depth[j] * Math.Sin(ang) * con_ratio;
                        nh_y[j] = b_y[j] + fit_depth[j] * Math.Cos(ang) * con_ratio;
                    }

                    double nh_x1_x = 0;
                    double nh_x1_y = 0;
                    double nh_x2_x = 0;
                    double nh_x2_y = 0;////一元二次方程与坐标轴的交点

                    //nh_x1_x = b_x[3] + Math.Abs(fit_x1) * Math.Cos(ang) * con_ratio;
                    //nh_x1_y = b_y[3] + Math.Abs(fit_x1) * Math.Sin(ang) * con_ratio;
                    //nh_x2_x = b_x[3] - Math.Abs(fit_x2) * Math.Cos(ang) * con_ratio;
                    //nh_x2_y = b_y[3] - Math.Abs(fit_x2) * Math.Sin(ang) * con_ratio;
                    ///////前面计算焊宽时，使焊缝斜边默认长度为7，拟合计算出来的曲线与x轴交点长度不一定为7，那么要对其压缩，使其变为7
                    //if (fit_x2 - fit_x1 != edge_longth )
                    //{
                    //    x2_revise_x = (fit_x2 - edge_longth / 2 ) * Math.Cos(ang) * con_ratio;
                    //    x2_revise_y = (fit_x2 - edge_longth / 2) * Math.Sin(ang) * con_ratio;
                    //    x1_revise_x = (fit_x1 + edge_longth / 2) * Math.Cos(ang) * con_ratio;
                    //    x1_revise_y = (fit_x1 + edge_longth / 2) * Math.Sin(ang) * con_ratio;
                    //    nh_x2_x = nh_x2_x + x2_revise_x;
                    //    nh_x2_y = nh_x2_y + x2_revise_y;
                    //    nh_x1_x = nh_x1_x + x1_revise_x;
                    //    nh_x1_y = nh_x1_y + x1_revise_y;
                    //}


                    ////////////////////////画抛物线
                    if (edge_longth == 7)
                    {
                        nh_x1_x = b_x[3] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[3] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[3] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[3] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x[3]),(int)(nh_y [3])),
                             new Point((int)(nh_x[4]),(int)(nh_y [4])),
                             new Point((int)(nh_x[5]),(int)(nh_y [5])),
                             new Point((int)(nh_x[6]),(int)(nh_y [6])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    else if (edge_longth == 5)
                    {
                        nh_x1_x = b_x[2] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[2] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[2] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[2] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x[3]),(int)(nh_y [3])),
                             new Point((int)(nh_x[4]),(int)(nh_y [4])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    else
                    {
                        nh_x1_x = b_x[1] + Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x1_y = b_y[1] + Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        nh_x2_x = b_x[1] - Math.Abs(edge_longth / 2) * Math.Cos(ang) * con_ratio;
                        nh_x2_y = b_y[1] - Math.Abs(edge_longth / 2) * Math.Sin(ang) * con_ratio;
                        Point[] p1 = new Point[]
                        {    
                             new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                             new Point((int)(nh_x[0]),(int)(nh_y [0])),
                             new Point((int)(nh_x[1]),(int)(nh_y [1])),
                             new Point((int)(nh_x[2]),(int)(nh_y [2])),
                             new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    //////////焊宽1标注
                    g.DrawLine(pen4, 165, 90, 165 + (int)(con_ratio*1.5 * Math.Sin(angle*Math .PI /180)), 90 + (int)(con_ratio*1.5 * Math.Cos(angle*Math .PI/180)));
                    g.DrawLine (pen4,165+(int)(con_ratio *wp_width1 *Math .Cos (angle*Math .PI /180 )),90-(int)(con_ratio *wp_width1 *Math .Sin (angle*Math .PI /180 )),
                        165+(int)(con_ratio *wp_width1 *Math .Cos (angle*Math .PI /180 ))+(int)(con_ratio*1.5 * Math.Sin(angle*Math .PI /180)),90-(int)(con_ratio *wp_width1 *Math .Sin (angle*Math .PI /180 ))+(int)(con_ratio*1.5 * Math.Cos(angle*Math .PI/180)));
                    g.DrawLine(pen4, 165 + (int)(con_ratio * wp_width1 * Math.Cos(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Sin(angle * Math.PI / 180)), 90 - (int)(con_ratio * wp_width1 * Math.Sin(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Cos(angle * Math.PI / 180)),
                        165 + (int)(con_ratio * wp_width1 * Math.Cos(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Sin(angle * Math.PI / 180)) + (int)(con_ratio * 3 * Math.Cos(angle * Math.PI / 180)),
                        90 - (int)(con_ratio * wp_width1 * Math.Sin(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Cos(angle * Math.PI / 180)) - (int)(con_ratio * 3 * Math.Sin(angle * Math.PI / 180)));
                    g.DrawLine(pen4, 165 + (int)(con_ratio * wp_width1 * Math.Cos(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Sin(angle * Math.PI / 180)) + (int)(con_ratio * 3 * Math.Cos(angle * Math.PI / 180)),
                        90 - (int)(con_ratio * wp_width1 * Math.Sin(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Cos(angle * Math.PI / 180)) - (int)(con_ratio * 3 * Math.Sin(angle * Math.PI / 180)),
                        165 + (int)(con_ratio * wp_width1 * Math.Cos(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Sin(angle * Math.PI / 180)) + (int)(con_ratio * 3 * Math.Cos(angle * Math.PI / 180)) + 30,
                         90 - (int)(con_ratio * wp_width1 * Math.Sin(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Cos(angle * Math.PI / 180)) - (int)(con_ratio * 3 * Math.Sin(angle * Math.PI / 180)));
                    /////////焊宽2标注
                    g.DrawLine(pen4, 165, 90, 165, 107);
                    g.DrawLine(pen4, 165 + (int)(con_ratio * wp_width2), 90, 165 + (int)(con_ratio * wp_width2), 107);
                    g.DrawLine(pen4, 165 + (int)(con_ratio * wp_width2), 104, 165 + (int)(con_ratio * wp_width2) + 24, 104);
                    //////////熔深1标注
                    g.DrawLine(pen4, 160, 90 + (int)(wp_depth1 * con_ratio), 255, 90 + (int)(wp_depth1 * con_ratio));
                    g.DrawLine(pen4, 253, 90 + (int)(wp_depth1 * con_ratio), 253, 90 + (int)(wp_depth1 * con_ratio) + 20);
                    g.DrawLine(pen4, 253, 90 + (int)(wp_depth1 * con_ratio) + 20, 283, 90 + (int)(wp_depth1 * con_ratio) + 20);
                    //////////熔深2标注
                    g.DrawLine(pen4, 165 - (int)(con_ratio * wp_depth2 * Math.Sin(angle * Math.PI / 180)), 90 - (int)(con_ratio * wp_depth2 * Math.Cos(angle * Math.PI / 180)),
                        165 - (int)(con_ratio * wp_depth2 * Math.Sin(angle * Math.PI / 180) - con_ratio * 7 * Math.Cos(angle * Math.PI / 180)), 90 - (int)(con_ratio * wp_depth2 * Math.Cos(angle * Math.PI / 180) + con_ratio * 7 * Math.Sin(angle * Math.PI / 180)));
                    g.DrawLine(pen4, 165 - (int)(con_ratio * wp_depth2 * Math.Sin(angle * Math.PI / 180) - con_ratio * 6.5 * Math.Cos(angle * Math.PI / 180)), 90 - (int)(con_ratio * wp_depth2 * Math.Cos(angle * Math.PI / 180) + con_ratio * 6.5 * Math.Sin(angle * Math.PI / 180)),
                        165 - (int)(con_ratio * wp_depth2 * Math.Sin(angle * Math.PI / 180) - con_ratio * 6.5 * Math.Cos(angle * Math.PI / 180)) - 25, 90 - (int)(con_ratio * wp_depth2 * Math.Cos(angle * Math.PI / 180) + con_ratio * 6.5 * Math.Sin(angle * Math.PI / 180)));
                    
                    //////////焊高标注
                   g.DrawLine (pen4, 165 + (int)(con_ratio * wp_high * Math.Sin(ang)), 90 - (int)(con_ratio * wp_high * Math.Cos(ang)),165 + (int)(con_ratio * wp_high * Math.Sin(ang))+40, 90 - (int)(con_ratio * wp_high * Math.Cos(ang)));
                    
                    pen4.StartCap = LineCap.ArrowAnchor;//定义线头的样式为箭头
                    pen4.EndCap = LineCap.ArrowAnchor;//定义线尾的样式为箭头
                     //////焊宽1箭头线
                      g.DrawLine(pen4, 165 + (int)(con_ratio * wp_width1 * Math.Cos(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Sin(angle * Math.PI / 180)), 90 - (int)(con_ratio * wp_width1 * Math.Sin(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Cos(angle * Math.PI / 180)),
                          165+(int)(con_ratio * 1 * Math.Sin(angle * Math.PI / 180)),90+(int)(con_ratio * 1 * Math.Cos(angle * Math.PI / 180)));        
                    //////焊宽2箭头线
                    g.DrawLine(pen4, 165, 104, 165 + (int)(con_ratio * wp_width2) , 104);
                    //////熔深1箭头线
                    g.DrawLine(pen4, 253, 90, 253, 90 + (int)(wp_depth1 * con_ratio));
                    //////熔深2箭头线
                    g.DrawLine(pen4, 165 + (int)( con_ratio * 6.5 * Math.Cos(angle * Math.PI / 180)), 90 - (int)( con_ratio * 6.5 * Math.Sin(angle * Math.PI / 180)),
                        165 + (int)( con_ratio * 6.5 * Math.Cos(angle * Math.PI / 180) - con_ratio * wp_depth2 * Math.Sin(angle * Math.PI / 180)), 90 - (int)(con_ratio * 6.5 * Math.Sin(angle * Math.PI / 180) + con_ratio * wp_depth2 * Math.Cos(angle * Math.PI / 180)));
                    
                    
                    /////焊高箭头线
                    g.DrawLine(pen4, 165, 90, 165 + (int)(con_ratio * wp_high * Math.Sin(ang)), 90 - (int)(con_ratio * wp_high * Math.Cos(ang)));
                    /////label的位置调整
                    label15.Location = new Point(183 + 165 + (int)(con_ratio * wp_width2) + 2, 239 + 108 - 18);
                    label13.Location = new Point(183 +165 + (int)(con_ratio * wp_width1 * Math.Cos(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Sin(angle * Math.PI / 180)) + (int)(con_ratio * 3 * Math.Cos(angle * Math.PI / 180)) ,
                        239 + 90 - (int)(con_ratio * wp_width1 * Math.Sin(angle * Math.PI / 180)) + (int)(con_ratio * 1 * Math.Cos(angle * Math.PI / 180)) - (int)(con_ratio * 3 * Math.Sin(angle * Math.PI / 180))-14);
                    label12.Location = new Point(183 + 253, 239 + 90 + (int)(wp_depth1 * con_ratio) + 4);
                    label16.Location = new Point(183 + 165 - (int)(con_ratio * wp_depth2 * Math.Sin(angle * Math.PI / 180) - con_ratio * 6.5 * Math.Cos(angle * Math.PI / 180)) - 25, 239 + 90 - (int)(con_ratio * wp_depth2 * Math.Cos(angle * Math.PI / 180) + con_ratio * 6.5 * Math.Sin(angle * Math.PI / 180)) - 14);
                    label14.Location = new Point(183 + 165 + (int)(con_ratio * wp_high * Math.Sin(ang))+10, 239 + 90 - (int)(con_ratio * wp_high * Math.Cos(ang))-15);
                    ////焊宽
                    label20.Text = Convert.ToString(Math.Round(wp_width1, 2, MidpointRounding.AwayFromZero)) + "  " + Convert.ToString(Math.Round(wp_width2, 2, MidpointRounding.AwayFromZero));
                    label13.Text = Convert.ToString(Math.Round(wp_width1, 2, MidpointRounding.AwayFromZero));
                    label15.Text = Convert.ToString(Math.Round(wp_width2, 2, MidpointRounding.AwayFromZero));

                    ////熔深
                    label21.Text = Convert.ToString(Math.Round(wp_depth1, 2, MidpointRounding.AwayFromZero)) + "  " + Convert.ToString(Math.Round(wp_depth2, 2, MidpointRounding.AwayFromZero));
                    label12.Text = Convert.ToString(Math.Round(wp_depth1, 2, MidpointRounding.AwayFromZero));
                    label16.Text = Convert.ToString(Math.Round(wp_depth2, 2, MidpointRounding.AwayFromZero));

                    //////焊高
                    label22.Text = Convert.ToString(Math.Round(wp_high, 2, MidpointRounding.AwayFromZero));
                    label14.Text = Convert.ToString(Math.Round(wp_high, 2, MidpointRounding.AwayFromZero));
                }
                #endregion


                #region///搭接焊缝的画图
                if (comboBox2.SelectedIndex == 3)
                {

                }
                #endregion


                #region////对接焊缝画图
                if (comboBox2.SelectedIndex == 4)
                {                    
                    label14.Visible = false;
                    label16.Visible = false;
                    label19.Visible = false;
                    label22.Visible = false;

                    double data_x1, data_y1;
                    pane4.ReverseTransform(mousePt, out data_x1, out data_y1);
                    for (int i = 0; i < original_num; i++)
                    {
                        x11 = i * longth / (original_num - 1);
                        if (data_x1 - x11 < longth / (original_num - 1))
                        {
                            if (data_x1 - x11 < (i + 1) * longth / (original_num - 1) - data_x1)
                            {
                                x11 = i * longth / (original_num - 1);
                                wp_depth1 = rongshenzhi1[i];
                                wp_width1 = hankuan1[i];
                                wp_width2 = hankuan2[i];
                            }
                            else
                            {
                                x11 = (i + 1) * longth / (original_num - 1);
                                wp_depth1 = rongshenzhi1[i + 1];
                                wp_width1 = hankuan1[i + 1];
                                wp_width2 = hankuan2[i + 1];
                            }
                            break;
                        }
                    }

                    float con_ratio = 0;
                    con_ratio = 220 / 50 * 2;
                    this.pictureBox1.Refresh();
                    Graphics g = this.pictureBox1.CreateGraphics();

                    Pen pen1 = new Pen(Color.Blue, 2);
                    Pen pen2 = new Pen(Color.Red, 3);
                    Pen pen3 = new Pen(Color.Green, 2);
                    Pen pen4 = new Pen(Color.LimeGreen, 1);
                    ////////////////////////////////////////
                    g.DrawLine(pen1, 30, 130, 250, 130);///画矩形
                    g.DrawLine(pen1, 30, 130, 30, 70);
                    g.DrawLine(pen1, 30, 70, 250, 70);
                    g.DrawLine(pen1, 250, 70, 250, 130);

                    double[] fit_depth = new double[edge_longth ];
                    for (int j = 0; j < edge_longth ; j++)
                    {
                        fit_depth[j] = nihe1[r, j];
                    }
                    double[] nh_x = new double[edge_longth ];
                    double[] nh_y = new double[edge_longth ];///7个拟合点的坐标值
                    for (int j = 0; j < edge_longth ; j++)    ///拟合点在焊缝斜边上投影对应的坐标值
                    {
                        nh_x[j] = 130 + j * 1 * con_ratio;
                        nh_y[j] = 70 + fit_depth[j] * con_ratio;
                    }
                    double nh_x1_x = 0;
                    double nh_x1_y = 0;
                    double nh_x2_x = 0;
                    double nh_x2_y = 0;////一元二次方程与坐标轴的交点
                    double zd_x;

                    ////////////////////////////////////////////////////////////////////////////////熔核形状的描绘
                    if (edge_longth == 7)
                    {
                        nh_x1_x = nh_x[3] - wp_width1 * con_ratio;
                        nh_x1_y = 70;
                        nh_x2_x = nh_x[3] + wp_width2 * con_ratio;
                        nh_x2_y = 70;
                        zd_x = nh_x[3];
                        Point[] p1 = new Point[]
                         {    
                            new Point((int)(nh_x1_x),(int)(nh_x1_y)),
                            new Point((int)(nh_x[0]),(int)(nh_y [0])),
                            new Point((int)(nh_x[1]),(int)(nh_y [1])),
                            new Point((int)(nh_x[2]),(int)(nh_y [2])),
                            new Point((int)(nh_x[3]),(int)(nh_y [3])),
                            new Point((int)(nh_x[4]),(int)(nh_y [4])),
                            new Point((int)(nh_x[5]),(int)(nh_y [5])),
                            new Point((int)(nh_x[6]),(int)(nh_y [6])),
                            new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1); ///画了焊缝的形状                     
                    }
                    else if (edge_longth == 5)
                    {
                        nh_x1_x = nh_x[2] - wp_width1 * con_ratio;
                        nh_x1_y = 70;
                        nh_x2_x = nh_x[2] + wp_width2 * con_ratio;
                        nh_x2_y = 70;
                        zd_x = nh_x[2];
                        Point[] p1 = new Point[]
                        {    
                            new Point((int)(nh_x1_x),(int)(nh_x1_y)),    
                            new Point((int)(nh_x[0]),(int)(nh_y [0])),
                            new Point((int)(nh_x[1]),(int)(nh_y [1])),
                            new Point((int)(nh_x[2]),(int)(nh_y [2])),
                            new Point((int)(nh_x[3]),(int)(nh_y [3])),
                            new Point((int)(nh_x[4]),(int)(nh_y [4])),   
                            new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                        };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    else
                    {
                        nh_x1_x = nh_x[1] - wp_width1 * con_ratio;
                        nh_x1_y = 70;
                        nh_x2_x = nh_x[1] + wp_width2 * con_ratio;
                        nh_x2_y = 70;
                        zd_x = nh_x[1];
                        Point[] p1 = new Point[]
                         {    
                            new Point((int)(nh_x1_x),(int)(nh_x1_y)),                             
                            new Point((int)(nh_x[0]),(int)(nh_y [0])),
                            new Point((int)(nh_x[1]),(int)(nh_y [1])),
                            new Point((int)(nh_x[2]),(int)(nh_y [2])),                             
                            new Point((int)(nh_x2_x),(int)(nh_x2_y)),
                         };
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen3, p1);
                    }
                    ////////////////////////////////////////////////////////
                    g.DrawLine(pen4, 90, (int)(70 + wp_depth1 * con_ratio), 150, (int)(70 + wp_depth1 * con_ratio));////熔深标注
                    g.DrawLine(pen4, 100, 70, 100, 60);////熔深标注
                    g.DrawLine(pen4, 100, 60, 70, 60);////熔深标注
                    g.DrawLine(pen4, (int)(zd_x ), 90, (int)(zd_x ), 150);///焊宽标注
                    g.DrawLine(pen4, (int)(nh_x1_x), 70, (int)(nh_x1_x), 150);///焊宽标注
                    g.DrawLine(pen4, (int)(nh_x2_x), 70, (int)(nh_x2_x), 150);///焊宽标注
                    g.DrawLine(pen4, (int)(nh_x1_x), 145, 80, 145);///焊宽标注
                    g.DrawLine(pen4, (int)(nh_x2_x), 145, 230, 145);///焊宽标注
                    pen4.StartCap = LineCap.ArrowAnchor;//定义线头的样式为箭头
                    pen4.EndCap = LineCap.ArrowAnchor;//定义线尾的样式为箭头 

                    g.DrawLine(pen4, 100, 70, 100, (int)(70 + wp_depth1 * con_ratio));
                    label12.Location = new Point(220 + 30, 240 + 45);////熔深文本框位置
                    label12.Text = Convert.ToString(Math.Round(wp_depth1, 2, MidpointRounding.AwayFromZero));///熔深尺寸
                    label21.Text = Convert.ToString(Math.Round(wp_depth1, 2, MidpointRounding.AwayFromZero));///熔深尺寸
                                                                                                             ///
                    g.DrawLine(pen4, (int)(nh_x[3]), 145, (int)(nh_x1_x), 145);///焊宽标注  
                    g.DrawLine(pen4, (int)(nh_x[3]), 145, (int)(nh_x2_x), 145);///焊宽标注
                    label13.Location = new Point(220 + 40, 240 + 130);////文本框位置 
                    label15.Location = new Point(220 + 160, 240 + 130);////文本框位置
                    label13.Text = Convert.ToString(Math.Round(wp_width1, 2, MidpointRounding.AwayFromZero));///焊宽尺寸
                    label15.Text = Convert.ToString(Math.Round(wp_width2, 2, MidpointRounding.AwayFromZero));///焊宽尺寸
                    label20.Text = Convert.ToString(Math.Round(wp_width1, 2, MidpointRounding.AwayFromZero)) + "  " + Convert.ToString(Math.Round(wp_width2, 2, MidpointRounding.AwayFromZero));
                }
                #endregion
            }
        }
        #endregion
        #region  保存采集数据
        string pathtxt;
        string timestr;
        FileStream fs;
        StreamWriter swt;
        private void SaveButton_Click_1(object sender, EventArgs e)
        {
            if (NoTextBox.Text == "")
            {
                MessageBox.Show("请输入工件编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (DistanceText.Text == "")
            {
                MessageBox.Show("请输入测量长度！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            timestr = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
            pathtxt = Application.StartupPath + "\\" + "datafile" + "\\" + NoTextBox.Text + "# at " + timestr + "数据" + ".txt";
            fs = File.Open(pathtxt, FileMode.Create, FileAccess.Write);
            swt = new StreamWriter(fs, Encoding.Default);
            string txt;
            txt = "工件号：" + " " + NoTextBox.Text;
            swt.WriteLine(txt);
            timestr = DateTime.Now.ToString();
            txt = "检测时间：" + " " + timestr;
            swt.WriteLine(txt);
            txt = "测量长度：" + " " + DistanceText.Text + " ";
            swt.WriteLine(txt);
            //txt = "焊缝种类：" + " " + comboBox1.SelectedItem.ToString() + " ";
            //swt.WriteLine(txt);
            for (int i = 0; i < original_num; i++)  //第一个数据不需要
            {
                swt.Write(i);  //采点序号
                swt.Write(',');
                //原始磁场值
                swt.Write(string.Format("{0:0.0}", data[0, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[1, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[2, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[3, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[4, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[5, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[6, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[7, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[8, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[9, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[10, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[11, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[12, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[13, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[14, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[15, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[16, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[17, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[18, i]));
                swt.Write(',');
                swt.Write(string.Format("{0:0.0}", data[19, i]));
                swt.Write("\r\n");
            }
            swt.Flush();
            swt.Close();
            fs.Close();
        }
        #endregion
        #region（清空text，退出窗体程序，串口选择，定时时钟）

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();  //本窗体退出
            Application.Exit(); //程序退出
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            serialPort1.PortName = comboBox1.Text;


        }
        private void timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTime.Now.ToString();
        }
        #endregion
        #region   (预览,读取数据)
        private void browseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "文本文件 (*.txt)|*.txt";
                open.FilterIndex = 1;
                open.RestoreDirectory = true;
                open.Title = "打开数据文件";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    FilePath = open.FileName;
                    readdata();

                }
            }
            runButton.Enabled = true;
        }
        //读以前保存的数据
        private void readdata()
        {
            System.IO.StreamReader sr_ScatterData = new System.IO.StreamReader(FilePath, UnicodeEncoding.GetEncoding("GB2312"));
            string temp_str = sr_ScatterData.ReadLine();//第一行 工件号
            temp_str.TrimStart();
            temp_str.TrimEnd();
            string[] tempData_str = temp_str.Split(' ').ToArray();
            NoTextBox.Text = tempData_str[1];
            sr_ScatterData.ReadLine();   //第二行 检测时间
            temp_str = sr_ScatterData.ReadLine();//第三行工件参数  
            temp_str.TrimStart();
            temp_str.TrimEnd();
            tempData_str = temp_str.Split(' ').ToArray();
            DistanceText.Text = tempData_str[1];
            longth = double.Parse(tempData_str[1]);

            //temp_str = sr_ScatterData.ReadLine();//第四行工件参数  
            //temp_str.TrimStart();
            //temp_str.TrimEnd();
            //tempData_str = temp_str.Split(' ').ToArray();

            long k = 0;
            while (!sr_ScatterData.EndOfStream)
            {
                temp_str = sr_ScatterData.ReadLine();
                temp_str.TrimStart();
                temp_str.TrimEnd();
                tempData_str = temp_str.Split(',').ToArray();

                if (tempData_str.Count() == 21)
                {
                    data[0, k] = double.Parse(tempData_str[1]);
                    data[1, k] = double.Parse(tempData_str[2]);
                    data[2, k] = double.Parse(tempData_str[3]);
                    data[3, k] = double.Parse(tempData_str[4]);
                    data[4, k] = double.Parse(tempData_str[5]);
                    data[5, k] = double.Parse(tempData_str[6]);
                    data[6, k] = double.Parse(tempData_str[7]);
                    data[7, k] = double.Parse(tempData_str[8]);
                    data[8, k] = double.Parse(tempData_str[9]);
                    data[9, k] = double.Parse(tempData_str[10]);
                    data[10, k] = double.Parse(tempData_str[11]);
                    data[11, k] = double.Parse(tempData_str[12]);
                    data[12, k] = double.Parse(tempData_str[13]);
                    data[13, k] = double.Parse(tempData_str[14]);
                    data[14, k] = double.Parse(tempData_str[15]);
                    data[15, k] = double.Parse(tempData_str[16]);
                    data[16, k] = double.Parse(tempData_str[17]);
                    data[17, k] = double.Parse(tempData_str[18]);
                    data[18, k] = double.Parse(tempData_str[19]);
                    data[19, k] = double.Parse(tempData_str[20]);
                    k++;
                }
            }
            original_num = k;
            sr_ScatterData.Close();
        }
        #endregion
        #region （实时曲线的画图）
        private void ZedGraph1Init()
        {
            //获取引用
            GraphPane myPane = zedGraphControl1.GraphPane;

            //清空原图像
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();
            zedGraphControl1.Refresh();

            //设置标题
            myPane.Title.Text = "实时曲线";
            //设置X轴说明文字
            myPane.XAxis.Title.Text = "点数";
            //设置Y轴说明文字
            myPane.YAxis.Title.Text = "磁场梯度（nT）";

            myPane.XAxis.MajorGrid.IsVisible = true;//底色画网格
            myPane.XAxis.MajorGrid.Color = Color.Green;
            myPane.XAxis.MinorGrid.IsVisible = true;
            myPane.XAxis.MinorGrid.Color = Color.Green;

            myPane.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane.XAxis.Scale.Max = 150;	//X轴最大100
            myPane.XAxis.Scale.MinorStep = 1;//X轴小步长1,也就是小间隔
            myPane.XAxis.Scale.MajorStep = 5;//X轴大步长为5，也就是显示文字的大间隔

            myPane.AxisChange();
        }
        private void ZedGraph1Addcurve()//添加图例
        {
            // 清除原图例
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.Refresh();
            zedGraphControl1.GraphPane.XAxis.Scale.Min = 0;
            zedGraphControl1.GraphPane.XAxis.Scale.Max = 150;
            if (chflag[0] == true)
            {
                PointPairList list0 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH1", list0, Color.Blue, SymbolType.None);
            }
            if (chflag[1] == true)
            {
                PointPairList list1 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH2", list1, Color.Red, SymbolType.None);
            }
            if (chflag[2] == true)
            {
                PointPairList list2 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH3", list2, Color.Brown, SymbolType.None);
            }
            if (chflag[3] == true)
            {
                PointPairList list3 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH4", list3, Color.Black, SymbolType.None);
            }
            if (chflag[4] == true)
            {
                PointPairList list4 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH5", list4, Color.Green, SymbolType.None);
            }
            if (chflag[5] == true)
            {
                PointPairList list5 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH6", list5, Color.Gray, SymbolType.None);
            }
            if (chflag[6] == true)
            {
                PointPairList list6 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH7", list6, Color.Yellow, SymbolType.None);
            }
            if (chflag[7] == true)
            {
                PointPairList list7 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH8", list7, Color.Purple, SymbolType.None);
            }
            if (chflag[8] == true)
            {
                PointPairList list8 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH9", list8, Color.Olive, SymbolType.None);
            }
            if (chflag[9] == true)
            {
                PointPairList list9 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH10", list9, Color.Gold, SymbolType.None);
            }
            if (chflag[10] == true)
            {
                PointPairList list10 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH11", list10, Color.GreenYellow, SymbolType.None);
            }
            if (chflag[11] == true)
            {
                PointPairList list11 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH12", list11, Color.DeepPink, SymbolType.None);
            }
            if (chflag[12] == true)
            {
                PointPairList list12 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH13", list12, Color.PowderBlue, SymbolType.None);
            }
            if (chflag[13] == true)
            {
                PointPairList list13 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH14", list13, Color.SeaGreen, SymbolType.None);
            }
            if (chflag[14] == true)
            {
                PointPairList list14 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH15", list14, Color.SandyBrown, SymbolType.None);
            }
            if (chflag[15] == true)
            {
                PointPairList list15 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH16", list15, Color.SpringGreen, SymbolType.None);
            }
            if (chflag[16] == true)
            {
                PointPairList list16 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH17", list16, Color.Tan, SymbolType.None);
            }
            if (chflag[17] == true)
            {
                PointPairList list17 = new ZedGraph.PointPairList();
                LineItem curve1 = zedGraphControl1.GraphPane.AddCurve("CH18", list17, Color.Teal, SymbolType.None);
            }


        }
        private void ZedGraph1Adddata()
        {
            zedGraphControl1.GraphPane.XAxis.Scale.Min = 0;
            zedGraphControl1.GraphPane.XAxis.Scale.Max = 150;
            int i = 0;

            if (chflag[0] == true)
            {
                LineItem curve0 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list0 = curve0.Points as IPointListEdit;
                list0.Add(displayrow, data[0, displayrow]);
                i++;
            }
            if (chflag[1] == true)
            {
                LineItem curve1 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list1 = curve1.Points as IPointListEdit;
                list1.Add(displayrow, data[1, displayrow]);
                i++;
            }
            if (chflag[2] == true)
            {
                LineItem curve2 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list2 = curve2.Points as IPointListEdit;
                list2.Add(displayrow, data[2, displayrow]);
                i++;
            }
            if (chflag[3] == true)
            {
                LineItem curve3 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list3 = curve3.Points as IPointListEdit;
                list3.Add(displayrow, data[3, displayrow]);
                i++;
            }
            if (chflag[4] == true)
            {
                LineItem curve4 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list4 = curve4.Points as IPointListEdit;
                list4.Add(displayrow, data[4, displayrow]);
                i++;
            }
            if (chflag[5] == true)
            {
                LineItem curve5 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list5 = curve5.Points as IPointListEdit;
                list5.Add(displayrow, data[5, displayrow]);
                i++;
            }
            if (chflag[6] == true)
            {
                LineItem curve6 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list6 = curve6.Points as IPointListEdit;
                list6.Add(displayrow, data[6, displayrow]);
                i++;
            }
            if (chflag[7] == true)
            {
                LineItem curve7 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list7 = curve7.Points as IPointListEdit;
                list7.Add(displayrow, data[7, displayrow]);
                i++;
            }
            if (chflag[8] == true)
            {
                LineItem curve8 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list8 = curve8.Points as IPointListEdit;
                list8.Add(displayrow, data[8, displayrow]);
                i++;
            }
            if (chflag[9] == true)
            {
                LineItem curve9 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list9 = curve9.Points as IPointListEdit;
                list9.Add(displayrow, data[9, displayrow]);
                i++;
            }
            if (chflag[10] == true)
            {
                LineItem curve10 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list10 = curve10.Points as IPointListEdit;
                list10.Add(displayrow, data[10, displayrow]);
                i++;
            }
            if (chflag[11] == true)
            {
                LineItem curve11 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list11 = curve11.Points as IPointListEdit;
                list11.Add(displayrow, data[11, displayrow]);
                i++;
            }
            if (chflag[12] == true)
            {
                LineItem curve12 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list12 = curve12.Points as IPointListEdit;
                list12.Add(displayrow, data[12, displayrow]);
                i++;
            }
            if (chflag[13] == true)
            {
                LineItem curve13 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list13 = curve13.Points as IPointListEdit;
                list13.Add(displayrow, data[13, displayrow]);
                i++;
            }
            if (chflag[14] == true)
            {
                LineItem curve14 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list14 = curve14.Points as IPointListEdit;
                list14.Add(displayrow, data[14, displayrow]);
                i++;
            }
            if (chflag[15] == true)
            {
                LineItem curve15 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list15 = curve15.Points as IPointListEdit;
                list15.Add(displayrow, data[15, displayrow]);
                i++;
            }
            if (chflag[16] == true)
            {
                LineItem curve16 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list16 = curve16.Points as IPointListEdit;
                list16.Add(displayrow, data[16, displayrow]);
                i++;
            }
            if (chflag[17] == true)
            {
                LineItem curve17 = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                IPointListEdit list17 = curve17.Points as IPointListEdit;
                list17.Add(displayrow, data[17, displayrow]);
                i++;
            }

            
            displayrow++;
            original_num = displayrow;

            Scale xScale = zedGraphControl1.GraphPane.XAxis.Scale;
            if (displayrow > xScale.Max - xScale.MajorStep)
            {
                xScale.Max = displayrow + xScale.MajorStep;
                xScale.Min = xScale.Max - 150;
            }
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

        }


        #endregion
        #region（显示通道选择）

        private void selectokButton_Click_1(object sender, EventArgs e)
        {
            chselectokflag = false;
            chflag[0] = true;
            chflag[1] = true;
            chflag[2] = true;
            chflag[3] = true;
            chflag[4] = true;
            chflag[5] = true;
            chflag[6] = true;
            chflag[7] = true;
            chflag[8] = true;
            chflag[9] = true;
            chflag[10] = true;
            chflag[11] = true;
            chflag[12] = true;
            chflag[13] = true;
            chflag[14] = true;
            chflag[15] = true;
            chflag[16] = true;
            chflag[17] = true;

            ZedGraph1Init();
            ZedGraph1Addcurve();

            ////////////////////////////////////////////////////////////////////////////////////////////////
            //点击确定按钮后对数据初始化
            displayrow = 0;
            i = 0;
            l = 0;
            n = 0;
            Array.Clear(data2, 0, data2.Length);
            Array.Clear(data, 0, data.Length);
            label20.Text = "";
            label13.Text = "";
            label15.Text = "";

            label21.Text = "";
            label12.Text = "";
            label16.Text = "";

            label22.Text = "";
            label14.Text = "";
           

            label27.Text = "";
        }

        #endregion
        #region(显示虚拟键盘)
        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("osk.exe");
        }
        #endregion
        #region(拟合图像的画线)
        private void draw_nihequxian()
        {
            //获取引用
            GraphPane myPane2 = zedGraphControl3.GraphPane;
            double x = 0;
            string CHname;


            //设置标题
            myPane2.Title.Text = "拟合图像";
            //设置X轴说明文字
            myPane2.XAxis.Title.Text = "距离（mm）";
            //设置Y轴说明文字
            myPane2.YAxis.Title.Text = "深度（mm）";
            myPane2.XAxis.MajorGrid.IsVisible = true;//底色画网格
            myPane2.XAxis.MajorGrid.Color = Color.Green;
            myPane2.XAxis.MinorGrid.IsVisible = true;
            myPane2.XAxis.MinorGrid.Color = Color.Green;
            //清空原图像
            myPane2.CurveList.Clear();
            myPane2.GraphObjList.Clear();
            zedGraphControl3.Refresh();

            myPane2.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane2.XAxis.Scale.Max = longth;	 //X轴最大值  
            myPane2.XAxis.Scale.MinorStep = 1;
            myPane2.XAxis.Scale.MajorStep = 5;
            zedGraphControl3.GraphPane.XAxis.Scale.Max += 1;
            //zedGraphControl2.GraphPane.XAxis.Scale.Min += 1;
            zedGraphControl3.ScrollMaxX = displayrow;

            zedGraphControl3.ScrollMinY = -500000;
            zedGraphControl3.ScrollMaxY = 500000;

            PointPairList nlist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;
            //PointPairList nlist1 = new ZedGraph.PointPairList();
            //ZedGraph.LineItem myCurve1;
        
            for (int j = 0; j < 7; j++)
            {
                x = (float)i * longth / (original_num - 1);
                nlist0.Add(j + 2, nihe1[1, j]);
                //nlist1.Add(j + 2, nihe2[comboBox5.SelectedIndex, j]);

            }
            CHname = "CH" + string.Format("{0}", 1);
            myCurve0 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
            //CHname = "CH" + string.Format("{0}", 2);
            //myCurve1 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist1, Color.Red, ZedGraph.SymbolType.None);
            //CHname = "CH"+string.Format("{0}", 3);
            //myCurve2 = zedGraphControl3.GraphPane.AddCurve(CHname, nlist2, Color.Brown, ZedGraph.SymbolType.None);

            myPane2.AxisChange();
            zedGraphControl3.Invalidate();
        }
        #endregion
        #region（各点深度的画线）
        private void draw_gedianshendu()
        {
            //获取引用
            GraphPane myPane1 = zedGraphControl2.GraphPane;
            double x = 0;
            string CHname;

            //设置标题
            myPane1.Title.Text = "各点深度";
            //设置X轴说明文字
            myPane1.XAxis.Title.Text = "距离（mm）";
            //设置Y轴说明文字
            myPane1.YAxis.Title.Text = "深度（mm）";    
            myPane1.XAxis.MajorGrid.IsVisible = true;//底色画网格
            myPane1.XAxis.MajorGrid.Color = Color.Green;
            myPane1.XAxis.MinorGrid.IsVisible = true;
            myPane1.XAxis.MinorGrid.Color = Color.Green;

            //清空原图像
            myPane1.CurveList.Clear();
            myPane1.GraphObjList.Clear();
            zedGraphControl2.Refresh();

            myPane1.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane1.XAxis.Scale.Max = longth;	 //X轴最大值  
            myPane1.XAxis.Scale.MinorStep = 1;
            myPane1.XAxis.Scale.MajorStep = 5;
            zedGraphControl2.GraphPane.XAxis.Scale.Max += 1;
            //zedGraphControl2.GraphPane.XAxis.Scale.Min += 1;
            zedGraphControl2.ScrollMaxX = original_num;

            zedGraphControl2.ScrollMinY = -500000;
            zedGraphControl2.ScrollMaxY = 500000;

            //PointPairList nlist1 = new ZedGraph.PointPairList();
            //ZedGraph.LineItem myCurve1;
            //PointPairList nlist2 = new ZedGraph.PointPairList();
            //ZedGraph.LineItem myCurve2;

            PointPairList nlist3 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve3;
            PointPairList nlist4 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve4;

            PointPairList nlist5 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve5;
            PointPairList nlist6 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve6;

            PointPairList nlist7 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve7;
            PointPairList nlist8 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve8;

            PointPairList nlist9 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve9;
            PointPairList nlist10 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve10;

            PointPairList nlist11 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve11;
            PointPairList nlist12 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve12;

            PointPairList nlist13 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve13;
            PointPairList nlist14 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve14;

            PointPairList nlist15 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve15;
            PointPairList nlist16 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve16;

            //PointPairList nlist17 = new ZedGraph.PointPairList();
            //ZedGraph.LineItem myCurve17;
            //PointPairList nlist18 = new ZedGraph.PointPairList();
            //ZedGraph.LineItem myCurve18;

            for (int i = 0; i < original_num; i++)
            {
                x = (float)i * longth / (original_num - 1);
                //nlist1.Add(x, hanfengdepth1[i]);
                //nlist2.Add(x, hanfengdepth2[i]);

                nlist3.Add(x, hanfengdepth3[i]);
                nlist4.Add(x, hanfengdepth4[i]);

                nlist5.Add(x, hanfengdepth5[i]);
                nlist6.Add(x, hanfengdepth6[i]);

                nlist7.Add(x, hanfengdepth7[i]);
                nlist8.Add(x, hanfengdepth8[i]);

                nlist9.Add(x, hanfengdepth9[i]);
                nlist10.Add(x, hanfengdepth10[i]);

                nlist11.Add(x, hanfengdepth11[i]);
                nlist12.Add(x, hanfengdepth12[i]);

                nlist13.Add(x, hanfengdepth13[i]);
                nlist14.Add(x, hanfengdepth14[i]);

                nlist15.Add(x, hanfengdepth15[i]);
                nlist16.Add(x, hanfengdepth16[i]);

                //nlist17.Add(x, hanfengdepth17[i]);
                //nlist18.Add(x, hanfengdepth18[i]);
            }

            //CHname = "CH" + string.Format("{0}", 1);
            //myCurve1 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist1, Color.Blue, ZedGraph.SymbolType.None);
            //CHname = "CH" + string.Format("{0}", 2);
            //myCurve2 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist2, Color.Red, ZedGraph.SymbolType.None);

            //CHname = "CH" + string.Format("{0}", 3);
            //myCurve3 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist3, Color.Brown, ZedGraph.SymbolType.None);
            CHname = "CH" + string.Format("{0}", 4);
            myCurve4 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist4, Color.Maroon, ZedGraph.SymbolType.None);

            //CHname = "CH" + string.Format("{0}", 5);
            //myCurve5 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist5, Color.Green, ZedGraph.SymbolType.None);
            CHname = "CH" + string.Format("{0}", 6);
            myCurve6 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist6, Color.DarkRed, ZedGraph.SymbolType.None);

            ////CHname = "CH" + string.Format("{0}", 7);
            ////myCurve7 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist7, Color.Yellow, ZedGraph.SymbolType.None);
            CHname = "CH" + string.Format("{0}", 8);
            myCurve8 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist8, Color.Navy, ZedGraph.SymbolType.None);

            ////CHname = "CH" + string.Format("{0}", 9);
            ////myCurve9 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist9, Color.Olive, ZedGraph.SymbolType.None);
            CHname = "CH" + string.Format("{0}", 10);
            myCurve10 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist10, Color.Gold, ZedGraph.SymbolType.None);

            ////CHname = "CH" + string.Format("{0}", 11);
            ////myCurve11 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist11, Color.GreenYellow, ZedGraph.SymbolType.None);
            CHname = "CH" + string.Format("{0}", 12);
            myCurve12 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist12, Color.DeepPink, ZedGraph.SymbolType.None);

            ////////CHname = "CH" + string.Format("{0}", 13);
            ////////myCurve13 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist13, Color.PowderBlue, ZedGraph.SymbolType.None);
            CHname = "CH" + string.Format("{0}", 14);
            myCurve14 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist14, Color.PaleGreen, ZedGraph.SymbolType.None);

            //////CHname = "CH" + string.Format("{0}", 15);
            //////myCurve15 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist15, Color.SandyBrown, ZedGraph.SymbolType.None);
            CHname = "CH" + string.Format("{0}", 16);
            myCurve16 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist16, Color.SpringGreen, ZedGraph.SymbolType.None);
           
            ////CHname = "CH" + string.Format("{0}", 17);
            ////myCurve17 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist17, Color.Tan, ZedGraph.SymbolType.None);
            ////CHname = "CH" + string.Format("{0}", 18);
            ////myCurve18 = zedGraphControl2.GraphPane.AddCurve(CHname, nlist18, Color.Teal, ZedGraph.SymbolType.None);
            myPane1.AxisChange();
            zedGraphControl2.Invalidate();
        }
        #endregion
        #region(焊宽焊高的画线)
        private void draw_hankuanhangao()
        {
            //获取引用
            GraphPane myPane3 = zedGraphControl4.GraphPane;
            double x = 0;
            string CHname;

            //设置标题
            myPane3.Title.Text = "焊宽、焊高";
            //设置X轴说明文字
            myPane3.XAxis.Title.Text = "距离（mm）";
            //设置Y轴说明文字
            myPane3.YAxis.Title.Text = "深度（mm）";
            myPane3.XAxis.MajorGrid.IsVisible = true;//底色画网格
            myPane3.XAxis.MajorGrid.Color = Color.Green;
            myPane3.XAxis.MinorGrid.IsVisible = true;
            myPane3.XAxis.MinorGrid.Color = Color.Green;
            //清空原图像
            myPane3.CurveList.Clear();
            myPane3.GraphObjList.Clear();
            zedGraphControl4.Refresh();

            myPane3.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane3.XAxis.Scale.Max = longth;	 //X轴最大值  
            myPane3.XAxis.Scale.MinorStep = 1;
            myPane3.XAxis.Scale.MajorStep = 5;
            zedGraphControl4.GraphPane.XAxis.Scale.Max += 1;
            //zedGraphControl2.GraphPane.XAxis.Scale.Min += 1;
            zedGraphControl4.ScrollMaxX = original_num;

            zedGraphControl4.ScrollMinY = -500000;
            zedGraphControl4.ScrollMaxY = 500000;



            PointPairList nlist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;
            PointPairList nlist1 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve1;
            PointPairList nlist2 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve2;
            for (int i = 0; i < original_num; i++)
            {
                x = (float)i * longth / (original_num - 1);
                nlist0.Add(x,hangao1[i] );
                nlist1.Add(x,hankuan1[i]);
                nlist2.Add(x, hankuan2[i]);
               
            }

            CHname = "焊高" + string.Format("{0}", 1);
            myCurve0 = zedGraphControl4.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
            CHname = "焊宽1" + string.Format("{0}", 2);
            myCurve1 = zedGraphControl4.GraphPane.AddCurve(CHname, nlist1, Color.Red, ZedGraph.SymbolType.None);
            CHname = "焊宽2" + string.Format("{0}", 3);
            myCurve2 = zedGraphControl4.GraphPane.AddCurve(CHname, nlist2, Color.Black, ZedGraph.SymbolType.None);

            myPane3.AxisChange();
            zedGraphControl4.Invalidate();
        }
        #endregion
        #region(熔深的画线)
        private void draw_hanfengrongshen()
        {
            //获取引用
            GraphPane myPane4 = zedGraphControl5.GraphPane;
            double x = 0;
            string CHname;

            //设置标题
            myPane4.Title.Text = "熔深曲线";
            //设置X轴说明文字
            myPane4.XAxis.Title.Text = "距离（mm）";
            //设置Y轴说明文字
            myPane4.YAxis.Title.Text = "深度（mm）";
            myPane4.XAxis.MajorGrid.IsVisible = true;//底色画网格
            myPane4.XAxis.MajorGrid.Color = Color.Green;
            myPane4.XAxis.MinorGrid.IsVisible = true;
            myPane4.XAxis.MinorGrid.Color = Color.Green;
            //清空原图像
            myPane4.CurveList.Clear();
            myPane4.GraphObjList.Clear();
            zedGraphControl5.Refresh();

            myPane4.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane4.XAxis.Scale.Max = longth;	 //X轴最大值  
            myPane4.XAxis.Scale.MinorStep = 1;
            myPane4.XAxis.Scale.MajorStep = 5;
            zedGraphControl5.GraphPane.XAxis.Scale.Max += 1;
            //zedGraphControl2.GraphPane.XAxis.Scale.Min += 1;
            zedGraphControl5.ScrollMaxX = original_num;

            zedGraphControl5.ScrollMinY = -500000;
            zedGraphControl5.ScrollMaxY = 500000;



            PointPairList nlist0 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve0;
            PointPairList nlist1 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve1;

            for (int i = 0; i < original_num; i++)
            {
                x = (float)i * longth / (original_num - 1);
                nlist0.Add(x, rongshenzhi1[i]);
                nlist1.Add(x, rongshenzhi2[i]);
               
            }

            CHname = "熔深" + string.Format("{0}", 1);
            myCurve0 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist0, Color.Blue, ZedGraph.SymbolType.None);
            //CHname = "熔深" + string.Format("{0}", 2);
            //myCurve1 = zedGraphControl5.GraphPane.AddCurve(CHname, nlist1, Color.Red, ZedGraph.SymbolType.None);

            myPane4.AxisChange();
            zedGraphControl5.Invalidate();
        }
        #endregion
        #region(画滤波后的曲线，转化为实际扫描长度)
        private void draw_yuanshiquxian0()
        {
            //获取引用
            GraphPane myPane0 = zedGraphControl1.GraphPane;
            double x = 0;
            string CHname;

            //设置标题
            myPane0.Title.Text = "实时曲线";
            //设置X轴说明文字
            myPane0.XAxis.Title.Text = "点数";
            //设置Y轴说明文字
            myPane0.YAxis.Title.Text = "磁场梯度（nT）";
            myPane0.XAxis.MajorGrid.IsVisible = true;//底色画网格
            myPane0.XAxis.MajorGrid.Color = Color.Green;
            myPane0.XAxis.MinorGrid.IsVisible = true;
            myPane0.XAxis.MinorGrid.Color = Color.Green;
            
            //清空原图像
            myPane0.CurveList.Clear();
            myPane0.GraphObjList.Clear();
            zedGraphControl1.Refresh();

            myPane0.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane0.XAxis.Scale.Max = longth;	 //X轴最大值  
            myPane0.XAxis.Scale.MinorStep = 1;
            myPane0.XAxis.Scale.MajorStep = 5;
            zedGraphControl1.GraphPane.XAxis.Scale.Max += 1;
            zedGraphControl1.ScrollMaxX = original_num;

            zedGraphControl1.ScrollMinY = -500000;
            zedGraphControl1.ScrollMaxY = 500000;

            PointPairList nlist2 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve2;

            for (int i = 0; i < original_num; i++)
            {
                x = (float)i * longth / (original_num - 1);//将扫描点数转换为实际长度
                nlist2.Add(x, data_handle[comboBox4.SelectedIndex, i]);

            }

            CHname = "CH" + string.Format("{0}", comboBox4.SelectedIndex+1);
            myCurve2 = zedGraphControl1.GraphPane.AddCurve(CHname, nlist2, Color.Brown, ZedGraph.SymbolType.None);
            //CHname = "CH" + string.Format("{0}", 4);
            
            myPane0.AxisChange();
            zedGraphControl1.Invalidate();
        }
        #endregion
        #region///画原始数据曲线
        private void draw_yuanshishuju()
        {
            //获取引用
            GraphPane myPane5 = zedGraphControl6.GraphPane;
            double x = 0;
            string CHname;

            //设置标题
            myPane5.Title.Text = "原始数据图";
            //设置X轴说明文字
            myPane5.XAxis.Title.Text = "距离/mm";
            //设置Y轴说明文字
            myPane5.YAxis.Title.Text = "磁场梯度（nT）";
            myPane5.XAxis.MajorGrid.IsVisible = true;//底色画网格
            myPane5.XAxis.MajorGrid.Color = Color.Green;
            myPane5.XAxis.MinorGrid.IsVisible = true;
            myPane5.XAxis.MinorGrid.Color = Color.Green;

            //清空原图像
            myPane5.CurveList.Clear();
            myPane5.GraphObjList.Clear();
            zedGraphControl6.Refresh();

            myPane5.XAxis.Scale.Min = 0;		//X轴最小值0
            myPane5.XAxis.Scale.Max = longth;	 //X轴最大值  
            myPane5.XAxis.Scale.MinorStep = 1;
            myPane5.XAxis.Scale.MajorStep = 5;
            zedGraphControl6.GraphPane.XAxis.Scale.Max += 1;
            zedGraphControl6.ScrollMaxX = original_num;

            zedGraphControl6.ScrollMinY = -500000;
            zedGraphControl6.ScrollMaxY = 500000;

            PointPairList nlist2 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve2;
            PointPairList nlist3 = new ZedGraph.PointPairList();
            ZedGraph.LineItem myCurve3;
            for (int i = 0; i < original_num; i++)
            {
                x = (float)i * longth / (original_num - 1);//将扫描点数转换为实际长度
                nlist2.Add(x, data[comboBox4.SelectedIndex, i]);
                nlist3.Add(x, data[comboBox4.SelectedIndex + 1, i]);
            }

            CHname = "CH" + string.Format("{0}", comboBox4.SelectedIndex + 1);
            myCurve2 = zedGraphControl6.GraphPane.AddCurve(CHname, nlist2, Color.Brown, ZedGraph.SymbolType.None);
            CHname = "CH" + string.Format("{0}", comboBox4.SelectedIndex + 2);
            myCurve3 = zedGraphControl6.GraphPane.AddCurve(CHname, nlist3, Color.Red, ZedGraph.SymbolType.None);
            
            myPane5.AxisChange();
            zedGraphControl6.Invalidate();
        }
        #endregion
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "钝角焊缝" || comboBox2.Text == "锐角焊缝")
            {
                label2.Visible = true;
                textBox1.Visible = true;
            }
            else
            {
                label2.Visible = false;
                textBox1.Visible = false;
            }

        }
        #region///通道选择
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)//选择通道
        {
            draw_yuanshiquxian0();
            draw_nihequxian();
            draw_gedianshendu();
            draw_hankuanhangao();
            draw_hanfengrongshen();
            draw_yuanshishuju();
        }
        #endregion

        
        
      
    }
}


