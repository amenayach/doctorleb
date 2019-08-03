import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { StarComponent } from '../star/star.component';

@Component({
  selector: 'app-stars',
  templateUrl: './stars.component.html',
  styleUrls: ['./stars.component.css']
})
export class StarsComponent implements OnInit {

  @Input() starCount: number;
  @Input() readOnly: boolean;
  @Output() rate = new EventEmitter();
  stars: number[] = [1, 2, 3, 4, 5];
  _rating : number;

  constructor() {
    this._rating = this.rating;
    const count = this.starCount < 0 ? 5 : this.starCount;
  }

  get rating(): number {
    return this._rating;
  }
  @Input() 
  set rating(value: number) {
    this._rating = value;
  }
  
  ngOnInit(): void {
    this._rating = this.rating;
  }

  onRate(star) {
    this.rate.emit(star);
    if (!this.readOnly)
      this._rating = star;
  }

}
