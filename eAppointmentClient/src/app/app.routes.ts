import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './components/not-found/not-found.component';

export const routes: Routes = [
    {
        path:"login",
        component:LoginComponent
    },
    {
        path:"",
        component:HomeComponent,
        children:[
            {
                path:"",
                component:HomeComponent
            }
        ]
    },
    {
        path:"**",
        component:NotFoundComponent
    }
];
