import { Component, OnInit } from '@angular/core';
import { Evento } from '../../../Models/evento';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { EventService } from '../../../Services/event.service';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { MakeCommentComponent } from '../../make-comment/make-comment.component';
import { CommentDetailsComponent } from '../../comment-details/comment-details.component';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [NgIf,DatePipe,MakeCommentComponent,CommentDetailsComponent,NgFor, RouterLink],
  templateUrl: './event-detail.component.html',
  styleUrl: './event-detail.component.css'
})
export class EventDetailComponent implements OnInit{
  evento?: Evento;
  isLoading = true;
  errorMessage = '';
  makingAComment = false;

  constructor(private route: ActivatedRoute, private eventService: EventService) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const eventId = params['id'];
      this.loadEvent(eventId);
    });
  }

  loadEvent(eventId: number): void {
    this.eventService.getEventById(eventId).subscribe({
      next: (data) => {
        this.evento = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Erro ao carregar detalhes do evento!';
        this.isLoading = false;
      }
    });
  }

  toggleMakingAComment(): void {
    this.makingAComment = !this.makingAComment;
  }
}
