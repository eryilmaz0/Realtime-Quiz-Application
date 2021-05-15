import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatchHubService } from 'src/app/Services/match-hub.service';

@Component({
  selector: 'Home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private matchHubService:MatchHubService) { }

  ngOnInit(): void 
  {
    if(sessionStorage.getItem("lastRoute") == "Matches"){

      this.matchHubService.RemoveUsersMatch();
    }
    
    sessionStorage.setItem("lastRoute","Home");
  }

}
