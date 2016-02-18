using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace RaviaPC
{
    class ServerSocket
    {
        private byte[] messageBufferIn = new byte[5];
        private byte[] messageBufferOut = new byte[10];
        Socket socket;

        public delegate void SendEventHandler(ServerSocket sender, int sent);
        public event SendEventHandler onSend;

        public delegate void ReceiveEventHandler(ServerSocket sender, string msg);
        public event ReceiveEventHandler onReceive;

        public ServerSocket(Socket s)
        {
            socket = s;
        }
        
        public bool connected
        {
            get
            {
                if (socket != null)
                {
                    return socket.Connected;
                }
                return false;
            }
        }

        public IPEndPoint endPoint
        {
            get
            {
                if (socket != null && socket.Connected)
                {
                    return (IPEndPoint)socket.RemoteEndPoint;
                }
                return new IPEndPoint(IPAddress.None, 0);
            }
        }

        public void Send(byte[] data, int index, int length)
        {
            socket.BeginSend(BitConverter.GetBytes(length), 0, 4, SocketFlags.None, SendCallback, null);
            socket.BeginSend(data, index, length, SocketFlags.None, SendCallback, null);
        }

        void SendCallback(IAsyncResult ar)
        {
            try
            {
                int sent = socket.EndSend(ar);

                if (onSend != null)
                {
                    onSend(this, sent);
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
        }

        public void ReceiveMessage()
        {
            socket.BeginReceive(messageBufferIn, 0, messageBufferIn.Length, SocketFlags.None, ReceiveCallbackIn, null);
        }

        void ReceiveCallbackIn(IAsyncResult ar)
        {
            try
            {
                int rec = socket.EndReceive(ar);
                string aMessage = Encoding.ASCII.GetString(messageBufferIn, 0, rec);

                if (onReceive != null)
                {
                    onReceive(this, aMessage);
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch(Exception ex) {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
        }

        public void ReceiveKeyboardAndMouseMessage()
        {
            socket.BeginReceive(messageBufferOut, 0, messageBufferOut.Length, SocketFlags.None, ReceiveCallbackOut, null);
        }

        void ReceiveCallbackOut(IAsyncResult ar)
        {
            try
            {
                int rec = socket.EndReceive(ar);
                string aMessage = Encoding.ASCII.GetString(messageBufferOut, 0, rec);

                if (onReceive != null)
                {
                    onReceive(this, aMessage);
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
        }

        public void Close()
        {
            if (socket != null)
            {
                socket.Disconnect(false);
                socket.Close();
            }

            socket = null;
        }
    }
}