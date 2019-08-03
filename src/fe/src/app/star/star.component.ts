import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-star',
  templateUrl: './star.component.html',
  styleUrls: ['./star.component.css']
})
export class StarComponent {
  @Input() active: boolean;
  @Input() position: number;
  @Input() readOnly: boolean;
  @Output() rate = new EventEmitter();

  getCursor() {
    return this.readOnly ? 'default' : 'pointer';
  }

  handleRate() {
    this.rate.emit(this.position);
  }
}
