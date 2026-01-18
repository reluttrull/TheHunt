export class LatLong {
    latitude: number = 0;
    longitude: number = 0;
}

export function getLocation(): Promise<LatLong> {
    return new Promise((resolve, reject) => {
        if (!navigator.geolocation) {
            reject('Geolocation is not supported by this browser.');
            return;
        }

        navigator.geolocation.getCurrentPosition(
        (position: GeolocationPosition) => {
            let latLong:LatLong = {
                latitude: position.coords.latitude,
                longitude: position.coords.longitude
            };
            resolve(latLong);
        },
        (error: GeolocationPositionError) => {
            reject(error);
        },
        {
            enableHighAccuracy: true,
            timeout: 10000,
            maximumAge: 0
        }
        );
    });
}