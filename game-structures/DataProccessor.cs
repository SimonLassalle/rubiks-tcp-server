using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic; 

using System.Runtime.Serialization.Json;
using Sockets;

namespace game_structures
{
    public class DataProccessor
    {
        public static Dictionary<int, SocketClient> clientsDict = new Dictionary<int, SocketClient>();

        public static void ManageData(SocketClient socket, string message)
        {
            /**
            * The data must follow this structure:
            *	method;;parameter1;;parameter2;;...
            **/
            Console.WriteLine(message);
            string[] data = message.Split(new string[] { ";;" }, StringSplitOptions.None);
            string method = data[0];
            int fromId = GetClientId(socket);
            switch (method) 
            {
                // Connect: creates the player instance,
                // which has to be deleted when the client
                // leaves the game server
                case "login":
                    LogIn(fromId, data);
                    break;
                
                // LogOut: delete the player from the
                // PlayerManager list
                case "logout":
                    LogOut(fromId);
                    break;			
                
                // Create: creates a match with a given UNIQUE name
                case "create":
                    CreateRoom(fromId, data);
                    break;
                
                // GetMatches: get all matches that are created
                case "getRooms":
                    GetRooms(fromId);
                    break;
                
                case "join":
                    JoinRoom(fromId, data);
                    break;
                    
                case "leave":
                    LeaveRoom(fromId, data);
                    break;
                    
                // case "getMatch":
                //     GetMatch(fromId);
                //     break;
                    
                case "ready":
                    Ready(fromId);
                    break;
                    
                // case "sendMap":
                //     SendMap(fromId, data);
                //     break;
                
                // Ping: ping the server
                case "ping":
                    Ping(fromId);
                    break;
                    
                // Sendto: send a message to a given client
                case "sendto":
                    SendTo(fromId, data[1], data[2]);
                    break;
                    
                default:
                    Send(fromId, "unknown;;" + message);
                    break;
            }
        }
        
        public static void LogIn(int fromId, string[] data)
        {
            try
            {
                Player p = PlayerManager.GetInstance().CreatePlayer(fromId, data[1]);
                Send(fromId, "login;;success;;" + p.id);
            }
            catch (Exception e)
            {
                Send(fromId, "login;;failed;;" + e.Message);
            }	
        }
        
        public static void LogOut(int fromId)
        {
            try
            {
                PlayerManager.GetInstance().DeletePlayer(fromId);
                Send(fromId, "logout;;success");
            }
            catch (Exception e)
            {
                Send(fromId, "logout;;failed;;" + e.Message);
            }		
        }
        
        public static void CreateRoom(int fromId, string[] data)
        {
            try
            {
                int maxPlayers = Int32.Parse(data[2]);
                RoomManager.GetInstance().CreateRoom(data[1], maxPlayers);
                Send(fromId, "create;;success;;" + data[1]);
            }
            catch (Exception e)
            {
                Send(fromId, "create;;failed;;" + e.Message);
            }
        }
        
        public static void Ready(int fromId)
        {
            try
            {
                //RoomManager.GetInstance().PlayerIsReady(fromId);
                Send(fromId, "ready;;success");
            }
            catch (Exception e)
            {
                Send(fromId, "ready;;failed;;" + e.Message);
            }
        }
        
        // public static void SendMap(int fromId, string[] data)
        // {
        //     byte[] byteArray = Encoding.ASCII.GetBytes(data[1]);
        //     DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Map));
        //     MemoryStream stream = new MemoryStream(byteArray);
        //     stream.Position = 0;
        //     Map map = (Map)ser.ReadObject(stream);
        //     PlayerManager.GetInstance().GetPlayer(fromId).map = map;
        //     Match match = MatchManager.GetInstance().GetMatchByPlayer(fromId);
        //     foreach	(int id in match.players.Keys)
        //     {
        //         if (id != fromId)
        //         {
        //             Send(id, "remoteChange;;success;;" + data[1]);
        //         }
        //     }
        //     match.CheckPlayerScore(fromId);
        // }
        
        public static void GetRooms(int fromId)
        {
            try
            {
                List<Room> rooms = RoomManager.GetInstance().GetRooms();
                
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Room>));
                MemoryStream stream = new MemoryStream();
                ser.WriteObject(stream, rooms);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                
                string json = sr.ReadToEnd();
                Send(fromId, "getRooms;;success;;" + json);
            }
            catch (Exception e)
            {
                Send(fromId, "getRooms;;failed;;" + e.Message);
            }
        }
        
        public static void JoinRoom(int fromId, string[] data)
        {
            try
            {
                RoomManager.GetInstance().JoinRoom(data[1], PlayerManager.GetInstance().GetPlayer(fromId));
                Send(fromId, "join;;success;;" + data[1]);
            }
            catch (Exception e)
            {
                Send(fromId, "join;;failed;;" + e.Message);
            }
        }
        
        public static void LeaveRoom(int fromId, string[] data)
        {
            try
            {
                RoomManager.GetInstance().LeaveRoom(data[1], PlayerManager.GetInstance().GetPlayer(fromId));
                Send(fromId, "leave;;success");
            }
            catch (Exception e)
            {
                Send(fromId, "leave;;failed;;" + e.Message);
            }
        }
        
        public static void SendTo(int fromId, string id, string message)
        {
            if (Int32.TryParse(id, out int toId))
            {
                Send(toId, message);
                Send(fromId, "sendto;;success;;id " + id);
            }
            else
            {
                Send(fromId, "sendto;;failed;;Invalid id " + id);
            }
        }
        
        public static void Ping(int fromId) 
        {
            Send(fromId, "pong");
        }
        
        public static void Send(int id, string message)
        {
            clientsDict[id].Send(message);
        }

        public static int GetClientId(SocketClient pSocket)
		{
			foreach (int key in clientsDict.Keys)
			{
				if (pSocket == clientsDict[key]){
					return key;
				}
			}
			return -1;
		}
    }
}