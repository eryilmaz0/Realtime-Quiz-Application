import { Component, OnInit } from '@angular/core';
import { AccessTokenModel } from 'src/app/Models/accessTokenModel';
import { AuthService } from 'src/app/Services/auth.service';
import { ConnectionHubService } from 'src/app/Services/connection-hub.service';
import { MatchHubService } from 'src/app/Services/match-hub.service';

@Component({
  selector: 'Navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  
  accessToken:AccessTokenModel;
  constructor(public _authService:AuthService,
              private connectionHubService:ConnectionHubService,
              private matchHubService:MatchHubService) { }

  ngOnInit(): void {

  
    if(this._authService.isAuthenticated){
      this.accessToken = this._authService.getAccessToken;
    }

  }


  Logout(){

    this._authService.Logout();
    this.DisconnectMatchHub();
  }



  DisconnectMatchHub(){

    this.matchHubService.DisconnectMatchHub();
  }

}
