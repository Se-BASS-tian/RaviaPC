using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaviaPC.SendInput
{
    class MyKeys
    {
        public string[,] Codes;

        public MyKeys()
        {
            Codes = this.setCodes();
        }

        public String[,] setCodes()
        {
            String[,] keyCodes = new String[73,2];

            int i = -1;

            keyCodes[++i, 0] = "Esc"; keyCodes[i, 1] = "0x1B";      // *** ESC ***   
            keyCodes[++i, 0] = "F1"; keyCodes[i, 1] = "0x70";       // *** F1 ***
            keyCodes[++i, 0] = "F2"; keyCodes[i, 1] = "0x71";       // *** F2 ***
            keyCodes[++i, 0] = "F3"; keyCodes[i, 1] = "0x72";       // *** F3 ***
            keyCodes[++i, 0] = "F4"; keyCodes[i, 1] = "0x73";       // *** F4 ***
            keyCodes[++i, 0] = "F5"; keyCodes[i, 1] = "0x74";       // *** F5 ***
            keyCodes[++i, 0] = "F6"; keyCodes[i, 1] = "0x75";       // *** F6 ***
            keyCodes[++i, 0] = "F7"; keyCodes[i, 1] = "0x76";       // *** F7 ***
            keyCodes[++i, 0] = "F8"; keyCodes[i, 1] = "0x77";       // *** F8 ***
            keyCodes[++i, 0] = "F9"; keyCodes[i, 1] = "0x78";       // *** F9 ***
            keyCodes[++i, 0] = "F10"; keyCodes[i, 1] = "0x79";      // *** F10 ***
            keyCodes[++i, 0] = "F11"; keyCodes[i, 1] = "0x7A";      // *** F11 ***
            keyCodes[++i, 0] = "F12"; keyCodes[i, 1] = "0x7B";      // *** F12 ***
            keyCodes[++i, 0] = "Del"; keyCodes[i, 1] = "0x2E";      // *** DELETE ***

            keyCodes[++i, 0] = "`"; keyCodes[i, 1] = "0xC0";        // *** ` ***                
            keyCodes[++i, 0] = "1"; keyCodes[i, 1] = "0x31";        // *** 1 ***
            keyCodes[++i, 0] = "2"; keyCodes[i, 1] = "0x32";        // *** 2 ***
            keyCodes[++i, 0] = "3"; keyCodes[i, 1] = "0x33";        // *** 3 ***
            keyCodes[++i, 0] = "4"; keyCodes[i, 1] = "0x34";        // *** 4 ***
            keyCodes[++i, 0] = "5"; keyCodes[i, 1] = "0x35";        // *** 5 ***
            keyCodes[++i, 0] = "6"; keyCodes[i, 1] = "0x36";        // *** 6 ***
            keyCodes[++i, 0] = "7"; keyCodes[i, 1] = "0x37";        // *** 7 ***
            keyCodes[++i, 0] = "8"; keyCodes[i, 1] = "0x38";        // *** 8 ***
            keyCodes[++i, 0] = "9"; keyCodes[i, 1] = "0x39";        // *** 9 ***
            keyCodes[++i, 0] = "0"; keyCodes[i, 1] = "0x30";        // *** 0 ***
            keyCodes[++i, 0] = "-"; keyCodes[i, 1] = "0x6D";        // *** ODEJMOWANIE - - ***
            keyCodes[++i, 0] = "="; keyCodes[i, 1] = "0xBB";        // *** RÓWNA SIĘ - = ***
            keyCodes[++i, 0] = "Bckspace"; keyCodes[i, 1] = "0x08"; // *** BACKSPACE ***
           
            keyCodes[++i, 0] = "Tab"; keyCodes[i, 1] = "0x09";      // *** TAB ***           
            keyCodes[++i, 0] = "Q"; keyCodes[i, 1] = "0x51";        // *** Q ***
            keyCodes[++i, 0] = "W"; keyCodes[i, 1] = "0x57";        // *** W ***
            keyCodes[++i, 0] = "E"; keyCodes[i, 1] = "0x45";        // *** E ***
            keyCodes[++i, 0] = "R"; keyCodes[i, 1] = "0x52";        // *** R ***
            keyCodes[++i, 0] = "T"; keyCodes[i, 1] = "0x54";        // *** T ***
            keyCodes[++i, 0] = "Y"; keyCodes[i, 1] = "0x59";        // *** Y ***
            keyCodes[++i, 0] = "U"; keyCodes[i, 1] = "0x55";        // *** U ***
            keyCodes[++i, 0] = "I"; keyCodes[i, 1] = "0x49";        // *** I ***
            keyCodes[++i, 0] = "O"; keyCodes[i, 1] = "0x4F";        // *** O ***
            keyCodes[++i, 0] = "P"; keyCodes[i, 1] = "0x50";        // *** P ***
            keyCodes[++i, 0] = "["; keyCodes[i, 1] = "0xDB";        // *** [ ***
            keyCodes[++i, 0] = "]"; keyCodes[i, 1] = "0xDD";        // *** ] ***
            keyCodes[++i, 0] = "\\"; keyCodes[i, 1] = "0xDC";       // *** \ ***
            
            keyCodes[++i, 0] = "CapsLk"; keyCodes[i, 1] = "0x14";   // *** CAPSLOCK ***           
            keyCodes[++i, 0] = "A"; keyCodes[i, 1] = "0x41";        // *** A ***
            keyCodes[++i, 0] = "S"; keyCodes[i, 1] = "0x53";        // *** S ***
            keyCodes[++i, 0] = "D"; keyCodes[i, 1] = "0x44";        // *** D ***
            keyCodes[++i, 0] = "F"; keyCodes[i, 1] = "0x46";        // *** F ***
            keyCodes[++i, 0] = "G"; keyCodes[i, 1] = "0x47";        // *** G ***
            keyCodes[++i, 0] = "H"; keyCodes[i, 1] = "0x48";        // *** H ***
            keyCodes[++i, 0] = "J"; keyCodes[i, 1] = "0x4A";        // *** J ***
            keyCodes[++i, 0] = "K"; keyCodes[i, 1] = "0x4B";        // *** K ***
            keyCodes[++i, 0] = "L"; keyCodes[i, 1] = "0x4C";        // *** L ***
            keyCodes[++i, 0] = ";"; keyCodes[i, 1] = "0xBA";        // *** ; ***
            keyCodes[++i, 0] = "'"; keyCodes[i, 1] = "0xDE";        // *** ' ***
            keyCodes[++i, 0] = "Enter"; keyCodes[i, 1] = "0x0D";    // *** ENTER ***

            keyCodes[++i, 0] = "Shift"; keyCodes[i, 1] = "0x10";    // *** SHIFT ***       
            keyCodes[++i, 0] = "Z"; keyCodes[i, 1] = "0x5A";        // *** Z ***
            keyCodes[++i, 0] = "X"; keyCodes[i, 1] = "0x58";        // *** X ***
            keyCodes[++i, 0] = "C"; keyCodes[i, 1] = "0x43";        // *** C ***
            keyCodes[++i, 0] = "V"; keyCodes[i, 1] = "0x56";        // *** V ***
            keyCodes[++i, 0] = "B"; keyCodes[i, 1] = "0x42";        // *** B ***     
            keyCodes[++i, 0] = "N"; keyCodes[i, 1] = "0x4E";        // *** N ***
            keyCodes[++i, 0] = "M"; keyCodes[i, 1] = "0x4D";        // *** M ***
            keyCodes[++i, 0] = ","; keyCodes[i, 1] = "0xBC";        // *** , ***
            keyCodes[++i, 0] = "."; keyCodes[i, 1] = "0xBE";        // *** . ***
            keyCodes[++i, 0] = "/"; keyCodes[i, 1] = "0xBF";        // *** / ***

            keyCodes[++i, 0] = "Ctrl"; keyCodes[i, 1] = "0x11";     // *** CTRL ***
            keyCodes[++i, 0] = "Alt"; keyCodes[i, 1] = "0x12";      // *** ALT ***
            keyCodes[++i, 0] = "Space"; keyCodes[i, 1] = "0x20";    // *** SPACE ***
            keyCodes[++i, 0] = "Up"; keyCodes[i, 1] = "0x26";       // *** STRZAŁKA UP ***
            keyCodes[++i, 0] = "Left"; keyCodes[i, 1] = "0x25";     // *** STRZAŁKA LEFT ***
            keyCodes[++i, 0] = "Down"; keyCodes[i, 1] = "0x28";     // *** STRZAŁKA DOWN ***
            keyCodes[++i, 0] = "Right"; keyCodes[i, 1] = "0x27";    // *** STRZAŁKA RIGHT ***

            return keyCodes;
        }
    }
}