import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { TokenService } from './token.service';
import { AuthService } from './auth.service';
declare var $:any;

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    isAuthorized? : boolean;

    constructor(private tokenService: TokenService, private authService : AuthService) {
        authService.isAuthorized$.subscribe(val => this.isAuthorized = val);
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if(this.isAuthorized && this.tokenService.isExpired()){
            this.authService.signOut();
            $('#UnauthorizedMessageBox').modal('show');
        }

        req = req.clone({
            setHeaders: {
                Authorization: `Bearer ${this.tokenService.getRawToken()}`
            }
        });

        return next.handle(req);
    }
}