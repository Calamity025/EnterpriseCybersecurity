import { Component, OnInit } from '@angular/core';
import { UserLogin } from '../model/userLogin';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Globals } from '../globals';
import { User } from '../model/user';
import { UserResponse } from '../model/userResponse';
import { TokenService } from '../token.service';

let PATH : string = "https://localhost:44399"

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  login : UserLogin = {login: null, password: null};
  loginPasswordErrors : string[] = []; 
  loginLoginErrors : string[] = []; 
  attempts: number = 0;

  constructor(
    private httpClient : HttpClient,
    private router : Router,
    private globals : Globals,
    private auth : TokenService
  ) { }

  ngOnInit() {
  }

  onLogInClick(){
    this.httpClient.post<UserResponse>(PATH + '/api/identities/' + this.attempts, this.login).subscribe(
      res => {
        this.globals.User = res.user;
        this.auth.persistToken(res.access_token);
        this.router.navigate(['/home']);
        
      },
      err => { 
        this.attempts++; 
        if(err.status === 400) 
          this.loginPasswordErrors[0] = err.error ; 
        if(err.status === 404){
          this.loginLoginErrors[0] = err.error;
        }});
  }
}
