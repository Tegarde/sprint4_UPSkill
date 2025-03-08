import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { SignInService } from './Services/sign-in.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, NgClass, NgIf, AsyncPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  isSideBarOpen = new BehaviorSubject<boolean>(false);

  constructor(private authService : SignInService, private router : Router) { }

  logout() {
    this.authService.signOut();
    this.router.navigate(['/']);
    this.isSideBarOpen.next(false);
  }

  goHome() {
    this.router.navigate(['/']);
    this.isSideBarOpen.next(false);
  }
}
