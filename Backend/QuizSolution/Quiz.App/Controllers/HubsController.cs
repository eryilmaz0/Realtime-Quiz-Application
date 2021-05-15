using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Quiz.App.Entities;
using Quiz.App.ResponseModels;
using Quiz.App.SignalR;
using Quiz.App.SignalR.Objects;

namespace Quiz.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubsController : ControllerBase
    {

        private readonly UserManager<User> _userManager;


        public HubsController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        [Route("ConnectedUsers")]
        public IActionResult GetConnectedUsersToHub()
        {
            return Ok(new DataResponseModel<List<ConnectedUser>>(SignalRConnectionManager.GetConnectedUsers(),true));
        }


        [HttpGet]
        [Route("Matches")]
        public IActionResult GetMatches()
        {
            return Ok(new DataResponseModel<List<Match>>(SignalRMatchingManager.GetMatches(), true));
        }



        [HttpGet]
        [Authorize]
        [Route("ActiveMatch")]
        public async Task<IActionResult> GetActiveMatch()
        {
            var user = await _userManager.GetUserAsync(User);
            var activeMatch = SignalRMatchingManager.GetActiveMatch(user.Id);

            if (activeMatch == null)
            {
                return BadRequest(new DataResponseModel<Match>(false));
            }

            return Ok(new DataResponseModel<Match>(activeMatch, true));
        }

    }
}
