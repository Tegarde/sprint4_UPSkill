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
  imports: [NgIf,DatePipe,MakeCommentComponent,CommentDetailsComponent,NgFor, RouterLink,CommonModule],
  templateUrl: './event-detail.component.html',
  styleUrl: './event-detail.component.css'
})
export class EventDetailComponent implements OnInit{
  evento!: Evento;
  isLoading = true;
  errorMessage = '';
  makingAComment = false;
  username : string = '';
  isAttending = false;
  user : TokenInfo | null = null;
  isStatusMenuOpen = false; // Controla se o menu está aberto
  statuses = ['Open', 'Closed', 'Canceled']; // Opções disponíveis
  msg: string = '';

  constructor(private route: ActivatedRoute, private eventService: EventService,  private authService : SignInService ) {}

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
  

  toggleMakingAComment(): void {
    this.makingAComment = !this.makingAComment;
  }



  toggleAttendEvent(): void {
    if (this.isAttending) {
      
      this.eventService.unattendEvent({username:this.username,eventId: this.evento.id}).subscribe({
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

  toggleStatusMenu() {
    this.isStatusMenuOpen = !this.isStatusMenuOpen;
  }
  updateStatus(status: string) {
    this.isStatusMenuOpen = false; // Fecha o menu após a seleção

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
