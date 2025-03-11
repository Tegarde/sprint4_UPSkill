import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { SignInService } from './Services/sign-in.service';
import { FormsModule } from '@angular/forms';
import { NotificationsComponent } from "./Components/notifications/notifications.component";
import { TokenInfo } from './Models/token-info';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, NgClass, NgIf, AsyncPipe, FormsModule, NotificationsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  user : TokenInfo | null = null;

  isSideBarOpen = new BehaviorSubject<boolean>(false);

  query : string = "";

  constructor(private authService : SignInService, private router : Router) { }

  ngOnInit(): void {
    this.authService.getUserSubject().subscribe((user) => this.user = user);
  }

  logout() {
    this.authService.signOut();
    this.router.navigate(['/']);
    this.isSideBarOpen.next(false);
  }

  goHome() {
    this.router.navigate(['/']);
    this.isSideBarOpen.next(false);
  }

  searchPosts() {
    console.log(this.query); 
    this.router.navigate(['/search', this.query]);
  }
}
