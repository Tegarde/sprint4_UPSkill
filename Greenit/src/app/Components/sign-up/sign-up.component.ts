import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SignUpService } from '../../Services/sign-up.service';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent {

  message?: string;

  signUpForm : FormGroup;

  constructor(private fb: FormBuilder, private signUpService: SignUpService, private router : Router) {
    this.signUpForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(16)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(16)]],

    });
  }

  onSubmit() {
    if (this.signUpForm.valid) {
      this.signUpService.register(this.signUpForm.value).subscribe({
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