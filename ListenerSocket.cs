using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace RaviaPC
{
    class ListenerSocket
    {
        public delegate void SocketAcceptedHandler(Socket e);
        public event SocketAcceptedHandler accepted;

        Socket listener;

        public bool running
        {
            get;
            private set;
        }

        public void Start(int port)
        {
            if (running)
                return;
            // Utworzenie lokalnego punktu końcowego dla gniazda.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            // Stworzenie gniazda TCP/IP.
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Powiązanie gniazda z lokalnym punktem końcowym (localEndPoint) i nasłuchiwanie połączeń przychodzących.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);  // Parametr metody Listen to maksymalna ilość połączeń oczekujących.

                listener.BeginAccept(AcceptCallback, null);
                running = true;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
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

        public void Stop()
        {
            if (!running)
                return;

            listener.Close();
            running = false;
        }

        void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket s = listener.EndAccept(ar);

               if (accepted != null)
                {
                    accepted(s);
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("SEND ERROR\n{0}", ex.Message);
            }
            catch (ArgumentException ex)
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

            if (running)
            {
                try
                {
                    listener.BeginAccept(AcceptCallback, null);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("SEND ERROR\n{0}", ex.Message);
                }
                catch (ArgumentOutOfRangeException ex)
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
        }
    }
}
