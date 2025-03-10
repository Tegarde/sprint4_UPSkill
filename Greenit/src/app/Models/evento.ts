import { Comment } from "./comment";

export interface Evento {

    id : number;

    description: string;

    location: string;

    date: Date;

    status : number;

    image? : string;

    comments: Comment[];
}
