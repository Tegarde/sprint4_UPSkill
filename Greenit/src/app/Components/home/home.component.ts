import { Component, OnInit } from '@angular/core';
import { MakePostComponent } from '../Post/make-post/make-post.component';
import { MonthlyListingComponent } from "../Post/monthly-listing/monthly-listing.component";
import { HottestPostsComponent } from "../Post/hottest-posts/hottest-posts.component";
import { DailyListingComponent } from "../Post/daily-listing/daily-listing.component";
import { SignInService } from '../../Services/sign-in.service';
import { AsyncPipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MakePostComponent, MonthlyListingComponent, HottestPostsComponent, DailyListingComponent, NgIf],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  user? : any | null = null;

  constructor(private authService : SignInService) {}

  ngOnInit(): void {
    this.authService.getUserSubject().subscribe((user) => this.user = user?.username);
  }
}
