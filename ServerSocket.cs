using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace PriceTicker
{
    internal class ServerSocket
    {

        private static readonly Socket serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new();
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 8095;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];
        String? AdressIpClient = null;

        public void SetupServer()
        {
            String localIP = String.Empty;
            using (Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint? EndPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = EndPoint.Address.ToString();
            }
            var endPoint = new IPEndPoint(IPAddress.Parse(localIP), PORT);
            Properties.Settings.Default.ClientAdresseIP = IPAddress.Parse(localIP).ToString();
            Properties.Settings.Default.ClientPort = PORT.ToString();
            Debug.WriteLine("Configuration du server...");
            serverSocket.Bind(endPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            Debug.WriteLine("Server démarré");
        }
        private void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;
            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            IPEndPoint ClientIP = (IPEndPoint)socket.RemoteEndPoint;
            IPAddress ipadress = ClientIP.Address;
            //DeviceName = Dns.GetHostEntry(ipadress).HostName.ToString();
            AdressIpClient = ipadress.ToString();
            WriteLogs(ipadress.ToString() + " s'est connecté ! ");
            
            Debug.WriteLine(AdressIpClient + " s'est connecté !");
            socket.Send(Encoding.UTF8.GetBytes("Salut petit client !"));
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;


            try
            {
                received = current.EndReceive(AR);

            }
            catch (SocketException)
            {
                Debug.WriteLine("Client déconnecté");
                WriteLogs("Client déconnecté");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                clientSockets.Remove(current);
                return;
            }


            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.UTF8.GetString(recBuf).Replace("\n", "").Replace("\r", "");



            if (text.ToLower() != "")
            {
                String[] datas = text.Split(";");
                WriteLogs("ID reçu : " + datas[1]);
                List<String> ProductSpecList = new List<String>();
                ProductSpecList = MainWindow.gui.FindPriceById(datas[1], datas[0]);
                Debug.WriteLine("Type d'étiquette : " + datas[0]);
                Debug.WriteLine("Id envoyé au client : " + ProductSpecList[0]);
                Debug.WriteLine("Libellé envoyé au client : " + ProductSpecList[1]);
                Debug.WriteLine("Prix envoyé au client : " + ProductSpecList[2]);

                current.Send(Encoding.UTF8.GetBytes(ProductSpecList[0] + ";" + ProductSpecList[1] + ";" + ProductSpecList[2]));
            }
            else
            {
                IPEndPoint ClientIP = (IPEndPoint)current.RemoteEndPoint;
                Debug.WriteLine("Déconnexion du client en cours");
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                clientSockets.Remove(current);
                Debug.WriteLine("Client déconnecté");
                WriteLogs(AdressIpClient + " s'est déconnecté ");
                return;
            }

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }

        private void WriteLogs(String logTxt)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Logs.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(logTxt);
                }
                if (LogsWindow.gui != null)
                {
                    LogsWindow.gui.UpdateLogsTxt();
                }


            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(logTxt);
                }
                if (LogsWindow.gui != null)
                {
                    LogsWindow.gui.UpdateLogsTxt();
                }
                
            }

        }

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {

            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

    }
}
