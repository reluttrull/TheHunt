import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Home } from './home/home';
import { Register } from './auth/register/register';
import { Login } from './auth/login/login';
import { Account } from './auth/account/account';
import { MyLocations } from './locations/my-locations/my-locations';
import { NearbyPlacesMap } from './places/nearby-places-map/nearby-places-map';
import { MyPlacesMap } from './places/my-places-map/my-places-map';
import { Place } from './places/place/place';
import { UnknownPlace } from './places/unknown-place/unknown-place';
import { authGuard } from './auth/auth.guard';

const routes: Routes = [
  { path: 'register', component: Register },
  { path: 'login', component: Login },
  { path: 'account', component: Account, pathMatch: 'full', canActivate: [authGuard] },
  { path: 'mylocations', component: MyLocations, pathMatch: 'full', canActivate: [authGuard]},
  { path: 'nearbymap', component: NearbyPlacesMap, pathMatch: 'full', canActivate: [authGuard]},
  { path: 'myplaces', component: MyPlacesMap, pathMatch: 'full', canActivate: [authGuard]},
  { path: 'place/:id', component: Place, pathMatch: 'full', canActivate: [authGuard]},
  { path: 'unknownplace/:id', component: UnknownPlace, pathMatch: 'full', canActivate: [authGuard]},
  { path: '', component: Home, pathMatch: 'full', canActivate: [authGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
