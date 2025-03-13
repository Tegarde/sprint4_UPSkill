import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
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
  /** Stores authenticated user information */
  user: TokenInfo | null = null;

  /** Controls the sidebar state (open/closed) */
  isSideBarOpen = new BehaviorSubject<boolean>(false);

  /** Stores the search query */
  query: string = "";

  /**
   * Constructor initializes required services
   * @param authService - Service for managing user authentication
   * @param router - Router for navigation
   */
  constructor(private authService: SignInService, private router: Router) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Subscribes to authentication changes
   */
  ngOnInit(): void {
    this.authService.getUserSubject().subscribe((user) => this.user = user);
  }

  /**
   * Logs out the user and redirects to the home page
   */
  logout(): void {
    this.authService.signOut();
    this.router.navigate(['/']);
    this.isSideBarOpen.next(false);
  }

  /**
   * Navigates to the home page and closes the sidebar
   */
  goHome(): void {
    this.router.navigate(['/']);
    this.isSideBarOpen.next(false);
  }

  /**
   * Initiates a post search based on the entered query
   */
  searchPosts(): void {
    console.log(this.query);
    this.router.navigate(['/search', this.query]);
  }
}