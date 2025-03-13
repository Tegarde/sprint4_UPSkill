import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EventService } from '../../../Services/event.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-create-event',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './create-event.component.html',
  styleUrl: './create-event.component.css'
})
export class CreateEventComponent {
  /** Form group for event creation */
  eventForm: FormGroup;

  /** Flag to indicate form submission status */
  isSubmitting = false;

  /** Message displayed for success or error */
  msg = '';

  /** Holds the selected file for upload */
  selectedFile: File | null = null;

  /**
   * Constructor initializes the form and injects required services
   * @param fb - FormBuilder for reactive forms
   * @param eventService - Service for event-related API calls
   * @param router - Router for navigation
   */
  constructor(private fb: FormBuilder, private eventService: EventService, private router: Router) {
    this.eventForm = this.fb.group({
      description: ['', Validators.required],
      location: ['', Validators.required],
      date: ['', Validators.required]
    });
  }

  /**
   * Handles file selection and stores the selected file
   * @param event - File input event
   */
  onFileSelected(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      this.selectedFile = inputElement.files[0];
    }
  }

  /**
   * Submits the form to create a new event with optional image upload
   */
  createEvent() {
    if (this.eventForm.invalid) {
      return; // Prevent submission if form is invalid
    }

    this.isSubmitting = true;

    // Prepare form data for submission
    const formData = new FormData();
    formData.append('description', this.eventForm.value.description);
    formData.append('location', this.eventForm.value.location);
    formData.append('date', this.eventForm.value.date);
    if (this.selectedFile) {
      formData.append('image', this.selectedFile);
    } else {
      formData.append('image', '');
    }

    // Call service to create event
    this.eventService.createEvent(formData).subscribe({
      next: (response) => {
        this.msg = response.message;
        this.router.navigate(['/events']);
      },
      error: (error) => {
        this.msg = error.error.message;
        this.isSubmitting = false;
      }
    });
  }
}