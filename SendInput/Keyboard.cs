using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaviaPC.SendInput
{
    class Keyboard
    {
        short virtualKey = 0;
        MySendInput mySendInput = new MySendInput();
        MyKeys keys = new MyKeys();

        public void KeyboardControl(string key)
        {
            for (int i = 0; i < 73; i++)
            {
                if (keys.Codes[i, 0] == key)
                {
                    virtualKey = Convert.ToInt16(keys.Codes[i, 1], 16);
                    break;
                }
            }
            mySendInput.KeyboardKey(virtualKey);
        }

        public void KeyboardExtraControl(string key)
        {
            if (key.Substring(0, 2).Equals("s0"))
            {
                for (int i = 0; i < 73; i++)
                    {
                        if (keys.Codes[i, 0] == key.Substring(2))
                        {
                            virtualKey = Convert.ToInt16(keys.Codes[i, 1], 16);
                            break;
                        }
                    }
                    mySendInput.KeyboardShiftKey(virtualKey);
                }
                else if (key.Substring(0, 2).Equals("c0"))
                {
                    for (int i = 0; i < 73; i++)
                    {
                        if (keys.Codes[i, 0] == key.Substring(2))
                        {
                            virtualKey = Convert.ToInt16(keys.Codes[i, 1], 16);
                            break;
                        }
                    }
                    mySendInput.KeyboardCtrlKey(virtualKey);
                }
                else if (key.Substring(0, 2).Equals("a0"))
                {
                    for (int i = 0; i < 73; i++)
                    {
                        if (keys.Codes[i, 0] == key.Substring(2))
                        {
                            virtualKey = Convert.ToInt16(keys.Codes[i, 1], 16);
                            break;
                        }
                    }
                    mySendInput.KeyboardAltKey(virtualKey);
                }
                else
                {
                    for (int i = 0; i < 73; i++)
                    {
                        if (keys.Codes[i, 0] == key)
                        {
                            virtualKey = Convert.ToInt16(keys.Codes[i, 1], 16);
                            break;
                        }
                    }
                    mySendInput.KeyboardKey(virtualKey);
                }
            } 
    }
}