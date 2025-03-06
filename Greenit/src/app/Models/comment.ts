export interface Comment {
    id : number
    content : string
    createdBy : string
    createdAt : string
    postId : number
    likedBy : number;
    commentsCounter : number;
}
