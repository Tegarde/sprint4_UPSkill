import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PostDetailsComponent } from './Components/Post/post-details/post-details.component';
import { LikeDislikeComponent } from './Components/like-dislike/like-dislike.component';
import { SignUpComponent } from './Components/sign-up/sign-up.component';
import { SignInComponent } from './Components/sign-in/sign-in.component';
import { SearchComponent } from './Components/search/search.component';
import { HomeComponent } from './Components/home/home.component';
import { CreateEventComponent } from './Components/Event/create-event/create-event.component';
import { ListEventComponent } from './Components/Event/list-event/list-event.component';
import { EventDetailComponent } from './Components/Event/event-detail/event-detail.component';
import { ProfileComponent } from './Components/profile/profile.component';

export const routes: Routes = [
    { path : '', component : HomeComponent },
    {path : 'post', component : PostDetailsComponent},
    {path : 'search/:query', component : SearchComponent},
    {path : 'login', component : SignInComponent},
    {path : 'register', component : SignUpComponent},
    {path : 'createEvent', component : CreateEventComponent},
    {path : 'listEvents', component : ListEventComponent},
    { path: 'events/:id', component: EventDetailComponent },
    { path: 'profile/:username', component: ProfileComponent }
];
