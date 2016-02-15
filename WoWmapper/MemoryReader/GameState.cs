using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWmapper.MemoryReader
{
        public enum GameState
        {
            LoggedOut = 0x00,
            LoggedIn = 0x01,
            NotRunning = 0xFF
        }
}
