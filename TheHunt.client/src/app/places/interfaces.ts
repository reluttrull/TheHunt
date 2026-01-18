export interface CreatePlaceRequest {
    id?: string | null;
    addedByUserId?: string | null;
    name: string;
    locationId: string;
    acceptedRadiusMeters: number;
    hint1: string;
    hint2: string;
    hint3: string;
}

export interface PlaceResponse {
    id: string;
    name: string;
    locationId: string;
    acceptedRadiusMeters: number;
    addedByUserId: string;
    addedDate: Date;
}