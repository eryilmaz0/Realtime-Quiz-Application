import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnectionBuilder } from '@aspnet/signalr';
import { ToastrService } from 'ngx-toastr';
import { GetQuestionModel } from '../Models/GetQuestionModel';
import { MatchModel } from '../Models/MatchMode';
import { AuthService } from './auth.service';
import { HubConnections } from './HubConnections';

@Injectable({
  providedIn: 'root'
})
export class MatchHubService {

  constructor(private authService:AuthService, private toastrService:ToastrService) { }


  public CreateMatchHub():void{

    HubConnections.MatchHubConnection =  new HubConnectionBuilder()
    .withUrl('https://localhost:44305' + '/matchHub',
     {accessTokenFactory : () => this.authService.getAccessToken.token}).build();

  }


  public StartMatchHub():void{

    HubConnections.MatchHubConnection.start().then(()=>{
      console.log("Connected to Match Hub.");
    }).catch  ((err)=>{
      this.toastrService.error("Match Hub'a Bağlanılamadı.");
    })
  }



  public DisconnectMatchHub():void{

    HubConnections.MatchHubConnection.stop();
  }


  public CreateMatch():void{

    HubConnections.MatchHubConnection.invoke("CreateMatch")
  }


  public JoinMatch(targerUserId:number):void{

    HubConnections.MatchHubConnection.invoke("JoinMatch",targerUserId);
  }


  public SetUserIsReady():void{

    HubConnections.MatchHubConnection.invoke("SetUserIsReady");
  }



  public RemoveUsersMatch():void{

    HubConnections.MatchHubConnection.invoke("RemoveMatch");
  }



  public GetRandomQuestion(oppositeUserConnectionId:string):void{

    HubConnections.MatchHubConnection.invoke("GetRandomQuestion", oppositeUserConnectionId);
  }


  public CheckAnswer(questionId:number, answer:string):void{

    HubConnections.MatchHubConnection.invoke("CheckAnswer", questionId, answer);
  }


  public SendMessage(message:string):void{

    HubConnections.MatchHubConnection.invoke("SendMessage", message);
  }

}
