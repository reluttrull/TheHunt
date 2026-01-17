import { Component, OnInit, inject, input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { LeafletDirective, LeafletLayerDirective } from '@bluehalo/ngx-leaflet';
import { tileLayer, latLng, circle } from 'leaflet';
import { LocationService } from '../location.service';
import { LocationResponse } from '../interfaces';

@Component({
  selector: 'app-location',
  imports: [DatePipe, LeafletDirective, LeafletLayerDirective],
  templateUrl: './location.html',
  styleUrl: './location.css',
})
export class Location implements OnInit {
  location = input.required<LocationResponse>();
  locationService = inject(LocationService);

  options = {
    layers: [tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 18, attribution: '...' })], 
    zoom: 15, 
    center: latLng(0,0)
  };
  layer = circle([0,0], { radius: 0});

  ngOnInit() {
    this.options.center = latLng(this.location().latitude, this.location().longitude);
    this.layer = circle([this.location().latitude, this.location().longitude], { radius: 50});
  }

}
