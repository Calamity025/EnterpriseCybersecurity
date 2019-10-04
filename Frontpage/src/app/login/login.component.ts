import { Component, OnInit } from '@angular/core';
import { UserLogin } from '../model/userLogin';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

let PATH : string = "https://localhost:44399"

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  login : UserLogin = {login: null, password: null};
  loginloginErrors : string[] = [];
  loginPasswordErrors : string[] = []; 

  constructor(
    private httpClient : HttpClient,
    private router : Router
  ) { }

  ngOnInit() {
  }

  onLogInClick(){
    this.httpClient.post<UserLogin>(PATH + '/api/identities', this.login).subscribe(res => this.router.navigate(['home']));
  }
}
