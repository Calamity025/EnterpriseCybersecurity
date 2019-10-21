import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { User } from './model/user';
import { TokenService } from './token.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isAuthorized$ = new BehaviorSubject<boolean>(false);
  currentUser$ = new BehaviorSubject<User>(null);

  constructor(
    private httpClient: HttpClient,
    private jwtService: TokenService
  ) { }

  public signOut() {
    this.isAuthorized$.next(null);
    this.currentUser$.next(null);
    this.jwtService.clearToken();
  }
}
