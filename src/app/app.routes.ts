import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { Dashboard } from './features/dashboard/dashboard';
import { TicketList } from './features/tickets/ticket-list/ticket-list';
import { TicketDetails } from './features/tickets/ticket-details/ticket-details';
import { CreateTicket } from './features/tickets/create-ticket/create-ticket';
import { EmployeeList } from './features/users/employee-list/employee-list';
import { ClientList } from './features/users/client-list/client-list';
import { AddEmployee } from './features/users/add-employee/add-employee';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'dashboard', component: Dashboard, canActivate: [authGuard] },
  { path: 'tickets', component: TicketList, canActivate: [authGuard] },
  { path: 'tickets/create', component: CreateTicket, canActivate: [authGuard] },
  { path: 'tickets/:id', component: TicketDetails, canActivate: [authGuard] },
  { path: 'employees', component: EmployeeList, canActivate: [authGuard] },
  { path: 'employees/add', component: AddEmployee, canActivate: [authGuard] },
  { path: 'clients', component: ClientList, canActivate: [authGuard] },
  { path: '**', redirectTo: 'login' }
];