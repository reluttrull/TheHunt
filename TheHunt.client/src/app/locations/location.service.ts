import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CreateLocationRequest, LocationResponse } from './interfaces';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class LocationService {
    http = inject(HttpClient);
    router = inject(Router);
    private baseUrl = environment.api;

    createLocation(latitude:number, longitude:number) {
        let request: CreateLocationRequest = {
            latitude: latitude,
            longitude: longitude
        };
        return this.http.post<LocationResponse>(`${this.baseUrl}/locations`, request);
    }
}