import { inject, Injectable, effect, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { RegisterRequest, LoginRequest, TokenResponse, UserResponse } from './interfaces';
import { environment } from '../../environments/environment';

const ACCESS_TOKEN_KEY = 'jwt';
const REFRESH_TOKEN_KEY = 'refresh_token';

@Injectable({ providedIn: 'root' })
export class AuthService {
    http = inject(HttpClient);
    router = inject(Router);
    private baseUrl = environment.api;
    public hasTokenSignal = signal<boolean>(this.hasToken());
    private readonly _user = signal<UserResponse | null>(null);
    readonly user = this._user.asReadonly();

    constructor() {
        effect(() => {
            if (this.hasTokenSignal()) {
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
        return this.http
            .post<TokenResponse>(`${this.baseUrl}/login`, formData)
            .pipe(tap(res => this.setTokens(res.accessToken, res.refreshToken)));
    }

    getUserInfo() {
        return this.http.get(`${this.baseUrl}/users/me`);
    }
    
    refreshToken() {
        const refreshToken = this.getRefreshToken();
        return this.http
            .post<TokenResponse>(`${this.baseUrl}/refresh`, { refreshToken: refreshToken })
            .pipe(tap(res => this.setTokens(res.accessToken, res.refreshToken)));
    }
    
    logout() {
        this.clear();
        this.router.navigate(['login']);
    }

    setTokens(accessToken: string, refreshToken: string): void {
        localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
        localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
        this.hasTokenSignal.set(true);
        this.loadUser();
    }
    
    clear(): void {
        localStorage.removeItem(ACCESS_TOKEN_KEY);
        localStorage.removeItem(REFRESH_TOKEN_KEY);
        this.hasTokenSignal.set(false);
        this._user.set(null);
    }
    
    getAccessToken(): string | null {
        return localStorage.getItem(ACCESS_TOKEN_KEY);
    }
    
    getRefreshToken(): string | null {
        return localStorage.getItem(REFRESH_TOKEN_KEY);
    }
    
    hasToken(): boolean {
        console.log(this.getAccessToken());
        return this.getAccessToken() != null;
    }
    
    private loadUser() {
        console.log(`${this.baseUrl}/users/me`);
        this.http.get<UserResponse>(`${this.baseUrl}/users/me`)
        .subscribe({
            next: user => this._user.set(user),
            error: () => this._user.set(null)
        });
    }
}