using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Quiz.App.Entities;
using Quiz.App.SignalR.Objects;

namespace Quiz.App.SignalR
{
    public class SignalRMatchingManager
    {

        private static readonly List<Match> _matchedUsers = new List<Match>();


        //1. KULLANICI KATILIRKEN
        public static int CreateMatch(ConnectedUser currentUser)
        {
            if (!CheckUserIsConnected(currentUser.User.Id))
            {
                return 0;
            }


            if (GetMatchByUser(currentUser) != null)
            {
                RemoveMatch(currentUser);
                return 2;
            }

            else
            {
                _matchedUsers.Add(new Match() { FirstUser = currentUser });
            }

            return 1;

        }





        //2. KULLANICI KATILIRKEN
        public static bool JoinMatch(ConnectedUser currentUser, ConnectedUser targetUser, ref string targetUserConnectionId)
        {

            if (targetUser == null)
            {
                return false;
            }

            //BU KULLANICI EŞLEŞME BEKLİYOR MU?
            if (GetWaitingMatchByUser(targetUser) == null)
            {
                return false;
            }

            var getWaitingMatch = _matchedUsers.FirstOrDefault(x => x.FirstUser.User.Id == targetUser.User.Id && x.SecondUser == null);
            targetUserConnectionId = getWaitingMatch.FirstUser.ConnectionId;

            _matchedUsers.Remove(getWaitingMatch);
            _matchedUsers.Insert(0, new Match(targetUser,currentUser));
            return true;
        }




        public static HubResult RemoveMatch(ConnectedUser currentUser)
        {
            var match = GetWaitingMatchByUser(currentUser);

            if (match != null)
            {
                _matchedUsers.Remove(match);
                return new HubResult(1);
            }

            match = GetMatchByUser(currentUser);

            if (match != null)
            {
                var oppositeConnectionId = GetOppositeUserConnection(currentUser.User.Id);
                _matchedUsers.Remove(match);
                return new HubResult(2,oppositeConnectionId);
            }

            return new HubResult(3);
        }




        public static bool SetUserIsReady(ConnectedUser currentUser, ref string firstConnectionId, ref string secondConnectionId)
        {
            if (!CheckUserIsConnected(currentUser.User.Id))
            {
                return false;
            }


            if (GetMatchByUser(currentUser) == null)
            {
                return false;
            }

            if (_matchedUsers.Where(x => x.SecondUser != null).FirstOrDefault(x => x.FirstUser.User.Id == currentUser.User.Id) != null)
            {
                var match = _matchedUsers.Where(x => x.SecondUser != null).FirstOrDefault(x => x.FirstUser.User.Id == currentUser.User.Id);
                firstConnectionId = match.FirstUser.ConnectionId;
                secondConnectionId = match.SecondUser.ConnectionId;

                _matchedUsers.Where(x => x.SecondUser != null).FirstOrDefault(x => x.FirstUser.User.Id == currentUser.User.Id).FirstUserIsReady = true;
                return true;
            } 

            else if (_matchedUsers.Where(x => x.SecondUser != null).FirstOrDefault(x => x.SecondUser.User.Id == currentUser.User.Id) != null)
            {
                var match = _matchedUsers.Where(x => x.SecondUser != null).FirstOrDefault(x => x.SecondUser.User.Id == currentUser.User.Id);
                firstConnectionId = match.FirstUser.ConnectionId;
                secondConnectionId = match.SecondUser.ConnectionId;

                _matchedUsers.Where(x => x.SecondUser != null).FirstOrDefault(x => x.SecondUser.User.Id == currentUser.User.Id).SecondUserIsReady = true;
                return true;
            }

            return false;


        }





        private static bool CheckUserIsConnected(long userId)
        {
            var user = SignalRConnectionManager.GetConnectedUser(userId);

            if (user == null)
            {
                return false;
            }

            return true;
        }




        //KULLANICININ OLDUĞU EŞLEŞME KAYDINI GETİR
        public static Match GetMatchByUser(ConnectedUser user)
        {
            var match = _matchedUsers.FirstOrDefault(x => x.FirstUser.User.Id == user.User.Id);

            //NULL İSE, İKİNCİ KULLANICILARI KONTROL ET
            if (match == null)
            {
                return _matchedUsers.Where(x => x.SecondUser != null).FirstOrDefault(x=>x.SecondUser.User.Id == user.User.Id);
               
            }

            return match;
        }





        public static Match GetActiveMatch(int userId)
        {
            return _matchedUsers.Where(x => x.SecondUser != null)
                .FirstOrDefault(x => (x.FirstUser.User.Id == userId || x.SecondUser.User.Id == userId) 
                                     && x.FirstUserIsReady == true && x.SecondUserIsReady == true);
        }





        //KULLANICININ OLDUĞU BEKLEYEN EŞLEŞME KAYDINI GETİR
        private static Match GetWaitingMatchByUser(ConnectedUser user)
        {
            return _matchedUsers.FirstOrDefault(x => (x.FirstUser.User.Id == user.User.Id) && (x.SecondUser == null));
        }





        public static List<Match> GetMatches()
        {
            return _matchedUsers.ToList();
        }





        public static string GetOppositeUserConnection(long userId)
        {
            var match = _matchedUsers.Where(x=>x.SecondUser != null)
                        .FirstOrDefault(x => x.FirstUser.User.Id == userId || x.SecondUser.User.Id == userId);


            return match.FirstUser.User.Id == userId 
                   ? match.SecondUser.ConnectionId 
                   : match.FirstUser.ConnectionId;
        }
    }
}