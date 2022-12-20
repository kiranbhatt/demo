import { Photo } from "./photo";


    export interface Member {
        id: string;
        name: string;
        age: number;
        photoUrl: string;
        knownAs: string;
        created: Date;
        lastActive: Date;
        gender: string;
        introducation: string;
        lookingFor: string;
        interests: string;
        city: string;
        country: string;
        photos: Photo[];
    }



