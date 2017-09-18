using System.Text;
using System.Net;
using System.Net.Sockets;

using Socket = System.Net.Sockets.TcpClient;

namespace WaterLogged.Listeners
{
    /// <summary>
    /// Implements a listener which spits out messages to a remote TCP Listener.
    /// </summary>
    public class TcpClientOut : Listener
    {
        /// <summary>
        /// The text encoding to use when sending messages.
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// A string to append onto messages when sending to signify the end of a transmission.
        /// </summary>
        public string MessageEnding { get; set; }
        /// <summary>
        /// The host's IP Address.
        /// </summary>
        public string HostAddress { get; private set; }
        /// <summary>
        /// The port on which to establish a connection.
        /// </summary>
        public int Port { get; private set; }

        private Socket _client;
        private NetworkStream _stream;
        
        /// <summary>
        /// Instantiates a TcpClientOut using ASCII text encoding.
        /// </summary>
        public TcpClientOut()
            : this(Encoding.ASCII)
        {
            
        }

        /// <summary>
        /// Instantiates a TcpClientOut using the specified text encoding.
        /// </summary>
        /// <param name="encoding">The text encoding to use.</param>
        public TcpClientOut(Encoding encoding)
        {
            Encoding = encoding;
            MessageEnding = ((char)0x03).ToString();
        }

        /// <summary>
        /// Attempts to connect to the remote host.
        /// </summary>
        /// <param name="hostAddress">The IP Address of the host.</param>
        /// <param name="port">The port to connect over.</param>
        /// <param name="asyncConnect">A value indicating if the function should hang until a connection is made.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void Connect(string hostAddress, int port, bool asyncConnect = false)
        {
            _client = new Socket();
            if (asyncConnect)
            {
                _client.ConnectAsync(IPAddress.Parse(hostAddress), port);
            }
            else
            {
                _client.ConnectAsync(IPAddress.Parse(hostAddress), port).Wait();
            }

            HostAddress = hostAddress;
            Port = port;
            _stream = _client.GetStream();
        }

        /// <summary>
        /// Closes out connections to the remote host.
        /// </summary>
        public void Disconnect()
        {
            _stream.Dispose();
            _client.Dispose();
        }

        /// <summary>
        /// Writes output over the TCP stream.
        /// </summary>
        /// <param name="value">The message to send.</param>
        /// <param name="tag">The tag associated with the message.</param>
        public override void Write(string value, string tag)
        {
            if (_stream == null || _client == null || !_client.Connected)
            {
                return;
            }
            var buffer = Encoding.GetBytes(value + MessageEnding);
            _stream.Write(buffer, 0, buffer.Length);
        }
    }
}
