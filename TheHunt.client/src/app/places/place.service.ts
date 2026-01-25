import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CreatePlaceRequest, GetAllPlacesRequest, PlaceResponse } from './interfaces';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PlaceService {
    http = inject(HttpClient);
    router = inject(Router);
    private baseUrl = environment.api;

    createPlace(locationId:string, name:string, acceptedRadiusMeters:number, hint1:string, hint2:string, hint3:string) {
        let request: CreatePlaceRequest = {
            id: null,
            addedByUserId: null,
            name: name,
            locationId: locationId,
            acceptedRadiusMeters: acceptedRadiusMeters,
            hint1: hint1,
            hint2: hint2,
            hint3: hint3
        };
        return this.http.post<PlaceResponse>(`${this.baseUrl}/places`, request);
    }

    getPlace(id:string) {
        return this.http.get<PlaceResponse>(`${this.baseUrl}/places/${id}`);
    }

    getAllPlacesForUser() {
        return this.http.get<PlaceResponse[]>(`${this.baseUrl}/places/me`);
    }
    
    getAllPlaces(reqLatitude:number, reqLongitude:number, userId:string|null = null, 
            minLatitude:number|null = null, maxLatitude:number|null = null, 
            minLongitude:number|null = null, maxLongitude:number|null = null) {
        let filters = [];
        filters.push(`requestLatitude=${reqLatitude}`);
        filters.push(`requestLongitude=${reqLongitude}`);
        if (userId) filters.push(`userId=${userId}`);
        if (minLatitude) filters.push(`minLatitude=${minLatitude}`);
        if (maxLatitude) filters.push(`maxLatitude=${maxLatitude}`);
        if (minLongitude) filters.push(`minLongitude=${minLongitude}`);
        if (maxLongitude) filters.push(`maxLongitude=${maxLongitude}`);
        let queryString = filters.join('&');
        return this.http.get<PlaceResponse[]>(`${this.baseUrl}/places${queryString.length > 0 ? '?' : ''}${queryString}`);
    }
}