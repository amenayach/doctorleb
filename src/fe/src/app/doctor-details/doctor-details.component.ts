import {Component, OnInit, Input} from '@angular/core';
import {Doctor} from '../objecmodel/doctor';
import {DataService} from '../common/data.service';
import {DoctorReviewComponent} from '../doctor-review/doctor-review.component';
import {MatDialog} from '@angular/material';
import {StorageService} from '../common/storage.service';

@Component({
  selector: 'app-doctor-details',
  templateUrl: './doctor-details.component.html',
  styleUrls: ['./doctor-details.component.css'],
  providers: [DataService, StorageService]
})
export class DoctorDetailsComponent implements OnInit {

  @Input() selectedResult: Doctor;
  @Input() doctorDetail: any;
  @Input() loadingDetail = false;
  @Input() loggedIn = false;
  user: any;

  constructor(private http: DataService, public dialog: MatDialog, public storage: StorageService) {
  }

  ngOnInit() {
    if (this.loggedIn) {
      this.user = this.storage.getJson(this.storage.UserInfo);
    }
  }

  openDialog(): void {
    var myReview = this.doctorDetail && this.doctorDetail.reviews ? this.doctorDetail.reviews.filter(rec => {
      return rec.userId = this.user.id;
    }) : [];

    if (myReview.length > 0) {
      myReview[0].doctorId = this.selectedResult.id;
    }

    let dialogRef = this.dialog.open(DoctorReviewComponent, {
      width: '50%',
      data: {selectedResult: myReview.length > 0 ? myReview[0] : {doctorId: this.selectedResult.id}}
    });

    dialogRef.afterClosed().subscribe(result => {
    });
  }

  closeMe() {
    this.selectedResult = null;
  }
}
