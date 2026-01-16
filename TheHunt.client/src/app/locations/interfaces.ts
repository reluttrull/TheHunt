export interface CreateLocationRequest {
    id?: string | null;
    latitude: number;
    longitude: number;
    recordedByUser?: string | null;
}

export interface LocationResponse {
    id: string;
    latitude: number;
    longitude: number;
    recordedByUser: string;
    recordedDate: Date;
}