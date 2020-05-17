using System;
using System.Threading.Tasks;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
#endif

namespace NativeWebSocket
{
	public delegate void WebSocketOpenEventHandler();
	public delegate void WebSocketMessageEventHandler(byte[] data);
	public delegate void WebSocketErrorEventHandler(string errorMsg);
	public delegate void WebSocketCloseEventHandler(WebSocketCloseCode closeCode);

	public enum WebSocketCloseCode
	{
		/* Do NOT use NotSet - it's only purpose is to indicate that the close code cannot be parsed. */
		NotSet = 0,
		Normal = 1000,
		Away = 1001,
		ProtocolError = 1002,
		UnsupportedData = 1003,
		Undefined = 1004,
		NoStatus = 1005,
		Abnormal = 1006,
		InvalidData = 1007,
		PolicyViolation = 1008,
		TooBig = 1009,
		MandatoryExtension = 1010,
		ServerError = 1011,
		TlsHandshakeFailure = 1015
	}

	public enum WebSocketState
	{
		Connecting,
		Open,
		Closing,
		Closed
	}

	public interface IWebSocket
	{
		event WebSocketOpenEventHandler OnOpen;
		event WebSocketMessageEventHandler OnMessage;
		event WebSocketErrorEventHandler OnError;
		event WebSocketCloseEventHandler OnClose;

		WebSocketState State { get; }
	}


	public static class WebSocketHelpers
	{
		public static WebSocketCloseCode ParseCloseCodeEnum(int closeCode)
		{

			if (WebSocketCloseCode.IsDefined(typeof(WebSocketCloseCode), closeCode))
			{
				return (WebSocketCloseCode)closeCode;
			}
			else
			{
				return WebSocketCloseCode.Undefined;
			}

		}

		public static WebSocketException GetErrorMessageFromCode(int errorCode, Exception inner)
		{
			switch (errorCode)
			{
				case -1: return new WebSocketUnexpectedException("WebSocket instance not found.", inner);
				case -2: return new WebSocketInvalidStateException("WebSocket is already connected or in connecting state.", inner);
				case -3: return new WebSocketInvalidStateException("WebSocket is not connected.", inner);
				case -4: return new WebSocketInvalidStateException("WebSocket is already closing.", inner);
				case -5: return new WebSocketInvalidStateException("WebSocket is already closed.", inner);
				case -6: return new WebSocketInvalidStateException("WebSocket is not in open state.", inner);
				case -7: return new WebSocketInvalidArgumentException("Cannot close WebSocket. An invalid code was specified or reason is too long.", inner);
				default: return new WebSocketUnexpectedException("Unknown error.", inner);
			}
		}
	}

	public class WebSocketException : Exception
	{
		public WebSocketException() { }
		public WebSocketException(string message) : base(message) { }
		public WebSocketException(string message, Exception inner) : base(message, inner) { }
	}

	public class WebSocketUnexpectedException : WebSocketException
	{
		public WebSocketUnexpectedException() { }
		public WebSocketUnexpectedException(string message) : base(message) { }
		public WebSocketUnexpectedException(string message, Exception inner) : base(message, inner) { }
	}

	public class WebSocketInvalidArgumentException : WebSocketException
	{
		public WebSocketInvalidArgumentException() { }
		public WebSocketInvalidArgumentException(string message) : base(message) { }
		public WebSocketInvalidArgumentException(string message, Exception inner) : base(message, inner) { }
	}

	public class WebSocketInvalidStateException : WebSocketException
	{
		public WebSocketInvalidStateException() { }
		public WebSocketInvalidStateException(string message) : base(message) { }
		public WebSocketInvalidStateException(string message, Exception inner) : base(message, inner) { }
	}


#if UNITY_WEBGL && !UNITY_EDITOR

   /// <summary>
    /// WebSocket class bound to JSLIB.
    /// </summary>
    public class WebSocket : IWebSocket
    {

        /* WebSocket JSLIB functions */
        [DllImport("__Internal")]
        public static extern int WebSocketConnect(int instanceId);
			
        [DllImport("__Internal")]
        public static extern int WebSocketClose(int instanceId, int code, string reason);

        [DllImport("__Internal")]
        public static extern int WebSocketSend(int instanceId, byte[] dataPtr, int dataLength);

        [DllImport("__Internal")]
        public static extern int WebSocketSendText(int instanceId, string message);

        [DllImport("__Internal")]
        public static extern int WebSocketGetState(int instanceId);

        protected int instanceId;

        public event WebSocketOpenEventHandler OnOpen;
        public event WebSocketMessageEventHandler OnMessage;
        public event WebSocketErrorEventHandler OnError;
        public event WebSocketCloseEventHandler OnClose;

        public WebSocket(string url)
        {
          if (!WebSocketFactory.isInitialized) {
            WebSocketFactory.Initialize();
          }

        	int instanceId = WebSocketFactory.WebSocketAllocate(url);
        	WebSocketFactory.instances.Add(instanceId, this);

            this.instanceId = instanceId;
        }

        ~WebSocket()
        {
            WebSocketFactory.HandleInstanceDestroy(this.instanceId);
        }

        public int GetInstanceId()
        {
            return this.instanceId;
        }

        public Task Connect()
        {
            int ret = WebSocketConnect(this.instanceId);

            if (ret < 0)
                throw WebSocketHelpers.GetErrorMessageFromCode(ret, null);

            return Task.CompletedTask;
        }

        public Task Close(WebSocketCloseCode code = WebSocketCloseCode.Normal, string reason = null)
        {
            int ret = WebSocketClose(this.instanceId, (int)code, reason);

            if (ret < 0)
                throw WebSocketHelpers.GetErrorMessageFromCode(ret, null);

            return Task.CompletedTask;
        }

        public Task Send(byte[] data)
        {
            int ret = WebSocketSend(this.instanceId, data, data.Length);

            if (ret < 0)
                throw WebSocketHelpers.GetErrorMessageFromCode(ret, null);

            return Task.CompletedTask;
        }

        public Task SendText(string message)
        {
            int ret = WebSocketSendText(this.instanceId, message);

            if (ret < 0)
                throw WebSocketHelpers.GetErrorMessageFromCode(ret, null);

            return Task.CompletedTask;
        }

        public WebSocketState State {
            get {
                int state = WebSocketGetState(this.instanceId);

                if (state < 0)
                    throw WebSocketHelpers.GetErrorMessageFromCode(state, null);

                switch (state)
                {
                    case 0:
                        return WebSocketState.Connecting;

                    case 1:
                        return WebSocketState.Open;

                    case 2:
                        return WebSocketState.Closing;

                    case 3:
                        return WebSocketState.Closed;

                    default:
                        return WebSocketState.Closed;
                }
            }
        }

        public void DelegateOnOpenEvent()
        {
            this.OnOpen?.Invoke();
        }

        public void DelegateOnMessageEvent(byte[] data)
        {
            this.OnMessage?.Invoke(data);
        }

        public void DelegateOnErrorEvent(string errorMsg)
        {
            this.OnError?.Invoke(errorMsg);
        }

        public void DelegateOnCloseEvent(int closeCode)
        {
            this.OnClose?.Invoke(WebSocketHelpers.ParseCloseCodeEnum(closeCode));
        }

    }


#else

    public class WebSocket : IWebSocket {
        public event WebSocketOpenEventHandler OnOpen;
        public event WebSocketMessageEventHandler OnMessage;
        public event WebSocketErrorEventHandler OnError;
        public event WebSocketCloseEventHandler OnClose;


        private WebSocketSharp.WebSocket _websocket;

        private Uri uri;
        // private ClientWebSocket m_Socket = new ClientWebSocket();

        public WebSocket(string url) {
            uri = new Uri(url.Replace("{host}", "localhost:8080"));

            string protocol = uri.Scheme;
            if (!protocol.Equals("ws") && !protocol.Equals("wss"))
                throw new ArgumentException("Unsupported protocol: " + protocol);
        }

        public async Task Connect() {
            try {
                _websocket = new WebSocketSharp.WebSocket(uri.ToString());
                _websocket.OnOpen += (sender, args) => OnOpen?.Invoke();
                _websocket.OnMessage += (sender, args) => OnMessage?.Invoke(args.RawData);
                _websocket.OnError += (sender, args) => OnError?.Invoke(args.Message);
                _websocket.OnClose += (sender, args) => OnClose?.Invoke((WebSocketCloseCode) args.Code); 
                
				await Task.Run(() => { _websocket.Connect(); });
            }
            catch (Exception ex) {
                OnError?.Invoke(ex.Message);
                OnClose?.Invoke(WebSocketCloseCode.Abnormal);
            }
        }

        public WebSocketState State {
            get {
                switch (_websocket.ReadyState) {
                    case WebSocketSharp.WebSocketState.Connecting:
                        return WebSocketState.Connecting;

                    case WebSocketSharp.WebSocketState.Open:
                        return WebSocketState.Open;

                    case WebSocketSharp.WebSocketState.Closing:
                        return WebSocketState.Closing;

                    case WebSocketSharp.WebSocketState.Closed:
                        return WebSocketState.Closed;

                    default:
                        return WebSocketState.Closed;
                }
            }
        }

        public Task Send(byte[] bytes) {
            /*  return Task.Run(() => 
                  _websocket.Send(bytes));*/
            _websocket.SendAsync(bytes, res => { });
            return Task.CompletedTask;
        }

        public Task SendText(string message) {
            _websocket.SendAsync(message, res => { });
            return Task.CompletedTask;
            //   return Task.Run(() => { _websocket.Send(message); });
        }

        
        public async Task Close() {
            await Task.Run(() => { _websocket.Close(); });

        }
    }
#endif


	///
	/// Factory
	///


	/// <summary>
	/// Class providing static access methods to work with JSLIB WebSocket or WebSocketSharp interface
	/// </summary>
	public static class WebSocketFactory
	{

#if UNITY_WEBGL && !UNITY_EDITOR
        /* Map of websocket instances */
        public static Dictionary<Int32, WebSocket> instances = new Dictionary<Int32, WebSocket>();

        /* Delegates */
        public delegate void OnOpenCallback(int instanceId);
        public delegate void OnMessageCallback(int instanceId, System.IntPtr msgPtr, int msgSize);
        public delegate void OnErrorCallback(int instanceId, System.IntPtr errorPtr);
        public delegate void OnCloseCallback(int instanceId, int closeCode);

        /* WebSocket JSLIB callback setters and other functions */
        [DllImport("__Internal")]
        public static extern int WebSocketAllocate(string url);

        [DllImport("__Internal")]
        public static extern void WebSocketFree(int instanceId);

        [DllImport("__Internal")]
        public static extern void WebSocketSetOnOpen(OnOpenCallback callback);

        [DllImport("__Internal")]
        public static extern void WebSocketSetOnMessage(OnMessageCallback callback);

        [DllImport("__Internal")]
        public static extern void WebSocketSetOnError(OnErrorCallback callback);

        [DllImport("__Internal")]
        public static extern void WebSocketSetOnClose(OnCloseCallback callback);

        /* If callbacks was initialized and set */
        public static bool isInitialized = false;

        /*
         * Initialize WebSocket callbacks to JSLIB
         */
        public static void Initialize()
        {

            WebSocketSetOnOpen(DelegateOnOpenEvent);
            WebSocketSetOnMessage(DelegateOnMessageEvent);
            WebSocketSetOnError(DelegateOnErrorEvent);
            WebSocketSetOnClose(DelegateOnCloseEvent);

            isInitialized = true;

        }

        /// <summary>
        /// Called when instance is destroyed (by destructor)
        /// Method removes instance from map and free it in JSLIB implementation
        /// </summary>
        /// <param name="instanceId">Instance identifier.</param>
        public static void HandleInstanceDestroy(int instanceId)
        {

            instances.Remove(instanceId);
            WebSocketFree(instanceId);

        }

        [MonoPInvokeCallback(typeof(OnOpenCallback))]
        public static void DelegateOnOpenEvent(int instanceId)
        {

            WebSocket instanceRef;

            if (instances.TryGetValue(instanceId, out instanceRef))
            {
                instanceRef.DelegateOnOpenEvent();
            }

        }

        [MonoPInvokeCallback(typeof(OnMessageCallback))]
        public static void DelegateOnMessageEvent(int instanceId, System.IntPtr msgPtr, int msgSize)
        {

            WebSocket instanceRef;

            if (instances.TryGetValue(instanceId, out instanceRef))
            {
                byte[] msg = new byte[msgSize];
                Marshal.Copy(msgPtr, msg, 0, msgSize);

                instanceRef.DelegateOnMessageEvent(msg);
            }

        }

        [MonoPInvokeCallback(typeof(OnErrorCallback))]
        public static void DelegateOnErrorEvent(int instanceId, System.IntPtr errorPtr)
        {

            WebSocket instanceRef;

            if (instances.TryGetValue(instanceId, out instanceRef))
            {

                string errorMsg = Marshal.PtrToStringAuto(errorPtr);
                instanceRef.DelegateOnErrorEvent(errorMsg);

            }

        }

        [MonoPInvokeCallback(typeof(OnCloseCallback))]
        public static void DelegateOnCloseEvent(int instanceId, int closeCode)
        {

            WebSocket instanceRef;

            if (instances.TryGetValue(instanceId, out instanceRef))
            {
                instanceRef.DelegateOnCloseEvent(closeCode);
            }

        }
#endif

		/// <summary>
		/// Create WebSocket client instance
		/// </summary>
		/// <returns>The WebSocket instance.</returns>
		/// <param name="url">WebSocket valid URL.</param>
		public static WebSocket CreateInstance(string url)
		{
			return new WebSocket(url);
		}

	}


}
