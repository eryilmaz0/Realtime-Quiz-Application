import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConnectedUserModel } from '../Models/ConnectedUserModel';
import { DataResponseModel } from '../Models/DataResponseModel';
import { MatchModel } from '../Models/MatchMode';

@Injectable({
  providedIn: 'root'
})
export class HubControllerService {

  private baseUrl:string =  "https://localhost:44305/api/hubs/";
  constructor(private _http:HttpClient) { }


 public GetConnectedUsers():Observable<DataResponseModel<ConnectedUserModel[]>>{

    return this._http.get<DataResponseModel<ConnectedUserModel[]>>(this.baseUrl+"ConnectedUsers");
 }


 public GetMatches():Observable<DataResponseModel<MatchModel[]>>{

  return this._http.get<DataResponseModel<MatchModel[]>>(this.baseUrl+"Matches");
 }


 public GetActiveMatch():Observable<DataResponseModel<MatchModel>>{

  return this._http.get<DataResponseModel<MatchModel>>(this.baseUrl + "ActiveMatch");
 }

 
}
