import { CommonModule, NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { SignUpService } from '../../Services/sign-up.service';
import { GreenitorComplete } from '../../Models/greenitor-complete';
import { Post } from '../../Models/post';
import { PostService } from '../../Services/post.service';
import { PostListingComponent } from '../Post/post-listing/post-listing.component';
import { GreenitorService } from '../../Services/greenitor.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule,PostListingComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  user? : GreenitorComplete;
  posts: Post[] =[];
  
  constructor(private userService: SignUpService, private postService: PostService, private greenitorService: GreenitorService) { }
  stats = {
    likesInPosts: 0,
    dislikesInPosts: 0,
    likesInComments: 0,
    eventAttendances: 0,
    posts: 0,
    comments: 0,
    favoritePosts: 0
  }

  async ngOnInit() {
  // ✅ Get user data
  this.userService.getUserByUsername('tegarde').subscribe({
    next: (user) => {
      this.user = user;
    },
    error: (err) => console.error("Error fetching user:", err)
  });

  // ✅ Get posts
  this.postService.getPostsByUser('tegarde').subscribe({
    next: (posts) => {
      this.posts = posts;
      console.log("Posts:", posts);
    },
    error: (err) => console.error("Error fetching posts:", err)
  });

  this.greenitorService.getGreenitorStats('tegarde').subscribe({next: (stats) => {this.stats = stats;},error: (err) => console.error("Error fetching stats:", err)});
  // ✅ Get user notifications    
  this.greenitorService.getGreenitorNotifications('tegarde').subscribe({next: (notifications) => {console.log("Notifications:", notifications);},error: (err) => console.error("Error fetching notifications:", err)});
  }
  }


