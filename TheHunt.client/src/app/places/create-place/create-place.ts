import { Component, input, output } from '@angular/core';
import { LocationResponse } from '../../locations/interfaces';

@Component({
  selector: 'app-create-place',
  imports: [],
  templateUrl: './create-place.html',
  styleUrl: './create-place.css',
})
export class CreatePlace {
  location = input.required<LocationResponse>();
  onReturn = output();

  cancel() {
    this.onReturn.emit();
  }
}
