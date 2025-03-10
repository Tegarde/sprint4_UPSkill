import { Comment } from "./comment";

export interface Evento {

    id : number;

    description: string;

    location: string;

    date: Date;

    status : string;

    comments: Comment[];
}
