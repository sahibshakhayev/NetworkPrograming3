using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ScreenShot_Client
{

    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UdpClient _udpClient;
        public MainWindow()
        {
            InitializeComponent();
            _udpClient = new UdpClient();
        }

        private void GetSCBt_Click(object sender)
        {
        
        
    }

        private  async void GetSCBt_Click_1(object sender, RoutedEventArgs e)
        {
            var serverEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);


            var commandData = Encoding.ASCII.GetBytes("screenshot");

            await _udpClient.SendAsync(commandData, commandData.Length, serverEndpoint);

            var imageData = await ReceiveData();
            var imageSource = ConvertToImageSource(imageData);
            Screenshot.Source = imageSource;
        }

        private async Task<byte[]> ReceiveData()
        {
            const int bufferSize = 1024;
            var buffer = new byte[bufferSize];
            var receivedData = new byte[0];

            while (true)
            {
                var result = await _udpClient.ReceiveAsync();
                var packetData = result.Buffer;
                receivedData = receivedData.Concat(packetData).ToArray();
                if (packetData.Length < bufferSize)
                {
                    break;
                }
            }

            return receivedData;
        }

        private BitmapImage ConvertToImageSource(byte[] imageData)
        {
            using (var memoryStream = new MemoryStream(imageData))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }




    }
    }

