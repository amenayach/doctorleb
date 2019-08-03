import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {

  public LastUrl = 'lastUrl';
  public UserInfo = 'userInfo';
  public ConfirmUserName = 'confirmUserName';

  constructor() { }

  setItem(key: string, value: string) {
    window.localStorage.setItem(key, value);
  }

  getItem(key: string): string {
    return window.localStorage.getItem(key);
  }

  getJson(key: string): any {
    var item = window.localStorage.getItem(key);

    return item && item.length > 0 ? JSON.parse(item) : null;
  }

  removeItem(key: string) {
    window.localStorage.removeItem(key);
  }

}
