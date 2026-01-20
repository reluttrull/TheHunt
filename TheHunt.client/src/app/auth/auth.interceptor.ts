import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../auth/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const csrfReq = req.clone({
    withCredentials: true,
    setHeaders: {
      'X-CSRF': '1'
    }
  });

  return next(csrfReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status !== 401) {
        return throwError(() => error);
      }

      const auth = inject(AuthService);

      return auth.refreshToken().pipe(
        switchMap(() => next(csrfReq)),
        catchError(err => {
          auth.logout();
          return throwError(() => err);
        })
      );
    })
  );
};