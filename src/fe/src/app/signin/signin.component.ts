import { Component, OnInit } from '@angular/core';
import { StorageService } from '../common/storage.service';
import { DataService } from '../common/data.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css'],
  providers: [StorageService, DataService]
})
export class SigninComponent implements OnInit {

  loginFailed = false;

  constructor(private storage: StorageService,
    private http: DataService) { }

  ngOnInit() {
  }

  closeMe() {
    this.redirect();
  }

  signMeIn(username: string, password: string) {
    this.loginFailed = false;
    if (username && password) {
      this.http.login(username, password).subscribe(data => {
        this.storage.setItem(this.storage.UserInfo, JSON.stringify(data));
        this.redirect();
      }, err => {
        this.loginFailed = true;
        console.log(err);
      });
    }
  }

  onKeydown(ev: KeyboardEvent, username: string, password: string) {
    if (ev.keyCode === 13) {
      this.signMeIn(username, password);
    }
  }

  redirect() {
    var lastUrl = this.storage.getItem(this.storage.LastUrl);

    window.location.href = lastUrl != null && lastUrl.length > 5 ? lastUrl : '/';
  }
}
