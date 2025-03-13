import { Component, OnInit } from '@angular/core';
import { Evento } from '../../../Models/evento';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { EventService } from '../../../Services/event.service';
import { CommonModule, DatePipe, NgFor, NgIf } from '@angular/common';
import { MakeCommentComponent } from '../../make-comment/make-comment.component';
import { CommentDetailsComponent } from '../../comment-details/comment-details.component';
import { SignInService } from '../../../Services/sign-in.service';
import { TokenInfo } from '../../../Models/token-info';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [NgIf, DatePipe, MakeCommentComponent, CommentDetailsComponent, NgFor, RouterLink, CommonModule],
  templateUrl: './event-detail.component.html',
  styleUrl: './event-detail.component.css'
})
export class EventDetailComponent implements OnInit {
  /** Holds event details */
  evento!: Evento;

  /** Indicates whether the event details are loading */
  isLoading = true;

  /** Stores an error message if loading fails */
  errorMessage = '';

  /** Controls visibility of the comment section */
  makingAComment = false;

  /** Stores the username of the authenticated user */
  username: string = '';

  /** Indicates if the user is attending the event */
  isAttending = false;

  /** Holds the authenticated user data */
  user: TokenInfo | null = null;

  /** Controls the status menu visibility */
  isStatusMenuOpen = false;

  /** List of available event statuses */
  statuses = ['Open', 'Closed', 'Canceled'];

  /** Message displayed after status update */
  msg: string = '';

  /**
   * Constructor initializes services for routing, event handling, and authentication
   * @param route - Provides access to route parameters
   * @param eventService - Handles API calls for event management
   * @param authService - Manages user authentication
   */
  constructor(private route: ActivatedRoute, private eventService: EventService, private authService: SignInService) {}

  /**
   * Initializes component:
   * - Retrieves authenticated user data
   * - Loads event details based on route parameters
   */
  ngOnInit(): void {
    this.authService.getUserSubject().subscribe((username) => {
      this.username = username!.username;
      this.user = username;
    });

    this.route.params.subscribe(params => {
      const eventId = params['id'];
      this.loadEvent(eventId);
    });
  }

  /**
   * Fetches event details based on event ID
   * @param eventId - ID of the event to be loaded
   */
  loadEvent(eventId: number): void {
    this.eventService.getEventById(eventId).subscribe({
      next: (data) => {
        this.evento = data;
        this.isLoading = false;
        this.eventService.isAttending(this.evento.id, this.username)
          .subscribe(resp => this.isAttending = resp);
      },
      error: () => {
        this.errorMessage = 'Erro ao carregar detalhes do evento!';
        this.isLoading = false;
      }
    });
  }

  /**
   * Toggles the comment section visibility
   */
  toggleMakingAComment(): void {
    this.makingAComment = !this.makingAComment;
  }

  /**
   * Toggles user attendance for the event
   */
  toggleAttendEvent(): void {
    if (this.isAttending) {
      this.eventService.unattendEvent({ username: this.username, eventId: this.evento.id }).subscribe({
        next: () => {
          this.isAttending = false;
          this.loadEvent(this.evento.id);
        },
        error: () => {
          this.errorMessage = 'Erro ao cancelar presença no evento!';
        }
      });
    } else {
      this.eventService.attendEvent({ username: this.username, eventId: this.evento.id }).subscribe({
        next: () => {
          this.isAttending = true;
          this.loadEvent(this.evento.id);
        },
        error: () => {
          this.errorMessage = 'Erro ao confirmar presença no evento!';
        }
      });
    }
  }

  /**
   * Toggles the status menu dropdown
   */
  toggleStatusMenu(): void {
    this.isStatusMenuOpen = !this.isStatusMenuOpen;
  }

  /**
   * Updates the event status
   * @param status - The new event status
   */
  updateStatus(status: string): void {
    this.isStatusMenuOpen = false; // Close menu after selection

    this.eventService.changeEventStatus(this.evento.id, status).subscribe({
      next: (res) => {
        this.msg = res.message;
        this.loadEvent(this.evento.id);
      },
      error: (err) => {
        console.error('Erro ao atualizar estado do evento', err);
        this.msg = 'Erro ao atualizar estado do evento.';
      }
    });
  }
}