import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { Location } from '../../locations/location/location';
import { PlaceService } from '../place.service';
import { LocationService } from '../../locations/location.service';
import { PlaceResponse } from '../interfaces';
import { LocationResponse } from '../../locations/interfaces';

@Component({
  selector: 'app-place',
  imports: [DatePipe, Location],
  templateUrl: './place.html',
  styleUrl: './place.css',
})
export class Place implements OnInit {
  id: string | null = null;
  isLoading = signal(true);
  route = inject(ActivatedRoute);
  placeService = inject(PlaceService);
  locationService = inject(LocationService);
  place = signal<PlaceResponse | null>(null);
  location = signal<LocationResponse | null>(null);

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.id = params.get('id');
      this.isLoading.set(true);
      
      this.placeService.getPlace(this.id ?? '').subscribe(p => {
        this.place.set(p);
        this.isLoading.set(false);
        if (this.place() != null) {
          console.log('passed in loc id', this.place()?.locationId);
          this.locationService.getLocation(this.place()?.locationId!).subscribe(l => {
            console.log('location is', l);
            this.location.set(l);
          });
        } else {
          // todo: throw error
        }
      });
    });
  }
}
