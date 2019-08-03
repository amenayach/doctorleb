import {Component, OnInit} from '@angular/core';
import {DataService} from '../common/data.service';
import {StorageService} from '../common/storage.service';

@Component({
  selector: 'app-confirm-account',
  templateUrl: './confirm-account.component.html',
  styleUrls: ['./confirm-account.component.css']
})
export class ConfirmAccountComponent implements OnInit {

  confirmationFailed = false;

  constructor(private storage: StorageService,
              private http: DataService) {
  }

  ngOnInit() {
  }

  sendConfirmation(code: string) {
    this.confirmationFailed = false;
    const username = this.storage.getItem(this.storage.ConfirmUserName);

    if (username && username.length > 0 && code && code.length > 3) {
      this.http.sendConfirmation(username, code).subscribe(data => {
        if (data && (<any>data).success === 'ConfirmEmail') {
          this.redirect();
        } else {
          this.confirmationFailed = true;
        }
      }, err => {
        this.confirmationFailed = true;
      });
    } else {
      this.confirmationFailed = true;
    }
  }

  closeMe() {
    this.redirect();
  }

  redirect() {
    const lastUrl = this.storage.getItem(this.storage.LastUrl);

    window.location.href = lastUrl != null && lastUrl.length > 5 ? lastUrl : '/';
  }
}
