using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskCQRS.Core
{
    public class Event : Message
    {
        public int Version;
    }
}
