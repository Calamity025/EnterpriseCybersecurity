import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import {tap} from 'rxjs/operators';
import { User } from './model/user';
import { TokenService } from './token.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserLogin } from './model/userLogin';
import { UserResponse } from './model/userResponse';
import { Globals } from './globals';
import { PasswordPayload } from './model/passwordPayload';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isAuthorized$ = new BehaviorSubject<boolean>(false);
  currentUser$ = new BehaviorSubject<User>(null);

  constructor(
    private httpClient: HttpClient,
    private globals : Globals,
    private jwtService: TokenService
  ) { }

  public signOut() {
    this.currentUser$.next(null);
    this.jwtService.clearToken();
  }

  public signIn(login : UserLogin) : Observable<UserResponse>{
    return this.httpClient.post<UserResponse>(this.globals.PATH + '/api/identities/', login)
      .pipe(tap(x => {
        if(x.user.status === "Active"){
          this.currentUser$.next(x.user);
          this.jwtService.persistToken(x.access_token);
        }
      }));
  }

  public getUsers() : Observable<User[]>{
    return this.httpClient.get<User[]>(this.globals.PATH + '/api/identities');
  }

  public restore(user : User) : Observable<void>{
    return this.httpClient.patch<void>(this.globals.PATH + '/api/identities/Active', `"${user.login}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})});
  }

  public ban(user : User) : Observable<void>{
    return this.httpClient.patch<void>(this.globals.PATH + '/api/identities/Banned', `"${user.login}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})});
  }

  public promote(user : User) : Observable<void>{
    return this.httpClient.put<void>(this.globals.PATH + '/api/identities/Admin', `"${user.login}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})});
  }

  public revoke(user : User) : Observable<void>{
    return this.httpClient.put<void>(this.globals.PATH + '/api/identities/User', `"${user.login}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})});
  }

  public changePassword(passwordPaylod : PasswordPayload) : Observable<any>{
    return this.httpClient.post<any>(this.globals.PATH + '/api/identities/password', passwordPaylod);
  }
}
