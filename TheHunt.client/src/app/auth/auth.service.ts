import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { RegisterRequest } from './interfaces';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private baseUrl = environment.api;
    http = inject(HttpClient);
    router = inject(Router);
    register(formData:RegisterRequest) {
        return this.http.post(`${this.baseUrl}/register`, formData);
    }
}