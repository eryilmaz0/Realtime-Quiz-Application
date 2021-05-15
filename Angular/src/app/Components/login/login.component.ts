import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginModel } from 'src/app/Models/loginModel';
import { AuthService } from 'src/app/Services/auth.service';
import { ConnectionHubService } from 'src/app/Services/connection-hub.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm:FormGroup;

  constructor(private _authService:AuthService,
              private route:ActivatedRoute,
              private router:Router,
              private toastr:ToastrService,
              private connectionHubService:ConnectionHubService) 
              { }



  ngOnInit(): void 
  {
    sessionStorage.setItem("lastRoute","Login");
    this.CreateLoginForm();
  }



  CreateLoginForm(){

    this.loginForm = new FormGroup({
      email : new FormControl("", [Validators.required, Validators.email]),
      password : new FormControl("", [Validators.required,Validators.minLength(6)])
    });
  }



  SubmitForm(){

    console.log(this.loginForm);

    if(!this.loginForm.valid){

      this.toastr.error("Bilgilerinizi Doğru Giriniz.");
    }

    else{

      let loginModel:LoginModel =  Object.assign({},this.loginForm.value);
      this._authService.Login(loginModel).subscribe(response=>{

        sessionStorage.setItem("AccessToken",JSON.stringify(response.data));
        this.toastr.success(response.message);
        this.router.navigate(["Home"])

      }, responseError => {

        console.log(responseError);
        this.toastr.error(responseError.error.message);
      })
    }
  }



  StartConnectionHubConnection(){

    this.connectionHubService.CreateConnectionHub();
    this.connectionHubService.StartConnectionHub();

  }


}
