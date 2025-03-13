import { Component, OnInit } from '@angular/core';
import { MakePostComponent } from '../Post/make-post/make-post.component';
import { MonthlyListingComponent } from "../Post/monthly-listing/monthly-listing.component";
import { HottestPostsComponent } from "../Post/hottest-posts/hottest-posts.component";
import { DailyListingComponent } from "../Post/daily-listing/daily-listing.component";
import { SignInService } from '../../Services/sign-in.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MakePostComponent, MonthlyListingComponent, HottestPostsComponent, DailyListingComponent, NgIf],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  /** Stores the authenticated user's information */
  user?: any | null = null;

  /**
   * Constructor initializes authentication service
   * @param authService - Service for handling user authentication
   */
  constructor(private authService: SignInService) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Subscribes to user authentication status and stores username
   */
  ngOnInit(): void {
    this.authService.getUserSubject().subscribe((user) => this.user = user?.username);
  }
}