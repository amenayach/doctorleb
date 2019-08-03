import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterModule, Routes } from '@angular/router';
import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
} from '@angular/material';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { StarComponent } from './star/star.component';
import { StarsComponent } from './stars/stars.component';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { SearchResultComponent } from './search-result/search-result.component';
import { DoctorDetailsComponent } from './doctor-details/doctor-details.component';
import { DoctorReviewComponent } from './doctor-review/doctor-review.component';
import { SigninComponent } from './signin/signin.component';
import { SignupComponent } from './signup/signup.component';
import { TicketComponent } from './ticket/ticket.component';
import { DoctorPagedReviewsComponent } from './doctor-paged-reviews/doctor-paged-reviews.component';
import { ConfirmAccountComponent } from './confirm-account/confirm-account.component';

const appRoutes: Routes = [
  { path: 'home/:s', component: HomeComponent },
  { path: 'doc/:id', component: DoctorPagedReviewsComponent },
  { path: 'contactus', component: ContactUsComponent },
  { path: 'signin', component: SigninComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'confirm', component: ConfirmAccountComponent },
  { path: 'ticket', component: TicketComponent },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  { path: '**', component: HomeComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    StarComponent,
    StarsComponent,
    HomeComponent,
    ContactUsComponent,
    SearchResultComponent,
    DoctorDetailsComponent,
    DoctorReviewComponent,
    SigninComponent,
    SignupComponent,
    TicketComponent,
    DoctorPagedReviewsComponent,
    ConfirmAccountComponent
  ],
  imports: [
    RouterModule.forRoot(
      appRoutes
    ),
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    BrowserModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    FlexLayoutModule
  ],
  entryComponents: [
    DoctorReviewComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
