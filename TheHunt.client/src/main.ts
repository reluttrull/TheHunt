import { platformBrowser } from '@angular/platform-browser';
import { AppModule } from './app/app-module';
import * as L from 'leaflet';

delete (L.Icon.Default.prototype as any)._getIconUrl;

L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'assets/leaflet/marker-icon-2x.png',
  iconUrl: 'assets/leaflet/marker-icon.png',
  shadowUrl: 'assets/leaflet/marker-shadow.png',
});

platformBrowser().bootstrapModule(AppModule, {
  
})
  .catch(err => console.error(err));
