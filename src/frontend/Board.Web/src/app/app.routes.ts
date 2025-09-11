import { Routes } from '@angular/router';
import { BoardComponent } from './pages/boards-section/board/board.component';
import { BoardsListComponent } from './pages/boards-section/boards-list/boards-list.component';
import { InfoComponent } from './pages/info/info.component';
import { AuthGuard } from '@auth0/auth0-angular';

export const routes: Routes =  [{
  path: '',
  canActivate: [AuthGuard],
  children: [
    {
      path: '',
      component: BoardsListComponent,
    },
    {
      path: 'board/:id',
      component: BoardComponent,
    },
    {
      path: 'boards',
      component: BoardsListComponent,
    },
    {
      path: 'info',
      component: InfoComponent,
    },
    {
      path: '**',
      redirectTo: '',
    },
  ],
}];

