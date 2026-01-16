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

  logout() {
    this.auth.logout();
  }

  isLoggedIn = computed(() => {
    console.log('computing', this.user());
    return !!this.user();
});
}
