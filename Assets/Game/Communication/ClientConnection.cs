using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

using System.Net;
using System.IO;

namespace Assets.Game.Communication
{
    public class ClientConnection
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        private NetworkStream serverStream;
        private TcpListener listener;
        private TcpClient client; //To talk back to the client
        private NetworkStream clientStream;
        private BinaryWriter writer;
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public int ClientPort { get; set; }

        public delegate void MessageReceivedEventHandler(object sender, EventArgs args);
        public event MessageReceivedEventHandler MessageReceived;

        public ClientConnection()
        {
            ServerIP = "127.0.0.1";
            ServerPort = 6000;
            ClientPort = 7000;
        }
        public void StartConnection()
        {
            clientSocket.Connect(ServerIP, ServerPort);
            serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("JOIN#");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            serverStream.Close();
            clientSocket.Close();
        }
        private void Recieve()
        {
            /*try
            {
                while (true)
                {
                    clientSocket.Connect("127.0.0.1", 7000);
                    serverStream = clientSocket.GetStream();
                    byte[] inStream = new byte[10025];
                    serverStream.Read(inStream, 0, 10025);
                    string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                    Console.WriteLine(returndata+"\n");
                    Console.WriteLine();
                


                }
            }catch(Exception ex){}*/
            bool errorOcurred = false;
            Socket connection = null; //The socket that is listened to   

            try
            {
                //Creating listening Socket

                listener = new TcpListener(IPAddress.Parse(ServerIP), ClientPort);
                listener.Start();


                while (true)
                {
                    //connection is connected socket

                    connection = listener.AcceptSocket();

                    if (connection.Connected)
                    {
                        //To read from socket create NetworkStream object associated with socket
                        this.serverStream = new NetworkStream(connection);

                        SocketAddress sockAdd = connection.RemoteEndPoint.Serialize();
                        string s = connection.RemoteEndPoint.ToString();

                        List<Byte> inputStr = new List<byte>();

                        int asw = 0;
                        while (asw != -1)
                        {
                            asw = this.serverStream.ReadByte();
                            inputStr.Add((Byte)asw);
                        }

                        string reply = Encoding.UTF8.GetString(inputStr.ToArray());
                        OnMessageReceived(reply);

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Communication (RECEIVING) Failed! \n " + e.Message);
                errorOcurred = true;
            }
            finally
            {
                if (connection != null)
                    if (connection.Connected)
                        connection.Close();

            }
        }
        public void SendData(String message)
        {

            //Opening the connection
            this.client = new TcpClient();

            try
            {


                this.client.Connect("127.0.0.1", ClientPort);

                if (this.client.Connected)
                {
                    //To write to the socket
                    this.clientStream = client.GetStream();

                    //Create objects for writing across stream
                    this.writer = new BinaryWriter(clientStream);
                    Byte[] tempStr = Encoding.ASCII.GetBytes(message);

                    //writing to the port                
                    this.writer.Write(tempStr);
                    Console.WriteLine("\t Data: " + message + " is sent to server ");
                    this.writer.Close();
                    this.clientStream.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Communication (WRITING) failed ");
            }
            finally
            {
                this.client.Close();
            }
        }

        public void StartReceiving()
        {
            Thread iThread = new Thread(new ThreadStart(this.Recieve));
            iThread.Start();
        }

        protected virtual void OnMessageReceived(string message)
        {
            if (MessageReceived != null)
            {
                MessageReceivedEventArgs eventArgs = new MessageReceivedEventArgs();
                eventArgs.Message = message;
                MessageReceived(this, eventArgs);
            }
        }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
