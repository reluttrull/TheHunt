import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../auth.service';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  auth = inject(AuthService);
  router = inject(Router);
  fb = inject(FormBuilder);
  form:FormGroup = this.fb.group({
    email: [null],
    password: [null]
  });
  validationErrors = signal<string[]>([]);
  isWaiting = signal(false);

  submit() {
    this.isWaiting.set(true);
    this.validationErrors.set([]);

    this.auth.login(this.form.value).subscribe({
      next: (res) => {
        this.isWaiting.set(false);
        console.log(res);
        this.router.navigate(['']);
      },
      error: (err) => {
        console.log(err);
        if (err.status == 400 && err.error?.errors) {
          for (var currentError in err.error.errors) {
            this.validationErrors.update(errs => [...errs, `${currentError}: ${err.error.errors[currentError]}`]);
          }
        } else if (err.error) {
          this.validationErrors.update(errs => [...errs, err.error]);
        } else {
          this.validationErrors.update(errs => [...errs, 'An unexpected error occurred.']);
        };
        this.isWaiting.set(false);
        return;
      }
    });
  }

}
