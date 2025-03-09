import { Component } from '@angular/core';
import { MakePostComponent } from '../Post/make-post/make-post.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MakePostComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
