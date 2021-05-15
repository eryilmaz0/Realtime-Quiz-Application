import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { HubControllerService } from '../Services/hub-controller.service';

@Injectable({
  providedIn: 'root'
})
export class CompetitionGuard implements CanActivate {


    constructor(private _hubControllerService:HubControllerService,private toastrService:ToastrService,private router:Router)
     {
      this._hubControllerService.GetActiveMatch().subscribe(response => {

        if(response.data == null){

          this.toastrService.error("Yarışmaya Başlamak İçin Eşleşmiş Olmalısınız.");
          this.router.navigate(["Competitions"]);
          return false;
        }

        return true;

      }, error => {

        this.toastrService.error("Yarışmaya Başlarken Bir Hata Oluştu.");
        this.router.navigate(["Competitions"]);
        return false;
      })
     }


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }
  
}
