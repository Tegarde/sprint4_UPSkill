import { Component } from '@angular/core';
import { MakePostComponent } from '../Post/make-post/make-post.component';
import { MonthlyListingComponent } from "../Post/monthly-listing/monthly-listing.component";
import { HottestPostsComponent } from "../Post/hottest-posts/hottest-posts.component";
import { DailyListingComponent } from "../Post/daily-listing/daily-listing.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MakePostComponent, MonthlyListingComponent, HottestPostsComponent, DailyListingComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
