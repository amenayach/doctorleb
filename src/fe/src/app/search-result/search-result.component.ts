import { Component, Input } from '@angular/core';
import { Doctor } from '../objecmodel/doctor';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchResultComponent {

  @Input() doctor: Doctor;

  constructor() { }
}
