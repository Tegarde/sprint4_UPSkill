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

  constructor(private fb: FormBuilder, private eventService: EventService, private router: Router) {
    this.eventForm = this.fb.group({
      description: ['', Validators.required],
      location: ['', Validators.required],
      date: ['', Validators.required]
    });
  }

  createEvent() {
    if (this.eventForm.invalid) {
      return; // Prevent submission if form is invalid
    }

    this.isSubmitting = true;

    this.eventService.createEvent(this.eventForm.value).subscribe({
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
