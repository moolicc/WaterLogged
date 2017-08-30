using System.Text;
using System.Net;
using System.Net.Sockets;

using Socket = System.Net.Sockets.TcpClient;

namespace WaterLogged.Listeners
{
    public class TcpClientOut : Listener
    {
        public Encoding Encoding { get; set; }
        public string MessageEnding { get; set; }
        public string HostAddress { get; private set; }
        public int Port { get; private set; }

        private Socket _client;
        private NetworkStream _stream;


        public TcpClientOut()
            : this(Encoding.ASCII)
        {
            
        }

        public TcpClientOut(Encoding encoding)
        {
            Encoding = encoding;
            MessageEnding = ((char)0x03).ToString();
        }

        public void Connect(string hostAddress, int port)
        {
            _client = new Socket();
            _client.ConnectAsync(IPAddress.Parse(hostAddress), port);

            HostAddress = hostAddress;
            Port = port;
            _stream = _client.GetStream();
        }

        public void Disconnect()
        {
        }


        public override void Write(string value, string tag)
        {
            var buffer = Encoding.GetBytes(value + MessageEnding);
            _stream.Write(buffer, 0, buffer.Length);
        }
    }
}
