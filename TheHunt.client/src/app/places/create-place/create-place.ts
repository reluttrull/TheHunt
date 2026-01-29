import { Component, inject, input, output, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { PlaceService } from '../place.service';
import { LocationResponse } from '../../locations/interfaces';

@Component({
  selector: 'app-create-place',
  imports: [ReactiveFormsModule],
  templateUrl: './create-place.html',
  styleUrl: './create-place.css',
})
export class CreatePlace {
  location = input.required<LocationResponse>();
  onReturn = output();
  placeService = inject(PlaceService);
  fb = inject(FormBuilder);
  form:FormGroup = this.fb.group({
    name: [null],
    acceptedRadiusMeters: [50],
    hint1: [null],
    hint2: [null],
    hint3: [null]
  });
  router = inject(Router);
  validationErrors = signal<string[]>([]);
  isWaiting = signal(false);

  submit() {
    this.placeService.createPlace(this.location().id, this.form.value.name, this.form.value.acceptedRadiusMeters, 
          this.form.value.hint1, this.form.value.hint2, this.form.value.hint3)
      .subscribe({
        next: res => {
          console.log('saved', res);
          this.router.navigate([`place/${res.id}`]);
        },
        error: err => console.error('save failed', err)
      });
  }

  cancel() {
    this.onReturn.emit();
  }
}
