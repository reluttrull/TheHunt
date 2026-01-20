import { inject, Injectable, effect, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap, map, catchError, of } from 'rxjs';
import { RegisterRequest, LoginRequest, TokenResponse, UserResponse } from './interfaces';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
    http = inject(HttpClient);
    router = inject(Router);
    private baseUrl = environment.api;
    public isAuthenticated = signal<boolean>(false);
    private readonly _user = signal<UserResponse | null>(null);
    readonly user = this._user.asReadonly();

    constructor() {
        effect(() => {
            if (this.check()) {
                this.loadUser();
            } else {
                this._user.set(null);
            }
        });
    }

    register(formData:RegisterRequest) {
        return this.http.post(`${this.baseUrl}/register`, formData);
    }

    login(formData:LoginRequest) {
        return this.http.post(`${this.baseUrl}/login`, formData,)
            .pipe(tap(() => {
                this.isAuthenticated.set(true);
                this.loadUser();
        }));
    }

    getUserInfo() {
        return this.http.get(`${this.baseUrl}/users/me`);
    }
    
    refreshToken() {
        return this.http.post(`${this.baseUrl}/refresh`, {});
    }
    
    logout() {
        this.http.post(`${this.baseUrl}/revoke`, {},).subscribe();
        this.isAuthenticated.set(false);
        this.router.navigate(['login']);
    }

    check() {
        return this.http.get(`${this.baseUrl}/users/me`,).pipe(
            map(() => true),
            catchError(() => of(false))
        );
    }

    hasToken(): boolean {
        return this.user != null;
    }
    
    private loadUser() {
        this.http.get<UserResponse>(`${this.baseUrl}/users/me`)
        .subscribe({
            next: user => this._user.set(user),
            error: () => this._user.set(null)
        });
    }
}
