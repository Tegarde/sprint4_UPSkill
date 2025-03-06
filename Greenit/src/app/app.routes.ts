import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PostDetailsComponent } from './Components/Post/post-details/post-details.component';

export const routes: Routes = [
    { path : '', component : AppComponent },
    {path : 'posts', component : PostDetailsComponent}
];
