import {Component, OnInit} from '@angular/core';
import {DataService} from '../common/data.service';
import {StorageService} from '../common/storage.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
  providers: [StorageService, DataService]
})
export class SignupComponent implements OnInit {

  fillRequiredFields = false;
  invalidMobileFormat = false;
  invalidEmailFormat = false;
  invalidUsername = false;
  passwordNotMatching = false;
  invalidPasswordFormat = false;

  usernameAllowedChars = 'abcdefghijklmnopqrstuvwxyz012345679';

  constructor(private storage: StorageService,
              private http: DataService,
              private router: Router) {
  }

  ngOnInit() {
  }

  closeMe() {
    this.redirect();
  }

  signMeUp(username: string, password: string, confirmPassword: string,
           mobile: string, firstname: string, middlename: string, familyname: string, email: string) {
    if (username && password && confirmPassword && mobile && firstname && middlename && familyname) {
      this.fillRequiredFields = false;
      this.invalidMobileFormat = false;
      this.invalidEmailFormat = false;
      this.passwordNotMatching = false;
      this.invalidUsername = false;
      this.invalidPasswordFormat = false;

      mobile = this.replaceAll(mobile, ' ', '');
      mobile = this.replaceAll(mobile, '+', '');
      if (mobile.length < 8) {
        this.invalidMobileFormat = true;
        return;
      }
      
      if (password.length < 8) {
        this.invalidPasswordFormat = true;
        return;
      }

      if (!this.validateUsername(username)) {
        this.invalidUsername = true;
        return;
      }

      const numbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
      if (mobile.split('').filter(x => numbers.indexOf(x) === -1).length > 0) {
        this.invalidMobileFormat = true;
        return;
      }

      if (!this.validateEmail(email)) {
        this.invalidEmailFormat = true;
        return;
      }

      if (password !== confirmPassword) {
        this.passwordNotMatching = true;
        return;
      }

      this.http.register(username, password, mobile, firstname, middlename, familyname, email).subscribe(data => {
        this.storage.setItem(this.storage.ConfirmUserName, username);
        this.redirect();
      }, err => {
        console.log(err);
      });
    } else {
      this.fillRequiredFields = true;
    }
  }

  onKeydown(ev: KeyboardEvent, username: string, password: string, confirmPassword: string,
            mobile: string, firstname: string, middlename: string, familyname: string, email: string) {
    if (ev.keyCode === 13) {
      this.signMeUp(username, password, confirmPassword, mobile, firstname, middlename, familyname, email);
    }
  }

  redirect() {
    this.router.navigate(['confirm']);
  }

  replaceAll(source, search, replacement) {
    return source.split(search).join(replacement);
  }

  validateEmail(email) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
  }

  validateUsername(username) {
    if (!username || username == null) {
      return false;
    }

    return username.toLowerCase().split('').filter(m => this.usernameAllowedChars.indexOf(m) === -1).length === 0;
  }
}
