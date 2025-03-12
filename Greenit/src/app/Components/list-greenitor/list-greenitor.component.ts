import { Component } from '@angular/core';
import { GreenitorComplete } from '../../Models/greenitor-complete';
import { GreenitorService } from '../../Services/greenitor.service';
import { NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-list-greenitor',
  standalone: true,
  imports: [NgFor,RouterLink],
  templateUrl: './list-greenitor.component.html',
  styleUrl: './list-greenitor.component.css'
})
export class ListGreenitorComponent {
  greenitors: GreenitorComplete[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private greenitorService: GreenitorService) {}

  ngOnInit(): void {
    this.loadGreenitors();
  }

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
