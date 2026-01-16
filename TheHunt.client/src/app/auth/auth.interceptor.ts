import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../auth/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);

  // never intercept refresh itself
  if (req.url.endsWith('/refresh')) {
    return next(req);
  }

  // attach access token if present
  const accessToken = auth.getAccessToken();
  const authReq = accessToken
    ? req.clone({
        setHeaders: { Authorization: `Bearer ${accessToken}` }
      })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      console.log('error caught by interceptor', error);
      // if not an auth problem, pass it through untouched
      if (error.status != 401) {
        return throwError(() => error);
      }

      // if no refresh token, hard fail
      if (!auth.getRefreshToken()) {
        auth.logout();
        return throwError(() => error);
      }

      // attempt refresh once, then retry original request
      return auth.refreshToken().pipe(
        switchMap(res => {
          const retryReq = req.clone({
            setHeaders: {
              Authorization: `Bearer ${res.accessToken}`
            }
          });
          return next(retryReq);
        }),
        catchError(refreshErr => {
          auth.logout();
          return throwError(() => refreshErr);
        })
      );
    })
  );
};
