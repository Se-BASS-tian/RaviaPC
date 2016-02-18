using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace RaviaPC.SendInput
{
    class MySendInput
    {
        public MySendInput() { }

        /***** DLL INPORTS *****/

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        // TYP ZDARZENIA WYJŚCIOWEGO
        const int INPUT_MOUSE = 0;
        const int INPUT_KEYBOARD = 1;

        // FLAGI DLA ZDARZEŃ KLAWIATURY
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;  // Jeśli określony, kod skanowania został poprzedzony prefiksem, że bajt ma wartość 0xE0 (224).
        const uint KEYEVENTF_KEYUP = 0x0002;  // Jeśli określony, przycisk jest zwolniony. Jeśli nie podano, przycisk jest wciśnięty.
        const uint KEYEVENTF_UNICODE = 0x0004; // Jeśli określony, wScan identyfikuje klucz a wVk jest ignorowany.
        const uint KEYEVENTF_SCANCODE = 0x0008; // Jeśli określony, system syntetyzuje klawisze VK_PACKET. Parametr wVk musi wtedy być zerowy. Flaga ta może być połączona tylko z flagą KEYEVENTF_KEYUP.

        // FLAGI DLA ZDARZEŃ MYSZKI
        const uint MOUSEEVENTF_MOVE = 0x0001;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            // Wartość X, jeśli ABSOLUTE jest przekazywana w fladze to jest to jest aktualna pozycja X i Y.
            // W przeciwnym wypadku jest do delta z ostatniej pozycji.
            public int dx;

            // Wartość Y, jeśli ABSOLUTE jest przekazywana w fladze to jest to jest aktualna pozycja X i Y.
            // W przeciwnym wypadku jest do delta z ostatniej pozycji.
            public int dy;

            // Dane zdarzenia scrolla myszki, X przyciski.
            public uint mouseData;

            // Pole ORable z różnymi flagami z informacjami o przyciskach i charakterem zdarzenia.
            public uint dwFlags;

            // Znacznik czasu dla zdarzenia, jeśli zero to system go zapewnia.
            public uint time;

            // Dodatkowe dane uzyskane przez wywołanie aplikacji przez GetMessageExtraInfo.
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            // Wirtualny klucz-kod. Kod musi być wartością w zakresie od 1 do 254. Jeżeli członek dwFlags określa KEYEVENTF_UNICODE, wVk musi być 0.
            public ushort wVk;

            // Kod skanowania sprzętowego dla klucza. Jeśli dwFlags określa KEYEVENTF_UNICODE, wScan określa znak Unicode, który ma być wysłany na pierwszym planie aplikacji.
            public ushort wScan;

            // Określa różne aspekty naciśnięcia klawisza. Ten element może być pewną kombinacją wartości.
            public uint dwFlags;

            // Znacznik czasu dla zdarzenia, w milisekundach. Jeśli parametr ten ma wartość zero, to system zapewnia własny znacznik czasu.
            public uint time;

            // Dodatkowe dane uzyskane przez wywołanie aplikacji przez GetMessageExtraInfo.
            public IntPtr dwExtraInfo;
        }

        // Dane przesłane w tablicy do SendInput.
        [StructLayout(LayoutKind.Explicit)]
        struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            // Dane wejściowe - Mysz
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            // Dane wejściowe - Klawiatura
            [FieldOffset(4)]
            public KEYBDINPUT ki;
        }


        private MOUSEINPUT createMouseInput(int x, int y, uint data, uint t, uint flag)
        {
            MOUSEINPUT mi = new MOUSEINPUT();
            mi.dx = x;
            mi.dy = y;
            mi.mouseData = data;
            mi.time = t;
            mi.dwFlags = flag;
            return mi;
        }

        private KEYBDINPUT createKeybdInput(short wVK, short wScan, uint flag)
        {
            KEYBDINPUT ki = new KEYBDINPUT();
            ki.wVk = (ushort)wVK;
            ki.wScan = (ushort)wScan;
            ki.time = 0;
            ki.dwExtraInfo = IntPtr.Zero;
            ki.dwFlags = flag;
            return ki;
        }

        // Przesuwa kursor w wybraną pozycję na ekranie. Za każdym razem najpierw przesuwa kursor w lewy górny róg,
        // następnie w miejsce wskazane przez współrzędne X,Y.
        public void sim_mov(int x, int y)
        {
            INPUT[] inp = new INPUT[2];
            inp[0].type = INPUT_MOUSE;
            inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE);
            inp[1].type = INPUT_MOUSE;
            inp[1].mi = createMouseInput(x * (65535 / (GetSystemMetrics(0) - 1)), y * (65535 / (GetSystemMetrics(1) - 1)), 0, 0, MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE);
            SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        }

        // Kliknięcie lewym przyciskiem myszy
        public void mouse_left_click()
        {
            INPUT[] inp = new INPUT[2];
            inp[0].type = INPUT_MOUSE;
            inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_LEFTDOWN);
            inp[1].type = INPUT_MOUSE;
            inp[1].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_LEFTUP);
            SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        }

        // Kliknięcie prawym przyciskiem myszy
        public void mouse_right_click()
        {
            INPUT[] inp = new INPUT[2];
            inp[0].type = INPUT_MOUSE;
            inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_RIGHTDOWN);
            inp[1].type = INPUT_MOUSE;
            inp[1].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_RIGHTUP);
            SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        }

        public void KeyboardKey(short virtualKey)
        {
            INPUT[] inp = new INPUT[2];

            if (virtualKey == 0x10)
            {
                virtualKey = (short)MapVirtualKey((uint)System.Windows.Forms.Keys.ShiftKey, (uint)0x0);   // Shift

                inp[0].type = INPUT_KEYBOARD;
                inp[0].ki = createKeybdInput(0, virtualKey, KEYEVENTF_SCANCODE);   // WCIŚNIĘCIE PRZYCISKU "SHIFT"
                inp[1].type = INPUT_KEYBOARD;
                inp[1].ki = createKeybdInput(0, virtualKey, KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP);
                SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
            }
            else if (virtualKey == 0x11) { 
                virtualKey = (short)MapVirtualKey((uint)System.Windows.Forms.Keys.ControlKey, (uint)0x0);   // Ctrl

                inp[0].type = INPUT_KEYBOARD;
                inp[0].ki = createKeybdInput(0, virtualKey, KEYEVENTF_SCANCODE);   // WCIŚNIĘCIE PRZYCISKU "CTRL"
                inp[1].type = INPUT_KEYBOARD;
                inp[1].ki = createKeybdInput(0, virtualKey, KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP);
                SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
            }
            else if (virtualKey == 0x12)
            {
                virtualKey = (short)MapVirtualKey((uint)System.Windows.Forms.Keys.Menu, (uint)0x0);   // Alt

                inp[0].type = INPUT_KEYBOARD;
                inp[0].ki = createKeybdInput(0, virtualKey, KEYEVENTF_SCANCODE);   // WCIŚNIĘCIE PRZYCISKU "ALT"
                inp[1].type = INPUT_KEYBOARD;
                inp[1].ki = createKeybdInput(0, virtualKey, KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP);
                SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
            }
            else
            {
                inp[0].type = INPUT_KEYBOARD;
                inp[0].ki = createKeybdInput(virtualKey, 0, KEYEVENTF_EXTENDEDKEY);
                inp[1].type = INPUT_KEYBOARD;
                inp[1].ki = createKeybdInput(virtualKey, 0, KEYEVENTF_KEYUP);
                SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
            }
        }

        // Wciśnięcie przycisku CTRL + virtualKey
        public void KeyboardCtrlKey(short virtualKey)
        {
            INPUT[] inp = new INPUT[4];
            uint ctrlkey = MapVirtualKey((uint)System.Windows.Forms.Keys.ControlKey, (uint)0x0);
            inp[0].type = INPUT_KEYBOARD;
            inp[0].ki = createKeybdInput(0, (short)ctrlkey, KEYEVENTF_SCANCODE);   // WCIŚNIĘCIE PRZYCISKU "CTRL"
            inp[1].type = INPUT_KEYBOARD;
            inp[1].ki = createKeybdInput(virtualKey, 0, KEYEVENTF_EXTENDEDKEY);
            Thread.Sleep(50);
            inp[2].type = INPUT_KEYBOARD;
            inp[2].ki = createKeybdInput(virtualKey, 0, KEYEVENTF_KEYUP);
            inp[3].type = INPUT_KEYBOARD;
            inp[3].ki = createKeybdInput(0, (short)ctrlkey, KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP);
            SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        }

        // Wciśnięcie przycisku ALT + virtualKey
        public void KeyboardAltKey(short virtualKey)
        {
            INPUT[] inp = new INPUT[4];
            uint altkey = MapVirtualKey((uint)System.Windows.Forms.Keys.Menu, (uint)0x0);
            inp[0].type = INPUT_KEYBOARD;
            inp[0].ki = createKeybdInput(0, (short)altkey, KEYEVENTF_SCANCODE);   // WCIŚNIĘCIE PRZYCISKU "ALT"
            inp[1].type = INPUT_KEYBOARD;
            inp[1].ki = createKeybdInput(virtualKey, 0, KEYEVENTF_EXTENDEDKEY);
            Thread.Sleep(50);
            inp[2].type = INPUT_KEYBOARD;
            inp[2].ki = createKeybdInput(virtualKey, 0, KEYEVENTF_KEYUP);
            inp[3].type = INPUT_KEYBOARD;
            inp[3].ki = createKeybdInput(0, (short)altkey, KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP);
            SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        }

        // Wciśnięcie przycisku SHIFT + virtualKey
        public void KeyboardShiftKey(short virtualKey)
        {
            INPUT[] inp = new INPUT[4];
            uint shiftkey = MapVirtualKey((uint)System.Windows.Forms.Keys.ShiftKey, (uint)0x0);
            inp[0].type = INPUT_KEYBOARD;
            inp[0].ki = createKeybdInput(0, (short)shiftkey, KEYEVENTF_SCANCODE);   // WCIŚNIĘCIE PRZYCISKU "SHIFT"
            inp[1].type = INPUT_KEYBOARD;
            inp[1].ki = createKeybdInput(virtualKey, 0, KEYEVENTF_EXTENDEDKEY);
            Thread.Sleep(50);
            inp[2].type = INPUT_KEYBOARD;
            inp[2].ki = createKeybdInput(virtualKey, 0, KEYEVENTF_KEYUP);
            inp[3].type = INPUT_KEYBOARD;
            inp[3].ki = createKeybdInput(0, (short)shiftkey, KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP);
            SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        }
    }
}
