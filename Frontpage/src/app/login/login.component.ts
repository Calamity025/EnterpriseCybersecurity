import { Component, OnInit } from '@angular/core';
import { UserLogin } from '../model/userLogin';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Globals } from '../globals';
import { User } from '../model/user';
import { UserResponse } from '../model/userResponse';
import { TokenService } from '../token.service';
import { AuthService } from '../auth.service';
import { UserCreation } from '../model/userCreation';
declare var $:any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  login : UserLogin = {login: null, password: null};
  register : UserCreation = {login : null, password : null, name : null}
  passwordConfirmation : string = null;
  loginPasswordError : string = null; 
  loginLoginError : string = null; 
  loading : boolean = false;
  passwordSame = true;
  loginError : string = null;
  passwordError : string = null;

  constructor(
    private router : Router,
    private globals : Globals,
    private auth : AuthService
  ) { }

  ngOnInit() {
    if(this.auth.currentUser$.value){
      this.router.navigate(['home']);
    }
  }

  onRegister(){
    this.loading = true;
    this.auth.create(this.register).subscribe(
      x => {
        this.router.navigate(['home']);
        this.loading = false;
      },
      err => {
        if(err.status === 400){
          this.passwordError = err.error;
          this.loginError = null;
        }
        else if(err.status === 404){
          this.loginError = err.error;
          this.passwordError = null;
        }
        this.loading = false;
      }
    )
  }

  onInput(){
    this.passwordSame = this.register.password === this.passwordConfirmation;
  }

  onLogInClick(){
    this.loading = true;
    this.auth.signIn(this.login).subscribe(
      res => {
          this.loginLoginError = null;
          this.loginPasswordError = null;
        if(res.user.status === "Active"){
          this.router.navigate(['/home']);
        }
        else{
          $('#modal').modal('show');
        }
        this.loading = false;
      },
      err => { 
        if(err.status === 400) { 
          this.loginPasswordError = err.error ; 
          this.loginLoginError = null;
        }
        else if(err.status === 404){
          this.loginLoginError = err.error;
          this.loginPasswordError = null;
        }
        this.loading = false;
      });
  }
}
