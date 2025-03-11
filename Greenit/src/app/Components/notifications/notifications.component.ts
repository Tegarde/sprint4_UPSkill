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
  notifications : any[] = [];

  userSubject! : TokenInfo | null;

  showNotifications = false;

  constructor(private authService : SignInService, private router : Router) { }

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

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
  }

  getNotifications() {
    this.authService.getUserNotifications().subscribe({
      next : (notifications) => this.notifications = notifications
    });
  }

  seePost(id : number) {
    this.router.navigate(['/post'], { queryParams: { id: id } });
    this.notifications = this.notifications.filter(notification => notification.postId !== id);
  }
}
