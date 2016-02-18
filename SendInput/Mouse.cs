using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaviaPC.SendInput
{
    class Mouse
    {
        MySendInput mySendInput = new MySendInput();

        public void MouseClickControl(string key)
        {
            switch (key)
            {
                case "ml0":
                    mySendInput.mouse_left_click();
                    break;
                case "mr0":
                    mySendInput.mouse_right_click();
                    break;
                default:
                    return;

            }
        }

        public void Move(int x, int y)
        {
            mySendInput.sim_mov(x, y);
        }
    }
}
