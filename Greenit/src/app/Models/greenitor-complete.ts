import { Badge } from "./badge"

export interface GreenitorComplete {
    username : string
    email : string
    password : string
    role : string
    interactions : number
    image : string
    badges : Badge[]
}
