import { Component, OnInit } from '@angular/core';
import { SignInService } from '../../Services/sign-in.service';
import { TokenInfo } from '../../Models/token-info';
import { NgClass, NgFor, NgIf } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [NgIf, NgFor, NgClass],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css'
})
export class NotificationsComponent implements OnInit {
  /** List of notifications received by the user */
  notifications: any[] = [];

  /** Holds authenticated user information */
  userSubject!: TokenInfo | null;

  /** Controls the visibility of the notifications dropdown */
  showNotifications = false;

  /**
   * Constructor initializes authentication and routing services
   * @param authService - Service for managing user authentication and fetching notifications
   * @param router - Router for navigation
   */
  constructor(private authService: SignInService, private router: Router) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Subscribes to the authenticated user data
   * - Fetches notifications if the user is authenticated
   */
  ngOnInit(): void {
    this.authService.getUserSubject().subscribe({
      next: (user) => {
        this.userSubject = user;
        
        if (user !== null) {
          this.getNotifications();
        }
      }
    });
  }

  /**
   * Toggles the visibility of the notifications dropdown
   */
  toggleNotifications(): void {
    this.showNotifications = !this.showNotifications;
  }

  /**
   * Fetches the user's notifications from the authentication service
   */
  getNotifications(): void {
    this.authService.getUserNotifications().subscribe({
      next: (notifications) => this.notifications = notifications
    });
  }

  /**
   * Navigates to a post and removes the corresponding notification
   * @param id - The ID of the post associated with the notification
   */
  seePost(id: number): void {
    this.router.navigate(['/post'], { queryParams: { id: id } });
    this.notifications = this.notifications.filter(notification => notification.postId !== id);
  }
}