using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Sockets;
using game_structures;

namespace SocketServerTestApp
{
	public class ServerApp
	{

		private static int clientCount = 0;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			// Instantiate a CSocketServer object
			SocketServer socketServer = new SocketServer();
			// Start listening for connections
			socketServer.Start("localhost", 9000, 1024, null, 
				new Sockets.MessageHandler(MessageHandlerServer),
				new Sockets.SocketServer.AcceptHandler(AcceptHandler),
				new Sockets.CloseHandler(CloseHandler),
				new Sockets.ErrorHandler(ErrorHandler));
			Console.WriteLine("Waiting for a client connection on Machine: {0} Port: {1}", System.Environment.MachineName, 9000);
			// Stay here until you are ready to shutdown the server    
			Console.ReadLine();
			socketServer.Dispose();
		}

		public static void ErrorHandler(SocketBase socket, Exception pException)
		{
			Console.WriteLine(pException.Message);
		}
		public static void CloseHandler(SocketBase socket)
		{
			Console.WriteLine("Close Handler");
			Console.WriteLine("IpAddress: " + socket.IpAddress);
			DataProccessor.clientsDict.Remove(DataProccessor.GetClientId((SocketClient)socket));
		}
		/// <summary> Called when a message is extracted from the socket </summary>
		/// <param name="pSocket"> The SocketClient object the message came from </param>
		/// <param name="iNumberOfBytes"> The number of bytes in the RawBuffer inside the SocketClient </param>
		static public void MessageHandlerServer(SocketBase socket, int iNumberOfBytes)
		{
			try
			{
				SocketClient pSocket = ((SocketClient)socket);
				// Find a complete message
				String strMessage = System.Text.ASCIIEncoding.ASCII.GetString(pSocket.RawBuffer, 0, iNumberOfBytes);

				Console.WriteLine("Message=<{0}>", strMessage );
				DataProccessor.ManageData(pSocket, strMessage);
			}
			catch (Exception pException)
			{
				Console.WriteLine(pException.Message);
			}
		}

		/// <summary> Called when a socket connection is accepted </summary>
		/// <param name="pSocket"> The SocketClient object the message came from </param>
		static public void AcceptHandler(SocketClient pSocket)
		{
			Console.WriteLine("Accept Handler");
			Console.WriteLine("IpAddress: " + pSocket.IpAddress);
			DataProccessor.clientsDict.Add(clientCount++, pSocket);
		}

	}
}
