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
        public EndPoint printerEndPoint;
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
        {   // UdpClient для получения данных
            UdpClient receiver = new UdpClient(
                Convert.ToInt16("192.168.1.1"));
            // адрес входящего подключения
            IPEndPoint remoteIp = null;
            try
            {
                //бесконечно читываем, пока не получим сообщение
                while (true)
                {
                    byte[] data =
                        receiver.Receive(
                           new IPEndPoint(ref IPAddress.Parse(tbIP.Text),
                    Convert.ToInt32(tbPort.Text))
                            ); // получаем данные
                               //преобразовываем UTF8 строку в нормальную.
                    String ^ message = System::Text::Encoding::UTF8->GetString(data);
                    //Пробуем прописать текст, но эьто не работает пока что.
                    //this->Invoke((MethodInvoker) {
                    //	this->tbChat->Text = message;
                    tbChat->Text += message + '\n';
                    //MessageBox::Show(message + '\n');

                    //SafeDel^ safedel = gcnew SafeDel(&Messenger::SafeTBChatAdd);

                }
            }
            catch (Exception^ex)
			{
                MessageBox::Show("Receeve error\n" + ex->Message);
            }

            finally
            {
                receiver->Close();
            }
        }

        private void bCSVImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (Convert.ToBoolean(openFileDialog.ShowDialog()))
            {
                CSVString = new StreamReader(openFileDialog.FileName).ReadToEnd();
            }

        }
        private void SendMessage(string message)
        {
            TcpClient tcpClient = new TcpClient();

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
