using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //для хранения данных из CSVString файла
        string CSVString;

        public IPEndPoint printerEndPoint;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void explander1_Collapsed(object sender, RoutedEventArgs e)
        {

        }



        private void tbIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)(\\.(?!$)|$)){4}");
            if (regex.Match(tbIP.Text).Success)
            {
                tbIP.Background = Brushes.LightGreen;
            }
            else
            {
                tbIP.Background = Brushes.IndianRed;
            }
        }

        private void bConnect_Click(object sender, RoutedEventArgs e)
        {
            //Получение устройств в сети
            //NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            //List<IPInterfaceProperties> iPInterfaceProperties = new List<IPInterfaceProperties>();
            //foreach (NetworkInterface networkInterface in networkInterfaces)
            //{
            //    iPInterfaceProperties.Add(networkInterface.GetIPProperties());



            //}
            printerEndPoint = new IPEndPoint(IPAddress.Parse(tbIP.Text),
                    Convert.ToInt32(tbPort.Text));


        }
        void ReceiveMessage()
        {   
            
        }

        private void bCSVImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (Convert.ToBoolean(openFileDialog.ShowDialog()))
            {
                CSVString = new StreamReader(openFileDialog.FileName).ReadToEnd();
            }

        }
        private async void SendMessage(string message)
        {

            TcpClient tcpClient = new TcpClient(printerEndPoint);
            try
            {
                tcpClient.ConnectAsync(IPAddress.Parse(tbIP.Text),Convert.ToInt32(tbPort.Text));
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Подключение установлено.");
            NetworkStream networkStream = tcpClient.GetStream();
            StreamWriter streamWriter = new StreamWriter(networkStream);
            StreamReader streamReader = new StreamReader(networkStream);
            streamWriter.AutoFlush = true;//Автоматически очищать буфер
            for (int i = 0; i < 10; i++)
            {
                
                await streamWriter.WriteLineAsync(DateTime.Now.ToLongDateString());
                
                string dataFromServer = await streamReader.ReadLineAsync();
                if (!string.IsNullOrEmpty(dataFromServer))
                {
                    MessageBox.Show(dataFromServer);
                }

            }

        }
        private void bPrint_Click(object sender, RoutedEventArgs e)
        {
            if (CSVString != null)
            {
                try
                {

                    //адрес для подключения
                    string remoteAddress = tbIP.Text;
                    // порт, к которому мы подключаемся
                    int remotePort = Convert.ToInt16(tbPort.Text);


                    SendMessage("sad"); // отправляем сообщение
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
