import { Component, OnInit, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { LeafletDirective, LeafletLayersDirective } from '@bluehalo/ngx-leaflet';
import * as L from 'leaflet';
import { PlaceService } from '../place.service';
import { PlaceResponse } from '../interfaces';
import { LatLong, getLocation } from '../../locations/utils';

@Component({
  selector: 'app-nearby-places-map',
  imports: [ReactiveFormsModule, LeafletDirective, LeafletLayersDirective],
  templateUrl: './nearby-places-map.html',
  styleUrl: './nearby-places-map.css',
})
export class NearbyPlacesMap implements OnInit {
  fb = inject(FormBuilder);
  filters:FormGroup = this.fb.group({
      maxDistanceMeters: [null]
  });
  areFiltersVisible = signal(false);

  placeService = inject(PlaceService);
  places = signal<PlaceResponse[]>([]);
  isMapReady = signal(false);
  latitude: number | undefined;
  longitude: number | undefined;

  options: L.MapOptions | undefined;
  layers: L.Layer[] = [];
  layer = L.circle([0,0], { radius: 0});
  markers: L.Layer[] = [];

  async ngOnInit() {
    await this.getAndLoadResults();
  }

  async getAndLoadResults() {
    let latLong:LatLong = await getLocation();
    this.latitude = latLong.latitude;
    this.longitude = latLong.longitude;
    let d = this.filters.value.maxDistanceMeters ?? 5;
    let minLat = this.latitude - (d / 111.1);
    let maxLat = this.latitude + (d / 111.1);
    let minLon = this.longitude - (d / (Math.abs(Math.cos(this.latitude * Math.PI / 180.0) * 111.1)));
    let maxLon = this.longitude + (d / (Math.abs(Math.cos(this.latitude * Math.PI / 180.0) * 111.1)));
    this.placeService.getAllPlaces(this.latitude, this.longitude, null, 
          minLat, maxLat, minLon, maxLon)
      .subscribe({
        next: res => {
          this.isMapReady.set(false);
          this.places.set(res);

          const markers = this.places().map(p =>
            L.marker([p.latitude, p.longitude])
          );

          var bounds = new L.LatLngBounds([
            new L.LatLng(minLat, minLon),
            new L.LatLng(minLat, maxLon),
            new L.LatLng(maxLat, minLon),
            new L.LatLng(maxLat, maxLon)
          ]);

          this.layer = L.circle([this.latitude!, this.longitude!], { radius: 50 });

          this.layers = [ this.layer, ...markers ];

          if (!this.options) {
            this.initializeMap(this.latitude ?? 0, this.longitude ?? 0, bounds);
          }

          this.isMapReady.set(true);
        },
        error: err => console.error('could not retrieve nearby places for user', err)
      });
  }

  initializeMap(lat:number, long:number, bounds:L.LatLngBounds) {
    this.layer = L.circle([lat, long], { radius: 50});
    this.options = {
      layers: [
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
          maxZoom: 18,
          attribution: '...'
        })
      ],
      maxBounds: bounds,
      center: L.latLng(lat, long)
    };
  }
  
  applyFilters() {
    this.areFiltersVisible.set(false);
    //this.options = undefined;
    this.getAndLoadResults();
  }
  
  clearFilters() {
    this.filters = this.fb.group({
      maxDistanceMeters: [null]
    });
    this.applyFilters();
  }
  
  toggleFiltersVisible() {
    this.areFiltersVisible.set(true);
  }
}
