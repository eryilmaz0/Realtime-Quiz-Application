import { Component, OnInit } from '@angular/core';
import { AccessTokenModel } from 'src/app/Models/accessTokenModel';
import { AuthService } from 'src/app/Services/auth.service';
import { HubConnections } from 'src/app/Services/HubConnections';
import { MatchHubService } from 'src/app/Services/match-hub.service';

@Component({
  selector: 'app-competitions',
  templateUrl: './competitions.component.html',
  styleUrls: ['./competitions.component.css']
})
export class CompetitionsComponent implements OnInit {


  
  constructor(private _authService:AuthService, private matchHubService:MatchHubService) { }

  ngOnInit(): void {

    if(sessionStorage.getItem("lastRoute") == "Matches"){

      this.matchHubService.RemoveUsersMatch();
    }

    sessionStorage.setItem("lastRoute","Competitions");
  }



}
