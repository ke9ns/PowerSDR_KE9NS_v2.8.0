using System.Diagnostics;
using System.Net.Sockets;

namespace HttpServer
{
    enum RequestType
    {
        GET_IMAGE,
        GET_HTML_INDEX_PAGE,
        SET_PARAM,
        GET_TRX_PARAM,
        UNKNOWN,
        ERROR
    }

    public abstract class IHandler
    {
        public IHandler(string body, TcpClient tcpClient)
        {
            m_tcpClient = tcpClient;
            m_body = body;
        }

        public static void setConsole(PowerSDR.Console console)
        {
            m_console = console;
        }

        public abstract void handle();

        protected void sendAnswer(byte[] data)
        {
            if (m_tcpClient == null) return;

            NetworkStream stream = m_tcpClient.GetStream();
            stream.ReadTimeout = 10;
            try
            {
                if (stream.CanWrite)
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            stream.Close();
            m_tcpClient.Close();
        }

        protected string m_body;
        protected TcpClient m_tcpClient;
        protected static PowerSDR.Console m_console;



    }
}