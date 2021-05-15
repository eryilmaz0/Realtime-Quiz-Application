import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../Services/auth.service';
@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private _authService:AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    
    
    if(this._authService.isAuthenticated){
    
      var token = this._authService.getAccessToken.token;

      let newRequest:HttpRequest<any>;
      newRequest = request.clone({
        setHeaders: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`
        }})

        return next.handle(newRequest);
  
    }
   
    return next.handle(request);
    
  }
}
