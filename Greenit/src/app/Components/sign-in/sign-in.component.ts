import { Component } from '@angular/core';
import { SignInService } from '../../Services/sign-in.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.css'
})
export class SignInComponent {
  /** Form group for user sign-in */
  signInForm: FormGroup;

  /** Message displayed for login success or failure */
  message?: string;

  /**
   * Constructor initializes authentication service, form builder, and router
   * @param signInService - Service handling user authentication
   * @param fb - FormBuilder for creating the reactive form
   * @param router - Router for navigation after successful login
   */
  constructor(private signInService: SignInService, private fb: FormBuilder, private router: Router) {
    this.signInForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  /**
   * Handles form submission for user login
   * - Validates form input
   * - Calls authentication service
   * - Redirects user upon successful login
   */
  onSubmit(): void {
    if (this.signInForm.valid) {
      this.signInService.signIn(this.signInForm.value.email, this.signInForm.value.password).subscribe({
        next: () => {
          this.message = "User logged in successfully";
          this.signInForm.reset();

          setTimeout(() => {
            this.router.navigate(['']);
          }, 1000);
        },
        error: () => {
          this.message = "User login failed";
        }
      });
    }
  }
}