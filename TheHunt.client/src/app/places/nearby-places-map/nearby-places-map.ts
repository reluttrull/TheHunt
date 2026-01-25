import { Component, OnInit, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { LeafletDirective, LeafletLayerDirective } from '@bluehalo/ngx-leaflet';
import * as L from 'leaflet';
import { PlaceService } from '../place.service';
import { PlaceResponse } from '../interfaces';
import { LatLong, getLocation } from '../../locations/utils';

@Component({
  selector: 'app-nearby-places-map',
  imports: [ReactiveFormsModule, LeafletDirective, LeafletLayerDirective],
  templateUrl: './nearby-places-map.html',
  styleUrl: './nearby-places-map.css',
})
export class NearbyPlacesMap implements OnInit {
  fb = inject(FormBuilder);
  filters:FormGroup = this.fb.group({
      minLatitude: [null],
      maxLatitude: [null],
      minLongitude: [null],
      maxLongitude: [null]
  });
  areFiltersVisible = signal(false);

  placeService = inject(PlaceService);
  places = signal<PlaceResponse[]>([]);
  isMapReady = signal(false);
  latitude: number | undefined;
  longitude: number | undefined;

  options: L.MapOptions | undefined;
  layer = L.circle([0,0], { radius: 0});
  markers: L.Layer[] = [];

  async ngOnInit() {
    await this.getAndLoadResults();
  }

  async getAndLoadResults() {
    let latLong:LatLong = await getLocation();
    this.latitude = latLong.latitude;
    this.longitude = latLong.longitude;
    this.placeService.getAllPlaces(this.latitude, this.longitude,
          null, this.filters.value.minLatitude, null, null)
      .subscribe({
        next: res => {
          this.places.set(res);
          this.markers = this.places().map(p => L.marker([p.latitude, p.longitude]));
          this.initializeMap(this.latitude ?? 0, this.longitude ?? 0);
          console.log(this.options, this.layer, this.markers);
          this.isMapReady.set(true);
        },
        error: err => console.error('could not retrieve nearby places for user', err)
      });
  }

  initializeMap(lat:number, long:number) {
    this.layer = L.circle([lat, long], { radius: 50});
    this.options = {
      layers: [
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
          maxZoom: 18,
          attribution: '...'
        })
      ],
      zoom: 15,
      center: L.latLng(lat, long)
    };
  }
  
  applyFilters() {
    this.getAndLoadResults();
  }
  
  clearFilters() {
    this.filters = this.fb.group({
      minLatitude: [null],
      maxLatitude: [null],
      minLongitude: [null],
      maxLongitude: [null]
    });
    this.applyFilters();
  }
  
  toggleFiltersVisible() {
    this.areFiltersVisible.set(true);
  }
}
