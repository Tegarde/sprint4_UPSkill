import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EventService } from '../../../Services/event.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-create-event',
  standalone: true,
  imports: [ReactiveFormsModule,NgIf],
  templateUrl: './create-event.component.html',
  styleUrl: './create-event.component.css'
})
export class CreateEventComponent {
  eventForm: FormGroup;
  isSubmitting = false;
  msg = '';
  selectedFile: File | null = null;

  constructor(private fb: FormBuilder, private eventService: EventService, private router: Router) {
    this.eventForm = this.fb.group({
      description: ['', Validators.required],
      location: ['', Validators.required],
      date: ['', Validators.required]
    });
  }

  onFileSelected(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      this.selectedFile = inputElement.files[0];
    }
  }

  createEvent() {
    if (this.eventForm.invalid) {
      return; // Prevent submission if form is invalid
    }

    this.isSubmitting = true;


  const formData = new FormData();
  formData.append('description', this.eventForm.value.description);
  formData.append('location', this.eventForm.value.location);
  formData.append('date',this.eventForm.value.date); 
  if (this.selectedFile) {
    formData.append('image', this.selectedFile); 
  } else {
    formData.append('image', ''); 
  }

    this.eventService.createEvent(formData).subscribe({
      next: (response) => {
        this.msg=response.message;
        this.router.navigate(['/events']); // Redirect to events list
      },
      error: (error) => {
        this.msg=error.error.message;
        this.isSubmitting = false;
      }
    });
  }
}
