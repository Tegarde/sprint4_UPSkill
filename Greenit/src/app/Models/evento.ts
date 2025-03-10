import { Comment } from "./comment";

export interface Evento {

    id : number;

    description: string;

    location: string;

    date: Date;

    status : string;

    image? : string;

    comments: Comment[];
}
