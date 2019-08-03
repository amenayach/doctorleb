import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DataService } from '../common/data.service';

@Component({
  selector: 'app-doctor-review',
  templateUrl: './doctor-review.component.html',
  styleUrls: ['./doctor-review.component.css'],
  providers: [DataService]
})
export class DoctorReviewComponent implements OnInit {

  selectedResult: any;
  user: any;

  constructor(
    public dialogRef: MatDialogRef<DoctorReviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public http: DataService
  ) {
    this.selectedResult = data.selectedResult;
  }

  ngOnInit() {
    this.http.getProfile().subscribe(data => {
      this.user = data;
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  save(): void {
    this.http.addReview({
      "doctorId": this.selectedResult.doctorId,
      "reviewerName": this.user.userName,
      "description": this.selectedResult.description,
      "rank": this.selectedResult.rank
    }).subscribe(data => {
      this.onNoClick();
    }, err => {
      console.log(err);
    });
  }

  onRate(ev) {
    this.selectedResult.rank = ev;
  }
}
