import { Component, OnInit } from '@angular/core';
import { DataService } from '../common/data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css'],
  providers: [DataService]
})
export class TicketComponent implements OnInit {

  disableSave: boolean;
  loggedIn: boolean = false;
  userEmail: string = '';
  desc: string = '';
  constructor(private http: DataService, private router: Router) { }

  ngOnInit() {
    this.http.getProfile().subscribe(data => {
      this.loggedIn = true;
      this.userEmail = data["email"];
    });
  }

  save(name: string, phone: string): void {
    this.disableSave = true;
    var emailToSent = this.userEmail; // && this.userEmail.length > 0 ? this.userEmail : email;
    this.http.addTicket({
      "name": name,
      "phoneNumber": phone,
      "email": emailToSent,
      "description": this.desc
    }).subscribe(data => {
      this.router.navigate(['/']);
    }, err => {
      console.log(err);
      this.disableSave = false;
    });
  }

  isValidPhone(phone: string): boolean {
    if (!phone || phone.length === 0) return true;
    var reg = /(03|76|80|81)([0-9]{6})+/g

    var result = reg.test(phone);
    console.log('valid phone: ' + result);
    return result;
  }

  isFormValid(name: string, phone: string): boolean {
    var isValidPhone = this.isValidPhone(phone);

    return name && name.length > 0 &&
      phone && phone.length > 0 &&
      isValidPhone &&
      this.userEmail && this.userEmail.length > 0 &&
      this.desc && this.desc.length > 0;
  }
}
