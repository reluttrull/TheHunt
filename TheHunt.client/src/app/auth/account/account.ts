import { Component, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-account',
  imports: [DatePipe],
  templateUrl: './account.html',
  styleUrl: './account.css',
})
export class Account {
  auth = inject(AuthService);
  user = this.auth.user;


}
