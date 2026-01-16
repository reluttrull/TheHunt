import { Component, computed, inject } from '@angular/core';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App {
  auth = inject(AuthService);
  user = this.auth.user;
  currentYear = new Date().getFullYear();

  logout() {
    this.auth.logout();
  }

  isLoggedIn = computed(() => {
    return !!this.user();
});
}
