import { Component, OnInit } from '@angular/core';
import { UserLogin } from '../model/userLogin';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Globals } from '../globals';
import { User } from '../model/user';
import { UserResponse } from '../model/userResponse';
import { TokenService } from '../token.service';
import { AuthService } from '../auth.service';
declare var $:any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  login : UserLogin = {login: null, password: null};
  loginPasswordError : string = null; 
  loginLoginError : string = null; 
  loading : boolean = false;

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
