import { Component } from '@angular/core';
import { GreenitorComplete } from '../../Models/greenitor-complete';
import { GreenitorService } from '../../Services/greenitor.service';
import { NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-list-greenitor',
  standalone: true,
  imports: [NgFor, RouterLink, NgIf],
  templateUrl: './list-greenitor.component.html',
  styleUrl: './list-greenitor.component.css'
})
export class ListGreenitorComponent {
  /** List of greenitors (users) fetched from the service */
  greenitors: any[] = [];

  /** Flag to indicate if data is currently loading */
  isLoading = true;

  /** Stores error message if data fetching fails */
  errorMessage = '';

  /**
   * Constructor initializes the GreenitorService
   * @param greenitorService - Service for fetching greenitor user data
   */
  constructor(private greenitorService: GreenitorService) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Calls the method to load greenitors
   */
  ngOnInit(): void {
    this.loadGreenitors();
  }

  /**
   * Fetches all greenitor users from the service
   * - Updates `greenitors` array with the fetched data
   * - Handles loading and error states
   */
  loadGreenitors(): void {
    this.greenitorService.getAllGreenitors().subscribe({
      next: (data) => {
        this.greenitors = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Erro ao carregar usuários!';
        this.isLoading = false;
      }
    });
  }
}