using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using RaviaPC.SendInput;

namespace RaviaPC
{
    public partial class Form1 : Form
    {
        ListenerSocket listenerIn;
        ListenerSocket listenerOut;
        ServerSocket serverIn;
        ServerSocket serverOut;
        
        public static int socketIn;
        public static int socketOut;
        public static String MyIp = "";
        public static Bitmap obraz;

        public string msg;
        
        AutoResetEvent receiveDoneIn = new AutoResetEvent(false);
        AutoResetEvent receiveDoneOut = new AutoResetEvent(false);
        Keyboard keyboard = new Keyboard();
        Mouse mouse = new Mouse();

        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINTAPI ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINTAPI
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll")]
        static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);

        const Int32 CURSOR_SHOWING = 0x00000001;

        public Form1()
        {
            InitializeComponent();
            MaximizeBox = false;
            button2.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getIP();
            pictureBox1.Image = Properties.Resources.icon;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void getIP() { 

            String NazwaHosta = Dns.GetHostName();
            IPHostEntry AdresyIP = Dns.GetHostEntry(NazwaHosta);

            foreach (IPAddress AdresIP in AdresyIP.AddressList)
            {
                if (AdresIP.ToString() == "127.0.0.1")
                {
                    listBox1.Items.Add("PC is not connected to the network !!!");
                    label2.Text = "???";
                }
                else if (AdresIP.AddressFamily == AddressFamily.InterNetwork)
                {
                    MyIp = AdresIP.ToString();
                    label2.Text = MyIp;
                }            
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            try
            {
                socketIn = Convert.ToInt32(textBox2.Text);
                socketOut = Convert.ToInt32(textBox3.Text);
                if (!string.IsNullOrEmpty(MyIp) && !(socketIn == null || socketIn == 0) && !(socketOut == null || socketOut == 0))
                {
                    if (socketIn <= 1023 || socketIn >= 65536)
                    {
                        textBox2.Text = String.Empty;
                        MessageBox.Show("Socket Input must be between 1024 - 65535");
                    }
                    else if (socketOut <= 1023 || socketOut >= 65536)
                    {
                        textBox3.Text = String.Empty;
                        MessageBox.Show("Socket Output must be between 1024 - 65535");
                    }
                    else if (socketIn == socketOut)
                    {
                        textBox2.Text = String.Empty;
                        textBox3.Text = String.Empty;
                        MessageBox.Show("Socket Input and Socket Output must be different");
                    }
                    else
                    {
                        button1.Enabled = false;
                        button2.Enabled = true;
                        listenerIn = new ListenerSocket();
                        listenerIn.accepted += new ListenerSocket.SocketAcceptedHandler(listenerIn_Accepted);

                        listenerOut = new ListenerSocket();
                        listenerOut.accepted += new ListenerSocket.SocketAcceptedHandler(listenerOut_Accepted);

                        Thread SendInputThread = new Thread(() => listenerOut.Start(socketOut));
                        SendInputThread.Start();
                        SendInputThread.Join();

                        Thread SendImageThread = new Thread(() => listenerIn.Start(socketIn));
                        SendImageThread.Start();
                        SendImageThread.Join();
                    }
                }
            }
            catch
            {
                MessageBox.Show("\"Socket In\" and \"Socket Out\" must be a number. Look at the instructions.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serverIn != null)
            {
                serverIn.Close();
                serverIn = null;               
            }

            if (serverOut != null)
            {
                serverOut.Close();
                serverOut = null;
            }

            listBox1.Items.Add("Stop listening or client disconnected !!!");
            button1.Enabled = true;
            button2.Enabled = false;
            listenerIn.Stop();
            listenerOut.Stop();
        }

        void listenerIn_Accepted(Socket e)
        {
            if (serverIn != null)
            {
                e.Close();
                return;
            }
            
            serverIn = new ServerSocket(e);
            serverIn.onSend += new ServerSocket.SendEventHandler(clientIn_OnSend);           
            serverIn.onReceive += new ServerSocket.ReceiveEventHandler(clientIn_OnReveive);

            Invoke((MethodInvoker)delegate
            {
                listBox1.Items.Add("Connected: " + serverIn.endPoint.ToString() + "   (SocketIn)");
            });

                do {
                    msg = "";
                    obraz = makeScreenShot(true);
                    SendImage(obraz);
                    serverIn.ReceiveMessage();
                    receiveDoneIn.WaitOne();
                } while(serverIn.connected);           
        }

        void listenerOut_Accepted(Socket e)
        {
            if (serverOut != null)
            {
                e.Close();
                return;
            }

            serverOut = new ServerSocket(e);
            serverOut.onReceive += new ServerSocket.ReceiveEventHandler(clientOut_OnReveive);
            
            Invoke((MethodInvoker)delegate
            {
                listBox1.Items.Add("Connected: " + serverOut.endPoint.ToString() + "   (SocketOut)");
            });

            while(serverOut.connected) {
                serverOut.ReceiveKeyboardAndMouseMessage();
                receiveDoneOut.WaitOne();
            }
        }

        void clientIn_OnSend(ServerSocket sender, int sent)
        {
            Invoke((MethodInvoker)delegate
            {
                label6.Text = string.Format("Data sent: {0} B = {1} kB", sent, (sent/1024));
            });
        }

        void clientIn_OnReveive(ServerSocket sender, string msg1)
        {
            msg = msg1;
            if (msg == "Next")
            {
                receiveDoneIn.Set();
            }
            else if (msg == "Close")
            {
                serverIn.Close();
                serverOut.Close();
                serverIn = null;
                serverOut = null;

                Invoke((MethodInvoker)delegate
                {
                    listBox1.Items.Add("Client disconnected !!!");
                });
            }
        }

        void clientOut_OnReveive(ServerSocket sender, string key)
        {
            if (key.Length >= 2)
            {
                if (key.Substring(0, 1).Equals("x"))
                {
                    string[] coordinates = key.Split('y');
                    coordinates[0] = coordinates[0].Remove(0,1);
                    int coordX = Int16.Parse(coordinates[0]);
                    int coordY = Int16.Parse(coordinates[1]);
                    mouse.Move(coordX, coordY);
                }
                else if ((key.Substring(0, 3).Equals("ml0")) || (key.Substring(0, 3).Equals("mr0")))
                {
                    mouse.MouseClickControl(key);
                }
                else
                {
                    keyboard.KeyboardExtraControl(key);
                }
            }
            else
            {
                keyboard.KeyboardControl(key);
            }

            receiveDoneOut.Set();
        }
      
        public static Bitmap makeScreenShot(bool CaptureMouse)
        {
            // Za pomocą Width i Height odczytujemy szerokość i wysokość ekranu. Ustawiamy dodatkowo 32-bitową głębię kolorów
            Bitmap bitmapa = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);  // Możliwe że przy słabej przepustowości trzeba będzie zmienić format !!!

            try
            {
                using (Graphics screenshot = Graphics.FromImage(bitmapa))
                {
                    //Metoda CopyFromScreen wykonuje zrzut ekranu. Przesyła ona piksele z interesującego nas fragmentu ekranu
                    screenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

                    if (CaptureMouse)
                    {
                        CURSORINFO pci;
                        pci.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(CURSORINFO));

                        if (GetCursorInfo(out pci))
                        {
                            if (pci.flags == CURSOR_SHOWING)
                            {
                                DrawIcon(screenshot.GetHdc(), pci.ptScreenPos.x, pci.ptScreenPos.y, pci.hCursor);
                                screenshot.ReleaseHdc();
                            }
                        }
                    }
                }
            }
            catch
            {
                bitmapa = null;
            }
            
            return bitmapa;
        }

        void SendImage(Bitmap path)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            path.Save(ms, ImageFormat.Jpeg);    // Zapisuje obraz do określonego strumienia o określonym formacie
            byte[] b = ms.GetBuffer();   
            bw.Close();          
            b = ms.ToArray();  
            ms.Dispose();

            serverIn.Send(b, 0, b.Length);
        }
    }
}
