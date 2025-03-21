import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SignUpService } from '../../Services/sign-up.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, NgIf],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent {
  /** Message displayed for success or error during registration */
  message?: string;

  /** Form group for user sign-up */
  signUpForm: FormGroup;

  /** Holds the selected file for profile picture upload */
  selectedFile: File | null = null;

  /**
   * Constructor initializes required services
   * @param fb - FormBuilder for creating the sign-up form
   * @param signUpService - Service for handling user registration
   * @param router - Router for navigation after successful registration
   */
  constructor(private fb: FormBuilder, private signUpService: SignUpService, private router: Router) {
    this.signUpForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(16)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(16)]],
    });
  }

  /**
   * Handles file selection for profile picture upload
   * @param event - File input event
   */
  onFileSelected(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      this.selectedFile = inputElement.files[0];
    }
  }

  /**
   * Submits the sign-up form
   * - Validates form inputs
   * - Uploads profile picture if selected
   * - Sends registration request to the backend
   */
  onSubmit(): void {
    if (this.signUpForm.valid) {
      const formData = new FormData();
      formData.append('username', this.signUpForm.value.username);
      formData.append('email', this.signUpForm.value.email);
      formData.append('password',this.signUpForm.value.password); 
      if (this.selectedFile) {
        formData.append('image', this.selectedFile); 
      } else {
        formData.append('image', ''); 
      }

      this.signUpService.register(formData).subscribe({
        next: () => {
          this.message = "User registered successfully";
          this.signUpForm.reset();

          setTimeout(() => {
            this.message = "";
            this.router.navigate(['']);
          }, 1000);
        },
        error: () => {
          this.message = "User registration failed";
        }
      });
    }
  }
}