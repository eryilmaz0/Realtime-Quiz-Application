import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccessTokenModel } from '../Models/accessTokenModel';
import { DataResponseModel } from '../Models/DataResponseModel';
import { LoginModel } from '../Models/loginModel';
import { RegisterModel } from '../Models/registerModel';
import { ResponseModel } from '../Models/ResponseModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl:string = "https://localhost:44305/api/auth/";


  constructor(private _http:HttpClient, private router:Router, private route:ActivatedRoute) { }


  public Register(registerModel:RegisterModel):Observable<ResponseModel>{

    return this._http.post<ResponseModel>(this.baseUrl+"Register", registerModel);
  }


  public Login(loginModel:LoginModel):Observable<DataResponseModel<AccessTokenModel>>{

    return this._http.post<DataResponseModel<AccessTokenModel>>(this.baseUrl+"Login", loginModel);
  }


  public get isAuthenticated():boolean{

    if(sessionStorage.getItem("AccessToken")){

      return true;
      
    }
    return false;
  }


  public get getAccessToken():AccessTokenModel{

    let accessToken:AccessTokenModel = JSON.parse(sessionStorage.getItem("AccessToken"));
     return accessToken;
    
  }


  public Logout(){

    if(this.isAuthenticated){
      sessionStorage.removeItem("AccessToken");
      this.router.navigate(["Home"])

    }
  }

}
