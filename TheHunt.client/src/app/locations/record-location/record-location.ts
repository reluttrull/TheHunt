import { Component, inject, signal } from '@angular/core';
import { Location } from '../location/location';
import { LocationService } from '../location.service';
import { LocationResponse } from '../interfaces';
import { LatLong, getLocation } from '../utils';

@Component({
  selector: 'app-record-location',
  imports: [Location],
  templateUrl: './record-location.html',
  styleUrl: './record-location.css',
})
export class RecordLocation {
  locationService = inject(LocationService);
  latitude: number | undefined;
  longitude: number | undefined;
  error: string | undefined;
  locationData = signal<LocationResponse | null>(null);
  // todo: eventually we'll navigate to a different component instead of displaying response here

  async record() {
    let latLong:LatLong = await getLocation();
    this.latitude = latLong.latitude;
    this.longitude = latLong.longitude;

    if (this.latitude != undefined && this.longitude != undefined) {
      this.locationService.createLocation(this.latitude, this.longitude)
        .subscribe({
          next: res => {
            this.locationData.set(res);
            console.log('saved', res);
          },
          error: err => console.error('save failed', err)
        });
    }
  }
}