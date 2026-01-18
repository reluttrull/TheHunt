import { Component, OnInit, inject, signal } from '@angular/core';
import { LeafletDirective, LeafletLayerDirective } from '@bluehalo/ngx-leaflet';
import * as L from 'leaflet';
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

  options: L.MapOptions | undefined;
  layer = L.circle([0,0], { radius: 0});
  markers: L.Layer[] = [];

  async ngOnInit() {
    let latLong:LatLong = await getLocation();
    this.latitude = latLong.latitude;
    this.longitude = latLong.longitude;
    console.log('user latlong', this.latitude, this.longitude);
    this.placeService.getAllPlaces(null, null, null, null, null)
      .subscribe({
        next: res => {
          this.places.set(res);
          this.places().forEach(p => {
            console.log('place latlong', p.latitude, p.longitude);
            this.markers.push(L.marker([p.latitude, p.longitude]
            //   , {
            //   icon: L.icon({
            //     ...L.Icon.Default.prototype.options,
            //     iconUrl: 'assets/marker-icon.png',
            //     iconRetinaUrl: 'assets/marker-icon-2x.png',
            //     shadowUrl: 'assets/marker-shadow.png'
            //   })
            // }
            ));
          });
          this.markers = [...this.markers];
        },
        error: err => console.error('could not retrieve nearby places for user', err)
      });

    this.options = {
      layers: [
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
          maxZoom: 18,
          attribution: '...'
        })
      ],
      zoom: 15,
      center: L.latLng(this.latitude, this.longitude)
    };
    this.layer = L.circle([this.latitude, this.longitude], { radius: 50});
  }
}
