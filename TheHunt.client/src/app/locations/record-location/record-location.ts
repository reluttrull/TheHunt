import { Component, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { LocationService } from '../location.service';
import { LocationResponse } from '../interfaces';

@Component({
  selector: 'app-record-location',
  imports: [DatePipe],
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
    await this.getLocation();

    if (this.latitude !== undefined && this.longitude !== undefined) {
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
  

  getLocation(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (!navigator.geolocation) {
        this.error = 'Geolocation is not supported by this browser.';
        reject(this.error);
        return;
      }

      navigator.geolocation.getCurrentPosition(
        (position: GeolocationPosition) => {
          this.latitude = position.coords.latitude;
          this.longitude = position.coords.longitude;
          console.log('Latitude:', this.latitude, 'Longitude:', this.longitude);
          resolve();
        },
        (error: GeolocationPositionError) => {
          this.error = error.message;
          console.error('Geolocation error:', error);
          reject(error);
        }
      );
    });
  }
}