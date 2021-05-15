using System.Collections.Generic;
using System.Linq;
using Quiz.App.Entities;
using Quiz.App.SignalR.Objects;

namespace Quiz.App.SignalR
{
    public static class SignalRConnectionManager
    {
        private static readonly List<ConnectedUser> _connectedUsers = new List<ConnectedUser>();
        

        public static void ConnectUser(ConnectedUser user)
        {
            var connectedUser = _connectedUsers.FirstOrDefault(x=>x.User.Id == user.User.Id);

            if (connectedUser != null)
            {
                //BAĞLANTIYI KESMEDEN TEKRAR BAĞLANIRSA
                _connectedUsers.Remove(connectedUser);
            }

            _connectedUsers.Add(user);

        }


        public static void DisconnectUser(long userId)
        {
            var user = _connectedUsers.FirstOrDefault(x => x.User.Id == userId);

            if (user != null)
            {
                _connectedUsers.Remove(user);
            }
        }


        public static ConnectedUser GetConnectedUser(long userId)
        {
            return _connectedUsers.FirstOrDefault(x => x.User.Id == userId);
        }



        public static List<ConnectedUser> GetConnectedUsers()
        {
            return _connectedUsers.ToList();
        }


    }
}