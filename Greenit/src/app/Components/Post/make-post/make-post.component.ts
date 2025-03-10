import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PostService } from '../../../Services/post.service';
import { SignInService } from '../../../Services/sign-in.service';
import { CategoryService } from '../../../Services/category.service';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-make-post',
  standalone: true,
  imports: [ReactiveFormsModule, NgFor, NgIf],
  templateUrl: './make-post.component.html',
  styleUrl: './make-post.component.css'
})
export class MakePostComponent implements OnInit {
  postForm : FormGroup;

  categories? : any[];

  showForm = false;

  selectedFile? : File

  constructor(private postService : PostService,
    private fb : FormBuilder,
    private authService : SignInService,
    private categoryService : CategoryService,
    private eRef : ElementRef
  ) {
    this.postForm = this.fb.group({
      'title' : ['', Validators.required],
      'content' : ['', Validators.required],
      'createdBy' : ['', Validators.required],
      'category' : ['', Validators.required],
    });
   }

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe((categories) => this.categories = categories);
    this.authService.getUserSubject().subscribe((user) => this.postForm.controls['createdBy'].setValue(user!.username));
  }

  submitPost() {
    if (!this.selectedFile) {
      this.postService.createPost(this.postForm.value).subscribe(() => window.location.reload());
    } else {
      let formData = new FormData();
      formData.append('title', this.postForm.value.title);
      formData.append('content', this.postForm.value.content);
      formData.append('createdBy', this.postForm.value.createdBy);
      formData.append('category', this.postForm.value.category);
      formData.append('image', this.selectedFile);

      this.postService.createPost(formData).subscribe(() => window.location.reload());
    }
  }

  toggleForm() {
    this.showForm = true;
  }

  @HostListener('document:click', ['$event'])
  handleClickOutside(event: Event) {
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.showForm = false;
      this.postForm.get('title')?.reset();
      this.postForm.get('content')?.reset();
      this.postForm.get('category')?.patchValue('');
      this.selectedFile = undefined;
    }
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }
}
