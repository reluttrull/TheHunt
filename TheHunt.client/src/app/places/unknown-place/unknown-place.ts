import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { Location } from '../../locations/location/location';
import { PlaceService } from '../place.service';
import { LocationService } from '../../locations/location.service';
import { UnknownPlaceResponse } from '../interfaces';
import { LocationResponse } from '../../locations/interfaces';

@Component({
  selector: 'app-unknown-place',
  imports: [DatePipe, Location],
  templateUrl: './unknown-place.html',
  styleUrl: './unknown-place.css',
})
export class UnknownPlace implements OnInit {
  id: string | null = null;
  isLoading = signal(true);
  route = inject(ActivatedRoute);
  placeService = inject(PlaceService);
  locationService = inject(LocationService);
  place = signal<UnknownPlaceResponse | null>(null);

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.id = params.get('id');
      this.isLoading.set(true);
      
      this.placeService.getUnknownPlace(this.id ?? '').subscribe(p => {
        this.place.set(p);
        this.isLoading.set(false);
      });
    });
  }
}
