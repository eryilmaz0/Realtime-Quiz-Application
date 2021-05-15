import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CompetitionPageComponent } from './Components/competition-page/competition-page.component';
import { CompetitionsComponent } from './Components/competitions/competitions.component';
import { HomeComponent } from './Components/home/home.component';
import { LoginComponent } from './Components/login/login.component';
import { MatchPageComponent } from './Components/match-page/match-page.component';
import { PageNotFoundComponent } from './Components/page-not-found/page-not-found.component';
import { RegisterComponent } from './Components/register/register.component';
import { CompetitionGuard } from './Guards/competition.guard';
import { LoginGuard } from './Guards/login.guard';

const routes: Routes = [
  {path:"", pathMatch:"full", component:HomeComponent},
  {path:"Home", component:HomeComponent},
  {path:"Register", component:RegisterComponent},
  {path:"Login", component:LoginComponent},
  {path:"Competitions", component:CompetitionsComponent, canActivate:[LoginGuard]},
  {path:"Match", component:MatchPageComponent, canActivate:[LoginGuard]},
  {path:"Competition", component:CompetitionPageComponent, canActivate:[LoginGuard]},
  {path:"**", component:PageNotFoundComponent}
  
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
