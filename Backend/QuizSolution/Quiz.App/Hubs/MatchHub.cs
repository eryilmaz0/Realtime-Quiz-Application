using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Quiz.App.Entities;
using Quiz.App.SignalR;
using Quiz.App.SignalR.Objects;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Quiz.App.Hubs
{
    [Authorize]
    public class MatchHub : Hub
    {

        private readonly UserManager<User> _userManager;



        public MatchHub(UserManager<User> userManager)
        {
            _userManager = userManager;
        }





        public override Task OnConnectedAsync()
        {
            var claims = Context.User.Claims;
            var userId = Convert.ToInt64(claims.FirstOrDefault(x => x.Type == "userId").Value);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            SignalRConnectionManager.ConnectUser(new ConnectedUser(user, Context.ConnectionId));
            return base.OnConnectedAsync();
        }




        public async Task CreateMatch()
        {
            var claims = Context.User.Claims;
            var userId = Convert.ToInt64(claims.FirstOrDefault(x => x.Type == "userId").Value);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            var result = SignalRMatchingManager.CreateMatch(new ConnectedUser(user, Context.ConnectionId));

            if (result == 0)
            {
                await Clients.Caller.SendAsync("createdMatch", false,0);
            }

            await Clients.Caller.SendAsync("createdMatch", true, result);
            await Clients.All.SendAsync("matchListChanged", SignalRMatchingManager.GetMatches());
        }




        public async Task JoinMatch(long targetUserId)
        {
            var claims = Context.User.Claims;
            var userId = Convert.ToInt64(claims.FirstOrDefault(x => x.Type == "userId").Value);

            var currentUser = SignalRConnectionManager.GetConnectedUser(userId);
            var targetUser = SignalRConnectionManager.GetConnectedUser(targetUserId);;
            string targetUserConnectionId = string.Empty;
            
            var result = SignalRMatchingManager.JoinMatch(currentUser, targetUser, ref targetUserConnectionId);

            if (!result)
            {
                await Clients.Caller.SendAsync("joinedMatch", false);
            }

            await Clients.Caller.SendAsync("joinedMatch", true);
            await Clients.Client(targetUserConnectionId).SendAsync("joinedMatch", true);
            await Clients.All.SendAsync("matchListChanged", SignalRMatchingManager.GetMatches());

        }




        public async Task SetUserIsReady()
        {
            var claims = Context.User.Claims;
            var userId = Convert.ToInt64(claims.FirstOrDefault(x => x.Type == "userId").Value);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            string firstConnectionId = string.Empty;
            string secondConnectionId = string.Empty;

            var result = SignalRMatchingManager.SetUserIsReady(new ConnectedUser(user, Context.ConnectionId), ref firstConnectionId, ref secondConnectionId);

            if (result)
            {
                var match = SignalRMatchingManager.GetMatchByUser(new ConnectedUser(user, Context.ConnectionId));

                if (match.FirstUserIsReady == true && match.SecondUserIsReady == true)
                {

                    await Clients.All.SendAsync("matchListChanged", SignalRMatchingManager.GetMatches());
                    await Clients.Client(firstConnectionId).SendAsync("eventStarting", true);
                    await Clients.Client(secondConnectionId).SendAsync("eventStarting", true);
                    await Clients.Caller.SendAsync("setUserIsReady", true);

                }

                else
                {
                    await Clients.All.SendAsync("matchListChanged", SignalRMatchingManager.GetMatches());
                    await Clients.Caller.SendAsync("setUserIsReady", true);

                }
            }

            else
            {
                await Clients.Caller.SendAsync("setUserIsReady", false);
            }

           

        }



        public async Task GetRandomQuestion(string oppositeUserConnectionId)
        {

            var randomQuestion = SignalRQuestionManager.GetRandomPictureQuestion();
            await Clients.Caller.SendAsync("GetRandomQuestion", randomQuestion);
            await Clients.Client(oppositeUserConnectionId).SendAsync("GetRandomQuestion", randomQuestion);
        }






        public async Task CheckAnswer(byte questionId, string answer)
        {
            var claims = Context.User.Claims;
            var userId = Convert.ToInt64(claims.FirstOrDefault(x => x.Type == "userId").Value);

            var answerResult = SignalRQuestionManager.CheckPictureQuestionAnsver(questionId, answer);

            if (answerResult)
            {
                await Clients.Client(SignalRMatchingManager.GetOppositeUserConnection(userId)).SendAsync("oppositeKnewTheQuestion", true);
                await Clients.Caller.SendAsync("checkAnswer", true);
                return;
            }

            await Clients.Caller.SendAsync("checkAnswer", false);


        }




        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var claims = Context.User.Claims;
            var userId = Convert.ToInt64(claims.FirstOrDefault(x => x.Type == "userId").Value);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            var result = SignalRMatchingManager.RemoveMatch(new ConnectedUser(user, Context.ConnectionId));

            if (result.Result == 1)
            {
                 Clients.All.SendAsync("matchListChanged", SignalRMatchingManager.GetMatches());
            }


            if (result.Result == 2)
            {
                Clients.All.SendAsync("matchListChanged", SignalRMatchingManager.GetMatches());
                Clients.Client(result.OppositeConnectionId).SendAsync("matchTerminated",true);
            }

            //2 İSE DİĞER KULLANICIYA DA HABER VER.

            SignalRConnectionManager.DisconnectUser(userId);
            return base.OnDisconnectedAsync(exception);
        }




        public async Task RemoveMatch()
        {
            var claims = Context.User.Claims;
            var userId = Convert.ToInt64(claims.FirstOrDefault(x => x.Type == "userId").Value);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            var result = SignalRMatchingManager.RemoveMatch(new ConnectedUser(user, Context.ConnectionId));

            if (result.Result == 1)
            {
               await Clients.All.SendAsync("matchListChanged", SignalRMatchingManager.GetMatches());
            }

            if (result.Result == 2)
            {
               await Clients.All.SendAsync("matchListChanged", SignalRMatchingManager.GetMatches());
               await Clients.Client(result.OppositeConnectionId).SendAsync("matchTerminated",true);
            }
        }




        public async Task SendMessage(string message)
        {
            var claims = Context.User.Claims;
            var userId = Convert.ToInt64(claims.FirstOrDefault(x => x.Type == "userId").Value);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            
            ChatMessage chatMessage = new ChatMessage(user,message);
            await Clients.Caller.SendAsync("messageSended", chatMessage);
            await Clients.Client(SignalRMatchingManager.GetOppositeUserConnection(userId)).SendAsync("messageSended", chatMessage);


        }


        
    }
}