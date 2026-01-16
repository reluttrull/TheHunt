import { Component } from '@angular/core';
import { RecordLocation } from '../locations/record-location/record-location';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.html',
  styleUrl: './home.css',
  imports: [RecordLocation],
})
export class Home {
}
