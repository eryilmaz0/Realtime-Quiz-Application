import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { RegisterModel } from 'src/app/Models/registerModel';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'Register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {


  RegisterForm:FormGroup;
 

  constructor(private _authService:AuthService,
              private toast:ToastrService,
              private router:Router,
              private route:ActivatedRoute) { }


  ngOnInit(): void {
    sessionStorage.setItem("lastRoute","Register");
    this.CrateRegisterForm();
  }



  CrateRegisterForm(){

    this.RegisterForm = new FormGroup({
      email: new FormControl("", [Validators.required, Validators.email]),
      name : new FormControl("", [Validators.required]),
      lastname: new FormControl("", [Validators.required]),
      password : new FormControl("", [Validators.required, Validators.minLength(6)]),
      passwordConfirm : new FormControl("", [Validators.required, Validators.minLength(6)]),

    },this.CheckPasswordsMatch)
  }


  CheckPasswordsMatch(form:FormGroup){

    return form.controls["password"].value === form.controls["passwordConfirm"].value ? null : {mismatch:true};

  }


  SubmitForm(){
    console.log(this.RegisterForm);

    if(!this.RegisterForm.valid && !this.RegisterForm.errors){
      this.toast.error("Bilgilerinizi Doğru Giriniz.");
    }

    else if(!this.RegisterForm.valid && this.RegisterForm.errors){
      this.toast.error("Şifreler Uyuşmuyor.");
    }

    else{

      let registerModel:RegisterModel = Object.assign({},this.RegisterForm.value);
      this._authService.Register(registerModel).subscribe(response=>{
        
        this.toast.success(response.message);
        this.router.navigate(["/Login"],{relativeTo:this.route})

      }, error => {

        console.log(error);
        this.toast.error(error.error.message);
        
      })

    }
  }

}
