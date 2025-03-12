import { AsyncPipe, DatePipe, NgClass, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Post } from '../../../Models/post';
import { PostService } from '../../../Services/post.service';
import { CategoryService } from '../../../Services/category.service';
import { PostListingComponent } from "../post-listing/post-listing.component";
import { TokenInfo } from '../../../Models/token-info';
import { SignInComponent } from '../../sign-in/sign-in.component';
import { SignInService } from '../../../Services/sign-in.service';
import { RouterLink } from '@angular/router';
import { AbstractControl, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';

@Component({
  selector: 'app-see-posts-with-filters',
  standalone: true,
  imports: [NgClass, NgIf, NgFor, DatePipe, RouterLink, ReactiveFormsModule, FormsModule],
  templateUrl: './see-posts-with-filters.component.html',
  styleUrl: './see-posts-with-filters.component.css'
})
export class SeePostsWithFiltersComponent implements OnInit {

  user : TokenInfo | null = null;

  isSideBarOpen = false;

  allPosts : Post[] = [];

  showingPosts : Post[] = [];

  categories : any[] = [];

  sorting = false;

  dateRangeForm: FormGroup;

  constructor(private postService : PostService, private categoryService : CategoryService, private authService : SignInService) {
    this.dateRangeForm = new FormGroup({
      startDate: new FormControl('', Validators.required),
      endDate: new FormControl('', [Validators.required, this.dateAfterStartValidator.bind(this)])
    });
  }

  dateAfterStartValidator(control: AbstractControl): ValidationErrors | null {
    const startDate = this.dateRangeForm?.get('startDate')?.value;
    if (startDate && control.value && new Date(control.value) <= new Date(startDate)) {
      return { invalidDateRange: true };  // Invalid if the end date is before or the same as the start date
    }
    return null;
  }

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe({
      next : (categories) => {
        this.categories = categories;
      }
    });

    this.postService.getAllPosts().subscribe({
      next : (posts) => {
        this.allPosts = posts.sort((a: Post, b:Post) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        this.authService.getUserSubject().subscribe({
          next : (user) => {
            this.user = user;
            this.setShowingPosts();
          }
        })
      }
    })
  }


  setShowingPosts() {
    if (this.user! !== null && this.user!.role === "Moderator") {
      this.showingPosts = this.allPosts;
    } else {
      this.showingPosts = this.allPosts.filter(p => p.status == true);
    }
  }

  sortByCategory() {
    if (!this.sorting) {
      this.showingPosts = this.showingPosts.sort((a, b) => a.category.localeCompare(b.category));
    } else {
      this.showingPosts = this.showingPosts.sort((a, b) => b.category.localeCompare(a.category));
    }
    this.sorting = !this.sorting;    
  }

  sortByCreator() {
    if (!this.sorting) {
      this.showingPosts = this.showingPosts.sort((a, b) => a.createdBy.localeCompare(b.createdBy));
    } else {
      this.showingPosts = this.showingPosts.sort((a, b) => b.createdBy.localeCompare(a.createdBy));
    }
    this.sorting = !this.sorting; 
  }

  sortByTitle() {
    if (!this.sorting) {
      this.showingPosts = this.showingPosts.sort((a, b) => a.title.localeCompare(b.title));
    } else {
      this.showingPosts = this.showingPosts.sort((a, b) => b.title.localeCompare(a.title));
    }
    this.sorting = !this.sorting;
  }

  sortByDate() {
    if (!this.sorting) {
      this.showingPosts = this.showingPosts.sort((a: Post, b:Post) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
    } else {
      this.showingPosts = this.showingPosts.sort((a: Post, b:Post) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime());
    }
    this.sorting = !this.sorting;
  }

  sortByStatus() {
    if (!this.sorting) {
      this.showingPosts = this.showingPosts.sort((a, b) => {
        return a.status === b.status ? 0 : a.status ? -1 : 1;
      });
    } else {
      this.showingPosts = this.showingPosts.sort((a, b) => {
        return a.status === b.status ? 0 : a.status ? 1 : -1;
      });
    }
    this.sorting = !this.sorting;
  }

  filterByCategory(event : Event) {
    const target = event.target as HTMLSelectElement | null;
    if (!target) {
      return;
    }

    const selectedValue = target.value;
    if (selectedValue == "All") {
      this.setShowingPosts();
    } else {
      this.setShowingPosts();
      this.showingPosts = this.showingPosts.filter(p => p.category == selectedValue);
    }
  }

  filterBetweenDates() {
    this.showingPosts = this.showingPosts.filter(a => {
      const startDate = new Date(this.dateRangeForm.get('startDate')?.value).getTime();
      const endDate = new Date(this.dateRangeForm.get('endDate')?.value).getTime();
      const createdAtDate = new Date(a.createdAt).getTime();
      return createdAtDate >= startDate && createdAtDate <= endDate;
    });
  }

  onStatusChange(id : number, status : boolean) {
    
    this.postService.changePostStatus(id, status).subscribe();
  }
}
