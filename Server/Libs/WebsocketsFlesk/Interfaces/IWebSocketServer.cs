#if UNITY_EDITOR || !UNITY_WEBGL
using System;

namespace Fleck
{
    public interface IWebSocketServer : IDisposable
    {
        void Start(Action<IWebSocketConnection> config);
    }
}

#endif