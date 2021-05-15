import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ToastrService } from 'ngx-toastr';
import { AccessTokenModel } from 'src/app/Models/accessTokenModel';
import { ConnectedUserModel } from 'src/app/Models/ConnectedUserModel';
import { MatchModel } from 'src/app/Models/MatchMode';
import { AuthService } from 'src/app/Services/auth.service';
import { ConnectionHubService } from 'src/app/Services/connection-hub.service';
import { HubControllerService } from 'src/app/Services/hub-controller.service';
import { HubConnections } from 'src/app/Services/HubConnections';
import { MatchHubService } from 'src/app/Services/match-hub.service';

@Component({
  selector: 'app-match-page',
  templateUrl: './match-page.component.html',
  styleUrls: ['./match-page.component.css']
})
export class MatchPageComponent implements OnInit {


  userInMatch:boolean = false;
  userJoinedMatch:boolean = false;
  userIsReady:boolean = false;
  eventIsStarting:boolean = false;
  activeMatches:MatchModel[];
  currentUser:AccessTokenModel;


  constructor(private matchHubService:MatchHubService,
              private hubControllerService:HubControllerService, 
              private connectionHubService:ConnectionHubService,
              private toastrService:ToastrService,
              private authService:AuthService,
              private router:Router) { }


  ngOnInit(): void {

    this.currentUser = this.authService.getAccessToken;
    let lastRoute = sessionStorage.getItem("lastRoute");

    

      //RECONNECT HUBS
      console.log("Reconnecting");
      this.connectionHubService.CreateConnectionHub();
      this.connectionHubService.StartConnectionHub();
    

    this.GetMatches();
    this.StartMatchHubConnection();
    sessionStorage.setItem("lastRoute","Matches");
 
  }



  public StartMatchHubConnection():void{

    this.matchHubService.CreateMatchHub();
    this.matchHubService.StartMatchHub();
    this.ListenCreatedMatch();
    this.ListenMatchListChanged();
    this.ListenJoinedMatch();
    this.ListenSetUserIsReady();
    this.ListenEventIsStarting();
    this.ListenMatchTerminated();
  }


  public CreateMatch():void{

    this.matchHubService.CreateMatch();
  }


  public ListenCreatedMatch():void{

    HubConnections.MatchHubConnection.on("createdMatch", (result,state) => {

      if(result === true){

          if(state === 1){

            this.userInMatch = true;
          }

          else if(state === 2){

            this.userInMatch = false;
          }

      }

      else{

        this.toastrService.error("Bir Hata Oluştu.");
      }
    
    })
  }



  public ListenMatchListChanged(){

    HubConnections.MatchHubConnection.on("matchListChanged", matchList => {

      console.log("Match List Changed");
      this.activeMatches = matchList;
      console.log(this.activeMatches);
      
    })
  }



  public ListenJoinedMatch(){

    HubConnections.MatchHubConnection.on("joinedMatch", result => {

      if(result === true){

        this.toastrService.success("Kullanıcı İle Eşleşildi.");
        this.userJoinedMatch = true;
        return;
      }

      this.toastrService.error("Kullanıcıyla Eşleşilemedi.");
      
    })
  }



  public JoinMatch(targetUserId:number){

    this.matchHubService.JoinMatch(targetUserId);
    
  }




  public SetUserIsReady(){
    this.matchHubService.SetUserIsReady();
  }


  public ListenSetUserIsReady(){

    HubConnections.MatchHubConnection.on("setUserIsReady", result => {

      if(result === true ){

        if( this.eventIsStarting === false){
          this.toastrService.success("Karşı Kullanıcı Hazır Olduğunda Yarışma Başlayacak.");
          console.log("listen set user is ready");
        }
        
        this.userIsReady = true;
        console.log(this.userIsReady);
      }
      else if(result === false){
        this.toastrService.error("Durumunuz Hazır Olarak Ayarlanamadı.");
      }
    
      
    })

  }




  public ListenEventIsStarting(){

    HubConnections.MatchHubConnection.on("eventStarting", result => {

     
      if(result === true){

        this.toastrService.info("Yarışma Başlıyor.");
        console.log("Yarışma Başlıyor.");
        this.eventIsStarting = true;
        this.StartEvent();

        return;
      }

      else{
        this.toastrService.error("Yarışma Başlatılamadı.");
      }

      
      
    })

  }


  public ListenMatchTerminated(){

    HubConnections.MatchHubConnection.on("matchTerminated", result => {

      if(result === true){

        this.userInMatch = false;
        this.userJoinedMatch = false;
        this.toastrService.warning("Kullanıcı Eşleşmeyi Sonlandırdı.");
      }
    }) 
  }


  public GetMatches(){

    this.hubControllerService.GetMatches().subscribe(response => {

      
      this.activeMatches = response.data;
      console.log(this.activeMatches);
      
    })
  }



  public StartEvent(){
    setTimeout(()=>{ this.router.navigate(["/Competition"]) }, 2000)
  }
  
}
