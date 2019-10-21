import { Component, OnInit } from '@angular/core';
import { Globals } from '../globals';
import { AuthService } from '../auth.service';
import { User } from '../model/user';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { PasswordPayload } from '../model/passwordPayload';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {
  users : Observable<User[]>;
  user : User = null;
  changeToggle = false;
  passwordPayload : PasswordPayload = {oldPassword : null, newPassword : null, passwordConfirmation : null}
  passwordSame = true;
  oldPasswordError : string = null;
  loading = false;
  
  constructor(private auth: AuthService,
    private router : Router) { }

  ngOnInit() {
    this.auth.currentUser$.subscribe(x => {
      this.user = x;
      if(x && x.role === "Admin"){
        this.users = this.auth.getUsers();
      }
      else if(!x){
        this.router.navigate([''])
      }
    }); 
  }

  submit(){
    this.loading = true;
    this.auth.changePassword(this.passwordPayload).subscribe(
      () => {
        this.loading = false; 
        this.changeToggle = false;
        this.oldPasswordError = null;
      },
      err =>{
        this.loading = false; 
        if(err.status === 400){
          this.oldPasswordError = err.error;
        }
      }
    );
  }

  onInput(){
    this.passwordSame = this.passwordPayload.newPassword === this.passwordPayload.passwordConfirmation;
  }

  ChangeToggle(){
    this.changeToggle = !this.changeToggle;
  }

  onSignOut(){
    this.auth.signOut();
    this.router.navigate(['']);
  }

  onRestore(user : User){
    this.auth.restore(user).subscribe(x => this.users = this.auth.getUsers());
  }

  onBan(user : User){
    this.auth.ban(user).subscribe(x => this.users = this.auth.getUsers());
  }

  onPromote(user : User){
    this.auth.promote(user).subscribe(x => this.users = this.auth.getUsers());
  }

  onRevoke(user : User){
    this.auth.revoke(user).subscribe(x => this.users = this.auth.getUsers());
  }

}
