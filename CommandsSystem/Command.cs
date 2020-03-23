using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsSystem
{
    public abstract class Command<T>: ICommand
        where T: class
    {
        public abstract void Run();



        public override string ToString()
        {
            return MessagePack.MessagePackSerializer.SerializeToJson<T>(this as T);
        }

        public abstract CommandType type();
    }
}
