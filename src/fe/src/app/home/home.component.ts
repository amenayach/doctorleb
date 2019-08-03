import {Component, ElementRef, ViewChild, OnInit, Input} from '@angular/core';
import {FormControl, FormsModule, NgModel} from '@angular/forms';
import {Router, ActivatedRoute, Params} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import {startWith} from 'rxjs/operators/startWith';
import {map} from 'rxjs/operators/map';
import {MatAutocomplete, MatAutocompleteModule, MatDialog, MatDialogModule} from '@angular/material';
import {Doctor} from '../objecmodel/doctor';
import {DataService} from '../common/data.service';
import {StorageService} from '../common/storage.service';
import {DoctorReviewComponent} from '../doctor-review/doctor-review.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [DataService, StorageService]
})
export class HomeComponent implements OnInit {

  private pageIndex: number = 0;
  hasMore: boolean = true;
  noResult: boolean = false;
  showAdvancedSearch: boolean = false;
  loggedIn: boolean = false;
  selectedSpcId: string = '';
  @ViewChild('auto', {read: ElementRef}) auto: ElementRef;
  private paramKeyword: string;
  title = 'app';
  keyword = '';
  selectedResult: Doctor;
  doctorDetail: any;
  specialties = [];
  loadingDetail: boolean = false;
  loadingSearch: boolean = false;
  redux = {
    results: [],
    doctors: []
  };

  ngOnInit(): void {
    this.activeRoute.params.subscribe(
      (params: Params) => {
        this.keyword = params['s'];
        this.paramKeyword = params['s'];
        this.selectedResult = null;
        this.doctorDetail = null;
        if (this.keyword && this.keyword !== '') {
          this.doSearch(0);
        }
      });
    document.addEventListener('keyup', keyEvent => {
      if (keyEvent.keyCode == 27 && document.querySelectorAll('mat-dialog-container').length === 0) {
        this.selectedResult = null;
      }
    });

    var userInfoString = this.storage.getItem(this.storage.UserInfo);
    this.loggedIn = userInfoString && userInfoString.length > 0;
  }

  constructor(private http: DataService,
              private route: Router,
              private activeRoute: ActivatedRoute,
              public dialog: MatDialog,
              private storage: StorageService) {
  }

  isMobile() {
    return false;
  }

  invisibleOnDesktop() {
    return true;
  }

  hideLeftSpace() {
    return this.redux.doctors && this.redux.doctors.length > 0 ? 'none' : 'flex';
  }

  onKey(event: any) {
    if (event.keyCode === 13) {
      this.redux.results = [];
      this.redux.doctors = [];
      this.search(0);
    }
    else if (event.target.value && event.target.value.length % 3 === 0) {
      this.http.getAutoComplete(event.target.value).subscribe(data => {
        this.redux.results = <Doctor[]>data;
      });
    }
  }

  searchButton() {
    this.redux.results = [];
    this.redux.doctors = [];
    this.search(0);
  }

  onAutoSelected() {
  }

  getLeftWidth() {
    return this.redux.doctors && this.redux.doctors.length > 0 ? '0' : '15%';
  }

  getRightWidth() {
    return this.redux.doctors && this.redux.doctors.length > 0 ? '40%' : '15%';
  }

  getSearchWidth() {
    return this.redux.doctors && this.redux.doctors.length > 0 ? '100%' : '60%';
  }

  search(pageIndex: number) {
    if (this.keyword === this.paramKeyword) {
      this.doSearch(pageIndex);
    }
    else {
      this.route.navigate(['/home', this.keyword]);
    }
  }

  loadMore() {
    this.pageIndex++;
    this.search(this.pageIndex);
  }

  doSearch(pageIndex: number) {
    this.loadingSearch = true;
    this.selectedResult = null;
    this.storage.setItem(this.storage.LastUrl, window.location.href);
    this.http.getAdvSearchResult(this.keyword, this.selectedSpcId, pageIndex).subscribe(data => {
      this.redux.doctors = this.redux.doctors.concat(<any[]>data);
      if ((<any[]>data).length === 0) this.hasMore = false;
      this.loadingSearch = false;
      this.noResult = this.redux.doctors.length === 0;
    }, err => {
      this.loadingSearch = false;
      this.noResult = false;
    });
  }

  cardClicked(selectedDoctor: Doctor, docid: string) {
    this.doctorDetail = {};
    this.loadingDetail = true;
    this.selectedResult = selectedDoctor;
    let docDetail = document.getElementById('docDetail');
    let docSearchResult = document.getElementById(docid);
    docDetail.style.top = this.getCoords(docSearchResult).top + 'px';
    docDetail.style.left = '0px';
    docDetail.style.width =
      (document.documentElement.clientWidth * (document.documentElement.clientWidth < 1260 ? 1 : 0.4)) + 'px';
    this.http.getDoctorInfo(selectedDoctor.id).subscribe(data => {
        this.doctorDetail = data;
        this.loadingDetail = false;
      },
      err => {
        this.loadingDetail = false;
      });
  }

  showAdvSearch() {
    this.showAdvancedSearch = true;
    if (this.specialties.length == 0) {
      this.loadingDetail = true;
      this.http.getSpecialties().subscribe(data => {
          this.specialties = [{id: null, nameAr: 'الكل'}];
          this.specialties = this.specialties.concat(<any[]>data);
          this.loadingDetail = false;
        },
        err => {
          this.loadingDetail = false;
        });
    }
  }

  private getCoords(elem) {
    var box = elem.getBoundingClientRect();

    var body = document.body;
    var docEl = document.documentElement;

    var scrollTop = window.pageYOffset || docEl.scrollTop || body.scrollTop;
    var scrollLeft = window.pageXOffset || docEl.scrollLeft || body.scrollLeft;

    var clientTop = docEl.clientTop || body.clientTop || 0;
    var clientLeft = docEl.clientLeft || body.clientLeft || 0;

    var top = box.top + scrollTop - clientTop;
    var left = box.left + scrollLeft - clientLeft;

    return {top: Math.round(top), left: Math.round(left)};
  }
}

