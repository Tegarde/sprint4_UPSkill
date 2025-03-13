import { Component } from '@angular/core';
import { EventService } from '../../../Services/event.service';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { Evento } from '../../../Models/evento';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-list-event',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, RouterLink],
  templateUrl: './list-event.component.html',
  styleUrl: './list-event.component.css'
})
export class ListEventComponent {
  /** Array to store the list of events */
  events: Evento[] = [];

  /** Flag to indicate if data is loading */
  isLoading = true;

  /** Stores any error messages during data retrieval */
  errorMessage = '';

  /** Selected event filter (default: 'Open') */
  selectedFilter: string = 'Open';

  /**
   * Constructor initializes the event service
   * @param eventService - Service for event-related API calls
   */
  constructor(private eventService: EventService) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Loads the events based on the selected filter
   */
  ngOnInit(): void {
    this.loadEvents();
  }

  /**
   * Fetches events based on the selected status filter
   */
  loadEvents(): void {
    this.isLoading = true;
    this.eventService.getEventsByStatus(this.selectedFilter).subscribe({
      next: (data) => {
        this.events = data;
        this.isLoading = false;
        this.errorMessage = '';
      },
      error: () => {
        this.errorMessage = 'Erro ao carregar eventos!';
        this.isLoading = false;
      },
    });
  }

  /**
   * Handles successful API response
   * @param data - List of events retrieved from the API
   */
  handleSuccess(data: Evento[]): void {
    this.events = data;
    this.isLoading = false;
    this.errorMessage = '';
  }

  /**
   * Handles API errors
   */
  handleError(): void {
    this.errorMessage = 'Erro ao carregar eventos!';
    this.isLoading = false;
  }

  /**
   * Updates the event filter based on user selection
   * @param event - Change event from the dropdown
   */
  onFilterChange(event: Event): void {
    const target = event.target as HTMLSelectElement | null;
    if (!target) return; // Garante que target não é null
  
    const selectedValue = target.value;
    console.log("Filtro selecionado:", selectedValue);
    this.selectedFilter = selectedValue;
    this.loadEvents();
  }
}