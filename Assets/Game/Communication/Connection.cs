using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Assets.Game.Communication
{
    public class Connection
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();//creating a connection to the server
        private NetworkStream serverStream;//to send data using the stream
        private TcpListener listener;//listen to the port
        private TcpClient client; //To talk back to the client
        private NetworkStream clientStream;//stream to send data
        private BinaryWriter writer;//to write to the allocated buffer
        public string ServerIP { get; set; }//ip address of server(127.0.0.1 if local host)
        public int ServerPort { get; set; }//server sends data using this port
        public int ClientPort { get; set; }//server recieves data using this port

        public delegate void MessageReceivedEventHandler(object sender, EventArgs args);
        public event MessageReceivedEventHandler MessageReceived;

        public Connection()
        {
            //initial configeration
            ServerIP = "127.0.0.1";
            ServerPort = 6000;
            ClientPort = 7000;
        }
        public void StartConnection()
        {
            //to start the connection a connetion is made to the server port and the using the assigned port Join request is sent
            clientSocket.Connect(ServerIP, ServerPort);
            serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("JOIN#");
            serverStream.Write(outStream, 0, outStream.Length); //sends join to the server
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
             * }
            }catch(Exception ex){}*/
            bool errorOcurred = false;
            Socket connection = null; //The socket listends to the messages sent by server 

            try
            {
                //Creating listening Socket

                listener = new TcpListener(IPAddress.Parse(ServerIP), ClientPort);//creating a connection to the server and listens to it
                listener.Start();


                while (true)
                {
                    //connection is connected socket

                    connection = listener.AcceptSocket();//socket recieved by the listener

                    if (connection.Connected)//if connected
                    {
                        //To read from socket create NetworkStream object associated with socket
                        this.serverStream = new NetworkStream(connection);

                        SocketAddress sockAdd = connection.RemoteEndPoint.Serialize();
                        string s = connection.RemoteEndPoint.ToString();

                        List<Byte> inputStr = new List<byte>();
                        //read byte by byte and add them to a list
                        int asw = 0;
                        while (asw != -1)
                        {
                            asw = this.serverStream.ReadByte();
                            inputStr.Add((Byte)asw);
                        }

                        string reply = Encoding.UTF8.GetString(inputStr.ToArray());//convert to a C# string object
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
                if (errorOcurred)
                    this.StartReceiving();

            }
        }
        public void SendData(String message)
        {
            //Opening the connection
            client = new TcpClient();

            try
            {

                client.Connect(ServerIP, ServerPort);//connects to the server

                if (client.Connected)
                {
                    //To write to the socket
                    clientStream = client.GetStream();//network stream is assigned

                    //Create objects for writing across stream
                    writer = new BinaryWriter(clientStream);
                    Byte[] tempStr = Encoding.ASCII.GetBytes(message);

                    //writing to the port                
                    writer.Write(tempStr);//write to the stream object
                    Console.WriteLine("\t Data: " + message + " is sent to server ");
                    writer.Close();
                    clientStream.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Communication (WRITING) failed ");
                Console.WriteLine(e.GetBaseException());
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
            //the recieve  method runs in a seperte thread

        }

        protected virtual void OnMessageReceived(string message)
        {
            if (MessageReceived != null)
            {
                //send the recieved meesage to MessageReceivedEventArgs object
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
