import { Component, OnInit, inject, signal } from '@angular/core';
import { Location } from '../location/location';
import { LocationService } from '../location.service';
import { LocationResponse } from '../interfaces';

@Component({
  selector: 'app-my-locations',
  imports: [Location],
  templateUrl: './my-locations.html',
  styleUrl: './my-locations.css',
})
export class MyLocations implements OnInit {
  locationService = inject(LocationService);
  myLocations = signal<LocationResponse[]>([]);

  ngOnInit() {
    this.loadMyLocations();
  }

  loadMyLocations() {
    this.locationService.getAllLocationsForUser()
      .subscribe({
        next: res => {
          this.myLocations.set(res);
        },
        error: err => console.error('could not retrieve locations for user', err)
      });
  }
}
