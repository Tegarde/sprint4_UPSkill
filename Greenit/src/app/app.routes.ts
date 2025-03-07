import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PostDetailsComponent } from './Components/Post/post-details/post-details.component';
import { LikeDislikeComponent } from './Components/like-dislike/like-dislike.component';
import { SignUpComponent } from './Components/sign-up/sign-up.component';

export const routes: Routes = [
    { path : '', component : AppComponent },
    {path : 'posts', component : PostDetailsComponent},
    {path : 'likes', component : LikeDislikeComponent},
    {path : 'signup', component : SignUpComponent},
];
