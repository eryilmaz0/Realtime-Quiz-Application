using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Quiz.App.Entities;
using Quiz.App.SignalR;
using Quiz.App.SignalR.Objects;

namespace Quiz.App.Hubs
{
    
   [Authorize]
    public class ConnectionHub :Hub
    {
       
    }
}