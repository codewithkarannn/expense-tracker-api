import { Routes } from '@angular/router';
import { AuthComponent } from './components/auth/auth.component';
import {DashboardComponent} from './components/dashboard/dashboard.component';
import {authGuard} from './auth.guard';

export const routes: Routes = [

    {
        path: 'login',
        component: AuthComponent,
        title: "Login"
    },
    {
      path: 'dashboard',
      component: DashboardComponent,
      title: "My Dashboard",
      canActivate: [authGuard]
    },
    {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full'
    }

];
