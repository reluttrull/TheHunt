import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../auth.service';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  auth = inject(AuthService);
  fb = inject(FormBuilder);
  form:FormGroup = this.fb.group({
    email: [null],
    userName: [null],
    password: [null],
    confirmPassword: [null]
  });
  validationErrors = signal<string[]>([]);
  isWaiting = signal(false);

  submit() {
    this.isWaiting.set(true);
    if (this.form.value.password != this.form.value.confirmPassword) {
      this.validationErrors.update(errs => [...errs, 'Typed passwords do not match.']);
      return;
    }
    this.auth.register(this.form.value).subscribe({
      next: (res) => {
        this.isWaiting.set(false);
        console.log(res);
        // todo: navigate to login
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
