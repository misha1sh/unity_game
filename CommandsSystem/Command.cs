
using System.IO;
using GameDevWare.Serialization;

namespace CommandsSystem
{
    public abstract class Command<T>: ICommand
        where T: class
    {
        public abstract void Run();

        public override string ToString()
        {
            var sb = new StringWriter();
            Json.Serialize(this as T, sb);
            return sb.ToString();
        }
    }
}
