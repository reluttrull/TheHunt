import { Component, OnInit, inject, input, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { LeafletDirective, LeafletLayerDirective } from '@bluehalo/ngx-leaflet';
import { tileLayer, latLng, marker } from 'leaflet';
import { LocationService } from '../location.service';
import { LocationResponse } from '../interfaces';
import { CreatePlace } from "../../places/create-place/create-place";

@Component({
  selector: 'app-location',
  imports: [DatePipe, LeafletDirective, LeafletLayerDirective, CreatePlace],
  templateUrl: './location.html',
  styleUrl: './location.css',
})
export class Location implements OnInit {
  location = input.required<LocationResponse>();
  definable = input<boolean>(true);
  locationService = inject(LocationService);
  isCreatePlaceVisible = signal(false);

  options = {
    layers: [tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 18, attribution: '...' })], 
    zoom: 15, 
    center: latLng(0,0)
  };
  layer = marker([0,0]);

  ngOnInit() {
    this.options.center = latLng(this.location().latitude, this.location().longitude);
    this.layer = marker([this.location().latitude, this.location().longitude]);
  }

  showCreatePlace() {
    this.isCreatePlaceVisible.set(true);
  }

  hideCreatePlace() {
    this.isCreatePlaceVisible.set(false);
  }
}
