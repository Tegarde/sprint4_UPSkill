import { Comment } from "./comment";

export interface Post {
    id : number;
    title : string;
    content : string;
    createdBy : string;
    createdAt : string;
    category : string;
    status : boolean;
    interactions : number;
    likedBy : number;
    dislikedBy : number;
    comments : Comment[];
    image? : string;
}
