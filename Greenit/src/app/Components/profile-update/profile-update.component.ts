import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { GreenitorService } from '../../Services/greenitor.service';
import { TokenService } from '../../Services/token.service';
import { SignInService } from '../../Services/sign-in.service';
import { concatMap, filter, switchMap, tap } from 'rxjs';
import { NgIf } from '@angular/common';
import { GreenitorComplete } from '../../Models/greenitor-complete';

@Component({
  selector: 'app-profile-update',
  standalone: true,
  imports: [ReactiveFormsModule,NgIf],
  templateUrl: './profile-update.component.html',
  styleUrl: './profile-update.component.css'
})
export class ProfileUpdateComponent implements OnInit {
  profileForm: FormGroup;
  selectedFile: File | null = null;
  username?: string;
  user?: GreenitorComplete;
  role?: string;
  msg: string = '';

  constructor(private fb: FormBuilder, private userService: GreenitorService, private signInService: SignInService) {
    this.profileForm = this.fb.group({
      email: [''],
      password: [''],
      confirmPassword: [''],
    });
  }

  ngOnInit() {
    this.signInService.getUserSubject().subscribe({
      next: (user) => {
        this.username = user!.username;
        this.role = user!.role;
        
        console.log('Fetching user by username:', this.username); // Debugging
  
        // Now fetch user data
        this.userService.getUserByUsername(this.username).subscribe({
          next: (data) => {
            this.user=data;
            console.log('User data:', data); // Debugging
            this.profileForm.patchValue({ email: data.email });
          },
          error: (err) => {
            console.error('Erro ao carregar os dados do utilizador', err);
            this.msg = 'Erro ao carregar os dados do utilizador.';
          }
        });
      },
      error: (err) => {
        console.error('Erro ao obter utilizador autenticado', err);
        this.msg = 'Erro ao obter utilizador autenticado.';
      }
    });
  }
  
  


  onFileSelected(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      this.selectedFile = inputElement.files[0];
    }
  }

  onSubmit() {
    if (this.profileForm.invalid) {
      this.msg = 'Por favor, preencha corretamente os campos.';
      return;
    }
    if (this.profileForm.get('password')?.value !== this.profileForm.get('confirmPassword')?.value 
    && (this.profileForm.get('password')?.value !== '' &&  this.profileForm.get('confirmPassword')?.value !== '')) {
      this.msg = 'As passwords não coincidem!';
      return;
    }
    const formData = new FormData();
    formData.append('email', this.profileForm.get('email')?.value);

    if (this.profileForm.get('password')?.value !== '')
    {
      formData.append('password', this.profileForm.get('password')?.value);
    }else{
      formData.append('password', '');
    }

    if (this.selectedFile) {

      this.userService.uploadProfilePicture(this.selectedFile).subscribe({
        next: (imageUrl) => {

          formData.append('image', imageUrl.fileName);


          this.userService.updateUser(this.username!, formData).subscribe({
            next: () => {
              this.msg = 'Profile updated successfully!';
              this.profileForm.get('password')?.reset();
              this.profileForm.get('confirmPassword')?.reset();
              this.selectedFile = null;
            },
            error: (err) => this.msg = 'Error updating profile',
          });
        },
        error: (err) => this.msg = 'Error uploading profile picture',
      });
    } else {
      formData.append('image', '');
      this.userService.updateUser(this.username!, formData).subscribe({
        next: () => {
          this.msg = 'Profile updated successfully!';
          this.profileForm.get('password')?.reset();
          this.profileForm.get('confirmPassword')?.reset();
          this.selectedFile = null;
        },
        error: (err) => this.msg = 'Error updating profile',
      });
    }

  }


}
