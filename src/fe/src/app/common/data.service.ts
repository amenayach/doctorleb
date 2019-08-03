import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class DataService {

  private ApiRoot = './api/';

  constructor(private http: HttpClient) {
    if (window.location.href.indexOf('localhost:') > -1) {
      this.ApiRoot = 'http://localhost:6778/api/';
    }
  }

  sendConfirmation(username: string, code: string) {
    return this.http.get(`${this.ApiRoot}Account/ConfirmEmail/${username}/${code}`, {
      withCredentials: true
    });
  }

  register(username: string, password: string, mobile: string, firstname: string, middlename: string, familyname: string, email: string) {
    return this.http.post(`${this.ApiRoot}Account/Register`,
      {
        'userName': username,
        'email': email,
        'password': password,
        'confirmPassword': password,
        'mobile': mobile,
        'firstName': firstname,
        'middleName': middlename,
        'lastName': familyname
      }, {
        withCredentials: true
      });
  }

  login(username: string, password: string) {
    return this.http.post(`${this.ApiRoot}Account/Login`,
      {
        'email': username,
        'password': password,
        'rememberMe': true
      }, {
        withCredentials: true
      });
  }

  logout() {
    return this.http.post(`${this.ApiRoot}Account/Logout`, null, {
      withCredentials: true
    });
  }

  getProfile() {
    return this.http.get(`${this.ApiRoot}Account/GetProfile`, {
      withCredentials: true
    });
  }

  addReview(review) {
    return this.http.post(`${this.ApiRoot}Doctor/reviews/create`,
      review
      , {
        withCredentials: true
      });
  }

  addTicket(review) {
    return this.http.post(`${this.ApiRoot}Doctor/tickets/create`,
      review
      , {
        withCredentials: true
      });
  }

  getAutoComplete(keyword: string) {
    return this.http.get(`${this.ApiRoot}Doctor/search/autocomplete/${keyword}`, {
      withCredentials: true
    });
  }

  getSearchResult(keyword: string, pageIndex: number) {
    return this.http.post(`${this.ApiRoot}Doctor/search`,
      {
        'keyword': keyword,
        'pageIndex': pageIndex
      }, {
        withCredentials: true
      });
  }

  getAdvSearchResult(keyword: string, specialityId: string, pageIndex: number) {
    return this.http.post(`${this.ApiRoot}Doctor/search`,
      {
        'keyword': keyword,
        'pageIndex': pageIndex,
        'specialityId': specialityId
      }, {
        withCredentials: true
      });
  }

  getDoctorInfo(doctorId: string) {
    return this.http.get(`${this.ApiRoot}Doctor/${doctorId}/info`, {
      withCredentials: true
    });
  }

  getDoctorReviews(doctorId: string, pageIndex: number) {
    return this.http.get(`${this.ApiRoot}Doctor/${doctorId}/reviews/${pageIndex}/10`, {
      withCredentials: true
    });
  }

  getSpecialties() {
    return this.http.get(`${this.ApiRoot}Doctor/specialities`, {
      withCredentials: true
    });
  }

}
