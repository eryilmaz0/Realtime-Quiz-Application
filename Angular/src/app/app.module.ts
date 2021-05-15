import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './Components/home/home.component';
import { PageNotFoundComponent } from './Components/page-not-found/page-not-found.component';
import { InformationPageComponent } from './Components/information-page/information-page.component';
import { NavbarComponent } from './Components/navbar/navbar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { RegisterComponent } from './Components/register/register.component'
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {ToastrModule} from 'ngx-toastr';
import { LoginComponent } from './Components/login/login.component';
import { CompetitionsComponent } from './Components/competitions/competitions.component';
import { AuthInterceptor } from './Interceptors/auth.interceptor';
import { MatchPageComponent } from './Components/match-page/match-page.component';
import { SecondUserPipe } from './Pipes/second-user.pipe';
import { CompetitionPageComponent } from './Components/competition-page/competition-page.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PageNotFoundComponent,
    InformationPageComponent,
    NavbarComponent,
    RegisterComponent,
    LoginComponent,
    CompetitionsComponent,
    MatchPageComponent,
    SecondUserPipe,
    CompetitionPageComponent
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({

      timeOut:1250,
      progressBar : true,
      progressAnimation: "increasing",
      preventDuplicates:true,
      positionClass:"toast-bottom-right"

    })
  ],
  providers: [
    {provide:HTTP_INTERCEPTORS, useClass:AuthInterceptor, multi:true}
   
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
