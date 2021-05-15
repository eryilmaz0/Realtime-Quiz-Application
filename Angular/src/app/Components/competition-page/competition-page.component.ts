import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { interval } from 'rxjs';
import { ChatMessageModel } from 'src/app/Models/ChatMessageModel';
import { ConnectedUserModel } from 'src/app/Models/ConnectedUserModel';
import { GetQuestionModel } from 'src/app/Models/GetQuestionModel';
import { MatchModel } from 'src/app/Models/MatchMode';
import { PictureQuestionModel } from 'src/app/Models/PictureQuestionModel';
import { AuthService } from 'src/app/Services/auth.service';
import { HubControllerService } from 'src/app/Services/hub-controller.service';
import { HubConnections } from 'src/app/Services/HubConnections';
import { MatchHubService } from 'src/app/Services/match-hub.service';

@Component({
  selector: 'app-competition-page',
  templateUrl: './competition-page.component.html',
  styleUrls: ['./competition-page.component.css']
})
export class CompetitionPageComponent implements OnInit {

  activeMatch:MatchModel;
  activePage:boolean = false;
  matchTerminated:boolean = false;

  firstUser:ConnectedUserModel;
  secondUser:ConnectedUserModel;
  firstUsersPoint:number = 0;
  secondUsersPoint:number = 0;

  pageRendered:boolean = false;


  activeQuestion:PictureQuestionModel = null;
  showPicture:boolean = false;
  disableTextbox:boolean = true;


  secondTimer:number = 5;
  timerIsActive:boolean = false;


  eventFinished:boolean = false;
  currentUserWon:boolean = false;
  oppositeUserWon:boolean = false;


  chatMessages:ChatMessageModel[] = []; 

  constructor(private _hubControllerService:HubControllerService, 
              private router:Router, 
              private toastrService:ToastrService,
              private matchHubService:MatchHubService,
              private authService:AuthService) { }

  ngOnInit(): void {

    
    this._hubControllerService.GetActiveMatch().subscribe(response => {

      this.StartConfigurations(response.data);

      setTimeout(()=>{ 
        this.activePage = true;
        this.toastrService.success("Yarışma Başlıyor"); 
        this.pageRendered = true;
        this.DecreaseSecondTimer();
        
       
        setTimeout(()=>{ 
          
          this.GetRandomQuestion();
        }, 5000)
         
        
      }, 2000)
     
      
      
    }, error => {

      if(error.error.success === false){

        setTimeout(()=>{ 
          this.router.navigate(["Match"]);
          this.toastrService.error("Yarışmak İçin Eşleşmeniz Gerekiyor."); 
        }, 2000)
        
      }

      else{
        setTimeout(()=>{ 
          this.router.navigate(["Match"])
          this.toastrService.error("Yarışmaya Başlarken Bir Hata Oluştu."); 
        }, 2000)
       
      }
      
        
    })

  }



  public StartConfigurations(response:MatchModel){

    this.ListenMatchTerminated();
    this.ListenGetRandomQuestion();
    this.ListenCheckAnswer();
    this.ListenOppositeKnewTheQuestion();
    this.ListenMessageSended();
    this.firstUser = response.firstUser;
    this.secondUser = response.secondUser;

  }


  public ListenMatchTerminated(){

    HubConnections.MatchHubConnection.on("matchTerminated", result => {

      if(result === true && this.eventFinished === false){

        this.matchTerminated = true;
        setTimeout(()=>{ 
          this.router.navigate(["Match"])
        }, 2000)
        
      }
    }) 
  }



  public ClearInput(input){
    input.value = "";
  }



  public SendAnswer(input){

      var hasNumber = /\d/;   
      if(hasNumber.test(input.value) == true){

        this.toastrService.error("Cevabınız Sayı İçeremez.");
        input.value = "";
      }

      else if(input.value == null ||input.value.length <= 0 || input.value == ""){

        this.toastrService.error("Lütfen Cevap Giriniz.")
      }

      else if(this.disableTextbox === false){

       this.matchHubService.CheckAnswer(this.activeQuestion.id, input.value);
      }

  }



  public GetRandomQuestion(){

    if(this.firstUser.user.id === this.authService.getAccessToken.userId){

      this.matchHubService.GetRandomQuestion(this.secondUser.connectionId);
    }
    
    
  }



  public ListenGetRandomQuestion(){

    HubConnections.MatchHubConnection.on("GetRandomQuestion", (randomQuestion:PictureQuestionModel) => {

      
      this.activeQuestion = randomQuestion;
      setTimeout(()=>{ 
        this.showPicture = true;
        this.disableTextbox = false;
      }, 1000)
    
    })
  }




  public ListenCheckAnswer(){

    HubConnections.MatchHubConnection.on("checkAnswer", result => {

      (document.getElementById("AnswerBox") as HTMLInputElement).value = ''
      
      if(result === true)
      {
        this.disableTextbox = true;
        this.toastrService.success("Doğru Cevap!");
        this.IncreaseCurrentUserPoint();
       
        setTimeout(()=>{ 
          this.showPicture = false;
          this.GetRandomQuestion();
        }, 2000)
      }

      else
      {
        this.toastrService.error("Yanlış Cevap.");
      }
        
    
    })
  }



  public ListenOppositeKnewTheQuestion(){

    HubConnections.MatchHubConnection.on("oppositeKnewTheQuestion", result => {

      

      if(result === true){

        (document.getElementById("AnswerBox") as HTMLInputElement).value = ''
        this.disableTextbox = true;
        this.IncreaseOppositeUserPoint();
        this.toastrService.info("Rakibiniz Soruyu Doğru Cevapladı.");

        setTimeout(()=>{ 
          this.showPicture = false;
          this.GetRandomQuestion();
        }, 2000)
      } 
    
    })
  }




  public ListenMessageSended(){

    HubConnections.MatchHubConnection.on("messageSended", (result:ChatMessageModel) => {

      console.log(result);
      this.chatMessages.push(result);
    })
  }



  public SendMessage(chatInput){

    if(chatInput.value == null ||chatInput.value.length <= 0 || chatInput.value == ""){

      this.toastrService.error("Lütfen Mesaj Giriniz.")
    }

    else{
      this.matchHubService.SendMessage(chatInput.value);
      chatInput.value = "";
    }
    
  }



  private IncreaseCurrentUserPoint(){

  

    if(this.firstUser.user.id === this.authService.getAccessToken.userId)
    {
      this.firstUsersPoint++;

      if(this.firstUsersPoint === 1){
        this.eventFinished = true;
        this.currentUserWon = true;
       
      }
    }
        
    else if(this.secondUser.user.id === this.authService.getAccessToken.userId)
    {
      this.secondUsersPoint++;

      if(this.secondUsersPoint === 1){
        this.eventFinished = true;
        this.currentUserWon = true;
       
      }
    }
        
      
  }



  private IncreaseOppositeUserPoint(){

    if(this.firstUser.user.id !== this.authService.getAccessToken.userId)
    {
      this.firstUsersPoint++;

      if(this.firstUsersPoint === 1)
      {
        this.eventFinished = true;
        this.oppositeUserWon = true;
      }
    }
   
else if(this.secondUser.user.id !== this.authService.getAccessToken.userId)
{

  this.secondUsersPoint++;

  if(this.secondUsersPoint === 1)
      {
        this.eventFinished = true;
        this.oppositeUserWon = true;
      }
}
    

  }


  private DecreaseSecondTimer(){

    this.timerIsActive = true;

    setTimeout(()=>{ 
      if(this.secondTimer > 0){

        this.secondTimer--;

        setTimeout(()=>{ 
          if(this.secondTimer > 0){
    
            this.secondTimer--;
    
            setTimeout(()=>{ 
              if(this.secondTimer > 0){
        
                this.secondTimer--;
        
                setTimeout(()=>{ 
                  if(this.secondTimer > 0){
            
                    this.secondTimer--;
            
                    setTimeout(()=>{ 
                      if(this.secondTimer > 0){
                
                        this.secondTimer--;
                        this.activePage = true;
                        this.pageRendered = true;
                        this.timerIsActive = false;
                      }
                    }, 1000)
                  }
                }, 1000)
              }
            }, 1000)
          }
        }, 1000)
      }
    }, 1000)

    


    

    

    


   


    
  }






  


  

}
