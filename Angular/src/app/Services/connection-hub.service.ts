import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from './auth.service';
import {HubConnections } from './HubConnections';


@Injectable({
  providedIn: 'root'
})
export class ConnectionHubService {

  constructor(private authService:AuthService, private toastrService:ToastrService) { }

  public CreateConnectionHub():void{

   HubConnections.ConnectionHubConnection = new HubConnectionBuilder()
    .withUrl('https://localhost:44305' + '/connectionHub',
     {accessTokenFactory : () => this.authService.getAccessToken.token}).build();
    
  }



  public StartConnectionHub():void{

    HubConnections.ConnectionHubConnection.start().then(()=>{
      console.log("Connected to Connection Hub.");
    }).catch  ((err)=>{
      this.toastrService.error("Connection Hub'a Bağlanılamadı.");
    })
  }



  public DisconnectConnectionHub():void{

    HubConnections.ConnectionHubConnection.stop();
  }
}
