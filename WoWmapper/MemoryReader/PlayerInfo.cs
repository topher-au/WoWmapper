using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWmapper.MemoryReader
{
    public class PlayerInfo
    {
        public uint CurrentHealth { get; set; }
        public object Level { get; internal set; }
        public uint MaxHealth { get; set; }
    }
}