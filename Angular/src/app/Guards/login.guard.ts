import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { AuthService } from '../Services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {

     constructor(private _authService:AuthService,private toastrService:ToastrService,private router:Router)
     {

     }


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    
      if(this._authService.isAuthenticated){
        return true;
      }

      else{
        this.toastrService.error("Giriş Yapınız.");
        this.router.navigate(["Login"]);
        return false;
      }
  }
  
}
