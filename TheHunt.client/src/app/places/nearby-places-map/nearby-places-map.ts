import { Component, OnInit, inject, signal } from '@angular/core';
import { LeafletDirective, LeafletLayerDirective } from '@bluehalo/ngx-leaflet';
import { tileLayer, latLng, circle } from 'leaflet';
import { PlaceService } from '../place.service';
import { PlaceResponse } from '../interfaces';
import { LatLong, getLocation } from '../../locations/utils';

@Component({
  selector: 'app-nearby-places-map',
  imports: [LeafletDirective, LeafletLayerDirective],
  templateUrl: './nearby-places-map.html',
  styleUrl: './nearby-places-map.css',
})
export class NearbyPlacesMap implements OnInit {
  placeService = inject(PlaceService);
  places = signal<PlaceResponse[]>([]);
  latitude: number | undefined;
  longitude: number | undefined;

  options = {
    layers: [tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 18, attribution: '...' })], 
    zoom: 15, 
    center: latLng(0,0)
  };
  layer = circle([0,0], { radius: 0});

  async ngOnInit() {
    let latLong:LatLong = await getLocation();
    this.latitude = latLong.latitude;
    this.longitude = latLong.longitude;

    this.options.center = latLng(this.latitude, this.longitude);
    this.layer = circle([this.latitude, this.longitude], { radius: 50});
  }
}
