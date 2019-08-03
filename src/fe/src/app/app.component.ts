import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormControl, FormsModule, NgModel } from '@angular/forms';

import { Observable } from 'rxjs/Observable';
import { startWith } from 'rxjs/operators/startWith';
import { map } from 'rxjs/operators/map';
import { MatAutocomplete, MatAutocompleteModule } from '@angular/material';
import { Doctor } from './objecmodel/doctor';
import { DataService } from './common/data.service';
import { StorageService } from './common/storage.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [DataService, StorageService]
})
export class AppComponent implements OnInit {
  @ViewChild("auto", { read: ElementRef }) auto: ElementRef;
  title = 'app';
  userInfo: any;
  keyword = '';
  redux = {
    results: [],
    doctors: []
  };

  ngOnInit(): void {
    this.http.getProfile().subscribe(data => {
      this.storage.setItem(this.storage.UserInfo, JSON.stringify(data));
      var userInfoString = this.storage.getItem(this.storage.UserInfo);
      this.userInfo = userInfoString && userInfoString.length > 0 ? JSON.parse(userInfoString) : null;
    }, err => {
      this.storage.removeItem(this.storage.UserInfo);
      this.userInfo = null;
      console.log(err);
    });
  }

  constructor(private http: DataService, private storage: StorageService) { }

  isMobile() { return false; }
  invisibleOnDesktop() { return true; }
  hideLeftSpace() { return this.redux.doctors && this.redux.doctors.length > 0 ? 'none' : 'flex'; }

  onKey(event: any) {
    if (event.keyCode === 13) {
      this.redux.results = [];
      this.search();
    }
    else if (event.target.value && event.target.value.length % 3 === 0) {
      this.http.getAutoComplete(event.target.value).subscribe(data => {
        this.redux.results = <Doctor[]>data;
      });
    }
  }

  onAutoSelected() {
    console.log('Option selected');
  }

  getLeftWidth() {
    return this.redux.doctors && this.redux.doctors.length > 0 ? '0' : '15%';
  }

  getSearchWidth() {
    return this.redux.doctors && this.redux.doctors.length > 0 ? '100%' : '60%';
  }

  search() {
    this.http.getSearchResult(this.keyword, 0).subscribe(data => {
      this.redux.doctors = <any[]>data;
    });
  }

  logout() {
    this.http.logout().subscribe(data => {
      this.storage.removeItem(this.storage.UserInfo);
      this.userInfo = null;
      window.location.reload();
    }, err => {
      this.storage.removeItem(this.storage.UserInfo);
      this.userInfo = null;
    });
  }
}
