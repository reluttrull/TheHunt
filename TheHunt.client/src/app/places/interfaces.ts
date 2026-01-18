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

export interface GetAllPlacesRequest {
    userId?: string | null;
    minLatitude?: number | null;
    maxLatitude?: number | null;
    minLongitude?: number | null;
    maxLongitude?: number | null;
}

export interface PlaceResponse {
    id: string;
    name: string;
    locationId: string;
    acceptedRadiusMeters: number;
    latitude: number;
    longitude: number;
    addedByUserId: string;
    addedDate: Date;
}