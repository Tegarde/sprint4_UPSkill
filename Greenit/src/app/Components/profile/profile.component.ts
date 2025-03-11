import { CommonModule, NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { SignUpService } from '../../Services/sign-up.service';
import { GreenitorComplete } from '../../Models/greenitor-complete';
import { Post } from '../../Models/post';
import { PostService } from '../../Services/post.service';
import { PostListingComponent } from '../Post/post-listing/post-listing.component';

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
  constructor(private userService: SignUpService,private postService : PostService) { }

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
  }
  }


