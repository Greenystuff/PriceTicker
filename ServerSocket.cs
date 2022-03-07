using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

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
            MainWindow.gui.IpAdressTxt.Text = IPAddress.Parse(localIP).ToString();
            MainWindow.gui.PortTxt.Text = PORT.ToString();
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
            //name = Dns.GetHostEntry(ipadress).HostName.ToString();
            AdressIpClient = ipadress.ToString();
            MainWindow.gui.UpdateLogText(ipadress.ToString() + " s'est connecté ! ", MainWindow.gui.LogBox);
            
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
                MainWindow.gui.UpdateLogText("Client déconnecté ", MainWindow.gui.LogBox);
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
                MainWindow.gui.UpdateLogText("ID reçu : " + text, MainWindow.gui.LogBox);
                MainWindow.gui.FindPriceById(text);
                Debug.WriteLine("ID reçu : " + text);
            }
            else
            {
                IPEndPoint ClientIP = (IPEndPoint)current.RemoteEndPoint;
                Debug.WriteLine("Déconnexion du client en cours");
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                clientSockets.Remove(current);
                Debug.WriteLine("Client déconnecté");
                MainWindow.gui.UpdateLogText(AdressIpClient + " s'est déconnecté ", MainWindow.gui.LogBox);
                return;
            }

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }

    }
}
