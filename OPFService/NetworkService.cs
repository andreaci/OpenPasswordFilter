using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace OPFService {
    class NetworkService
    {
        ApplicationChecks applicationChecks = null;

        public NetworkService()
        {
            applicationChecks = new ApplicationChecks();
        }

        public void main()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint local = new IPEndPoint(ip, 5999);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try 
            {
                listener.Bind(local);
                listener.Listen(64);

                while (true) {
                    Socket client = listener.Accept();
                    new Thread(() => handle(client)).Start();
                }
            } catch (Exception e) {
                ApplicationConfiguration.Instance.LogException(e);
            }
        }

        public void handle(Socket client)
        {
            try
            {
                NetworkStream netStream = new NetworkStream(client);
                StreamReader istream = new StreamReader(netStream);
                StreamWriter ostream = new StreamWriter(netStream);

                string command = istream.ReadLine();

                if (command == ApplicationConfiguration.Instance.ClientRecognitionKey)
                {
                    string userName = istream.ReadLine();
                    string password = istream.ReadLine();

                    bool contained = applicationChecks.CheckPasswordRequirements(userName, password, out String failedCheck);
                    contained = !contained;

                    ostream.WriteLine(contained.ToString().ToLower());
                    ostream.WriteLine(failedCheck);
                }
                else
                    ostream.WriteLine("ERROR");

                ostream.Flush();

            }
            catch (Exception e)
            {
                ApplicationConfiguration.Instance.LogException(e);
            }
            finally
            {
                client.Close();
                client.Dispose();
            }

            GC.Collect();
        }
    }
}
