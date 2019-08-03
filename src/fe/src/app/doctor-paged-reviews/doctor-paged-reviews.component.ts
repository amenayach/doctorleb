import {Component, Input, OnInit} from '@angular/core';
import {Location} from '@angular/common';
import {DataService} from '../common/data.service';
import {ActivatedRoute, Params} from '@angular/router';
import {StorageService} from '../common/storage.service';
import {DoctorReviewComponent} from '../doctor-review/doctor-review.component';
import {MatDialog} from '@angular/material';

@Component({
  selector: 'app-doctor-paged-reviews',
  templateUrl: './doctor-paged-reviews.component.html',
  styleUrls: ['./doctor-paged-reviews.component.css'],
  providers: [DataService]
})
export class DoctorPagedReviewsComponent implements OnInit {

  loggedIn = false;
  user: any;
  private doctorId: string;
  private pageIndex: number = 0;
  private loadingDetail: boolean;
  private hasMore: boolean = true;
  public doctorDetail: any;
  private doctorReviews: any = [];

  constructor(private http: DataService,
              private location: Location,
              private activeRoute: ActivatedRoute,
              private storage: StorageService,
              public dialog: MatDialog) {
  }

  ngOnInit() {
    const userInfoString = this.storage.getItem(this.storage.UserInfo);
    this.loggedIn = userInfoString && userInfoString.length > 0;
    this.user = this.storage.getJson(this.storage.UserInfo);
    this.activeRoute.params.subscribe(
      (params: Params) => {
        this.doctorId = params['id'];
        this.http.getDoctorInfo(this.doctorId).subscribe(data => {
            this.doctorDetail = data;
            console.log(this.doctorDetail);
            this.loadReviews();
            this.loadingDetail = false;
          },
          err => {
            this.loadingDetail = false;
          });
      });
  }

  loadReviews() {
    this.http.getDoctorReviews(this.doctorId, this.pageIndex).subscribe(data => {
        if (this.hasMore) {
          this.doctorReviews = this.doctorReviews.concat(data);
          this.pageIndex++;
          if (!data || (<Array<any>>data).length === 0) {
            this.hasMore = false;
          }
          this.loadingDetail = false;
        }
      },
      err => {
        this.loadingDetail = false;
      });
  }

  goBack() {
    this.location.back();
  }

  openDialog(): void {
    const myReview = this.doctorDetail && this.doctorDetail.reviews ? this.doctorDetail.reviews.filter(rec => {
      return rec.userId = this.user.id;
    }) : [];

    if (myReview.length > 0) {
      myReview[0].doctorId = this.doctorId;
    }

    const dialogRef = this.dialog.open(DoctorReviewComponent, {
      width: '50%',
      data: {selectedResult: myReview.length > 0 ? myReview[0] : {doctorId: this.doctorId}}
    });

    dialogRef.afterClosed().subscribe(result => {
    });
  }
}
