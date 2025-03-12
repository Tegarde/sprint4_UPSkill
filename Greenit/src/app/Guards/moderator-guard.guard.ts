import { inject } from '@angular/core';
import { ActivatedRoute, CanActivateFn, Router } from '@angular/router';
import { SignInService } from '../Services/sign-in.service';
import { catchError, map, of } from 'rxjs';

export const moderatorGuardGuard: CanActivateFn = (route, state) => {
  const signInService = inject(SignInService);
  const router = inject(Router);

  return signInService.getUserSubject().pipe(
    map((user) => {
      if (!user) {
        setTimeout(() => {
          router.navigate(['login']);
          alert('You must be signed in to view this page.');
          return false;
        });        
      } 
      if (user!.role !== 'Moderator') {
        alert('You must be a moderator to view this page.');
        setTimeout(()=> {
          router.navigate(['']);
          return false;
        });
      }
      return true;
    }),
    catchError(() => {
      setTimeout(()=> router.navigate(['login']), 1000);
      return of(false);
    })
  );
};
