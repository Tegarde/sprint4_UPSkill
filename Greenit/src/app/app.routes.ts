import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PostDetailsComponent } from './Components/Post/post-details/post-details.component';
import { LikeDislikeComponent } from './Components/like-dislike/like-dislike.component';
import { SignUpComponent } from './Components/sign-up/sign-up.component';
import { SignInComponent } from './Components/sign-in/sign-in.component';
import { SearchComponent } from './Components/search/search.component';

export const routes: Routes = [
    { path : '', component : AppComponent },
    {path : 'posts', component : PostDetailsComponent},
    {path : 'search/:query', component : SearchComponent},
    {path : 'login', component : SignInComponent},
    {path : 'register', component : SignUpComponent},
];
