import { Component } from '@angular/core';
import { EventService } from '../../../Services/event.service';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { Evento } from '../../../Models/evento';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-list-event',
  standalone: true,
  imports: [NgIf,NgFor,DatePipe,RouterLink],
  templateUrl: './list-event.component.html',
  styleUrl: './list-event.component.css'
})
export class ListEventComponent {
  events: Evento[] = [];
  isLoading = true;
  errorMessage = '';
  selectedFilter: string = 'Open'; 

  constructor(private eventService: EventService) {}

  ngOnInit(): void {
    this.loadEvents();
  }

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
  

  handleSuccess(data: Evento[]): void {
    this.events = data;
    this.isLoading = false;
    this.errorMessage = '';
  }

  handleError(): void {
    this.errorMessage = 'Erro ao carregar eventos!';
    this.isLoading = false;
  }

  onFilterChange(event: Event): void {
    const target = event.target as HTMLSelectElement | null;
    if (!target) return; // Garante que target não é null
  
    const selectedValue = target.value;
    console.log("Filtro selecionado:", selectedValue);
    this.selectedFilter = selectedValue;
    this.loadEvents();
  }
}
