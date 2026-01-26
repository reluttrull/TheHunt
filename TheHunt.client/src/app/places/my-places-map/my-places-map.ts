import { Component, OnInit, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { LeafletDirective, LeafletLayersDirective } from '@bluehalo/ngx-leaflet';
import * as L from 'leaflet';
import { PlaceService } from '../place.service';
import { PlaceResponse } from '../interfaces';
import { LatLong, getLocation } from '../../locations/utils';

@Component({
  selector: 'app-my-places-map',
  imports: [ReactiveFormsModule, LeafletDirective, LeafletLayersDirective],
  templateUrl: './my-places-map.html',
  styleUrl: './my-places-map.css',
})
export class MyPlacesMap implements OnInit {
  fb = inject(FormBuilder);
  filters:FormGroup = this.fb.group({
      maxDistanceKm: [null]
  });
  areFiltersVisible = signal(false);

  placeService = inject(PlaceService);
  places = signal<PlaceResponse[]>([]);

  fitBounds!: L.LatLngBounds;
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
    let d = this.filters.value.maxDistanceKm ?? 5;
    let minLat = this.latitude - (d / 111.1);
    let maxLat = this.latitude + (d / 111.1);
    let minLon = this.longitude - (d / (Math.abs(Math.cos(this.latitude * Math.PI / 180.0) * 111.1)));
    let maxLon = this.longitude + (d / (Math.abs(Math.cos(this.latitude * Math.PI / 180.0) * 111.1)));
    this.placeService.getAllPlacesForUser(this.latitude, this.longitude, 
          minLat, maxLat, minLon, maxLon)
      .subscribe({
        next: res => {
          this.isMapReady.set(false);
          this.places.set(res);

          const markers = this.places().map(p =>
            L.marker([p.latitude, p.longitude])
          );
          console.log(`${markers.length} markers`, markers);

          this.fitBounds = L.latLngBounds([
            [minLat, minLon],
            [maxLat, maxLon]
          ]);

          this.layer = L.circle([this.latitude!, this.longitude!], { radius: 50 });

          this.layers = [ this.layer, ...markers ];

          if (!this.options) {
            this.initializeMap(this.latitude ?? 0, this.longitude ?? 0);
          }

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
      zoom: 13,
      center: L.latLng(lat, long)
    };
  }
  
  applyFilters() {
    this.areFiltersVisible.set(false);
    this.getAndLoadResults();
  }
  
  clearFilters() {
    this.filters = this.fb.group({
      maxDistanceKm: [null]
    });
    this.applyFilters();
  }
  
  toggleFiltersVisible() {
    this.areFiltersVisible.set(true);
  }
}
