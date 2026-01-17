import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { App } from './app';
import { Home } from './home/home';
import { Register } from './auth/register/register';
import { Login } from './auth/login/login';
import { Account } from './auth/account/account';
import { MyLocations } from './locations/my-locations/my-locations';
import { authGuard } from './auth/auth.guard';

const routes: Routes = [
  { path: 'register', component: Register },
  { path: 'login', component: Login },
  { path: 'account', component: Account, pathMatch: 'full', canActivate: [authGuard] },
  { path: 'mylocations', component: MyLocations, pathMatch: 'full', canActivate: [authGuard]},
  { path: '', component: Home, pathMatch: 'full', canActivate: [authGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
