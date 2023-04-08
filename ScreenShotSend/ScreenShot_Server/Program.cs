using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;





 var _udpClient = new UdpClient(27001);
var _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

Console.WriteLine("Screenshot Server Started...");

while (true)
{
    Console.WriteLine("Waiting Conection...");
    var data = _udpClient.Receive(ref _remoteEndPoint);
    var command = Encoding.ASCII.GetString(data);

    if (command == "screenshot")
    {
        var screenshotData = GetScreenshot();
        Console.WriteLine($"Screenshot Requested from IP: {_remoteEndPoint.Address}");
        SendData(screenshotData);
    }
}


static byte[] GetScreenshot()
{

    const int SM_CXSCREEN = 0;
    const int SM_CYSCREEN = 1;

    var screenWidth = GetSystemMetrics(SM_CXSCREEN);
    var screenHeight = GetSystemMetrics(SM_CYSCREEN);


    using (var bitmap = new Bitmap(screenWidth,screenHeight))
    {
        using (var g = Graphics.FromImage(bitmap))
        {
            g.CopyFromScreen(Point.Empty, Point.Empty, new Size(screenWidth,screenHeight));
        }
        using (var memoryStream = new MemoryStream())
        {
            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            return memoryStream.ToArray();
        }
    }
}

void SendData(byte[] data)
{
    const int bufferSize = 1024;
    var totalPackets = (int)Math.Ceiling((double)data.Length / bufferSize);

    for (var i = 0; i < totalPackets; i++)
    {
        var packetSize = Math.Min(bufferSize, data.Length - i * bufferSize);
        var packetData = new byte[packetSize];
        Array.Copy(data, i * bufferSize, packetData, 0, packetSize);
        _udpClient.Send(packetData, packetSize, _remoteEndPoint);
        Thread.Sleep(10); 
    }
}

[DllImport("user32.dll")]
static extern int GetSystemMetrics(int nIndex);










